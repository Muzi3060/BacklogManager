using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using BacklogManager.Data;
using BacklogManager.Services;
using BacklogManager.Aparencia;

namespace BacklogManager.Backup;

internal static class ValidadorBackup
{
    internal static DocumentoBackup Ler(byte[] bytes)
    {
        var d = ArquivosBackup.Decodificar<DocumentoBackup>(bytes);
        if (d.Formato != "MinhasRealizacoes.BackupLocal" || d.Versao != 1 || d.CriadoEm == default ||
            d.Componentes == null || !d.Componentes.ContainsKey("Biblioteca") || !d.Componentes.ContainsKey("Colecoes") ||
            d.Componentes.Keys.Any(k => k is not ("Biblioteca" or "Colecoes" or "Aparencia" or "Visualizacao")))
            throw new InvalidDataException("Formato de backup completo não suportado.");
        long total = 0;
        foreach (var c in d.Componentes.Values)
        {
            if (c == null || c.Sha256 != ArquivosBackup.Hash(c.Conteudo))
                throw new InvalidDataException("Integridade de componente inválida.");
            total += c.Conteudo?.Length ?? 0;
        }
        if (total > ArquivosBackup.Limite) throw new InvalidDataException("Conteúdo muito grande.");
        return d;
    }
    internal static DadosValidados Validar(DocumentoBackup d)
    {
        // Os leitores existentes validam cópias temporárias. Nunca chamamos Salvar nos arquivos ativos.
        var pasta = Path.Combine(Path.GetTempPath(), "MinhasRealizacoes.Validacao", Guid.NewGuid().ToString("N"));
        CaminhosDadosLocais.SemLinks(pasta);
        if (OperatingSystem.IsWindows()) Directory.CreateDirectory(pasta);
        else Directory.CreateDirectory(pasta, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute);
        try
        {
            foreach (var (nome, componente) in d.Componentes)
                if (componente.Conteudo != null)
                {
                    if (nome is "Biblioteca" or "Colecoes")
                    {
                        using var memoria = new MemoryStream(componente.Conteudo);
                        using var leitor = new StreamReader(memoria, System.Text.Encoding.UTF8, true);
                        using var json = JsonDocument.Parse(leitor.ReadToEnd());
                        ArquivosBackup.ValidarChaves(json.RootElement);
                    }
                    File.WriteAllBytes(Path.Combine(pasta, nome), componente.Conteudo);
                }
            var biblioteca = new ServicoBiblioteca(new ArmazenamentoJsonBiblioteca(new FileManager(Path.Combine(pasta, "Biblioteca"))));
            biblioteca.Carregar();
            var colecoes = new ServicoColecoes(new ArmazenamentoColecoesJson(Path.Combine(pasta, "Colecoes")), biblioteca);
            colecoes.Carregar();
            var ids = biblioteca.Itens.Where(i => i.IdInterno != null).Select(i => i.IdInterno!.Value).ToHashSet();
            var ausentes = colecoes.Colecoes.Sum(c => c.Obras.Count(id => !ids.Contains(id)));
            var preferenciasValidas = true;
            var aviso = ausentes == 0 ? "" : $"{ausentes} referências de obras ausentes serão preservadas. ";
            try
            {
                foreach (var nome in new[] { "Visualizacao", "Aparencia" })
                    if (d.Componentes.TryGetValue(nome, out var c) && c.Conteudo != null)
                    {
                        using var m = new MemoryStream(c.Conteudo); using var r = new StreamReader(m);
                        using var j = JsonDocument.Parse(r.ReadToEnd()); ArquivosBackup.ValidarChaves(j.RootElement);
                    }
                if (d.Componentes.TryGetValue("Visualizacao", out var v) && v.Conteudo != null)
                    new ArmazenamentoPreferenciasBiblioteca(Path.Combine(pasta, "Visualizacao")).Carregar();
                if (d.Componentes.TryGetValue("Aparencia", out var a) && a.Conteudo != null &&
                    new ArmazenamentoAparenciaJson(Path.Combine(pasta, "Aparencia")).Carregar().Aviso != null)
                    preferenciasValidas = false;
            }
            catch (Exception e) when (ServicoBackupLocal.Falha(e))
            { preferenciasValidas = false; }
            if (!preferenciasValidas) aviso += "Preferências incompatíveis: restaure somente os dados.";
            return new(biblioteca.Itens.Count, colecoes.Colecoes.Count,
                colecoes.Colecoes.Sum(c => c.Obras.Count), ausentes, preferenciasValidas, aviso,
                string.Join("; ", biblioteca.Itens.GroupBy(i => i.Catalogo.Tipo).Select(g => $"{Models.TextosMidia.Plural(g.Key)}: {g.Count()}")));
        }
        finally { Directory.Delete(pasta, true); }
    }
}
