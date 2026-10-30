using System;
using System.IO;
using System.Linq;
using BacklogManager.Backup;

namespace BacklogManager.Configuracao;

public static class ResolvedorDiretorios
{
    public static string Resolver(string[] argumentos, bool windows, string pastaLocal, string pastaUsuario,
        string? xdg = null)
    {
        var indices = argumentos.Select((valor, indice) => (valor, indice)).Where(p => p.valor == "--data-dir").ToArray();
        if (indices.Length > 1) throw new ArgumentException("Informe --data-dir somente uma vez.");
        if (indices.Length == 1)
        {
            var i = indices[0].indice;
            if (i + 1 >= argumentos.Length || !Path.IsPathFullyQualified(argumentos[i + 1]))
                throw new ArgumentException("--data-dir exige um caminho absoluto.");
            return Path.GetFullPath(argumentos[i + 1]);
        }
        var baseLocal = windows ? pastaLocal : !string.IsNullOrWhiteSpace(xdg) && Path.IsPathFullyQualified(xdg)
            ? xdg : Path.Combine(pastaUsuario, ".local", "share");
        if (!Path.IsPathFullyQualified(baseLocal)) throw new IOException("Não foi possível identificar a pasta pessoal de dados.");
        return Path.Combine(baseLocal, windows ? "MinhasRealizacoes" : "minhas-realizacoes", windows ? "Dados" : "dados");
    }

    public static string Atual(string[] argumentos) => Resolver(argumentos, OperatingSystem.IsWindows(),
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), Environment.GetEnvironmentVariable("XDG_DATA_HOME"));

    public static CaminhosDadosLocais Preparar(string pasta)
    {
        CaminhosDadosLocais.SemLinks(pasta);
        if (OperatingSystem.IsWindows()) Directory.CreateDirectory(pasta);
        else Directory.CreateDirectory(pasta, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute);
        // Provar acesso antes dos leitores: jamais recorrer silenciosamente à pasta dos binários.
        var prova = Path.Combine(pasta, ".acesso-" + Guid.NewGuid().ToString("N") + ".tmp");
        try { using var f = new FileStream(prova, FileMode.CreateNew, FileAccess.Write, FileShare.None); f.Flush(true); }
        finally { if (File.Exists(prova)) File.Delete(prova); }
        return new CaminhosDadosLocais(Path.Combine(pasta, "backlog.json"));
    }

    public static bool TemDados(CaminhosDadosLocais caminhos) => caminhos.Arquivos.Values.Any(File.Exists) || File.Exists(caminhos.Diario);
}
