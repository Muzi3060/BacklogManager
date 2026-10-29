using System;
using System.IO;
using System.Text.Json;
using BacklogManager.Models;

namespace BacklogManager.Data;

// Preferência independente da biblioteca: uma falha aqui nunca bloqueia o CRUD.
public sealed class ArmazenamentoPreferenciasBiblioteca
{
    public string? CaminhoArquivo { get; }
    private VisualizacaoBiblioteca _memoria;
    public ArmazenamentoPreferenciasBiblioteca(string? caminhoArquivo = null) =>
        CaminhoArquivo = caminhoArquivo == null ? null : Path.GetFullPath(caminhoArquivo);

    public VisualizacaoBiblioteca Carregar()
    {
        if (CaminhoArquivo == null) return _memoria;
        byte[] conteudo;
        try { conteudo = File.ReadAllBytes(CaminhoArquivo); }
        catch (FileNotFoundException) { return VisualizacaoBiblioteca.Grade; }
        var dados = JsonSerializer.Deserialize<Preferencias>(conteudo);
        if (dados is not { Versao: 1 } || !Enum.IsDefined(dados.Visualizacao))
            throw new InvalidDataException("Preferência de visualização inválida.");
        return dados.Visualizacao;
    }

    public void Salvar(VisualizacaoBiblioteca visualizacao)
    {
        if (!Enum.IsDefined(visualizacao)) throw new ArgumentOutOfRangeException(nameof(visualizacao));
        if (CaminhoArquivo == null) { _memoria = visualizacao; return; }
        using var bloqueio = new FileStream(CaminhoArquivo + ".lock", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        BacklogManager.Backup.CaminhosDadosLocais.VerificarGravacao(CaminhoArquivo);
        var temporario = CaminhoArquivo + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            using (var arquivo = new FileStream(temporario, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                JsonSerializer.Serialize(arquivo, new Preferencias { Versao = 1, Visualizacao = visualizacao });
                arquivo.Flush(true);
            }
            File.Move(temporario, CaminhoArquivo, true);
        }
        finally { if (File.Exists(temporario)) File.Delete(temporario); }
    }

    private sealed class Preferencias
    {
        public required int Versao { get; init; }
        public required VisualizacaoBiblioteca Visualizacao { get; init; }
    }
}
