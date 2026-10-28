using BacklogManager.Models;
using BacklogManager.Services;
using Xunit;
using static BacklogManager.Tests.Fase2.ApoioMultimidia;

namespace BacklogManager.Tests.Fase2;

public sealed class FormularioMultimidiaTests
{
    private static DadosFormularioJogo Dados(ProgressoPessoal? p = null) => new("consulta", "Fantasia", "Na Fila", 4, "Review", p);
    private static ItemBiblioteca Preparar(TipoMidia tipo, ProgressoPessoal? p = null) => FormularioObra.Preparar(Dados(p), Catalogo(tipo), "consulta", null, tipo);
    [Fact(DisplayName = "FM01 | Cadastro de anime | Catálogo fictício explicitamente identificado")]
    public void Anime() => Assert.True(Preparar(TipoMidia.Anime).Catalogo.Simulado);
    [Fact(DisplayName = "FM02 | Cadastro de série | Tipo da seleção mantido")]
    public void Serie() => Assert.Equal(TipoMidia.Serie, Preparar(TipoMidia.Serie).Catalogo.Tipo);
    [Fact(DisplayName = "FM03 | Cadastro de filme | Referência simulada não vira ID de jogo")]
    public void Filme() => Assert.Null(Preparar(TipoMidia.Filme).SteamAppIdLegado);
    [Fact(DisplayName = "FM04 | Cadastro de mangá | Gêneros mantidos no catálogo")]
    public void Manga() => Assert.NotEmpty(Preparar(TipoMidia.Manga).Catalogo.Generos);
    [Fact(DisplayName = "FM05 | Cadastro de livro | Progresso de página opcional aceito")]
    public void Livro() => Assert.Equal(12, Preparar(TipoMidia.Livro, new ProgressoPessoal { Pagina = 12 }).Pessoal.Progresso!.Pagina);
    [Fact(DisplayName = "FM06 | Seleção de outra categoria | Cadastro recusado sem trocar tipo implicitamente")]
    public void OutraCategoria() => Assert.Throws<ValidacaoFormularioException>(() => FormularioObra.Preparar(Dados(), Catalogo(TipoMidia.Livro), "consulta", null, TipoMidia.Anime));
    [Fact(DisplayName = "FM07 | Novo tipo com origem simulada sem flag | Dados fictícios não podem ser apresentados como reais")]
    public void SemFlag() => Assert.False(FormularioObra.SelecaoValida(Catalogo(TipoMidia.Livro) with { Simulado = false }, TipoMidia.Livro));
    [Fact(DisplayName = "FM08 | Simulado com origem incoerente | Seleção recusada")]
    public void OutraOrigem() => Assert.False(FormularioObra.SelecaoValida(Catalogo(TipoMidia.Anime) with { Origem = "IGDB" }, TipoMidia.Anime));
    [Fact(DisplayName = "FM09 | Tipo desconhecido no formulário | Validação explícita antes de salvar")]
    public void TipoInvalido() => Assert.Throws<ValidacaoFormularioException>(() => FormularioObra.Preparar(Dados(), Catalogo(TipoMidia.Livro), "consulta", null, (TipoMidia)999));
    [Fact(DisplayName = "FM10 | Texto alterado após escolher simulado | Seleção antiga recusada")]
    public void TextoAlterado() => Assert.Throws<ValidacaoFormularioException>(() => FormularioObra.Preparar(Dados() with { TextoTitulo = "outro" }, Catalogo(TipoMidia.Livro), "consulta", null, TipoMidia.Livro));
    [Fact(DisplayName = "FM11 | Editar livro sem nova pesquisa | Catálogo e progresso escolhido preservados")]
    public void Editar()
    {
        var item = Item(TipoMidia.Livro); var resultado = FormularioObra.Preparar(Dados(new ProgressoPessoal { Pagina = 25 }), null, null, item, TipoMidia.Livro); Assert.Same(item.Catalogo, resultado.Catalogo); Assert.Equal(25, resultado.Pessoal.Progresso!.Pagina);
    }
    [Fact(DisplayName = "FM12 | Mudar tipo de item em edição | Operação recusada sem reclassificar registro")]
    public void EditarTipo() => Assert.Throws<ValidacaoFormularioException>(() => FormularioObra.Preparar(Dados(), null, null, Item(TipoMidia.Livro), TipoMidia.Filme));
    [Fact(DisplayName = "FM13 | Progresso negativo no cadastro | Aviso específico de progresso inválido")]
    public void Negativo() => Assert.Equal("Progresso inválido", Assert.Throws<ValidacaoFormularioException>(() => Preparar(TipoMidia.Livro, new ProgressoPessoal { Pagina = -1 })).Titulo);
    [Fact(DisplayName = "FM14 | Horas em filme | Unidade incompatível recusada")]
    public void UnidadeErrada() => Assert.Throws<ValidacaoFormularioException>(() => Preparar(TipoMidia.Filme, new ProgressoPessoal { HorasJogadas = 1 }));
    [Fact(DisplayName = "FM15 | Campo vazio de progresso | Null mantido sem preencher duração estimada")]
    public void Vazio() => Assert.Null(Preparar(TipoMidia.Anime, new ProgressoPessoal()).Pessoal.Progresso);
    [Fact(DisplayName = "FM16 | Progresso zero informado | Zero mantido como informação pessoal")]
    public void Zero() => Assert.Equal(0, Preparar(TipoMidia.Anime, new ProgressoPessoal { EpisodiosAssistidos = 0 }).Pessoal.Progresso!.EpisodiosAssistidos);
    [Fact(DisplayName = "FM17 | Episódio sem temporada | Campo complementar continua opcional")]
    public void SerieParcial() => Assert.Equal(3, Preparar(TipoMidia.Serie, new ProgressoPessoal { Episodio = 3 }).Pessoal.Progresso!.Episodio);
    [Fact(DisplayName = "FM18 | Volume sem capítulo | Campo complementar continua opcional")]
    public void MangaParcial() => Assert.Equal(2, Preparar(TipoMidia.Manga, new ProgressoPessoal { Volume = 2 }).Pessoal.Progresso!.Volume);
    [Fact(DisplayName = "FM19 | Filme com minutos fracionários | Precisão preservada")]
    public void Minutos() => Assert.Equal(12.5m, Preparar(TipoMidia.Filme, new ProgressoPessoal { MinutosAssistidos = 12.5m }).Pessoal.Progresso!.MinutosAssistidos);
    [Fact(DisplayName = "FM20 | Jogo IGDB e horas pessoais | Referência IGDB permanece independente do progresso")]
    public void Jogo() { var item = Preparar(TipoMidia.Jogo, new ProgressoPessoal { HorasJogadas = 5 }); Assert.Equal("42", item.Catalogo.IdentificadorExterno); Assert.Equal(5m, item.Pessoal.Progresso!.HorasJogadas); }
}
