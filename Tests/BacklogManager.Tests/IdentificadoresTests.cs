using System.Text.Json;
using BacklogManager.Data;
using BacklogManager.Models;
using Xunit;

namespace BacklogManager.Tests;

public sealed class IdentificadoresTests
{
    [Theory]
    [InlineData("I01", 1010)]
    [InlineData("I02", null)]
    [InlineData("I03", 0)]
    [InlineData("I04", -1)]
    [InlineData("I05", int.MaxValue)]
    public void IdLegadoPreservadoSemInferirOrigem(string id, int? legado)
    {
        _ = id; using var pasta = new PastaTeste(); var jogo = PastaTeste.Jogo(); jogo.SteamAppId = legado;
        File.WriteAllText(pasta.Arquivo, JsonSerializer.Serialize(new[] { jogo }));
        var a = new FileManager(pasta.Arquivo); var carregados = a.Carregar(); a.Salvar(carregados);
        var salvo = Assert.Single(a.Carregar()); Assert.Equal(legado, salvo.SteamAppId); Assert.Null(salvo.IgdbId);
        Assert.DoesNotContain("IgdbId", File.ReadAllText(pasta.Arquivo));
    }

    [Fact(DisplayName = "I06 | Novo identificador IGDB | Roundtrip mantém ID separado do legado")]
    public void NovoId()
    {
        using var pasta = new PastaTeste(); var jogo = PastaTeste.Jogo(); jogo.SteamAppId = null; jogo.IgdbId = 123;
        var a = new FileManager(pasta.Arquivo); a.Carregar(); a.Salvar(new[] { jogo });
        var lido = Assert.Single(a.Carregar()); Assert.Equal(123, lido.IgdbId); Assert.Null(lido.SteamAppId);
    }
    [Fact(DisplayName = "I07 | Dois identificadores no JSON | Nenhum sobrescreve o outro")]
    public void Ambos()
    {
        var jogo = PastaTeste.Jogo(); jogo.IgdbId = 77;
        var lido = JsonSerializer.Deserialize<BacklogItem>(JsonSerializer.Serialize(jogo))!;
        Assert.Equal(1010, lido.SteamAppId); Assert.Equal(77, lido.IgdbId);
    }
    [Fact(DisplayName = "I08 | IDs legados repetidos | Registros continuam distintos")]
    public void Repetidos()
    {
        using var pasta = new PastaTeste(); var a = new FileManager(pasta.Arquivo); a.Carregar();
        a.Salvar(new[] { PastaTeste.Jogo("Um"), PastaTeste.Jogo("Dois") }); Assert.Equal(2, a.Carregar().Count);
    }
    [Fact(DisplayName = "I09 | Identificador IGDB zero | Gravação rejeitada sem descartar original")]
    public void IgdbZero() => Rejeitar(0);
    [Fact(DisplayName = "I10 | Identificador IGDB negativo | Gravação rejeitada sem descartar original")]
    public void IgdbNegativo() => Rejeitar(-1);
    private static void Rejeitar(int id)
    {
        using var pasta = new PastaTeste(); var a = new FileManager(pasta.Arquivo); a.Carregar();
        var jogo = PastaTeste.Jogo(); jogo.IgdbId = id;
        Assert.Throws<InvalidDataException>(() => a.Salvar(new[] { jogo })); Assert.False(File.Exists(pasta.Arquivo));
    }
    [Fact(DisplayName = "I11 | Legado sem propriedade SteamAppId | Carregado com ID nulo sem conversão")]
    public void PropriedadeAusente()
    {
        var jogo = JsonSerializer.Deserialize<BacklogItem>("{\"Titulo\":\"Jogo\",\"Categoria\":\"RPG\",\"Status\":\"Na Fila\"}")!;
        Assert.Null(jogo.SteamAppId); Assert.Null(jogo.IgdbId);
    }
    [Fact(DisplayName = "I12 | Identificador IGDB máximo | Inteiro positivo preservado sem heurística")]
    public void IgdbMaximo()
    {
        using var pasta = new PastaTeste(); var jogo = PastaTeste.Jogo(); jogo.IgdbId = int.MaxValue;
        var a = new FileManager(pasta.Arquivo); a.Carregar(); a.Salvar(new[] { jogo }); Assert.Equal(int.MaxValue, Assert.Single(a.Carregar()).IgdbId);
    }
}
