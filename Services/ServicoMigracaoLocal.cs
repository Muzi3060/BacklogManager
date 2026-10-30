using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BacklogManager.Backup;
using BacklogManager.Configuracao;

namespace BacklogManager.Services;

public sealed record PreviaMigracao(string Origem, string Destino, int Obras, int Colecoes, int Vinculos,
    bool PreferenciasValidas, string Aviso, IReadOnlyDictionary<string, string> Hashes);

public sealed class ServicoMigracaoLocal(CaminhosDadosLocais destino, string? pastaSeguranca = null)
{
    private readonly ServicoBackupLocal _destino = new(destino, pastaSeguranca);
    internal Action<string>? Etapa { get; set; }
    public string PastaSeguranca => _destino.PastaSeguranca;

    public static string? IdentificarLegado(CaminhosDadosLocais destino, params string[] pastas)
        => pastas.Select(Path.GetFullPath).Distinct().FirstOrDefault(p =>
            !p.Equals(destino.Pasta, StringComparison.OrdinalIgnoreCase) && File.Exists(Path.Combine(p, "backlog.json")));

    private CaminhosDadosLocais Origem(string pasta)
    {
        if (!Path.IsPathFullyQualified(pasta)) throw new IOException("Selecione uma pasta absoluta.");
        var origem = new CaminhosDadosLocais(Path.Combine(pasta, "backlog.json"));
        foreach (var p in new[] { origem.Pasta, destino.Pasta, PastaSeguranca }) CaminhosDadosLocais.SemLinks(p);
        static bool Sobrepoe(string a,string b) => a.Equals(b,StringComparison.OrdinalIgnoreCase) ||
            a.StartsWith(b.TrimEnd(Path.DirectorySeparatorChar)+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase);
        if (Sobrepoe(origem.Pasta,destino.Pasta) || Sobrepoe(destino.Pasta,origem.Pasta) ||
            Sobrepoe(PastaSeguranca,origem.Pasta) || Sobrepoe(origem.Pasta,PastaSeguranca))
            throw new IOException("Origem, destino e backup precisam ficar em pastas separadas, sem sobreposição.");
        if (File.Exists(origem.Diario)) throw new IOException("A origem tem recuperação pendente. Recupere-a antes de importar.");
        if (ResolvedorDiretorios.TemDados(destino)) throw new IOException("O destino já contém dados ou recuperação pendente. Use a restauração em Configurações.");
        if (!File.Exists(origem.Arquivos["Biblioteca"])) throw new IOException("A origem não contém backlog.json.");
        return origem;
    }
    private static Dictionary<string,byte[]?> Capturar(CaminhosDadosLocais origem) => origem.Arquivos.Values
        .SelectMany(p=>new[]{p,p+".bak"}).ToDictionary(p=>p,p=> { CaminhosDadosLocais.SemLinks(p); return ArquivosBackup.Ler(p); });
    private static Dictionary<string,string> Hashes(Dictionary<string,byte[]?> arquivos) => arquivos.ToDictionary(p=>p.Key,p=>ArquivosBackup.Hash(p.Value));
    private static DocumentoBackup Documento(CaminhosDadosLocais origem,Dictionary<string,byte[]?> arquivos) => new()
    {
        Formato="MinhasRealizacoes.BackupLocal",Versao=1,CriadoEm=DateTimeOffset.UtcNow,
        Componentes=origem.Arquivos.ToDictionary(p=>p.Key,p=>new ComponenteBackup {Conteudo=arquivos[p.Value],Sha256=ArquivosBackup.Hash(arquivos[p.Value])})
    };
    private static void Conferir(IReadOnlyDictionary<string,string> esperado,Dictionary<string,byte[]?> atual)
    {
        if(esperado.Count!=atual.Count || atual.Any(p=>!esperado.TryGetValue(p.Key,out var hash) || hash!=ArquivosBackup.Hash(p.Value)))
            throw new IOException("A origem mudou após a prévia. Selecione novamente.");
    }
    public PreviaMigracao Prever(string pasta)
    {
        var origem=Origem(pasta);
        using var trava=new ServicoBackupLocal(origem).Bloquear();
        Origem(origem.Pasta);
        var arquivos=Capturar(origem);var v=ValidadorBackup.Validar(Documento(origem,arquivos));
        return new(origem.Pasta,destino.Pasta,v.Obras,v.Colecoes,v.Vinculos,v.PreferenciasValidas,v.Aviso,Hashes(arquivos));
    }
    public string Migrar(PreviaMigracao previa,bool preferencias,bool confirmacao)
    {
        if(!confirmacao) throw new InvalidOperationException("Confirme a cópia dos dados antes de continuar.");
        if(previa.Destino!=destino.Pasta) throw new IOException("A prévia pertence a outro destino.");
        var origem=Origem(previa.Origem);
        using var trava=new ServicoBackupLocal(origem).Bloquear();
        Origem(origem.Pasta);
        var arquivos=Capturar(origem);Conferir(previa.Hashes,arquivos);
        var doc=Documento(origem,arquivos);var v=ValidadorBackup.Validar(doc);
        if(preferencias && !v.PreferenciasValidas) throw new IOException("Preferências incompatíveis. Desmarque a importação de preferências.");
        // Só os oito arquivos conhecidos entram na cópia privada; nenhum diretório é copiado recursivamente.
        CaminhosDadosLocais.SemLinks(PastaSeguranca);
        var seguranca=Path.Combine(PastaSeguranca,"migracao-"+Guid.NewGuid().ToString("N"));
        if(OperatingSystem.IsWindows()) Directory.CreateDirectory(seguranca);
        else Directory.CreateDirectory(seguranca,UnixFileMode.UserRead|UnixFileMode.UserWrite|UnixFileMode.UserExecute);
        foreach(var (arquivo,bytes) in arquivos.Where(p=>p.Value!=null))
        {
            var copia=Path.Combine(seguranca,Path.GetFileName(arquivo));
            ArquivosBackup.Gravar(copia,bytes!);
            Etapa?.Invoke("copia-gravada");
            if(ArquivosBackup.Hash(ArquivosBackup.Ler(copia))!=previa.Hashes[arquivo]) throw new IOException("O backup da origem não foi verificado.");
        }
        ArquivosBackup.Gravar(Path.Combine(seguranca,"manifesto.json"),JsonSerializer.SerializeToUtf8Bytes(
            arquivos.Where(p=>p.Value!=null).ToDictionary(p=>Path.GetFileName(p.Key),p=>previa.Hashes[p.Key]),ArquivosBackup.Opcoes));
        Etapa?.Invoke("backup-verificado");Conferir(previa.Hashes,Capturar(origem));
        if(!preferencias) doc=doc with {Componentes=doc.Componentes.Where(p=>p.Key is "Biblioteca" or "Colecoes").ToDictionary()};
        var envelope=Path.Combine(seguranca,"importacao.json");
        ArquivosBackup.Gravar(envelope,JsonSerializer.SerializeToUtf8Bytes(doc,ArquivosBackup.Opcoes));
        ValidadorBackup.Ler(ArquivosBackup.Ler(envelope)!);
        var restauracao=_destino.Prever(envelope);
        // O estado vazio e seus hashes são conferidos novamente pelo serviço existente, sob o lock do destino.
        if(ResolvedorDiretorios.TemDados(destino)) throw new IOException("O destino mudou. Operação cancelada.");
        _destino.Etapa=e=>{ Etapa?.Invoke(e); if(e=="antes-confirmacao") Conferir(previa.Hashes,Capturar(origem)); };
        _destino.Restaurar(restauracao,preferencias,true);
        var lidos=Documento(destino,Capturar(destino));ValidadorBackup.Validar(lidos);
        foreach(var (nome,c) in doc.Componentes)
            if(c.Sha256!=lidos.Componentes[nome].Sha256) throw new IOException("A releitura da migração falhou. Preserve os backups e reabra o aplicativo.");
        Conferir(previa.Hashes,Capturar(origem));
        return seguranca;
    }
}
