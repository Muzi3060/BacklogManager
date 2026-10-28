using BacklogManager.Models;
using BacklogManager.Services;
using Xunit;
using static BacklogManager.Tests.Fase1.ApoioArquitetura;

namespace BacklogManager.Tests.Fase1;

public sealed class FormularioJogoTests
{
    private static readonly DadosFormularioJogo Dados = new("Pesq", "RPG", "Na Fila", 4.5m, "Review");
    private static ItemBiblioteca Novo(DadosFormularioJogo? dados = null, ObraCatalogo? obra = null) =>
        FormularioJogo.Preparar(dados ?? Dados, obra ?? Jogo().Catalogo, "Pesq", null);

    [Fact(DisplayName = "FO01 | Preparar cadastro válido | Catálogo selecionado e dados pessoais separados")]
    public void NovoValido()
    {
        var obra = Jogo().Catalogo; var item = Novo(obra: obra);
        Assert.Same(obra, item.Catalogo); Assert.Equal(4.5, item.Pessoal.Nota); Assert.Null(item.SteamAppIdLegado);
    }
    [Fact(DisplayName = "FO02 | Cadastro sem seleção | Mensagem específica de seleção")]
    public void SemSelecao()
    {
        var erro = Assert.Throws<ValidacaoFormularioException>(() => FormularioJogo.Preparar(Dados, null, "Pesq", null));
        Assert.Equal("Selecione um jogo", erro.Titulo);
    }
    [Fact(DisplayName = "FO03 | Texto alterado após seleção | Cadastro bloqueado antes de gravar")]
    public void TextoAlterado() => Assert.Throws<ValidacaoFormularioException>(() => Novo(Dados with { TextoTitulo = "Outro" }));
    [Fact(DisplayName = "FO04 | Nota vazia | Mensagem de nota inválida")]
    public void NotaVazia() => Assert.Equal("Nota inválida", Assert.Throws<ValidacaoFormularioException>(() => Novo(Dados with { Nota = null })).Titulo);
    [Fact(DisplayName = "FO05 | Nota abaixo de zero | Entrada recusada")]
    public void NotaNegativa() => Assert.Throws<ValidacaoFormularioException>(() => Novo(Dados with { Nota = -0.5m }));
    [Fact(DisplayName = "FO06 | Nota acima de cinco | Entrada recusada")]
    public void NotaAlta() => Assert.Throws<ValidacaoFormularioException>(() => Novo(Dados with { Nota = 5.5m }));
    [Fact(DisplayName = "FO07 | Nota zero | Limite inferior aceito")]
    public void NotaZero() => Assert.Equal(0, Novo(Dados with { Nota = 0 }).Pessoal.Nota);
    [Fact(DisplayName = "FO08 | Nota cinco | Limite superior aceito")]
    public void NotaCinco() => Assert.Equal(5, Novo(Dados with { Nota = 5 }).Pessoal.Nota);
    [Fact(DisplayName = "FO09 | Categoria em branco | Mensagem de dados incompletos")]
    public void CategoriaVazia() => Assert.Equal("Dados incompletos", Assert.Throws<ValidacaoFormularioException>(() => Novo(Dados with { Categoria = " " })).Titulo);
    [Fact(DisplayName = "FO10 | Status ausente | Entrada recusada sem valor padrão inventado")]
    public void StatusVazio() => Assert.Throws<ValidacaoFormularioException>(() => Novo(Dados with { Status = null }));
    [Fact(DisplayName = "FO11 | Nova review vazia | Texto padrão preserva comportamento anterior")]
    public void ReviewVazia() => Assert.Equal("Nenhuma Review", Novo(Dados with { Review = " " }).Pessoal.Review);
    [Fact(DisplayName = "FO12 | Nova review com espaços nas bordas | Texto aparado sem remover quebras internas")]
    public void ReviewTrim() => Assert.Equal("Uma\nreview", Novo(Dados with { Review = "  Uma\nreview  " }).Pessoal.Review);
    [Fact(DisplayName = "FO13 | Nova review com mil caracteres | Limite aceito integralmente")]
    public void ReviewLimite() => Assert.Equal(1000, Novo(Dados with { Review = new string('x', 1000) }).Pessoal.Review.Length);
    [Fact(DisplayName = "FO14 | Nova review acima do limite | Entrada recusada sem truncar")]
    public void ReviewAcima() => Assert.Throws<ValidacaoFormularioException>(() => Novo(Dados with { Review = new string('x', 1001) }));
    [Fact(DisplayName = "FO15 | Editar review antiga longa sem alterá-la | Texto antigo mantido integralmente")]
    public void LegadoLongo()
    {
        var item = Jogo(); item = item with { Pessoal = item.Pessoal with { Review = new string('x', 1200) } };
        var atualizado = FormularioJogo.Preparar(Dados with { Review = item.Pessoal.Review }, null, null, item);
        Assert.Equal(item.Pessoal.Review, atualizado.Pessoal.Review);
    }
    [Fact(DisplayName = "FO16 | Editar dados pessoais de legado | Catálogo, ID legado e review vazia preservados")]
    public void EditarLegado()
    {
        var item = Jogo() with { SteamAppIdLegado = 361855 }; item = item with { Pessoal = item.Pessoal with { Review = "" } };
        var atualizado = FormularioJogo.Preparar(Dados with { Review = "", Nota = 2 }, null, null, item);
        Assert.Same(item.Catalogo, atualizado.Catalogo); Assert.Equal(361855, atualizado.SteamAppIdLegado); Assert.Equal("", atualizado.Pessoal.Review);
    }
    [Fact(DisplayName = "FO17 | Seleção de outra mídia | Formulário atual de jogos recusa o cadastro")]
    public void OutraMidia() => Assert.Throws<ValidacaoFormularioException>(() => Novo(obra: Jogo().Catalogo with { Tipo = TipoMidia.Livro }));
    [Fact(DisplayName = "FO18 | Seleção de outro provedor | Cadastro recusado sem fingir origem IGDB")]
    public void OutroCatalogo() => Assert.Throws<ValidacaoFormularioException>(() => Novo(obra: Jogo().Catalogo with { Origem = "Outro" }));
    [Fact(DisplayName = "FO19 | Seleção com ID inválido | Cadastro recusado")]
    public void IdInvalido() => Assert.Throws<ValidacaoFormularioException>(() => Novo(obra: Jogo().Catalogo with { IdentificadorExterno = "0" }));
    [Fact(DisplayName = "FO20 | Editar sem selecionar novo catálogo | Título e identificador existentes mantidos")]
    public void EditarNaoTrocaCatalogo()
    {
        var item = Jogo(); var outro = Jogo("Outro").Catalogo with { IdentificadorExterno = "99" };
        var atualizado = FormularioJogo.Preparar(Dados, outro, "Pesq", item);
        Assert.Same(item.Catalogo, atualizado.Catalogo); Assert.Equal("42", atualizado.Catalogo.IdentificadorExterno);
    }
    [Fact(DisplayName = "FO21 | ID externo com zeros à esquerda | Seleção recusada antes de chegar ao armazenamento legado")]
    public void IdNaoCanonico() => Assert.Throws<ValidacaoFormularioException>(() => Novo(obra: Jogo().Catalogo with { IdentificadorExterno = "042" }));

}
