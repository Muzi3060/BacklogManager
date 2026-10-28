using System.Text.Json;
using BacklogManager.Data;
using BacklogManager.Models;
using Xunit;
using static BacklogManager.Tests.Fase1.ApoioArquitetura;

namespace BacklogManager.Tests.Fase1;

public sealed class ConversorBibliotecaTests
{
    private static BacklogItem Voltar(BacklogItem r) => ConversorBibliotecaJson.ParaRegistro(ConversorBibliotecaJson.ParaModelo(r));

    [Fact(DisplayName = "MP01 | Separar registro legado | Título no catálogo e informações pessoais no bloco pessoal")]
    public void Separar()
    {
        var r = PastaTeste.Jogo(); var m = ConversorBibliotecaJson.ParaModelo(r);
        Assert.Equal(r.Titulo, m.Catalogo.Titulo); Assert.Equal(r.Categoria, m.Pessoal.Categoria);
        Assert.Equal(r.Status, m.Pessoal.Status); Assert.Equal(r.Nota, m.Pessoal.Nota); Assert.Equal(r.Review, m.Pessoal.Review);
    }
    [Fact(DisplayName = "MP02 | ID legado sem IGDB | Origem externa continua desconhecida")]
    public void LegadoNaoInferido()
    {
        var m = ConversorBibliotecaJson.ParaModelo(PastaTeste.Jogo());
        Assert.Equal(1010, m.SteamAppIdLegado); Assert.Null(m.Catalogo.Origem); Assert.Null(m.Catalogo.IdentificadorExterno);
    }
    [Fact(DisplayName = "MP03 | Registro com IgdbId | Referência IGDB identificada explicitamente")]
    public void IgdbConfirmado()
    {
        var r = PastaTeste.Jogo(); r.IgdbId = 42;
        var m = ConversorBibliotecaJson.ParaModelo(r);
        Assert.Equal("IGDB", m.Catalogo.Origem); Assert.Equal("42", m.Catalogo.IdentificadorExterno);
    }
    [Fact(DisplayName = "MP04 | Dois identificadores no registro | Ambos preservados no roundtrip")]
    public void DoisIds()
    {
        var r = PastaTeste.Jogo(); r.IgdbId = 77;
        var volta = Voltar(r); Assert.Equal(1010, volta.SteamAppId); Assert.Equal(77, volta.IgdbId);
    }
    [Fact(DisplayName = "MP05 | IDs ausentes | Valores continuam nulos no roundtrip")]
    public void SemIds()
    {
        var r = PastaTeste.Jogo(); r.SteamAppId = null;
        var volta = Voltar(r); Assert.Null(volta.SteamAppId); Assert.Null(volta.IgdbId);
    }
    [Fact(DisplayName = "MP06 | ID legado zero | Valor preservado sem correção presumida")]
    public void LegadoZero() { var r = PastaTeste.Jogo(); r.SteamAppId = 0; Assert.Equal(0, Voltar(r).SteamAppId); }
    [Fact(DisplayName = "MP07 | ID legado negativo | Valor preservado sem reinterpretar origem")]
    public void LegadoNegativo() { var r = PastaTeste.Jogo(); r.SteamAppId = -3; Assert.Equal(-3, Voltar(r).SteamAppId); }
    [Fact(DisplayName = "MP08 | ID IGDB máximo | Inteiro máximo preservado")]
    public void IdMaximo() { var r = PastaTeste.Jogo(); r.IgdbId = int.MaxValue; Assert.Equal(int.MaxValue, Voltar(r).IgdbId); }
    [Fact(DisplayName = "MP09 | Review com espaços e quebras | Texto exato preservado no roundtrip")]
    public void ReviewExata() { var r = PastaTeste.Jogo(); r.Review = "  antiga\nreview  "; Assert.Equal(r.Review, Voltar(r).Review); }
    [Fact(DisplayName = "MP10 | Título Unicode | Texto preservado sem normalização")]
    public void Unicode() { var r = PastaTeste.Jogo("Ação 日本語 🎮"); Assert.Equal(r.Titulo, Voltar(r).Titulo); }
    [Fact(DisplayName = "MP11 | Categoria pessoal | Categoria não convertida em gênero de catálogo")]
    public void CategoriaPessoal()
    {
        var m = ConversorBibliotecaJson.ParaModelo(PastaTeste.Jogo());
        Assert.Equal("RPG", m.Pessoal.Categoria); Assert.Empty(m.Catalogo.Generos);
    }
    [Fact(DisplayName = "MP12 | Extras de JSON ao carregar | Dicionário original isolado do modelo")]
    public void ExtrasIsoladosEntrada()
    {
        var r = PastaTeste.Jogo(); r.DadosAdicionais = new() { ["futuro"] = JsonSerializer.SerializeToElement(new { valor = 42 }) };
        var m = ConversorBibliotecaJson.ParaModelo(r); r.DadosAdicionais.Clear();
        Assert.Equal(42, m.DadosAdicionais!["futuro"].GetProperty("valor").GetInt32());
    }
    [Fact(DisplayName = "MP13 | Extras de JSON ao salvar | Alterar registro convertido não altera o modelo")]
    public void ExtrasIsoladosSaida()
    {
        var r = PastaTeste.Jogo(); r.DadosAdicionais = new() { ["futuro"] = JsonSerializer.SerializeToElement(42) };
        var m = ConversorBibliotecaJson.ParaModelo(r); var volta = ConversorBibliotecaJson.ParaRegistro(m); volta.DadosAdicionais!.Clear();
        Assert.Equal(42, m.DadosAdicionais!["futuro"].GetInt32());
    }
    [Fact(DisplayName = "MP14 | Catálogo compartilhado por registros pessoais | Editar um registro não altera o outro")]
    public void SeparacaoPessoal()
    {
        var um = Jogo(); var dois = new ItemBiblioteca(um.Catalogo, um.Pessoal with { Nota = 1 });
        Assert.Same(um.Catalogo, dois.Catalogo); Assert.Equal(4.5, um.Pessoal.Nota); Assert.Equal(1, dois.Pessoal.Nota);
    }
    [Fact(DisplayName = "MP15 | Preparar outra edição do catálogo | Informações pessoais permanecem independentes")]
    public void SeparacaoCatalogo()
    {
        var um = Jogo(); var dois = um with { Catalogo = um.Catalogo with { Titulo = "Outro título" } };
        Assert.Same(um.Pessoal, dois.Pessoal); Assert.Equal("Jogo de teste", um.Catalogo.Titulo); Assert.Equal("Outro título", dois.Catalogo.Titulo);
    }
    [Fact(DisplayName = "MP16 | Review legada longa e status desconhecido | Nenhum truncamento na conversão")]
    public void LegadoLongo()
    {
        var r = PastaTeste.Jogo(); r.Review = new string('x', 1200); r.Status = "Antigo";
        var volta = Voltar(r); Assert.Equal(r.Review, volta.Review); Assert.Equal("Antigo", volta.Status);
    }
    [Fact(DisplayName = "MP17 | Modelos das seis mídias | Cada tipo pode representar uma obra sem ativar telas ou APIs")]
    public void SeisMidias()
    {
        var tipos = Enum.GetValues<TipoMidia>(); Assert.Equal(6, tipos.Length);
        foreach (var tipo in tipos)
        {
            var obra = new ObraCatalogo { Tipo = tipo, Titulo = "Obra de teste", Origem = "catálogo futuro", IdentificadorExterno = "abc", CapaUrl = "https://example.invalid/capa.png", Generos = new[] { "Fantasia" } };
            Assert.Equal(tipo, obra.Tipo); Assert.Equal("abc", obra.IdentificadorExterno); Assert.Single(obra.Generos);
        }
    }
    [Fact(DisplayName = "MP18 | Registro completo após conversão | JSON semanticamente igual sem novo envelope")]
    public void MesmoJson()
    {
        var r = PastaTeste.Jogo(); r.IgdbId = 42;
        Assert.Equal(JsonSerializer.Serialize(r), JsonSerializer.Serialize(Voltar(r)));
    }
    [Fact(DisplayName = "MP19 | Registro legado sem IGDB no salvamento | Campo IgdbId continua omitido")]
    public void IgdbOmitido() => Assert.DoesNotContain("IgdbId", JsonSerializer.Serialize(Voltar(PastaTeste.Jogo())));
    [Fact(DisplayName = "MP20 | Review legada vazia | Valor vazio preservado na conversão")]
    public void ReviewVazia() { var r = PastaTeste.Jogo(); r.Review = ""; Assert.Equal("", Voltar(r).Review); }
}
