using Avalonia.Headless.XUnit;
using Avalonia.Platform;
using BacklogManager.Contracts;
using BacklogManager.Models;
using BacklogManager.Services;
using Xunit;

namespace BacklogManager.Tests.Fase2;

public sealed class RoteadorTests
{
    private sealed class JogosSimuladosDeTeste : IPesquisaCatalogo
    {
        public int Chamadas { get; private set; }
        public Task<IReadOnlyList<ObraCatalogo>> BuscarAsync(string termo, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested(); Chamadas++;
            return Task.FromResult<IReadOnlyList<ObraCatalogo>>(Array.Empty<ObraCatalogo>());
        }
    }
    [Fact(DisplayName = "RT01 | Jogos sem credenciais | Roteador mantém pesquisa de jogos indisponível")]
    public void JogosAusentes() => Assert.Null(new RoteadorPesquisaCatalogo(null).Obter(TipoMidia.Jogo));
    [Fact(DisplayName = "RT02 | Jogos com serviço fornecido | Mesma instância IGDB é preservada")]
    public void JogosFornecidos() { var jogo = new JogosSimuladosDeTeste(); Assert.Same(jogo, new RoteadorPesquisaCatalogo(jogo).Obter(TipoMidia.Jogo)); }
    [Fact(DisplayName = "RT03 | Rota de anime sem credenciais | Catálogo local disponível")]
    public void Anime() => Assert.IsType<CatalogoSimulado>(new RoteadorPesquisaCatalogo(null).Obter(TipoMidia.Anime));
    [Fact(DisplayName = "RT04 | Rota de série sem credenciais | Catálogo local disponível")]
    public void Serie() => Assert.IsType<CatalogoSimulado>(new RoteadorPesquisaCatalogo(null).Obter(TipoMidia.Serie));
    [Fact(DisplayName = "RT05 | Rota de filme sem credenciais | Catálogo local disponível")]
    public void Filme() => Assert.IsType<CatalogoSimulado>(new RoteadorPesquisaCatalogo(null).Obter(TipoMidia.Filme));
    [Fact(DisplayName = "RT06 | Rota de mangá sem credenciais | Catálogo local disponível")]
    public void Manga() => Assert.IsType<CatalogoSimulado>(new RoteadorPesquisaCatalogo(null).Obter(TipoMidia.Manga));
    [Fact(DisplayName = "RT07 | Rota de livro sem credenciais | Catálogo local disponível")]
    public void Livro() => Assert.IsType<CatalogoSimulado>(new RoteadorPesquisaCatalogo(null).Obter(TipoMidia.Livro));
    [Fact(DisplayName = "RT08 | Tipo desconhecido no roteador | Tipo recusado em vez de usar categoria errada")]
    public void TipoDesconhecido() => Assert.Throws<ArgumentOutOfRangeException>(() => new RoteadorPesquisaCatalogo(null).Obter((TipoMidia)999));
    [Fact(DisplayName = "RT09 | Consultar cinco mídias simuladas | Nenhuma chamada feita ao serviço de jogos")]
    public async Task SemChamadaIgdb()
    {
        var jogos = new JogosSimuladosDeTeste(); var roteador = new RoteadorPesquisaCatalogo(jogos);
        foreach (var tipo in Enum.GetValues<TipoMidia>().Where(t => t != TipoMidia.Jogo))
            Assert.NotEmpty(await roteador.Obter(tipo)!.BuscarAsync("simulado", TestContext.Current.CancellationToken));
        Assert.Equal(0, jogos.Chamadas);
    }
    [Fact(DisplayName = "RT10 | Instanciar catálogo simulado de jogos | Recusa evita substituir a IGDB por dados falsos")]
    public void NaoSimularJogos() => Assert.Throws<ArgumentOutOfRangeException>(() => new CatalogoSimulado(TipoMidia.Jogo));
    [Fact(DisplayName = "RT11 | Alternar rotas repetidamente | Serviço de jogos e catálogos locais mantêm instâncias")]
    public void RotasEstaveis()
    {
        var r = new RoteadorPesquisaCatalogo(new JogosSimuladosDeTeste()); var anime = r.Obter(TipoMidia.Anime); r.Obter(TipoMidia.Jogo); Assert.Same(anime, r.Obter(TipoMidia.Anime));
    }
    [AvaloniaFact(DisplayName = "RT12 | Referências de capas simuladas | Recursos locais existem sem acesso à rede")]
    public async Task CapasLocais()
    {
        foreach (var tipo in Enum.GetValues<TipoMidia>().Where(t => t != TipoMidia.Jogo))
        {
            var obra = (await new CatalogoSimulado(tipo).BuscarAsync("simulado", TestContext.Current.CancellationToken))[0];
            Assert.True(AssetLoader.Exists(new Uri(obra.CapaUrl!)));
        }
    }
}
