using System.Text.Json;
using BacklogManager.Models;
using BacklogManager.Services;
using Xunit;
using static BacklogManager.Tests.Fase2.ApoioMultimidia;

namespace BacklogManager.Tests.Fase2;

public sealed class CatalogosSimuladosTests
{
    [Fact(DisplayName = "SM01 | Anime: Pesquisar exemplos | Três resultados fictícios identificados")]
    public async Task SM01()
    {
        var servico = new CatalogoSimulado(TipoMidia.Anime);
        var obras = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); Assert.Equal(3, obras.Count); Assert.All(obras, o => { Assert.True(o.Simulado); Assert.Contains("simulado", o.Titulo); });
    }
    [Fact(DisplayName = "SM02 | Anime: Pesquisar termo vazio | Lista vazia sem inventar resultado")]
    public async Task SM02()
    {
        var servico = new CatalogoSimulado(TipoMidia.Anime);
        Assert.Empty(await servico.BuscarAsync("", TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM03 | Anime: Pesquisar termo nulo | Lista vazia sem exceção")]
    public async Task SM03()
    {
        var servico = new CatalogoSimulado(TipoMidia.Anime);
        Assert.Empty(await servico.BuscarAsync(null!, TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM04 | Anime: Pesquisar com caixa diferente | Pesquisa ignora maiúsculas")]
    public async Task SM04()
    {
        var servico = new CatalogoSimulado(TipoMidia.Anime);
        Assert.Equal(3, (await servico.BuscarAsync("SIMULADO", TestContext.Current.CancellationToken)).Count);
    }
    [Fact(DisplayName = "SM05 | Anime: Pesquisar título parcial | Somente a obra correspondente")]
    public async Task SM05()
    {
        var servico = new CatalogoSimulado(TipoMidia.Anime);
        var todas = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); var termo = todas[0].Titulo.Split(' ').First(palavra => palavra.Length > 3); Assert.Single(await servico.BuscarAsync(termo, TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM06 | Anime: Pesquisar termo ausente | Lista vazia válida")]
    public async Task SM06()
    {
        var servico = new CatalogoSimulado(TipoMidia.Anime);
        Assert.Empty(await servico.BuscarAsync("zzzz-inexistente", TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM07 | Anime: Cancelar antes da pesquisa | Cancelamento respeitado")]
    public async Task SM07()
    {
        var servico = new CatalogoSimulado(TipoMidia.Anime);
        using var cts = new CancellationTokenSource(); cts.Cancel(); await Assert.ThrowsAnyAsync<OperationCanceledException>(() => servico.BuscarAsync("simulado", cts.Token));
    }
    [Fact(DisplayName = "SM08 | Anime: Repetir pesquisa | IDs estáveis e distintos no catálogo")]
    public async Task SM08()
    {
        var servico = new CatalogoSimulado(TipoMidia.Anime);
        var um = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); var dois = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); Assert.Equal(um.Select(o => o.IdentificadorExterno), dois.Select(o => o.IdentificadorExterno)); Assert.Equal(3, um.Select(o => o.IdentificadorExterno).Distinct().Count());
    }
    [Fact(DisplayName = "SM09 | Anime: Consultar metadados | Tipo, gêneros, capa local e origem simulada disponíveis")]
    public async Task SM09()
    {
        var servico = new CatalogoSimulado(TipoMidia.Anime);
        var o = (await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken))[0]; Assert.Equal(TipoMidia.Anime, o.Tipo); Assert.Equal("Simulado", o.Origem); Assert.NotEmpty(o.Generos); Assert.StartsWith("avares://BacklogManager/Assets/", o.CapaUrl!);
    }
    [Fact(DisplayName = "SM10 | Anime: Salvar resultado do catálogo | Origem simulada e referências preservadas após recarga")]
    public async Task SM10()
    {
        var servico = new CatalogoSimulado(TipoMidia.Anime);
        var o = (await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken))[0]; var lido = Roundtrip(new ItemBiblioteca(o, Item(TipoMidia.Anime).Pessoal)); Assert.True(lido.Catalogo.Simulado); Assert.Equal(o.IdentificadorExterno, lido.Catalogo.IdentificadorExterno); Assert.Equal(o.CapaUrl, lido.Catalogo.CapaUrl);
    }
    [Fact(DisplayName = "SM11 | Serie: Pesquisar exemplos | Três resultados fictícios identificados")]
    public async Task SM11()
    {
        var servico = new CatalogoSimulado(TipoMidia.Serie);
        var obras = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); Assert.Equal(3, obras.Count); Assert.All(obras, o => { Assert.True(o.Simulado); Assert.Contains("simulado", o.Titulo); });
    }
    [Fact(DisplayName = "SM12 | Serie: Pesquisar termo vazio | Lista vazia sem inventar resultado")]
    public async Task SM12()
    {
        var servico = new CatalogoSimulado(TipoMidia.Serie);
        Assert.Empty(await servico.BuscarAsync("", TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM13 | Serie: Pesquisar termo nulo | Lista vazia sem exceção")]
    public async Task SM13()
    {
        var servico = new CatalogoSimulado(TipoMidia.Serie);
        Assert.Empty(await servico.BuscarAsync(null!, TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM14 | Serie: Pesquisar com caixa diferente | Pesquisa ignora maiúsculas")]
    public async Task SM14()
    {
        var servico = new CatalogoSimulado(TipoMidia.Serie);
        Assert.Equal(3, (await servico.BuscarAsync("SIMULADO", TestContext.Current.CancellationToken)).Count);
    }
    [Fact(DisplayName = "SM15 | Serie: Pesquisar título parcial | Somente a obra correspondente")]
    public async Task SM15()
    {
        var servico = new CatalogoSimulado(TipoMidia.Serie);
        var todas = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); var termo = todas[0].Titulo.Split(' ').First(palavra => palavra.Length > 3); Assert.Single(await servico.BuscarAsync(termo, TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM16 | Serie: Pesquisar termo ausente | Lista vazia válida")]
    public async Task SM16()
    {
        var servico = new CatalogoSimulado(TipoMidia.Serie);
        Assert.Empty(await servico.BuscarAsync("zzzz-inexistente", TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM17 | Serie: Cancelar antes da pesquisa | Cancelamento respeitado")]
    public async Task SM17()
    {
        var servico = new CatalogoSimulado(TipoMidia.Serie);
        using var cts = new CancellationTokenSource(); cts.Cancel(); await Assert.ThrowsAnyAsync<OperationCanceledException>(() => servico.BuscarAsync("simulado", cts.Token));
    }
    [Fact(DisplayName = "SM18 | Serie: Repetir pesquisa | IDs estáveis e distintos no catálogo")]
    public async Task SM18()
    {
        var servico = new CatalogoSimulado(TipoMidia.Serie);
        var um = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); var dois = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); Assert.Equal(um.Select(o => o.IdentificadorExterno), dois.Select(o => o.IdentificadorExterno)); Assert.Equal(3, um.Select(o => o.IdentificadorExterno).Distinct().Count());
    }
    [Fact(DisplayName = "SM19 | Serie: Consultar metadados | Tipo, gêneros, capa local e origem simulada disponíveis")]
    public async Task SM19()
    {
        var servico = new CatalogoSimulado(TipoMidia.Serie);
        var o = (await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken))[0]; Assert.Equal(TipoMidia.Serie, o.Tipo); Assert.Equal("Simulado", o.Origem); Assert.NotEmpty(o.Generos); Assert.StartsWith("avares://BacklogManager/Assets/", o.CapaUrl!);
    }
    [Fact(DisplayName = "SM20 | Serie: Salvar resultado do catálogo | Origem simulada e referências preservadas após recarga")]
    public async Task SM20()
    {
        var servico = new CatalogoSimulado(TipoMidia.Serie);
        var o = (await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken))[0]; var lido = Roundtrip(new ItemBiblioteca(o, Item(TipoMidia.Serie).Pessoal)); Assert.True(lido.Catalogo.Simulado); Assert.Equal(o.IdentificadorExterno, lido.Catalogo.IdentificadorExterno); Assert.Equal(o.CapaUrl, lido.Catalogo.CapaUrl);
    }
    [Fact(DisplayName = "SM21 | Filme: Pesquisar exemplos | Três resultados fictícios identificados")]
    public async Task SM21()
    {
        var servico = new CatalogoSimulado(TipoMidia.Filme);
        var obras = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); Assert.Equal(3, obras.Count); Assert.All(obras, o => { Assert.True(o.Simulado); Assert.Contains("simulado", o.Titulo); });
    }
    [Fact(DisplayName = "SM22 | Filme: Pesquisar termo vazio | Lista vazia sem inventar resultado")]
    public async Task SM22()
    {
        var servico = new CatalogoSimulado(TipoMidia.Filme);
        Assert.Empty(await servico.BuscarAsync("", TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM23 | Filme: Pesquisar termo nulo | Lista vazia sem exceção")]
    public async Task SM23()
    {
        var servico = new CatalogoSimulado(TipoMidia.Filme);
        Assert.Empty(await servico.BuscarAsync(null!, TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM24 | Filme: Pesquisar com caixa diferente | Pesquisa ignora maiúsculas")]
    public async Task SM24()
    {
        var servico = new CatalogoSimulado(TipoMidia.Filme);
        Assert.Equal(3, (await servico.BuscarAsync("SIMULADO", TestContext.Current.CancellationToken)).Count);
    }
    [Fact(DisplayName = "SM25 | Filme: Pesquisar título parcial | Somente a obra correspondente")]
    public async Task SM25()
    {
        var servico = new CatalogoSimulado(TipoMidia.Filme);
        var todas = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); var termo = todas[0].Titulo.Split(' ').First(palavra => palavra.Length > 3); Assert.Single(await servico.BuscarAsync(termo, TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM26 | Filme: Pesquisar termo ausente | Lista vazia válida")]
    public async Task SM26()
    {
        var servico = new CatalogoSimulado(TipoMidia.Filme);
        Assert.Empty(await servico.BuscarAsync("zzzz-inexistente", TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM27 | Filme: Cancelar antes da pesquisa | Cancelamento respeitado")]
    public async Task SM27()
    {
        var servico = new CatalogoSimulado(TipoMidia.Filme);
        using var cts = new CancellationTokenSource(); cts.Cancel(); await Assert.ThrowsAnyAsync<OperationCanceledException>(() => servico.BuscarAsync("simulado", cts.Token));
    }
    [Fact(DisplayName = "SM28 | Filme: Repetir pesquisa | IDs estáveis e distintos no catálogo")]
    public async Task SM28()
    {
        var servico = new CatalogoSimulado(TipoMidia.Filme);
        var um = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); var dois = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); Assert.Equal(um.Select(o => o.IdentificadorExterno), dois.Select(o => o.IdentificadorExterno)); Assert.Equal(3, um.Select(o => o.IdentificadorExterno).Distinct().Count());
    }
    [Fact(DisplayName = "SM29 | Filme: Consultar metadados | Tipo, gêneros, capa local e origem simulada disponíveis")]
    public async Task SM29()
    {
        var servico = new CatalogoSimulado(TipoMidia.Filme);
        var o = (await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken))[0]; Assert.Equal(TipoMidia.Filme, o.Tipo); Assert.Equal("Simulado", o.Origem); Assert.NotEmpty(o.Generos); Assert.StartsWith("avares://BacklogManager/Assets/", o.CapaUrl!);
    }
    [Fact(DisplayName = "SM30 | Filme: Salvar resultado do catálogo | Origem simulada e referências preservadas após recarga")]
    public async Task SM30()
    {
        var servico = new CatalogoSimulado(TipoMidia.Filme);
        var o = (await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken))[0]; var lido = Roundtrip(new ItemBiblioteca(o, Item(TipoMidia.Filme).Pessoal)); Assert.True(lido.Catalogo.Simulado); Assert.Equal(o.IdentificadorExterno, lido.Catalogo.IdentificadorExterno); Assert.Equal(o.CapaUrl, lido.Catalogo.CapaUrl);
    }
    [Fact(DisplayName = "SM31 | Manga: Pesquisar exemplos | Três resultados fictícios identificados")]
    public async Task SM31()
    {
        var servico = new CatalogoSimulado(TipoMidia.Manga);
        var obras = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); Assert.Equal(3, obras.Count); Assert.All(obras, o => { Assert.True(o.Simulado); Assert.Contains("simulado", o.Titulo); });
    }
    [Fact(DisplayName = "SM32 | Manga: Pesquisar termo vazio | Lista vazia sem inventar resultado")]
    public async Task SM32()
    {
        var servico = new CatalogoSimulado(TipoMidia.Manga);
        Assert.Empty(await servico.BuscarAsync("", TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM33 | Manga: Pesquisar termo nulo | Lista vazia sem exceção")]
    public async Task SM33()
    {
        var servico = new CatalogoSimulado(TipoMidia.Manga);
        Assert.Empty(await servico.BuscarAsync(null!, TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM34 | Manga: Pesquisar com caixa diferente | Pesquisa ignora maiúsculas")]
    public async Task SM34()
    {
        var servico = new CatalogoSimulado(TipoMidia.Manga);
        Assert.Equal(3, (await servico.BuscarAsync("SIMULADO", TestContext.Current.CancellationToken)).Count);
    }
    [Fact(DisplayName = "SM35 | Manga: Pesquisar título parcial | Somente a obra correspondente")]
    public async Task SM35()
    {
        var servico = new CatalogoSimulado(TipoMidia.Manga);
        var todas = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); var termo = todas[0].Titulo.Split(' ').First(palavra => palavra.Length > 3); Assert.Single(await servico.BuscarAsync(termo, TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM36 | Manga: Pesquisar termo ausente | Lista vazia válida")]
    public async Task SM36()
    {
        var servico = new CatalogoSimulado(TipoMidia.Manga);
        Assert.Empty(await servico.BuscarAsync("zzzz-inexistente", TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM37 | Manga: Cancelar antes da pesquisa | Cancelamento respeitado")]
    public async Task SM37()
    {
        var servico = new CatalogoSimulado(TipoMidia.Manga);
        using var cts = new CancellationTokenSource(); cts.Cancel(); await Assert.ThrowsAnyAsync<OperationCanceledException>(() => servico.BuscarAsync("simulado", cts.Token));
    }
    [Fact(DisplayName = "SM38 | Manga: Repetir pesquisa | IDs estáveis e distintos no catálogo")]
    public async Task SM38()
    {
        var servico = new CatalogoSimulado(TipoMidia.Manga);
        var um = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); var dois = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); Assert.Equal(um.Select(o => o.IdentificadorExterno), dois.Select(o => o.IdentificadorExterno)); Assert.Equal(3, um.Select(o => o.IdentificadorExterno).Distinct().Count());
    }
    [Fact(DisplayName = "SM39 | Manga: Consultar metadados | Tipo, gêneros, capa local e origem simulada disponíveis")]
    public async Task SM39()
    {
        var servico = new CatalogoSimulado(TipoMidia.Manga);
        var o = (await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken))[0]; Assert.Equal(TipoMidia.Manga, o.Tipo); Assert.Equal("Simulado", o.Origem); Assert.NotEmpty(o.Generos); Assert.StartsWith("avares://BacklogManager/Assets/", o.CapaUrl!);
    }
    [Fact(DisplayName = "SM40 | Manga: Salvar resultado do catálogo | Origem simulada e referências preservadas após recarga")]
    public async Task SM40()
    {
        var servico = new CatalogoSimulado(TipoMidia.Manga);
        var o = (await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken))[0]; var lido = Roundtrip(new ItemBiblioteca(o, Item(TipoMidia.Manga).Pessoal)); Assert.True(lido.Catalogo.Simulado); Assert.Equal(o.IdentificadorExterno, lido.Catalogo.IdentificadorExterno); Assert.Equal(o.CapaUrl, lido.Catalogo.CapaUrl);
    }
    [Fact(DisplayName = "SM41 | Livro: Pesquisar exemplos | Três resultados fictícios identificados")]
    public async Task SM41()
    {
        var servico = new CatalogoSimulado(TipoMidia.Livro);
        var obras = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); Assert.Equal(3, obras.Count); Assert.All(obras, o => { Assert.True(o.Simulado); Assert.Contains("simulado", o.Titulo); });
    }
    [Fact(DisplayName = "SM42 | Livro: Pesquisar termo vazio | Lista vazia sem inventar resultado")]
    public async Task SM42()
    {
        var servico = new CatalogoSimulado(TipoMidia.Livro);
        Assert.Empty(await servico.BuscarAsync("", TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM43 | Livro: Pesquisar termo nulo | Lista vazia sem exceção")]
    public async Task SM43()
    {
        var servico = new CatalogoSimulado(TipoMidia.Livro);
        Assert.Empty(await servico.BuscarAsync(null!, TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM44 | Livro: Pesquisar com caixa diferente | Pesquisa ignora maiúsculas")]
    public async Task SM44()
    {
        var servico = new CatalogoSimulado(TipoMidia.Livro);
        Assert.Equal(3, (await servico.BuscarAsync("SIMULADO", TestContext.Current.CancellationToken)).Count);
    }
    [Fact(DisplayName = "SM45 | Livro: Pesquisar título parcial | Somente a obra correspondente")]
    public async Task SM45()
    {
        var servico = new CatalogoSimulado(TipoMidia.Livro);
        var todas = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); var termo = todas[0].Titulo.Split(' ').First(palavra => palavra.Length > 3); Assert.Single(await servico.BuscarAsync(termo, TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM46 | Livro: Pesquisar termo ausente | Lista vazia válida")]
    public async Task SM46()
    {
        var servico = new CatalogoSimulado(TipoMidia.Livro);
        Assert.Empty(await servico.BuscarAsync("zzzz-inexistente", TestContext.Current.CancellationToken));
    }
    [Fact(DisplayName = "SM47 | Livro: Cancelar antes da pesquisa | Cancelamento respeitado")]
    public async Task SM47()
    {
        var servico = new CatalogoSimulado(TipoMidia.Livro);
        using var cts = new CancellationTokenSource(); cts.Cancel(); await Assert.ThrowsAnyAsync<OperationCanceledException>(() => servico.BuscarAsync("simulado", cts.Token));
    }
    [Fact(DisplayName = "SM48 | Livro: Repetir pesquisa | IDs estáveis e distintos no catálogo")]
    public async Task SM48()
    {
        var servico = new CatalogoSimulado(TipoMidia.Livro);
        var um = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); var dois = await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken); Assert.Equal(um.Select(o => o.IdentificadorExterno), dois.Select(o => o.IdentificadorExterno)); Assert.Equal(3, um.Select(o => o.IdentificadorExterno).Distinct().Count());
    }
    [Fact(DisplayName = "SM49 | Livro: Consultar metadados | Tipo, gêneros, capa local e origem simulada disponíveis")]
    public async Task SM49()
    {
        var servico = new CatalogoSimulado(TipoMidia.Livro);
        var o = (await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken))[0]; Assert.Equal(TipoMidia.Livro, o.Tipo); Assert.Equal("Simulado", o.Origem); Assert.NotEmpty(o.Generos); Assert.StartsWith("avares://BacklogManager/Assets/", o.CapaUrl!);
    }
    [Fact(DisplayName = "SM50 | Livro: Salvar resultado do catálogo | Origem simulada e referências preservadas após recarga")]
    public async Task SM50()
    {
        var servico = new CatalogoSimulado(TipoMidia.Livro);
        var o = (await servico.BuscarAsync("simulado", TestContext.Current.CancellationToken))[0]; var lido = Roundtrip(new ItemBiblioteca(o, Item(TipoMidia.Livro).Pessoal)); Assert.True(lido.Catalogo.Simulado); Assert.Equal(o.IdentificadorExterno, lido.Catalogo.IdentificadorExterno); Assert.Equal(o.CapaUrl, lido.Catalogo.CapaUrl);
    }
}
