using BacklogManager.Services;
using Xunit;

namespace BacklogManager.Tests;

public sealed class ConfiguracaoTests
{
    [Theory]
    [InlineData("C01", "cliente", null, "legado-simulado", true, "legado-simulado")]
    [InlineData("C02", "cliente", "novo-simulado", null, true, "novo-simulado")]
    [InlineData("C03", "cliente", "novo-simulado", "legado-simulado", true, "novo-simulado")]
    [InlineData("C04", "cliente", "", "legado-simulado", true, "legado-simulado")]
    [InlineData("C05", "cliente", "   ", "legado-simulado", true, "legado-simulado")]
    [InlineData("C06", " cliente ", " novo-simulado ", null, true, "novo-simulado")]
    [InlineData("C07", null, "novo-simulado", null, false, "novo-simulado")]
    [InlineData("C08", "", "novo-simulado", null, false, "novo-simulado")]
    [InlineData("C09", "cliente", null, null, false, "")]
    [InlineData("C10", "cliente", null, "   ", false, "")]
    [InlineData("C11", "cliente\r\ninjetado", "novo-simulado", null, false, "novo-simulado")]
    [InlineData("C12", "cliente", "token simulado", null, false, "token simulado")]
    [InlineData("C13", "cliente", "token\r\ninjetado", "legado-simulado", false, "token\r\ninjetado")]
    public void ConfiguracaoValidaCompatibilidadeEPrecedencia(string id, string? cliente, string? novo, string? legado, bool esperado, string tokenEsperado)
    {
        _ = id; var variaveis = new Dictionary<string, string?> { ["IGDB_CLIENT_ID"] = cliente, ["IGDB_ACCESS_TOKEN"] = novo, ["IGDB_ACESS_TOKEN"] = legado };
        var valido = ConfiguracaoIgdb.TentarLer(out var clientId, out var token, nome => variaveis.GetValueOrDefault(nome));
        Assert.Equal(esperado, valido); Assert.Equal(cliente?.Trim() ?? "", clientId); Assert.Equal(tokenEsperado, token);
    }

    [Fact(DisplayName = "C14 | Credenciais inválidas no serviço | Exceção segura sem expor valores")]
    public void CredenciaisInvalidas()
    {
        using var cliente = new HttpClient(new HttpSimulado());
        var erro = Assert.Throws<ArgumentException>(() => new IgdbService(cliente, "", "segredo-simulado"));
        Assert.DoesNotContain("segredo-simulado", erro.Message);
    }
}
