using BacklogManager.Configuracao;
using Xunit;

namespace BacklogManager.Tests.Fase1;

public sealed class ComposicaoTests : IDisposable
{
    private readonly PastaTeste _pasta = new();
    public void Dispose() => _pasta.Dispose();
    private static Func<string, string?> Variaveis(string? legado = null, string? novo = null, string? cliente = "cliente-simulado") =>
        nome => nome switch { "IGDB_CLIENT_ID" => cliente, "IGDB_ACESS_TOKEN" => legado, "IGDB_ACCESS_TOKEN" => novo, _ => null };

    [Fact(DisplayName = "CO01 | Composição sem credenciais | Biblioteca local disponível sem pesquisa ou recurso HTTP")]
    public void SemCredenciais()
    {
        var d = ComposicaoAplicativo.Criar(_pasta.Arquivo, _ => null);
        Assert.Null(d.Pesquisa); Assert.Null(d.RecursoPesquisa); d.Biblioteca.Carregar(); Assert.True(d.Biblioteca.Carregada);
    }
    [Fact(DisplayName = "CO02 | Composição com token legado | Adaptador IGDB e cliente próprio criados")]
    public void TokenLegado()
    {
        var d = ComposicaoAplicativo.Criar(_pasta.Arquivo, Variaveis(legado: "legado-simulado"));
        using (d.RecursoPesquisa) { Assert.NotNull(d.Pesquisa); Assert.IsType<HttpClient>(d.RecursoPesquisa); }
    }
    [Fact(DisplayName = "CO03 | Composição com token corrigido | Adaptador IGDB disponível sem nome legado")]
    public void TokenCorrigido()
    {
        var d = ComposicaoAplicativo.Criar(_pasta.Arquivo, Variaveis(novo: "novo-simulado"));
        using (d.RecursoPesquisa) { Assert.NotNull(d.Pesquisa); }
    }
    [Fact(DisplayName = "CO04 | Configuração ausente | Fábrica HTTP nem sequer é chamada")]
    public void NaoAlocarCliente()
    {
        var chamadas = 0;
        var d = ComposicaoAplicativo.Criar(_pasta.Arquivo, _ => null, () => { chamadas++; return new HttpClient(); });
        Assert.Equal(0, chamadas); Assert.Null(d.RecursoPesquisa);
    }
    [Fact(DisplayName = "CO05 | Ambos os tokens na composição | Cabeçalho usa token corrigido pela cadeia real de adaptadores")]
    public async Task PrecedenciaReal()
    {
        using var http = new HttpSimulado();
        http.Responder = (r, _) => { Assert.Equal("novo-simulado", r.Headers.Authorization!.Parameter); return Task.FromResult(HttpSimulado.Json("[]")); };
        var d = ComposicaoAplicativo.Criar(_pasta.Arquivo, Variaveis("legado-simulado", "novo-simulado"), () => new HttpClient(http));
        using (d.RecursoPesquisa) { Assert.Empty(await d.Pesquisa!.BuscarAsync("Jogo", TestContext.Current.CancellationToken)); }
        Assert.Equal(1, http.Chamadas);
    }
    [Fact(DisplayName = "CO06 | Credencial com quebra interna | Pesquisa desativada sem criar HTTP")]
    public void CredencialInvalida()
    {
        var d = ComposicaoAplicativo.Criar(_pasta.Arquivo, Variaveis(novo: "abc\r\nxyz"));
        Assert.Null(d.Pesquisa); Assert.Null(d.RecursoPesquisa);
    }
    [Fact(DisplayName = "CO07 | Caminho informado na composição | Origem mantida sem mudança automática de diretório")]
    public void Caminho()
    {
        var d = ComposicaoAplicativo.Criar(_pasta.Arquivo, _ => null);
        Assert.Equal(Path.GetFullPath(_pasta.Arquivo), d.Biblioteca.DescricaoOrigem);
        Assert.False(File.Exists(_pasta.Arquivo));
    }
    [Fact(DisplayName = "CO08 | Cliente criado pela composição | Timeout de quinze segundos configurado")]
    public void Timeout()
    {
        var d = ComposicaoAplicativo.Criar(_pasta.Arquivo, Variaveis(novo: "simulado"));
        using (d.RecursoPesquisa) { Assert.Equal(TimeSpan.FromSeconds(15), Assert.IsType<HttpClient>(d.RecursoPesquisa).Timeout); }
    }
    [Fact(DisplayName = "CO09 | Compor aplicação com arquivo existente | Construção não carrega nem grava os dados")]
    public void SemEfeitoArquivo()
    {
        File.WriteAllText(_pasta.Arquivo, "arquivo propositalmente inválido"); var bytes = File.ReadAllBytes(_pasta.Arquivo);
        var d = ComposicaoAplicativo.Criar(_pasta.Arquivo, _ => null);
        Assert.False(d.Biblioteca.Carregada); Assert.Equal(bytes, File.ReadAllBytes(_pasta.Arquivo));
    }
    [Fact(DisplayName = "CO10 | Duas composições independentes | Clientes e serviços não compartilham estado de sessão")]
    public void Independentes()
    {
        var um = ComposicaoAplicativo.Criar(_pasta.Arquivo, Variaveis(novo: "simulado"));
        var dois = ComposicaoAplicativo.Criar(_pasta.Arquivo, Variaveis(novo: "simulado"));
        using (um.RecursoPesquisa) using (dois.RecursoPesquisa)
        {
            Assert.NotSame(um.RecursoPesquisa, dois.RecursoPesquisa); Assert.NotSame(um.Biblioteca, dois.Biblioteca);
            um.Biblioteca.Carregar(); Assert.False(dois.Biblioteca.Carregada);
        }
    }
}
