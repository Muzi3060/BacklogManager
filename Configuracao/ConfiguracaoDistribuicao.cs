using System.Reflection;
using System.Linq;
namespace BacklogManager.Configuracao;
public static class ConfiguracaoDistribuicao
{
    // Endereço oficial definido no build; nunca inferido do Git na máquina de quem usa a beta.
    public static string? RepositorioReleases => typeof(ConfiguracaoDistribuicao).Assembly
        .GetCustomAttributes<AssemblyMetadataAttribute>().FirstOrDefault(a=>a.Key=="RepositorioReleases")?.Value;
}
