using System;
using System.Collections.Generic;
using System.Linq;
using BacklogManager.Contracts;
using BacklogManager.Models;

namespace BacklogManager.Data;

public sealed class ArmazenamentoJsonBiblioteca : IArmazenamentoBiblioteca
{
    private readonly FileManager _arquivos;
    public string DescricaoOrigem => _arquivos.CaminhoArquivo;

    public ArmazenamentoJsonBiblioteca(FileManager arquivos)
    {
        ArgumentNullException.ThrowIfNull(arquivos);
        _arquivos = arquivos;
    }

    public IReadOnlyList<ItemBiblioteca> Carregar() =>
        _arquivos.Carregar().Select(ConversorBibliotecaJson.ParaModelo).ToArray();

    public void Salvar(IReadOnlyList<ItemBiblioteca> itens)
    {
        ArgumentNullException.ThrowIfNull(itens);
        var registros = itens.Select(ConversorBibliotecaJson.ParaRegistro).ToArray();
        _arquivos.Salvar(registros);
    }
}
