using System;
using System.Linq;

namespace BacklogManager.Services;

internal static class ConfiguracaoIgdb
{
    internal static bool TentarLer(out string clientId, out string accessToken,
        Func<string, string?>? lerVariavel = null)
    {
        lerVariavel ??= Environment.GetEnvironmentVariable;
        clientId = lerVariavel("IGDB_CLIENT_ID")?.Trim() ?? string.Empty;
        // A grafia corrigida tem preferência; valores vazios permitem o nome legado.
        var corrigido = lerVariavel("IGDB_ACCESS_TOKEN");
        accessToken = (string.IsNullOrWhiteSpace(corrigido)
            ? lerVariavel("IGDB_ACESS_TOKEN") : corrigido)?.Trim() ?? string.Empty;
        return CredencialValida(clientId) && CredencialValida(accessToken);
    }

    internal static bool CredencialValida(string? valor) =>
        !string.IsNullOrWhiteSpace(valor) && !valor.Any(c => char.IsWhiteSpace(c) || char.IsControl(c));
}
