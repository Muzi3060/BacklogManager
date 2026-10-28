using System.Net;
using BacklogManager.Models;
using BacklogManager.Services;
using Xunit;

namespace BacklogManager.Tests.Fase1;

public sealed class PesquisaCatalogoTests : IDisposable
{
    private readonly HttpSimulado _http = new();
    private readonly HttpClient _cliente;
    private readonly PesquisaJogosIgdb _pesquisa;
    public PesquisaCatalogoTests()
    {
        _cliente = new HttpClient(_http);
        _pesquisa = new PesquisaJogosIgdb(new IgdbService(_cliente, "cliente-simulado", "token-simulado"));
        _http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json("[{\"id\":42,\"name\":\"Jogo\"}]"));
    }
    public void Dispose() => _cliente.Dispose();
    private Task<IReadOnlyList<ObraCatalogo>> Buscar(string termo = "Jogo") => _pesquisa.BuscarAsync(termo, TestContext.Current.CancellationToken);
    [Fact(DisplayName = "PE01 | Pesquisa IGDB pelo contrato | Retorna modelo de catálogo de jogo com referência IGDB")]
    public async Task ModeloCatalogo() { var obra = Assert.Single(await Buscar()); Assert.Equal(TipoMidia.Jogo, obra.Tipo); Assert.Equal("IGDB", obra.Origem); Assert.Equal("42", obra.IdentificadorExterno); Assert.Equal("Jogo", obra.Titulo); }
    [Fact(DisplayName = "PE02 | Pesquisa sem jogos | Lista vazia mantida pelo adaptador")]
    public async Task SemJogos() { _http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json("[]")); Assert.Empty(await Buscar()); }
    [Fact(DisplayName = "PE03 | Vários resultados | Ordem dos resultados e IDs preservados")]
    public async Task Ordem() { _http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json("[{\"id\":2,\"name\":\"Dois\"},{\"id\":1,\"name\":\"Um\"}]")); Assert.Equal(new[] { "2", "1" }, (await Buscar()).Select(o => o.IdentificadorExterno)); }
    [Fact(DisplayName = "PE04 | Pesquisa com texto Unicode | Título retornado preservado")]
    public async Task Unicode() { _http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json("[{\"id\":1,\"name\":\"Ação 日本語\"}]")); Assert.Equal("Ação 日本語", Assert.Single(await Buscar("Ação")).Titulo); }
    [Fact(DisplayName = "PE05 | HTTP 401 através do adaptador | Falha de autenticação preservada sem lista enganosa")]
    public async Task Autenticacao() { _http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json("{}", HttpStatusCode.Unauthorized)); Assert.Contains("autenticação", (await Assert.ThrowsAsync<PesquisaIgdbException>(() => Buscar())).Message); }
    [Fact(DisplayName = "PE06 | HTTP 429 através do adaptador | Falha de limite preservada")]
    public async Task Limite() { _http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json("{}", HttpStatusCode.TooManyRequests)); Assert.Contains("limite", (await Assert.ThrowsAsync<PesquisaIgdbException>(() => Buscar())).Message); }
    [Fact(DisplayName = "PE07 | HTTP 503 através do adaptador | Falha de indisponibilidade preservada")]
    public async Task Servidor() { _http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json("{}", HttpStatusCode.ServiceUnavailable)); Assert.Contains("indisponível", (await Assert.ThrowsAsync<PesquisaIgdbException>(() => Buscar())).Message); }
    [Fact(DisplayName = "PE08 | JSON inválido através do adaptador | Exceção controlada preservada")]
    public async Task JsonInvalido() { _http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json("{")); await Assert.ThrowsAsync<PesquisaIgdbException>(() => Buscar()); }
    [Fact(DisplayName = "PE09 | Cancelamento antes da pesquisa | Nenhum HTTP enviado pelo adaptador")]
    public async Task CanceladoAntes() { using var cts = new CancellationTokenSource(); cts.Cancel(); await Assert.ThrowsAnyAsync<OperationCanceledException>(() => _pesquisa.BuscarAsync("Jogo", cts.Token)); Assert.Equal(0, _http.Chamadas); }
    [Fact(DisplayName = "PE10 | Cancelamento durante HTTP | Sinal propagado pelo contrato até o transporte")]
    public async Task CanceladoDurante()
    {
        var iniciou = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        _http.Responder = async (_, token) => { iniciou.SetResult(); await Task.Delay(Timeout.Infinite, token); return HttpSimulado.Json("[]"); };
        using var cts = new CancellationTokenSource(); var busca = _pesquisa.BuscarAsync("Jogo", cts.Token);
        await iniciou.Task.WaitAsync(TimeSpan.FromSeconds(3), TestContext.Current.CancellationToken); cts.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => busca);
    }
    [Fact(DisplayName = "PE11 | Resultado de pesquisa | Nenhum dado pessoal inventado nem download de capa")]
    public async Task SomenteCatalogo() { var obra = Assert.Single(await Buscar()); Assert.Empty(obra.Generos); Assert.Null(obra.CapaUrl); Assert.Equal(1, _http.Chamadas); }
    [Fact(DisplayName = "PE12 | Recuperar pesquisa após conexão falha | Próxima chamada retorna catálogo válido")]
    public async Task Recuperacao()
    {
        _http.Responder = (_, _) => throw new HttpRequestException(); await Assert.ThrowsAsync<PesquisaIgdbException>(() => Buscar());
        _http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json("[{\"id\":42,\"name\":\"Jogo\"}]")); Assert.Single(await Buscar());
    }
}
