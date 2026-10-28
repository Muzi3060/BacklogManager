using System.Text.Json;
using BacklogManager.Models;
using BacklogManager.Services;
using Xunit;
using static BacklogManager.Tests.Fase2.ApoioMultimidia;

namespace BacklogManager.Tests.Fase2;

public sealed class ProgressoTests
{
    [Fact(DisplayName = "PG01 | Jogo: Progresso ausente | Campo opcional aceito")]
    public void PG01()
    {
        RegrasProgresso.Validar(TipoMidia.Jogo, null);
    }
    [Fact(DisplayName = "PG02 | Jogo: Progresso vazio | Nenhuma unidade inventada")]
    public void PG02()
    {
        var p = new ProgressoPessoal(); RegrasProgresso.Validar(TipoMidia.Jogo, p); Assert.True(p.Vazio);
    }
    [Fact(DisplayName = "PG03 | Jogo: Valor zero | Zero distinguido de ausência")]
    public void PG03()
    {
        var p = new ProgressoPessoal { HorasJogadas = 0 }; RegrasProgresso.Validar(TipoMidia.Jogo, p); Assert.False(p.Vazio);
    }
    [Fact(DisplayName = "PG04 | Jogo: Valor positivo | Progresso pessoal aceito")]
    public void PG04()
    {
        var p = new ProgressoPessoal { HorasJogadas = 12.5m }; RegrasProgresso.Validar(TipoMidia.Jogo, p); Assert.Equal(12.5m, p.HorasJogadas);
    }
    [Fact(DisplayName = "PG05 | Jogo: Valor negativo | Entrada recusada")]
    public void PG05()
    {
        Assert.Throws<InvalidDataException>(() => RegrasProgresso.Validar(TipoMidia.Jogo, new ProgressoPessoal { HorasJogadas = -1 }));
    }
    [Fact(DisplayName = "PG06 | Jogo: Campo de outra mídia | Unidade incompatível recusada")]
    public void PG06()
    {
        Assert.Throws<InvalidDataException>(() => RegrasProgresso.Validar(TipoMidia.Jogo, new ProgressoPessoal { Pagina = 1 }));
    }
    [Fact(DisplayName = "PG07 | Jogo: Valor máximo representável | Limite do tipo aceito sem converter unidade")]
    public void PG07()
    {
        var p = new ProgressoPessoal { HorasJogadas = decimal.MaxValue }; RegrasProgresso.Validar(TipoMidia.Jogo, p); Assert.Equal(decimal.MaxValue, p.HorasJogadas);
    }
    [Fact(DisplayName = "PG08 | Jogo: Formulário sem progresso | Seleção de catálogo não gera duração pessoal")]
    public void PG08()
    {
        var o = Catalogo(TipoMidia.Jogo); var d = new DadosFormularioJogo("busca", "Fantasia", "Na Fila", 4, ""); var item = FormularioObra.Preparar(d, o, "busca", null, TipoMidia.Jogo); Assert.Null(item.Pessoal.Progresso);
    }
    [Fact(DisplayName = "PG09 | Jogo: Exibir unidade correta | Texto corresponde ao tipo de mídia")]
    public void PG09()
    {
        Assert.Contains("h jogadas", TextosMidia.Progresso(TipoMidia.Jogo, new ProgressoPessoal { HorasJogadas = 12.5m }));
    }
    [Fact(DisplayName = "PG10 | Jogo: Persistir e recarregar progresso | Valor pessoal preservado no JSON")]
    public void PG10()
    {
        var p = new ProgressoPessoal { HorasJogadas = 12.5m }; var lido = Roundtrip(Item(TipoMidia.Jogo, p)); Assert.Equal(p, lido.Pessoal.Progresso);
    }
    [Fact(DisplayName = "PG11 | Anime: Progresso ausente | Campo opcional aceito")]
    public void PG11()
    {
        RegrasProgresso.Validar(TipoMidia.Anime, null);
    }
    [Fact(DisplayName = "PG12 | Anime: Progresso vazio | Nenhuma unidade inventada")]
    public void PG12()
    {
        var p = new ProgressoPessoal(); RegrasProgresso.Validar(TipoMidia.Anime, p); Assert.True(p.Vazio);
    }
    [Fact(DisplayName = "PG13 | Anime: Valor zero | Zero distinguido de ausência")]
    public void PG13()
    {
        var p = new ProgressoPessoal { EpisodiosAssistidos = 0 }; RegrasProgresso.Validar(TipoMidia.Anime, p); Assert.False(p.Vazio);
    }
    [Fact(DisplayName = "PG14 | Anime: Valor positivo | Progresso pessoal aceito")]
    public void PG14()
    {
        var p = new ProgressoPessoal { EpisodiosAssistidos = 12 }; RegrasProgresso.Validar(TipoMidia.Anime, p); Assert.Equal(12, p.EpisodiosAssistidos);
    }
    [Fact(DisplayName = "PG15 | Anime: Valor negativo | Entrada recusada")]
    public void PG15()
    {
        Assert.Throws<InvalidDataException>(() => RegrasProgresso.Validar(TipoMidia.Anime, new ProgressoPessoal { EpisodiosAssistidos = -1 }));
    }
    [Fact(DisplayName = "PG16 | Anime: Campo de outra mídia | Unidade incompatível recusada")]
    public void PG16()
    {
        Assert.Throws<InvalidDataException>(() => RegrasProgresso.Validar(TipoMidia.Anime, new ProgressoPessoal { HorasJogadas = 1 }));
    }
    [Fact(DisplayName = "PG17 | Anime: Valor máximo representável | Limite do tipo aceito sem converter unidade")]
    public void PG17()
    {
        var p = new ProgressoPessoal { EpisodiosAssistidos = int.MaxValue }; RegrasProgresso.Validar(TipoMidia.Anime, p); Assert.Equal(int.MaxValue, p.EpisodiosAssistidos);
    }
    [Fact(DisplayName = "PG18 | Anime: Formulário sem progresso | Seleção de catálogo não gera duração pessoal")]
    public void PG18()
    {
        var o = Catalogo(TipoMidia.Anime); var d = new DadosFormularioJogo("busca", "Fantasia", "Na Fila", 4, ""); var item = FormularioObra.Preparar(d, o, "busca", null, TipoMidia.Anime); Assert.Null(item.Pessoal.Progresso);
    }
    [Fact(DisplayName = "PG19 | Anime: Exibir unidade correta | Texto corresponde ao tipo de mídia")]
    public void PG19()
    {
        Assert.Contains("episódios", TextosMidia.Progresso(TipoMidia.Anime, new ProgressoPessoal { EpisodiosAssistidos = 12 }));
    }
    [Fact(DisplayName = "PG20 | Anime: Persistir e recarregar progresso | Valor pessoal preservado no JSON")]
    public void PG20()
    {
        var p = new ProgressoPessoal { EpisodiosAssistidos = 12 }; var lido = Roundtrip(Item(TipoMidia.Anime, p)); Assert.Equal(p, lido.Pessoal.Progresso);
    }
    [Fact(DisplayName = "PG21 | Serie: Progresso ausente | Campo opcional aceito")]
    public void PG21()
    {
        RegrasProgresso.Validar(TipoMidia.Serie, null);
    }
    [Fact(DisplayName = "PG22 | Serie: Progresso vazio | Nenhuma unidade inventada")]
    public void PG22()
    {
        var p = new ProgressoPessoal(); RegrasProgresso.Validar(TipoMidia.Serie, p); Assert.True(p.Vazio);
    }
    [Fact(DisplayName = "PG23 | Serie: Valor zero | Zero distinguido de ausência")]
    public void PG23()
    {
        var p = new ProgressoPessoal { Temporada = 0 }; RegrasProgresso.Validar(TipoMidia.Serie, p); Assert.False(p.Vazio);
    }
    [Fact(DisplayName = "PG24 | Serie: Valor positivo | Progresso pessoal aceito")]
    public void PG24()
    {
        var p = new ProgressoPessoal { Temporada = 2 }; RegrasProgresso.Validar(TipoMidia.Serie, p); Assert.Equal(2, p.Temporada);
    }
    [Fact(DisplayName = "PG25 | Serie: Valor negativo | Entrada recusada")]
    public void PG25()
    {
        Assert.Throws<InvalidDataException>(() => RegrasProgresso.Validar(TipoMidia.Serie, new ProgressoPessoal { Temporada = -1 }));
    }
    [Fact(DisplayName = "PG26 | Serie: Campo de outra mídia | Unidade incompatível recusada")]
    public void PG26()
    {
        Assert.Throws<InvalidDataException>(() => RegrasProgresso.Validar(TipoMidia.Serie, new ProgressoPessoal { HorasJogadas = 1 }));
    }
    [Fact(DisplayName = "PG27 | Serie: Segundo campo opcional | Campo complementar aceito")]
    public void PG27()
    {
        var p = new ProgressoPessoal { Episodio = 3 }; RegrasProgresso.Validar(TipoMidia.Serie, p); Assert.Equal(3, p.Episodio);
    }
    [Fact(DisplayName = "PG28 | Serie: Segundo campo negativo | Complemento negativo recusado")]
    public void PG28()
    {
        Assert.Throws<InvalidDataException>(() => RegrasProgresso.Validar(TipoMidia.Serie, new ProgressoPessoal { Episodio = -1 }));
    }
    [Fact(DisplayName = "PG29 | Serie: Exibir unidade correta | Texto corresponde ao tipo de mídia")]
    public void PG29()
    {
        Assert.Contains("Temporada", TextosMidia.Progresso(TipoMidia.Serie, new ProgressoPessoal { Temporada = 2 }));
    }
    [Fact(DisplayName = "PG30 | Serie: Persistir e recarregar progresso | Valor pessoal preservado no JSON")]
    public void PG30()
    {
        var p = new ProgressoPessoal { Temporada = 2 }; var lido = Roundtrip(Item(TipoMidia.Serie, p)); Assert.Equal(p, lido.Pessoal.Progresso);
    }
    [Fact(DisplayName = "PG31 | Filme: Progresso ausente | Campo opcional aceito")]
    public void PG31()
    {
        RegrasProgresso.Validar(TipoMidia.Filme, null);
    }
    [Fact(DisplayName = "PG32 | Filme: Progresso vazio | Nenhuma unidade inventada")]
    public void PG32()
    {
        var p = new ProgressoPessoal(); RegrasProgresso.Validar(TipoMidia.Filme, p); Assert.True(p.Vazio);
    }
    [Fact(DisplayName = "PG33 | Filme: Valor zero | Zero distinguido de ausência")]
    public void PG33()
    {
        var p = new ProgressoPessoal { MinutosAssistidos = 0 }; RegrasProgresso.Validar(TipoMidia.Filme, p); Assert.False(p.Vazio);
    }
    [Fact(DisplayName = "PG34 | Filme: Valor positivo | Progresso pessoal aceito")]
    public void PG34()
    {
        var p = new ProgressoPessoal { MinutosAssistidos = 45.5m }; RegrasProgresso.Validar(TipoMidia.Filme, p); Assert.Equal(45.5m, p.MinutosAssistidos);
    }
    [Fact(DisplayName = "PG35 | Filme: Valor negativo | Entrada recusada")]
    public void PG35()
    {
        Assert.Throws<InvalidDataException>(() => RegrasProgresso.Validar(TipoMidia.Filme, new ProgressoPessoal { MinutosAssistidos = -1 }));
    }
    [Fact(DisplayName = "PG36 | Filme: Campo de outra mídia | Unidade incompatível recusada")]
    public void PG36()
    {
        Assert.Throws<InvalidDataException>(() => RegrasProgresso.Validar(TipoMidia.Filme, new ProgressoPessoal { Pagina = 1 }));
    }
    [Fact(DisplayName = "PG37 | Filme: Valor máximo representável | Limite do tipo aceito sem converter unidade")]
    public void PG37()
    {
        var p = new ProgressoPessoal { MinutosAssistidos = decimal.MaxValue }; RegrasProgresso.Validar(TipoMidia.Filme, p); Assert.Equal(decimal.MaxValue, p.MinutosAssistidos);
    }
    [Fact(DisplayName = "PG38 | Filme: Formulário sem progresso | Seleção de catálogo não gera duração pessoal")]
    public void PG38()
    {
        var o = Catalogo(TipoMidia.Filme); var d = new DadosFormularioJogo("busca", "Fantasia", "Na Fila", 4, ""); var item = FormularioObra.Preparar(d, o, "busca", null, TipoMidia.Filme); Assert.Null(item.Pessoal.Progresso);
    }
    [Fact(DisplayName = "PG39 | Filme: Exibir unidade correta | Texto corresponde ao tipo de mídia")]
    public void PG39()
    {
        Assert.Contains("min assistidos", TextosMidia.Progresso(TipoMidia.Filme, new ProgressoPessoal { MinutosAssistidos = 45.5m }));
    }
    [Fact(DisplayName = "PG40 | Filme: Persistir e recarregar progresso | Valor pessoal preservado no JSON")]
    public void PG40()
    {
        var p = new ProgressoPessoal { MinutosAssistidos = 45.5m }; var lido = Roundtrip(Item(TipoMidia.Filme, p)); Assert.Equal(p, lido.Pessoal.Progresso);
    }
    [Fact(DisplayName = "PG41 | Manga: Progresso ausente | Campo opcional aceito")]
    public void PG41()
    {
        RegrasProgresso.Validar(TipoMidia.Manga, null);
    }
    [Fact(DisplayName = "PG42 | Manga: Progresso vazio | Nenhuma unidade inventada")]
    public void PG42()
    {
        var p = new ProgressoPessoal(); RegrasProgresso.Validar(TipoMidia.Manga, p); Assert.True(p.Vazio);
    }
    [Fact(DisplayName = "PG43 | Manga: Valor zero | Zero distinguido de ausência")]
    public void PG43()
    {
        var p = new ProgressoPessoal { Capitulo = 0 }; RegrasProgresso.Validar(TipoMidia.Manga, p); Assert.False(p.Vazio);
    }
    [Fact(DisplayName = "PG44 | Manga: Valor positivo | Progresso pessoal aceito")]
    public void PG44()
    {
        var p = new ProgressoPessoal { Capitulo = 12 }; RegrasProgresso.Validar(TipoMidia.Manga, p); Assert.Equal(12, p.Capitulo);
    }
    [Fact(DisplayName = "PG45 | Manga: Valor negativo | Entrada recusada")]
    public void PG45()
    {
        Assert.Throws<InvalidDataException>(() => RegrasProgresso.Validar(TipoMidia.Manga, new ProgressoPessoal { Capitulo = -1 }));
    }
    [Fact(DisplayName = "PG46 | Manga: Campo de outra mídia | Unidade incompatível recusada")]
    public void PG46()
    {
        Assert.Throws<InvalidDataException>(() => RegrasProgresso.Validar(TipoMidia.Manga, new ProgressoPessoal { Pagina = 1 }));
    }
    [Fact(DisplayName = "PG47 | Manga: Segundo campo opcional | Campo complementar aceito")]
    public void PG47()
    {
        var p = new ProgressoPessoal { Volume = 3 }; RegrasProgresso.Validar(TipoMidia.Manga, p); Assert.Equal(3, p.Volume);
    }
    [Fact(DisplayName = "PG48 | Manga: Segundo campo negativo | Complemento negativo recusado")]
    public void PG48()
    {
        Assert.Throws<InvalidDataException>(() => RegrasProgresso.Validar(TipoMidia.Manga, new ProgressoPessoal { Volume = -1 }));
    }
    [Fact(DisplayName = "PG49 | Manga: Exibir unidade correta | Texto corresponde ao tipo de mídia")]
    public void PG49()
    {
        Assert.Contains("Capítulo", TextosMidia.Progresso(TipoMidia.Manga, new ProgressoPessoal { Capitulo = 12 }));
    }
    [Fact(DisplayName = "PG50 | Manga: Persistir e recarregar progresso | Valor pessoal preservado no JSON")]
    public void PG50()
    {
        var p = new ProgressoPessoal { Capitulo = 12 }; var lido = Roundtrip(Item(TipoMidia.Manga, p)); Assert.Equal(p, lido.Pessoal.Progresso);
    }
    [Fact(DisplayName = "PG51 | Livro: Progresso ausente | Campo opcional aceito")]
    public void PG51()
    {
        RegrasProgresso.Validar(TipoMidia.Livro, null);
    }
    [Fact(DisplayName = "PG52 | Livro: Progresso vazio | Nenhuma unidade inventada")]
    public void PG52()
    {
        var p = new ProgressoPessoal(); RegrasProgresso.Validar(TipoMidia.Livro, p); Assert.True(p.Vazio);
    }
    [Fact(DisplayName = "PG53 | Livro: Valor zero | Zero distinguido de ausência")]
    public void PG53()
    {
        var p = new ProgressoPessoal { Pagina = 0 }; RegrasProgresso.Validar(TipoMidia.Livro, p); Assert.False(p.Vazio);
    }
    [Fact(DisplayName = "PG54 | Livro: Valor positivo | Progresso pessoal aceito")]
    public void PG54()
    {
        var p = new ProgressoPessoal { Pagina = 120 }; RegrasProgresso.Validar(TipoMidia.Livro, p); Assert.Equal(120, p.Pagina);
    }
    [Fact(DisplayName = "PG55 | Livro: Valor negativo | Entrada recusada")]
    public void PG55()
    {
        Assert.Throws<InvalidDataException>(() => RegrasProgresso.Validar(TipoMidia.Livro, new ProgressoPessoal { Pagina = -1 }));
    }
    [Fact(DisplayName = "PG56 | Livro: Campo de outra mídia | Unidade incompatível recusada")]
    public void PG56()
    {
        Assert.Throws<InvalidDataException>(() => RegrasProgresso.Validar(TipoMidia.Livro, new ProgressoPessoal { HorasJogadas = 1 }));
    }
    [Fact(DisplayName = "PG57 | Livro: Segundo campo opcional | Campo complementar aceito")]
    public void PG57()
    {
        var p = new ProgressoPessoal { Capitulo = 3 }; RegrasProgresso.Validar(TipoMidia.Livro, p); Assert.Equal(3, p.Capitulo);
    }
    [Fact(DisplayName = "PG58 | Livro: Segundo campo negativo | Complemento negativo recusado")]
    public void PG58()
    {
        Assert.Throws<InvalidDataException>(() => RegrasProgresso.Validar(TipoMidia.Livro, new ProgressoPessoal { Capitulo = -1 }));
    }
    [Fact(DisplayName = "PG59 | Livro: Exibir unidade correta | Texto corresponde ao tipo de mídia")]
    public void PG59()
    {
        Assert.Contains("Página", TextosMidia.Progresso(TipoMidia.Livro, new ProgressoPessoal { Pagina = 120 }));
    }
    [Fact(DisplayName = "PG60 | Livro: Persistir e recarregar progresso | Valor pessoal preservado no JSON")]
    public void PG60()
    {
        var p = new ProgressoPessoal { Pagina = 120 }; var lido = Roundtrip(Item(TipoMidia.Livro, p)); Assert.Equal(p, lido.Pessoal.Progresso);
    }
}
