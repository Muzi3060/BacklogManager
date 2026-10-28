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
            string.IsNullOrWhiteSpace(item.Catalogo.Titulo) || item.Catalogo.Generos == null || item.Catalogo.Plataformas == null ||
            string.IsNullOrWhiteSpace(item.Pessoal.Categoria) || string.IsNullOrWhiteSpace(item.Pessoal.Status) ||
            item.Pessoal.Review == null || !double.IsFinite(item.Pessoal.Nota) || item.Pessoal.Nota < 0 || item.Pessoal.Nota > 5)
            throw new InvalidDataException("A coleção contém um registro inválido. Nenhum registro foi descartado.");
        RegrasProgresso.Validar(item.Catalogo.Tipo, item.Pessoal.Progresso);
    }

    public IReadOnlyList<ItemBiblioteca> Filtrar(string? termo, string? status, TipoMidia? tipo = null)
        => Consultar(new ConsultaBiblioteca { Termo = termo, Status = status, Tipo = tipo });

    public IReadOnlyList<ItemBiblioteca> Consultar(ConsultaBiblioteca consulta)
    {
        ArgumentNullException.ThrowIfNull(consulta);
        if (!Enum.IsDefined(consulta.Ordenacao) || consulta.Tipo is { } tipo && !Enum.IsDefined(tipo) ||
            consulta.NotaMinima is { } nota && (!double.IsFinite(nota) || nota < 0 || nota > 5) ||
            consulta.AdicionadaDe > consulta.AdicionadaAte)
            throw new ArgumentException("Filtros inválidos. Use nota de 0 a 5 e um intervalo de datas válido.", nameof(consulta));
        var busca = consulta.Termo?.Trim() ?? string.Empty;
        var itens = _itens.Where(item =>
            (consulta.Tipo == null || item.Catalogo.Tipo == consulta.Tipo) &&
            (consulta.Status == null || item.Pessoal.Status == consulta.Status) &&
            (item.Catalogo.Titulo.Contains(busca, StringComparison.OrdinalIgnoreCase) ||
             item.Pessoal.Categoria.Contains(busca, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrWhiteSpace(consulta.Genero) || GenerosParaFiltro(item).Contains(consulta.Genero, StringComparer.OrdinalIgnoreCase)) &&
            (string.IsNullOrWhiteSpace(consulta.Plataforma) || item.Catalogo.Tipo == TipoMidia.Jogo &&
                item.Catalogo.Plataformas.Contains(consulta.Plataforma, StringComparer.OrdinalIgnoreCase)) &&
            (consulta.NotaMinima == null || item.Pessoal.Nota >= consulta.NotaMinima) &&
            (consulta.AdicionadaDe == null || item.DataAdicao is { } de && DateOnly.FromDateTime(de.LocalDateTime) >= consulta.AdicionadaDe) &&
            (consulta.AdicionadaAte == null || item.DataAdicao is { } ate && DateOnly.FromDateTime(ate.LocalDateTime) <= consulta.AdicionadaAte));
        // OrderBy é estável: empates conservam a ordem e não descartam duplicatas.
        return (consulta.Ordenacao switch
        {
            OrdenacaoBiblioteca.TituloAscendente => itens.OrderBy(i => i.Catalogo.Titulo, StringComparer.CurrentCultureIgnoreCase),
            OrdenacaoBiblioteca.TituloDescendente => itens.OrderByDescending(i => i.Catalogo.Titulo, StringComparer.CurrentCultureIgnoreCase),
            OrdenacaoBiblioteca.MaiorNota => itens.OrderByDescending(i => i.Pessoal.Nota),
            OrdenacaoBiblioteca.MenorNota => itens.OrderBy(i => i.Pessoal.Nota),
            OrdenacaoBiblioteca.AdicaoRecente => itens.OrderBy(i => i.DataAdicao == null).ThenByDescending(i => i.DataAdicao),
            OrdenacaoBiblioteca.AdicaoAntiga => itens.OrderBy(i => i.DataAdicao == null).ThenBy(i => i.DataAdicao),
            _ => itens
        }).ToArray();
    }

    public static System.Collections.Generic.IEnumerable<string> GenerosParaFiltro(ItemBiblioteca item) =>
        item.Catalogo.Generos.Count > 0 ? item.Catalogo.Generos : new[] { item.Pessoal.Categoria };

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
