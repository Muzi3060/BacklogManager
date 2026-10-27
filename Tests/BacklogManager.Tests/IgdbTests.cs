using System.Net;
using BacklogManager.Services;
using Xunit;

namespace BacklogManager.Tests;

public sealed class IgdbTests : IDisposable
{
    private readonly HttpSimulado _http = new();
    private readonly HttpClient _cliente;
    private readonly IgdbService _servico;
    public IgdbTests() { _cliente = new(_http); _servico = new(_cliente, "cliente-simulado", "token-simulado"); }
    public void Dispose() => _cliente.Dispose();
    [Fact(DisplayName = "H01 | Pesquisa bem-sucedida | POST no endpoint correto com headers e IDs")]
    public async Task Sucesso()
    {
        _http.Responder = async (r, _) =>
        {
            Assert.Equal(HttpMethod.Post, r.Method); Assert.Equal("https://api.igdb.com/v4/games", r.RequestUri!.ToString());
            Assert.Equal("cliente-simulado", Assert.Single(r.Headers.GetValues("Client-ID")));
            Assert.Equal("Bearer", r.Headers.Authorization!.Scheme); Assert.Equal("token-simulado", r.Headers.Authorization.Parameter);
            Assert.Equal("text/plain", r.Content!.Headers.ContentType!.MediaType);
            Assert.Equal("search \"Persona\"; fields id,name; limit 15;", await r.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
            return HttpSimulado.Json("[{\"id\":123,\"name\":\"Persona\"}]");
        };
        var jogo = Assert.Single(await _servico.BuscarJogosAsync("Persona", TestContext.Current.CancellationToken)); Assert.Equal(123, jogo.Id); Assert.Equal("Persona", jogo.Name);
    }

    [Fact(DisplayName = "H02 | Busca sem resultados | Lista vazia sem falha")]
    public async Task SemResultado() => Assert.Empty(await _servico.BuscarJogosAsync("Ausente", TestContext.Current.CancellationToken));

    [Theory]
    [InlineData("H03", 401, "autenticação")]
    [InlineData("H04", 403, "autenticação")]
    [InlineData("H05", 429, "limite")]
    [InlineData("H06", 500, "indisponível")]
    [InlineData("H07", 503, "indisponível")]
    [InlineData("H08", 400, "HTTP 400")]
    [InlineData("H09", 404, "HTTP 404")]
    public async Task FalhaHttpProduzMensagemSeguraSemRepeticaoAutomatica(string id, int status, string mensagem)
    {
        _ = id; _http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json("token-real-nao-deve-aparecer", (HttpStatusCode)status));
        var erro = await Assert.ThrowsAsync<PesquisaIgdbException>(() => _servico.BuscarJogosAsync("Jogo", TestContext.Current.CancellationToken));
        Assert.Contains(mensagem, erro.Message); Assert.DoesNotContain("token-real", erro.Message); Assert.Equal(1, _http.Chamadas);
    }

    [Fact(DisplayName = "H10 | Falha de conexão | Mensagem de conexão sem detalhes sensíveis")]
    public async Task Rede()
    {
        _http.Responder = (_, _) => throw new HttpRequestException("detalhe-secreto");
        var erro = await Assert.ThrowsAsync<PesquisaIgdbException>(() => _servico.BuscarJogosAsync("Jogo", TestContext.Current.CancellationToken));
        Assert.Contains("conexão", erro.Message); Assert.DoesNotContain("detalhe-secreto", erro.Message);
    }

    [Fact(DisplayName = "H11 | Timeout de transporte | Mensagem distinta de cancelamento do usuário")]
    public async Task Timeout()
    {
        _http.Responder = (_, _) => throw new TaskCanceledException("timeout");
        var erro = await Assert.ThrowsAsync<PesquisaIgdbException>(() => _servico.BuscarJogosAsync("Jogo", TestContext.Current.CancellationToken));
        Assert.Contains("demorou", erro.Message);
    }

    [Theory]
    [InlineData("H12", "{")]
    [InlineData("H13", "null")]
    [InlineData("H14", "{}")]
    [InlineData("H15", "[null]")]
    [InlineData("H16", "[{\"id\":0,\"name\":\"Jogo\"}]")]
    [InlineData("H17", "[{\"id\":-1,\"name\":\"Jogo\"}]")]
    [InlineData("H18", "[{\"id\":1,\"name\":null}]")]
    [InlineData("H19", "[{\"id\":1,\"name\":\" \"}]")]
    [InlineData("H20", "[{\"id\":\"abc\",\"name\":\"Jogo\"}]")]
    [InlineData("H21", "")]
    public async Task RespostaInvalidaGeraFalhaSemListaParcial(string id, string json)
    {
        _ = id; _http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json(json));
        await Assert.ThrowsAsync<PesquisaIgdbException>(() => _servico.BuscarJogosAsync("Jogo", TestContext.Current.CancellationToken));
    }

    [Fact(DisplayName = "H22 | Cancelamento antes do envio | Nenhuma chamada HTTP")]
    public async Task CanceladoAntes()
    {
        using var cts = new CancellationTokenSource(); cts.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => _servico.BuscarJogosAsync("Jogo", cts.Token)); Assert.Equal(0, _http.Chamadas);
    }

    [Fact(DisplayName = "H23 | Cancelamento durante HTTP | Token interrompe transporte")]
    public async Task CanceladoDurante()
    {
        var iniciou = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        _http.Responder = async (_, token) => { iniciou.SetResult(); await Task.Delay(System.Threading.Timeout.Infinite, token); return HttpSimulado.Json("[]"); };
        using var cts = new CancellationTokenSource(); var busca = _servico.BuscarJogosAsync("Jogo", cts.Token);
        await iniciou.Task.WaitAsync(TimeSpan.FromSeconds(3), TestContext.Current.CancellationToken); cts.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => busca);
    }

    [Fact(DisplayName = "H24 | Recuperação após erro HTTP | Próxima busca retorna resultado")]
    public async Task Recuperacao()
    {
        _http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json("{}", HttpStatusCode.ServiceUnavailable));
        await Assert.ThrowsAsync<PesquisaIgdbException>(() => _servico.BuscarJogosAsync("Jogo", TestContext.Current.CancellationToken));
        _http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json("[{\"id\":2,\"name\":\"Jogo\"}]"));
        Assert.Single(await _servico.BuscarJogosAsync("Jogo", TestContext.Current.CancellationToken)); Assert.Equal(2, _http.Chamadas);
    }

    [Fact(DisplayName = "H25 | Dois serviços compartilham cliente | Headers independentes sem duplicação")]
    public async Task HeadersIndependentes()
    {
        var outro = new IgdbService(_cliente, "outro-cliente", "outro-token-simulado");
        _http.Responder = (r, _) =>
        {
            Assert.Single(r.Headers.GetValues("Client-ID"));
            Assert.Equal(_http.Chamadas == 1 ? "cliente-simulado" : "outro-cliente", r.Headers.GetValues("Client-ID").Single());
            return Task.FromResult(HttpSimulado.Json("[]"));
        };
        await _servico.BuscarJogosAsync("Um", TestContext.Current.CancellationToken); await outro.BuscarJogosAsync("Dois", TestContext.Current.CancellationToken);
        Assert.False(_cliente.DefaultRequestHeaders.Contains("Client-ID")); Assert.Null(_cliente.DefaultRequestHeaders.Authorization);
    }

    [Fact(DisplayName = "H26 | JSON com maiúsculas | Desserialização mantém compatibilidade")]
    public async Task Maiusculas()
    {
        _http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json("[{\"ID\":9,\"NAME\":\"Jogo\"}]"));
        Assert.Equal(9, Assert.Single(await _servico.BuscarJogosAsync("Jogo", TestContext.Current.CancellationToken)).Id);
    }

    [Fact(DisplayName = "H27 | Requisições sucessivas | Cliente reutilizado permanece disponível")]
    public async Task Reuso() { await _servico.BuscarJogosAsync("Um", TestContext.Current.CancellationToken); await _servico.BuscarJogosAsync("Dois", TestContext.Current.CancellationToken); Assert.Equal(2, _http.Chamadas); }

    [Fact(DisplayName = "H28 | Resposta HTTP concluída | Conteúdo descartado após leitura")]
    public async Task Descarte()
    {
        var conteudo = new ConteudoObservavel(); _http.Responder = (_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = conteudo });
        await _servico.BuscarJogosAsync("Jogo", TestContext.Current.CancellationToken); Assert.True(conteudo.Descartado);
    }
    private sealed class ConteudoObservavel : StringContent
    {
        public bool Descartado { get; private set; }
        public ConteudoObservavel() : base("[]") { }
        protected override void Dispose(bool disposing) { Descartado = true; base.Dispose(disposing); }
    }

    [Theory]
    [InlineData("Q01", null, null)]
    [InlineData("Q02", "", null)]
    [InlineData("Q03", "   ", null)]
    [InlineData("Q04", "  Jogo  ", "Jogo")]
    [InlineData("Q05", "Jogo \"Especial\"", "Jogo \\\"Especial\\\"")]
    [InlineData("Q06", "C:\\Jogos", "C:\\\\Jogos")]
    [InlineData("Q07", "Ação 日本語 🎮", "Ação 日本語 🎮")]
    [InlineData("Q08", "Jogo; limit 500;", "Jogo; limit 500;")]
    [InlineData("Q09", "\"; fields *;", "\\\"; fields *;")]
    [InlineData("Q10", "King's Quest", "King's Quest")]
    public async Task TermosValidosSaoEscapadosOuNaoEnviados(string id, string? termo, string? esperado)
    {
        _ = id; _http.Responder = async (r, _) =>
        { Assert.Equal($"search \"{esperado}\"; fields id,name; limit 15;", await r.Content!.ReadAsStringAsync(TestContext.Current.CancellationToken)); return HttpSimulado.Json("[]"); };
        Assert.Empty(await _servico.BuscarJogosAsync(termo!, TestContext.Current.CancellationToken)); Assert.Equal(esperado == null ? 0 : 1, _http.Chamadas);
    }

    [Theory]
    [InlineData("Q11", "Linha\nOutra")]
    [InlineData("Q12", "Linha\rOutra")]
    [InlineData("Q13", "Jogo\0")]
    public async Task ControlesRecusadosAntesDoEnvio(string id, string termo)
    { _ = id; await Assert.ThrowsAsync<PesquisaIgdbException>(() => _servico.BuscarJogosAsync(termo, TestContext.Current.CancellationToken)); Assert.Equal(0, _http.Chamadas); }

    [Fact(DisplayName = "Q14 | Limite de 200 caracteres | Busca aceita")]
    public async Task LimiteAceito() { await _servico.BuscarJogosAsync(new string('a', 200), TestContext.Current.CancellationToken); Assert.Equal(1, _http.Chamadas); }
    [Fact(DisplayName = "Q15 | Mais de 200 caracteres | Busca recusada antes do HTTP")]
    public async Task LimiteRecusado() { await Assert.ThrowsAsync<PesquisaIgdbException>(() => _servico.BuscarJogosAsync(new string('a', 201), TestContext.Current.CancellationToken)); Assert.Equal(0, _http.Chamadas); }
}
