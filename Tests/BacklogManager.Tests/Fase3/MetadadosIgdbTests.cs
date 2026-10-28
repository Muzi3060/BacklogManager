using BacklogManager.Models;
using BacklogManager.Services;
using Xunit;

namespace BacklogManager.Tests.Fase3;

public sealed class MetadadosIgdbTests
{
    private static async Task<ObraCatalogo> Buscar(string extras)
    {
        using var h = new HttpSimulado { Responder = (_, _) => Task.FromResult(HttpSimulado.Json("[{\"id\":42,\"name\":\"Alfa\"" + extras + "}]")) };
        using var c = new HttpClient(h);
        return Assert.Single(await new PesquisaJogosIgdb(new IgdbService(c, "cliente-teste", "token-teste")).BuscarAsync("Alfa", TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "CI01 | IGDB com image_id | Referência HTTPS de capa grande é construída")]
    public async Task ImageId() => Assert.Equal("https://images.igdb.com/igdb/image/upload/t_cover_big/co_42.jpg", (await Buscar(",\"cover\":{\"image_id\":\"co_42\"}")).CapaUrl);
    [Fact(DisplayName = "CI02 | IGDB sem capa | Jogo continua disponível com capa ausente")]
    public async Task SemCapa() { var i = await Buscar(""); Assert.Null(i.CapaUrl); Assert.Equal("42", i.IdentificadorExterno); }
    [Fact(DisplayName = "CI03 | Capa explicitamente null | Compatibilidade com catálogo sem imagem")]
    public async Task CapaNula() => Assert.Null((await Buscar(",\"cover\":null")).CapaUrl);
    [Fact(DisplayName = "CI04 | Capa com URL relativa ao protocolo | URL é normalizada para HTTPS")]
    public async Task Protocolo() => Assert.Equal("https://images.igdb.com/igdb/image/upload/t_thumb/a.jpg", (await Buscar(",\"cover\":{\"url\":\"//images.igdb.com/igdb/image/upload/t_thumb/a.jpg\"}")).CapaUrl);
    [Fact(DisplayName = "CI05 | image_id com tentativa de caminho | ID inseguro não vira URL")]
    public async Task IdInvalido() => Assert.Null((await Buscar(",\"cover\":{\"image_id\":\"../../segredo\"}")).CapaUrl);
    [Fact(DisplayName = "CI06 | URL de capa fora do CDN | Referência não autorizada é ignorada")]
    public async Task OutroHost() => Assert.Null((await Buscar(",\"cover\":{\"url\":\"https://outro.test/capa.jpg\"}")).CapaUrl);
    [Fact(DisplayName = "CI07 | Gêneros múltiplos | Nomes são separados do gênero pessoal")]
    public async Task Generos() => Assert.Equal(new[] { "RPG", "Aventura" }, (await Buscar(",\"genres\":[{\"name\":\"RPG\"},{\"name\":\"Aventura\"}]")).Generos);
    [Fact(DisplayName = "CI08 | Plataformas múltiplas | Nomes das plataformas são mantidos")]
    public async Task Plataformas() => Assert.Equal(new[] { "PC", "Switch" }, (await Buscar(",\"platforms\":[{\"name\":\"PC\"},{\"name\":\"Switch\"}]")).Plataformas);
    [Fact(DisplayName = "CI09 | Nomes incompletos ou repetidos | Nulos e vazios ignorados e repetidos removidos")]
    public async Task NomesInvalidos() { var i = await Buscar(",\"genres\":[null,{}, {\"name\":\" \"},{\"name\":\" RPG \"},{\"name\":\"rpg\"}]"); Assert.Equal(new[] { "RPG" }, i.Generos); }
    [Fact(DisplayName = "CI10 | Resposta antiga id e name | Novos campos opcionais não são exigidos")]
    public async Task Antiga() { var i = await Buscar(""); Assert.Empty(i.Generos); Assert.Empty(i.Plataformas); Assert.Equal("Alfa", i.Titulo); Assert.Equal("IGDB", i.Origem); Assert.False(i.Simulado); }
    [Fact(DisplayName = "CI11 | Plataformas e gêneros null | Coleções vazias em vez de nulas")]
    public async Task Nulos() { var i = await Buscar(",\"genres\":null,\"platforms\":null"); Assert.Empty(i.Generos); Assert.Empty(i.Plataformas); }
    [Fact(DisplayName = "CI12 | image_id e URL simultâneos | image_id válido tem prioridade")]
    public async Task Prioridade() => Assert.EndsWith("/co42.jpg", (await Buscar(",\"cover\":{\"image_id\":\"co42\",\"url\":\"https://outro.test/a.jpg\"}")).CapaUrl);
    [Fact(DisplayName = "CI13 | Consulta expandida | POST, endpoint, headers e limite continuam corretos")]
    public async Task ProtocoloHttp()
    {
        using var h = new HttpSimulado { Responder = async (r, _) => { Assert.Equal(HttpMethod.Post, r.Method); Assert.Equal("https://api.igdb.com/v4/games", r.RequestUri!.AbsoluteUri); Assert.Equal("cliente-teste", Assert.Single(r.Headers.GetValues("Client-ID"))); Assert.Equal("Bearer", r.Headers.Authorization!.Scheme); var body = await r.Content!.ReadAsStringAsync(TestContext.Current.CancellationToken); Assert.Contains("cover.image_id,cover.url,genres.name,platforms.name", body); Assert.EndsWith("limit 15;", body); return HttpSimulado.Json("[]"); } };
        using var c = new HttpClient(h); Assert.Empty(await new PesquisaJogosIgdb(new IgdbService(c, "cliente-teste", "token-teste")).BuscarAsync("Alfa", TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "CI14 | Cancelamento antes da busca | Nenhum pedido HTTP é enviado")]
    public async Task Cancelamento() { using var h = new HttpSimulado(); using var c = new HttpClient(h); using var cts = new CancellationTokenSource(); cts.Cancel(); await Assert.ThrowsAnyAsync<OperationCanceledException>(() => new PesquisaJogosIgdb(new IgdbService(c, "c", "t")).BuscarAsync("Alfa", cts.Token)); Assert.Equal(0, h.Chamadas); }
    [Fact(DisplayName = "CI15 | Metadados completos persistidos | Capa, gêneros e plataforma sobrevivem ao reinício")]
    public async Task Persistir() { var obra = await Buscar(",\"cover\":{\"image_id\":\"co42\"},\"genres\":[{\"name\":\"RPG\"}],\"platforms\":[{\"name\":\"PC\"}]"); var item = new ItemBiblioteca(obra, ConsultasTests.Item().Pessoal); var r = Fase2.ApoioMultimidia.Roundtrip(item); Assert.Equal(obra.CapaUrl, r.Catalogo.CapaUrl); Assert.Equal(obra.Plataformas, r.Catalogo.Plataformas); Assert.Equal(obra.Generos, r.Catalogo.Generos); }
}
