using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;

namespace BacklogManager.Backup;

internal static class ArquivosBackup
{
    internal const int Limite = 64 * 1024 * 1024;
    internal static readonly JsonSerializerOptions Opcoes = new() { WriteIndented = true, MaxDepth = 64 };
    internal static string Hash(byte[]? bytes) => bytes == null ? "AUSENTE" : Convert.ToHexString(SHA256.HashData(bytes));
    internal static byte[]? Ler(string caminho, int limite = Limite)
    {
        try
        {
            using var f = new FileStream(caminho, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (f.Length > limite) throw new InvalidDataException("Arquivo excede o limite permitido.");
            using var m = new MemoryStream();
            var buffer = new byte[81920];
            int lidos;
            while ((lidos = f.Read(buffer, 0, buffer.Length)) > 0)
            {
                if (m.Length + lidos > limite) throw new InvalidDataException("Arquivo excede o limite permitido.");
                m.Write(buffer, 0, lidos);
            }
            return m.ToArray();
        }
        catch (FileNotFoundException) { return null; }
        catch (DirectoryNotFoundException) { return null; }
    }
    internal static T Decodificar<T>(byte[] bytes, int limite = Limite)
    {
        if (bytes.Length > limite) throw new InvalidDataException("Backup excede o limite de 64 MiB.");
        using var m = new MemoryStream(bytes);
        using var leitor = new StreamReader(m, System.Text.Encoding.UTF8, true);
        var texto = leitor.ReadToEnd();
        using var doc = JsonDocument.Parse(texto, new JsonDocumentOptions { MaxDepth = 64 });
        ValidarChaves(doc.RootElement);
        return JsonSerializer.Deserialize<T>(texto, Opcoes) ?? throw new InvalidDataException("Documento nulo.");
    }
    internal static void ValidarChaves(JsonElement e)
    {
        if (e.ValueKind == JsonValueKind.Object)
        {
            var nomes = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);
            foreach (var p in e.EnumerateObject())
            {
                if (!nomes.Add(p.Name)) throw new InvalidDataException("Propriedade JSON duplicada.");
                ValidarChaves(p.Value);
            }
        }
        else if (e.ValueKind == JsonValueKind.Array)
            foreach (var p in e.EnumerateArray()) ValidarChaves(p);
    }
    internal static void Gravar(string caminho, byte[] bytes, bool backup = false)
    {
        CaminhosDadosLocais.SemLinks(caminho);
        var temporario = caminho + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            var opcoes = new FileStreamOptions { Mode = FileMode.CreateNew, Access = FileAccess.Write, Share = FileShare.None };
            if (!OperatingSystem.IsWindows()) opcoes.UnixCreateMode = UnixFileMode.UserRead | UnixFileMode.UserWrite;
            using (var f = new FileStream(temporario, opcoes))
            { f.Write(bytes); f.Flush(true); }
            if (File.Exists(caminho)) File.Replace(temporario, caminho, backup ? caminho + ".bak" : null);
            else File.Move(temporario, caminho, false);
        }
        finally { try { File.Delete(temporario); } catch (IOException) { } catch (UnauthorizedAccessException) { } }
    }
}
