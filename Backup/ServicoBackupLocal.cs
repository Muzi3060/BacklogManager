using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BacklogManager.Backup;

public sealed class ServicoBackupLocal
{
    public CaminhosDadosLocais Caminhos { get; }
    public string PastaSeguranca { get; }
    public string? Pendencia { get; private set; }
    public string? UltimoBackupSeguranca { get; private set; }
    internal Action<string>? Etapa { get; set; }
    internal Action? ConferirEstadoCarregado { get; set; }
    public ServicoBackupLocal(CaminhosDadosLocais caminhos, string? pastaSeguranca = null)
    {
        Caminhos = caminhos;
        PastaSeguranca = Path.GetFullPath(pastaSeguranca ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Backups", "MinhasRealizacoes"));
    }
    private void Disponivel()
    {
        if (File.Exists(Caminhos.Diario) || Pendencia != null)
            throw new InvalidOperationException("Recuperação pendente. Use a ação de recuperação nas Configurações. " + Pendencia);
    }
    private DocumentoBackup Capturar(bool preferencias = true) => new()
    {
        Formato = "MinhasRealizacoes.BackupLocal", Versao = 1, CriadoEm = DateTimeOffset.UtcNow,
        Componentes = Caminhos.Arquivos.Where(p => preferencias || p.Key is "Biblioteca" or "Colecoes")
            .ToDictionary(p => p.Key, p => Componente(ArquivosBackup.Ler(p.Value)))
    };
    private static ComponenteBackup Componente(byte[]? bytes) => new() { Conteudo = bytes, Sha256 = ArquivosBackup.Hash(bytes) };
    private static byte[] Codificar(DocumentoBackup doc)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(doc, ArquivosBackup.Opcoes);
        if (bytes.Length > ArquivosBackup.Limite) throw new InvalidDataException("Backup excede o limite de 64 MiB.");
        return bytes;
    }
    private static string HashEstado(DocumentoBackup d) => string.Join("|", d.Componentes.OrderBy(p => p.Key).Select(p => p.Key + ":" + p.Value.Sha256));
    private void Conferir(DocumentoBackup estado)
    {
        if (HashEstado(estado) != HashEstado(Capturar())) throw new IOException("Os arquivos mudaram durante a operação. Reabra e tente novamente.");
    }
    public void Exportar(string destino, bool incluirPreferencias)
    {
        Disponivel(); Caminhos.ValidarDestino(destino);
        using var bloqueios = Bloquear();
        Disponivel(); ConferirEstadoCarregado?.Invoke();
        var estado = Capturar(); var d = Capturar(incluirPreferencias);
        var validacao = ValidadorBackup.Validar(d);
        if (incluirPreferencias && !validacao.PreferenciasValidas)
            throw new InvalidDataException("Preferências incompatíveis. Exporte sem preferências ou corrija-as antes.");
        Etapa?.Invoke("exportacao-validada"); Conferir(estado);
        EscreverVerificado(destino, d);
        Conferir(estado);
    }
    private static void EscreverVerificado(string destino, DocumentoBackup d, Action? aposGravar = null)
    {
        var bytes = Codificar(d);
        ArquivosBackup.Gravar(destino, bytes, true);
        aposGravar?.Invoke();
        var releitura = ArquivosBackup.Ler(destino) ?? throw new IOException("Backup não encontrado após gravação.");
        ValidadorBackup.Ler(releitura);
        if (ArquivosBackup.Hash(bytes) != ArquivosBackup.Hash(releitura)) throw new IOException("Verificação do backup falhou.");
    }
    public PreviaRestauracao Prever(string arquivo)
    {
        Disponivel();
        var bytes = ArquivosBackup.Ler(arquivo) ?? throw new FileNotFoundException("Backup não encontrado.");
        var d = ValidadorBackup.Ler(bytes); var v = ValidadorBackup.Validar(d);
        using var bloqueios = Bloquear(); Disponivel();
        var atual = Capturar();
        int obras = -1, colecoes = -1;
        var avisoAtual = "";
        try { var a = ValidadorBackup.Validar(atual); obras = a.Obras; colecoes = a.Colecoes; }
        catch (Exception e) when (Falha(e))
        { avisoAtual = "Os dados atuais não puderam ser interpretados. A cópia de segurança preservará seus bytes; a recuperação desse conteúdo pode exigir análise manual. "; }
        return new(Path.GetFullPath(arquivo), ArquivosBackup.Hash(bytes), v.Obras, v.Colecoes, v.Vinculos, v.Ausentes,
            obras, colecoes, v.PreferenciasValidas, d.Componentes.Any(p => p.Key is "Aparencia" or "Visualizacao" && p.Value.Conteudo != null),
            avisoAtual + v.Aviso, v.PorMidia, d.CriadoEm, HashEstado(atual),
            string.Join(", ", d.Componentes.Where(p => p.Key is "Aparencia" or "Visualizacao" && p.Value.Conteudo != null)
                .Select(p => p.Key == "Aparencia" ? "aparência" : "visualização")),
            d.Componentes.TryGetValue("Aparencia", out var aparencia) && aparencia.Conteudo != null,
            d.Componentes.TryGetValue("Visualizacao", out var visualizacao) && visualizacao.Conteudo != null);
    }
    public string Restaurar(PreviaRestauracao previa, bool restaurarPreferencias, bool confirmacao)
    {
        if (!confirmacao) throw new InvalidOperationException("Confirme explicitamente que os dados atuais serão substituídos.");
        Disponivel();
        using var bloqueios = Bloquear(); Disponivel();
        var bytes = ArquivosBackup.Ler(previa.Arquivo) ?? throw new FileNotFoundException("Backup não encontrado.");
        if (ArquivosBackup.Hash(bytes) != previa.HashArquivo) throw new IOException("O backup mudou após a prévia. Selecione-o novamente.");
        var novo = ValidadorBackup.Ler(bytes); var validacao = ValidadorBackup.Validar(novo);
        if (restaurarPreferencias && !validacao.PreferenciasValidas) throw new InvalidDataException("Preferências incompatíveis. Restaure somente os dados.");
        var anterior = Capturar();
        if (HashEstado(anterior) != previa.HashEstadoAtual) throw new IOException("Os dados atuais mudaram após a prévia. Selecione o backup novamente.");
        Etapa?.Invoke("antes-backup");
        // Cópia de segurança preserva os bytes atuais mesmo quando o estado anterior está danificado.
        // A verificação é de formato/hash; a prévia sinaliza quando as contagens atuais são desconhecidas.
        CaminhosDadosLocais.SemLinks(PastaSeguranca);
        if (PastaSeguranca.Equals(Caminhos.Pasta, StringComparison.OrdinalIgnoreCase) ||
            PastaSeguranca.StartsWith(Caminhos.Pasta + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            throw new IOException("A pasta de segurança precisa ficar fora da pasta dos dados.");
        if (OperatingSystem.IsWindows()) Directory.CreateDirectory(PastaSeguranca);
        else Directory.CreateDirectory(PastaSeguranca, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute);
        var seguranca = Path.Combine(PastaSeguranca, "antes-restauracao-" + DateTime.UtcNow.ToString("yyyyMMdd-HHmmss") + "-" + Guid.NewGuid().ToString("N") + ".json");
        Caminhos.ValidarDestino(seguranca);
        EscreverVerificado(seguranca, anterior, () => Etapa?.Invoke("backup-gravado"));
        UltimoBackupSeguranca = seguranca;
        Etapa?.Invoke("backup-verificado"); Conferir(anterior);
        var alvos = novo.Componentes.Where(p => p.Key is "Biblioteca" or "Colecoes" || restaurarPreferencias && p.Value.Conteudo != null)
            .ToDictionary(p => p.Key, p => p.Value);
        var diario = new DiarioRestauracao { Versao = 1, Estado = "Aplicando", Anterior = anterior, Novos = alvos };
        var preparados = new Dictionary<string, string>();
        var aplicacaoIniciada = false;
        try
        {
            // Prepare e faça flush de todos os novos arquivos antes de tocar no conjunto ativo.
            foreach (var (nome, c) in alvos.Where(p => p.Value.Conteudo != null))
            {
                var temporario = Caminhos.Arquivos[nome] + "." + Guid.NewGuid().ToString("N") + ".tmp";
                preparados[nome] = temporario;
                ArquivosBackup.Gravar(temporario, c.Conteudo!);
                if (ArquivosBackup.Hash(ArquivosBackup.Ler(temporario)) != c.Sha256)
                    throw new IOException("Preparação de " + nome + " falhou.");
            }
            Etapa?.Invoke("arquivos-preparados"); Conferir(anterior);
            GravarDiario(diario); aplicacaoIniciada = true; Etapa?.Invoke("diario-gravado");
            foreach (var (nome, c) in alvos)
            {
                Etapa?.Invoke("antes-" + nome);
                var caminho = Caminhos.Arquivos[nome];
                CaminhosDadosLocais.SemLinks(caminho);
                if (c.Conteudo == null) File.Delete(caminho);
                else if (File.Exists(caminho)) File.Replace(preparados[nome], caminho, null);
                else File.Move(preparados[nome], caminho, false);
                Etapa?.Invoke("depois-" + nome);
                // O diário inicial já contém todas as etapas esperadas; cada estado efetivo é conferido pelo hash.
                GravarDiario(diario);
            }
            foreach (var (nome, c) in alvos) ConferirComponente(nome, c);
            Etapa?.Invoke("antes-confirmacao");
            GravarDiario(diario with { Estado = "Confirmada" });
            Etapa?.Invoke("confirmada");
            File.Delete(Caminhos.Diario);
            Pendencia = null;
            return seguranca;
        }
        catch (Exception e) when (Falha(e))
        {
            if (!aplicacaoIniciada)
                throw new IOException("Restauração cancelada antes da substituição dos dados. Backup anterior: " + seguranca, e);
            try
            {
                // Se houve erro após o marcador de confirmação, torne a intenção de rollback durável.
                GravarDiario(diario); Reverter(diario); File.Delete(Caminhos.Diario); Pendencia = null;
            }
            catch (Exception recuperacao) when (Falha(recuperacao))
            {
                Pendencia = "Falha na recuperação. Arquivos preservados para análise. " + recuperacao.Message;
                throw new IOException("Restauração não concluída; recuperação pendente. Backup anterior: " + seguranca, e);
            }
            throw new IOException("Restauração cancelada; o estado anterior foi recuperado e verificado. Backup: " + seguranca, e);
        }
        finally
        {
            foreach (var p in preparados.Values)
            { try { File.Delete(p); } catch (IOException) { } catch (UnauthorizedAccessException) { } }
        }
    }
    private void Substituir(string nome, ComponenteBackup c)
    {
        var caminho = Caminhos.Arquivos[nome]; CaminhosDadosLocais.SemLinks(caminho);
        if (c.Conteudo == null) File.Delete(caminho);
        else ArquivosBackup.Gravar(caminho, c.Conteudo);
    }
    private void ConferirComponente(string nome, ComponenteBackup c)
    {
        if (ArquivosBackup.Hash(ArquivosBackup.Ler(Caminhos.Arquivos[nome])) != c.Sha256)
            throw new IOException("Verificação de " + nome + " falhou.");
    }
    private void GravarDiario(DiarioRestauracao d)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(d, ArquivosBackup.Opcoes);
        // O diário pode conter dois conjuntos de 64 MiB; o limite próprio é aplicado na leitura de recuperação.
        ArquivosBackup.Gravar(Caminhos.Diario, bytes);
    }
    private void Reverter(DiarioRestauracao d)
    {
        // Confira TODOS os alvos antes de alterar qualquer um: alterações externas desconhecidas bloqueiam a recuperação.
        foreach (var (nome, novo) in d.Novos)
        {
            var hash = ArquivosBackup.Hash(ArquivosBackup.Ler(Caminhos.Arquivos[nome]));
            if (hash != novo.Sha256 && hash != d.Anterior.Componentes[nome].Sha256)
                throw new IOException("Mudança externa em " + nome + ". Recuperação automática bloqueada.");
        }
        foreach (var nome in d.Novos.Keys)
        { Etapa?.Invoke("rollback-" + nome); Substituir(nome, d.Anterior.Componentes[nome]); }
        foreach (var nome in d.Novos.Keys) ConferirComponente(nome, d.Anterior.Componentes[nome]);
    }
    public bool Recuperar()
    {
        if (!File.Exists(Caminhos.Diario)) { Pendencia = null; return true; }
        try
        {
            using var bloqueios = Bloquear();
            CaminhosDadosLocais.SemLinks(Caminhos.Diario);
            var info = new FileInfo(Caminhos.Diario);
            if (info.Length > ArquivosBackup.Limite * 3L) throw new InvalidDataException("Diário muito grande.");
            // Limite do diário é maior que o do envelope porque contém os dois estados.
            var bytes = ArquivosBackup.Ler(Caminhos.Diario, ArquivosBackup.Limite * 3)
                ?? throw new IOException("Diário não encontrado durante a recuperação.");
            var d = ArquivosBackup.Decodificar<DiarioRestauracao>(bytes, ArquivosBackup.Limite * 3);
            if (d.Versao != 1 || d.Estado is not ("Aplicando" or "Confirmada") || d.Anterior == null || d.Novos == null ||
                !d.Novos.ContainsKey("Biblioteca") || !d.Novos.ContainsKey("Colecoes") ||
                d.Anterior.Componentes == null || d.Anterior.Componentes.Count != 4 || d.Novos.Keys.Any(k => !Caminhos.Arquivos.ContainsKey(k)))
                throw new InvalidDataException("Diário incompatível. Nenhum arquivo foi descartado.");
            ValidadorBackup.Ler(Codificar(d.Anterior));
            foreach (var (nome, c) in d.Novos)
                if (c == null || c.Sha256 != ArquivosBackup.Hash(c.Conteudo) || !d.Anterior.Componentes.ContainsKey(nome))
                    throw new InvalidDataException("Diário inválido.");
            if (d.Estado == "Confirmada") foreach (var (nome, c) in d.Novos) ConferirComponente(nome, c);
            else Reverter(d);
            File.Delete(Caminhos.Diario); Pendencia = null; return true;
        }
        catch (Exception e) when (Falha(e)) { Pendencia = "Recuperação pendente: " + e.Message; return false; }
    }
    internal IDisposable Bloquear()
    {
        var locks = new List<FileStream>();
        try
        {
            foreach (var p in new[] { Caminhos.Trava }.Concat(Caminhos.Arquivos.Values.OrderBy(p => p, StringComparer.Ordinal).Select(p => p + ".lock")))
            { CaminhosDadosLocais.SemLinks(p); locks.Add(new FileStream(p, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None)); }
            return new Travas(locks);
        }
        catch { foreach (var f in locks) f.Dispose(); throw; }
    }
    private sealed class Travas(System.Collections.Generic.List<FileStream> arquivos) : IDisposable
    { public void Dispose() { foreach (var f in arquivos) f.Dispose(); } }
    internal static bool Falha(Exception e) => e is IOException or InvalidDataException or UnauthorizedAccessException or System.Security.SecurityException or
        JsonException or InvalidOperationException or ArgumentException or NotSupportedException;
}
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
internal sealed record DiarioRestauracao
{
    public required int Versao { get; init; }
    public required string Estado { get; init; }
    public required DocumentoBackup Anterior { get; init; }
    public required Dictionary<string, ComponenteBackup> Novos { get; init; }
}
