using System.Text.Json;
using BacklogManager.Models;
using BacklogManager.Services;
using BacklogManager.Data;
using BacklogManager.Tests.Fase1;
using BacklogManager.Tests.Fase2;
using Xunit;
namespace BacklogManager.Tests.AjustesFase3;

public sealed class GenerosLegadosTests
{
    [Fact(DisplayName = "GL01 | Legado sem identificador externo | Categoria antiga permanece no modelo e JSON")]
    public void SemId() { var i = ConversorBibliotecaJson.ParaModelo(PastaTeste.Jogo()); Assert.Equal("RPG", ApoioMultimidia.Roundtrip(i).Pessoal.Categoria); Assert.Null(i.Catalogo.IdentificadorExterno); }
    [Fact(DisplayName = "GL02 | Steam legado opaco | Edição pessoal não converte Steam em IGDB")]
    public void Steam() { var dto = PastaTeste.Jogo(); dto.SteamAppId = 361855; var i = ConversorBibliotecaJson.ParaModelo(dto); var r = FormularioJogo.Preparar(new(i.Catalogo.Titulo, null, "Zerado", 5m, "Nova"), null, null, i); var salvo = ConversorBibliotecaJson.ParaRegistro(ApoioMultimidia.Roundtrip(r)); Assert.Equal(361855, salvo.SteamAppId); Assert.Null(salvo.IgdbId); Assert.Equal(dto.Categoria, salvo.Categoria); }
    [Fact(DisplayName = "GL03 | Gêneros externos e categoria antiga coexistem | Exibição usa catálogo e persistência preserva categoria")]
    public void Prioridade() { var i = ApoioArquitetura.Jogo() with { Catalogo = CadastroIgdbTests.Jogo }; var r = ApoioMultimidia.Roundtrip(i); Assert.Equal("RPG, Adventure", r.GenerosExibidos); Assert.Equal("RPG", r.Pessoal.Categoria); }
    [Fact(DisplayName = "GL04 | Legado sem gêneros externos | Categoria legada aparece com identificação explícita")]
    public void RotuloLegado() => Assert.Equal("Categoria legada: RPG", ApoioArquitetura.Jogo().GenerosExibidos);
    [Fact(DisplayName = "GL05 | Categoria antiga fora das opções | Edição preserva valor sem classificação obrigatória")]
    public void Desconhecida() { var i = ApoioArquitetura.Jogo(); i = i with { Pessoal = i.Pessoal with { Categoria = "Categoria histórica" } }; var r = FormularioJogo.Preparar(new(i.Catalogo.Titulo, "Outra", "Na Fila", 4m, ""), null, null, i); Assert.Equal("Categoria histórica", r.Pessoal.Categoria); }
    [Fact(DisplayName = "GL06 | Legado filtrado por categoria | Filtro continua encontrando dados antigos")]
    public void FiltroLegado() { var (s, _) = ApoioArquitetura.Carregada(ApoioArquitetura.Jogo()); Assert.Single(s.Consultar(new() { Genero = "RPG" })); }
    [Fact(DisplayName = "GL07 | Jogo sem gêneros externos nem categoria | Filtro não oferece categoria fictícia")]
    public void SemGenero() { var i = FormularioJogo.Preparar(new("Título oficial", null, "Na Fila", 4m, ""), CadastroIgdbTests.Jogo with { Generos = Array.Empty<string>() }, "Título oficial", null); var (s, _) = ApoioArquitetura.Carregada(i); Assert.Empty(s.Consultar(new() { Genero = "RPG" })); Assert.Empty(ServicoBiblioteca.GenerosParaFiltro(i)); }
    [Fact(DisplayName = "GL08 | Categoria vazia sem origem IGDB | Persistência continua recusando registro inválido")]
    public void VaziaSemIgdb() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "[{\"Titulo\":\"Legado\",\"Categoria\":\"\",\"Status\":\"Na Fila\",\"Review\":\"\"}]"); Assert.Throws<InvalidDataException>(() => new FileManager(p.Arquivo).Carregar()); }
    [Fact(DisplayName = "GL09 | Categoria null mesmo com IGDB | Nulo inválido não é silenciosamente convertido")]
    public void Nula() { using var p = new PastaTeste(); var dto = PastaTeste.Jogo(); dto.IgdbId = 42; dto.Categoria = null!; File.WriteAllText(p.Arquivo, JsonSerializer.Serialize(new[] { dto })); Assert.Throws<InvalidDataException>(() => new FileManager(p.Arquivo).Carregar()); }
    [Fact(DisplayName = "GL10 | Outras cinco mídias | Seleção simulada e categoria pessoal continuam disponíveis")]
    public void OutrasMidias() { foreach (var tipo in Enum.GetValues<TipoMidia>().Skip(1)) { var c = ApoioMultimidia.Catalogo(tipo); var r = FormularioObra.Preparar(new(c.Titulo, "Categoria pessoal", "Na Fila", 4m, ""), c, c.Titulo, null, tipo); Assert.Equal("Categoria pessoal", r.Pessoal.Categoria); Assert.True(r.Catalogo.Simulado); } }
    [Fact(DisplayName = "GL11 | Extensões JSON antigas | Edição não remove campos adicionais")]
    public void Extras() { using var d = JsonDocument.Parse("123"); var i = ApoioArquitetura.Jogo() with { DadosAdicionais = new Dictionary<string, JsonElement> { ["MinhaExtensao"] = d.RootElement.Clone() } }; var r = ApoioMultimidia.Roundtrip(i); Assert.Equal(123, r.DadosAdicionais!["MinhaExtensao"].GetInt32()); }
    [Fact(DisplayName = "GL12 | Coleção mista antiga e nova | Recarga preserva categorias antigas e vazio intencional no novo jogo")]
    public void Mista() { using var p = new PastaTeste(); var a = new ArmazenamentoJsonBiblioteca(new FileManager(p.Arquivo)); a.Carregar(); var novo = FormularioJogo.Preparar(new("Título oficial", null, "Na Fila", 4m, ""), CadastroIgdbTests.Jogo, "Título oficial", null); a.Salvar(new[] { ApoioArquitetura.Jogo(), novo, ApoioMultimidia.Item(TipoMidia.Livro) }); var r = a.Carregar(); Assert.Equal(3, r.Count); Assert.Equal("RPG", r[0].Pessoal.Categoria); Assert.Empty(r[1].Pessoal.Categoria); Assert.Equal("Fantasia", r[2].Pessoal.Categoria); }
}
