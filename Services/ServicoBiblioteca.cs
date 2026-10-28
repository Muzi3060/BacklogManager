using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BacklogManager.Contracts;
using BacklogManager.Models;

namespace BacklogManager.Services;

public sealed class ServicoBiblioteca
{
    private readonly IArmazenamentoBiblioteca _armazenamento;
    private IReadOnlyList<ItemBiblioteca> _itens = Array.Empty<ItemBiblioteca>();
    public IReadOnlyList<ItemBiblioteca> Itens => _itens;
    public bool Carregada { get; private set; }
    public string DescricaoOrigem => _armazenamento.DescricaoOrigem;

    public ServicoBiblioteca(IArmazenamentoBiblioteca armazenamento)
    {
        ArgumentNullException.ThrowIfNull(armazenamento);
        _armazenamento = armazenamento;
    }

    public void Carregar()
    {
        Carregada = false;
        var candidatos = _armazenamento.Carregar().ToList();
        foreach (var item in candidatos) Validar(item);
        _itens = candidatos.AsReadOnly();
        Carregada = true;
    }

    public void Adicionar(ItemBiblioteca item)
    {
        Validar(item);
        var candidatos = _itens.ToList();
        candidatos.Add(item);
        Confirmar(candidatos);
    }

    public void Atualizar(ItemBiblioteca original, DadosPessoaisBiblioteca dadosPessoais)
    {
        var indice = Encontrar(original);
        ArgumentNullException.ThrowIfNull(dadosPessoais);
        var atualizado = original with { Pessoal = dadosPessoais };
        Validar(atualizado);
        var candidatos = _itens.ToList();
        candidatos[indice] = atualizado;
        Confirmar(candidatos);
    }

    public void Remover(ItemBiblioteca item)
    {
        var indice = Encontrar(item);
        var candidatos = _itens.ToList();
        candidatos.RemoveAt(indice);
        Confirmar(candidatos);
    }

    private int Encontrar(ItemBiblioteca item)
    {
        // Registros iguais continuam independentes: removemos a instância selecionada.
        for (var i = 0; i < _itens.Count; i++)
            if (ReferenceEquals(_itens[i], item)) return i;
        throw new InvalidOperationException("O item não pertence à coleção carregada.");
    }

    private void Confirmar(List<ItemBiblioteca> candidatos)
    {
        if (!Carregada)
            throw new InvalidOperationException("Carregue a coleção com sucesso antes de salvar.");
        _armazenamento.Salvar(candidatos.AsReadOnly());
        _itens = candidatos.AsReadOnly();
    }

    private static void Validar(ItemBiblioteca item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (item.Catalogo == null || item.Pessoal == null || !Enum.IsDefined(item.Catalogo.Tipo) ||
            string.IsNullOrWhiteSpace(item.Catalogo.Titulo) || item.Catalogo.Generos == null ||
            string.IsNullOrWhiteSpace(item.Pessoal.Categoria) || string.IsNullOrWhiteSpace(item.Pessoal.Status) ||
            item.Pessoal.Review == null || !double.IsFinite(item.Pessoal.Nota) || item.Pessoal.Nota < 0 || item.Pessoal.Nota > 5)
            throw new InvalidDataException("A coleção contém um registro inválido. Nenhum registro foi descartado.");
    }

    public IReadOnlyList<ItemBiblioteca> Filtrar(string? termo, string? status)
    {
        var busca = termo?.Trim() ?? string.Empty;
        return _itens.Where(item =>
            (status == null || item.Pessoal.Status == status) &&
            (item.Catalogo.Titulo.Contains(busca, StringComparison.OrdinalIgnoreCase) ||
             item.Pessoal.Categoria.Contains(busca, StringComparison.OrdinalIgnoreCase))).ToArray();
    }

    public IReadOnlyList<ItemBiblioteca> ObterRecentes() => _itens.Reverse().Take(3).ToArray();

    public ResumoBiblioteca ObterResumo()
    {
        var zerados = _itens.Count(item => item.Pessoal.Status == "Zerado");
        return new ResumoBiblioteca(_itens.Count, _itens.Count(item => item.Pessoal.Status == "Jogando"),
            zerados, _itens.Count(item => item.Pessoal.Status == "Na Fila"),
            _itens.Count == 0 ? 0 : (double)zerados / _itens.Count * 100);
    }
}

public sealed record ResumoBiblioteca(int Total, int Jogando, int Zerados, int NaFila, double Progresso);
