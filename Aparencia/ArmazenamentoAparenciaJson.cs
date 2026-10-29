using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using BacklogManager.Data;

namespace BacklogManager.Aparencia;

// Arquivo próprio: restaurar aparência nunca grava biblioteca, coleções ou a preferência de grade/lista.
public sealed class ArmazenamentoAparenciaJson : IArmazenamentoAparencia
{
    public string CaminhoArquivo { get; }
    private JsonObject _documento = new();
    private string? _hash;
    private bool _carregado;
    private bool _versaoFutura;
    public ArmazenamentoAparenciaJson(string caminho) => CaminhoArquivo = Path.GetFullPath(caminho);
    public LeituraAparencia Carregar()
    {
        _carregado = false;
        var bytes = Ler(); _hash = Hash(bytes); _documento = new(); _versaoFutura = false;
        string? aviso = null;
        if (bytes != null)
        {
            try
            {
                using var memoria = new MemoryStream(bytes);
                using var leitor = new StreamReader(memoria, System.Text.Encoding.UTF8, true);
                var texto = leitor.ReadToEnd();
                using var json = JsonDocument.Parse(texto);
                ValidarPropriedades(json.RootElement);
                _documento = JsonNode.Parse(texto) as JsonObject ?? throw new JsonException();
                _versaoFutura = _documento["Versao"] is JsonValue v && v.TryGetValue<int>(out var versao) && versao > 1;
                if (_versaoFutura) aviso = "A aparência foi criada por uma versão mais recente. Usando valores conhecidos; gravação bloqueada para preservar o arquivo.";
            }
            catch (Exception e) when (e is JsonException or ArgumentException or InvalidOperationException)
            { _documento = new(); aviso = "Preferência de aparência inválida. Usando padrões; uma alteração explícita guardará o original em .bak."; }
        }
        var tema = LerEnum(_documento["Tema"], TemaVisual.Moderno);
        var modo = LerEnum(_documento["Modo"], ModoVisual.Claro);
        var conjuntos = new Dictionary<string, IReadOnlyDictionary<ElementoCor, string>>();
        if (_documento["Personalizacoes"] is JsonObject salvas)
            foreach (var t in Enum.GetValues<TemaVisual>())
                foreach (var m in Enum.GetValues<ModoVisual>())
                {
                    var chave = PreferenciasAparencia.Chave(t, m);
                    if (salvas[chave] is not JsonObject cores) continue;
                    var validas = new Dictionary<ElementoCor, string>();
                    foreach (var e in Enum.GetValues<ElementoCor>())
                        if (cores[e.ToString()] is JsonValue valor && valor.TryGetValue<string>(out var texto) && CoresAparencia.TentarNormalizar(texto, out var cor)) validas[e] = cor;
                    if (validas.Count > 0) conjuntos[chave] = new ReadOnlyDictionary<ElementoCor, string>(validas);
                }
        _carregado = true;
        return new(new PreferenciasAparencia { Tema = tema, Modo = modo, Personalizacoes = new ReadOnlyDictionary<string, IReadOnlyDictionary<ElementoCor, string>>(conjuntos) }, aviso);
    }
    public void Salvar(PreferenciasAparencia preferencias)
    {
        if (!_carregado || _versaoFutura) throw new InvalidOperationException("Reabra as preferências numa versão compatível antes de salvar.");
        if (!Enum.IsDefined(preferencias.Tema) || !Enum.IsDefined(preferencias.Modo)) throw new InvalidDataException("Tema ou modo inválido.");
        var candidato = (JsonObject)_documento.DeepClone();
        candidato["Versao"] = 1; candidato["Tema"] = preferencias.Tema.ToString(); candidato["Modo"] = preferencias.Modo.ToString();
        var conjuntos = candidato["Personalizacoes"] as JsonObject ?? new JsonObject();
        foreach (var t in Enum.GetValues<TemaVisual>())
            foreach (var m in Enum.GetValues<ModoVisual>())
            {
                var chave = PreferenciasAparencia.Chave(t, m);
                var cores = conjuntos[chave] as JsonObject ?? new JsonObject();
                foreach (var e in Enum.GetValues<ElementoCor>()) cores.Remove(e.ToString());
                if (preferencias.Personalizacoes.TryGetValue(chave, out var escolhas))
                    foreach (var (e, cor) in escolhas)
                    {
                        if (!Enum.IsDefined(e)) throw new InvalidDataException("Elemento de aparência desconhecido.");
                        cores[e.ToString()] = CoresAparencia.Normalizar(cor);
                    }
                if (cores.Count > 0) conjuntos[chave] = cores.Parent == null ? cores : cores.DeepClone();
                else conjuntos.Remove(chave);
            }
        candidato["Personalizacoes"] = conjuntos.Parent == null ? conjuntos : conjuntos.DeepClone();
        var bytes = JsonSerializer.SerializeToUtf8Bytes(candidato, new JsonSerializerOptions { WriteIndented = true });
        using var bloqueio = new FileStream(CaminhoArquivo + ".lock", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        BacklogManager.Backup.CaminhosDadosLocais.VerificarGravacao(CaminhoArquivo);
        if (_hash != Hash(Ler())) throw new ArquivoAlteradoException();
        var temporario = CaminhoArquivo + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            using (var arquivo = new FileStream(temporario, FileMode.CreateNew, FileAccess.Write, FileShare.None)) { arquivo.Write(bytes); arquivo.Flush(true); }
            if (_hash == null) File.Move(temporario, CaminhoArquivo, false);
            else File.Replace(temporario, CaminhoArquivo, CaminhoArquivo + ".bak");
            _hash = Hash(bytes); _documento = candidato;
        }
        finally { try { File.Delete(temporario); } catch (IOException) { } catch (UnauthorizedAccessException) { } }
    }
    private static void ValidarPropriedades(JsonElement elemento)
    {
        // JsonObject materializa dicionários sob demanda. Detectamos ambiguidades antes de acessar cores.
        if (elemento.ValueKind == JsonValueKind.Object)
        {
            var nomes = new HashSet<string>(StringComparer.Ordinal);
            foreach (var propriedade in elemento.EnumerateObject())
            {
                if (!nomes.Add(propriedade.Name)) throw new JsonException("Propriedades repetidas nas preferências.");
                ValidarPropriedades(propriedade.Value);
            }
        }
        else if (elemento.ValueKind == JsonValueKind.Array)
            foreach (var filho in elemento.EnumerateArray()) ValidarPropriedades(filho);
    }

    private byte[]? Ler()
    {
        try { return File.ReadAllBytes(CaminhoArquivo); }
        catch (FileNotFoundException) { return null; }
        catch (DirectoryNotFoundException) { return null; }
    }
    private static string? Hash(byte[]? bytes) => bytes == null ? null : Convert.ToHexString(SHA256.HashData(bytes));
    private static T LerEnum<T>(JsonNode? no, T padrao) where T : struct, Enum
        => no is JsonValue v && v.TryGetValue<string>(out var texto) && Enum.TryParse<T>(texto, out var valor) && Enum.IsDefined(valor) ? valor : padrao;
}
