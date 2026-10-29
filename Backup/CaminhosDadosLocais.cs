using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace BacklogManager.Backup;

public sealed class CaminhosDadosLocais
{
    public IReadOnlyDictionary<string, string> Arquivos { get; }
    public string Pasta { get; }
    public string Diario => Path.Combine(Pasta, ".mr-restauracao.json");
    public string Trava => Path.Combine(Pasta, ".mr-backup.lock");
    public CaminhosDadosLocais(string biblioteca)
    {
        biblioteca = Path.GetFullPath(biblioteca);
        Pasta = Path.GetDirectoryName(biblioteca)!;
        var visualizacao = Path.Combine(Pasta, "biblioteca.preferencias.json");
        if (visualizacao == biblioteca) visualizacao += ".visualizacao.json";
        var aparencia = Path.Combine(Pasta, "aparencia.preferencias.json");
        if (aparencia == biblioteca) aparencia += ".visual.json";
        Arquivos = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>
        {
            ["Biblioteca"] = biblioteca,
            ["Colecoes"] = Path.Combine(Pasta, Path.GetFileNameWithoutExtension(biblioteca) + ".colecoes.json"),
            ["Visualizacao"] = visualizacao, ["Aparencia"] = aparencia
        });
        if (Arquivos.Values.Distinct(StringComparer.OrdinalIgnoreCase).Count() != 4)
            throw new InvalidDataException("Os arquivos locais não podem compartilhar caminhos.");
        if (Arquivos.Values.Any(p => p.Equals(Diario, StringComparison.OrdinalIgnoreCase) ||
            p.Equals(Trava, StringComparison.OrdinalIgnoreCase) || Arquivos.Values.Any(a => p.Equals(a + ".lock", StringComparison.OrdinalIgnoreCase))))
            throw new InvalidDataException("O caminho de dados coincide com um arquivo de coordenação reservado.");
    }
    internal static void SemLinks(string caminho)
    {
        for (var p = Path.GetFullPath(caminho); !string.IsNullOrEmpty(p); p = Path.GetDirectoryName(p))
            if ((File.Exists(p) || Directory.Exists(p)) && (File.GetAttributes(p) & FileAttributes.ReparsePoint) != 0)
                throw new IOException("Escolha um caminho sem links simbólicos para esta operação.");
    }
    public void ValidarDestino(string destino)
    {
        destino = Path.GetFullPath(destino);
        SemLinks(destino);
        foreach (var p in Arquivos.Values.Append(Diario).Append(Trava))
            if (destino.Equals(p, StringComparison.OrdinalIgnoreCase) ||
                destino.StartsWith(p + ".", StringComparison.OrdinalIgnoreCase))
                throw new IOException("O backup não pode substituir arquivos usados pelo aplicativo.");
    }
    public static void VerificarGravacao(string arquivo)
    {
        if (File.Exists(Path.Combine(Path.GetDirectoryName(Path.GetFullPath(arquivo))!, ".mr-restauracao.json")))
            throw new InvalidOperationException("Há recuperação pendente. Reabra Configurações antes de gravar.");
    }
}
