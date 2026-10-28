using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BacklogManager.Contracts;
using BacklogManager.Models;

namespace BacklogManager.Services;

public sealed class ServicoColecoes
{
    private readonly IArmazenamentoColecoes _armazenamento;
    private readonly ServicoBiblioteca _biblioteca;
    private IReadOnlyList<ColecaoPersonalizada> _colecoes = Array.Empty<ColecaoPersonalizada>();
    public IReadOnlyList<ColecaoPersonalizada> Colecoes => _colecoes;
    public bool Carregadas { get; private set; }
    public string DescricaoOrigem => _armazenamento.DescricaoOrigem;

    public ServicoColecoes(IArmazenamentoColecoes armazenamento, ServicoBiblioteca biblioteca)
    {
        ArgumentNullException.ThrowIfNull(armazenamento);
        ArgumentNullException.ThrowIfNull(biblioteca);
        _armazenamento = armazenamento;
        _biblioteca = biblioteca;
    }

    public void Carregar()
    {
        Carregadas = false;
        var candidatas = _armazenamento.Carregar();
        try { RegrasColecoes.Validar(candidatas); }
        catch (ArgumentException erro) { throw new InvalidDataException("Dados de coleções inválidos. O arquivo original foi preservado.", erro); }
        _colecoes = Copiar(candidatas);
        Carregadas = true;
    }

    public Guid Criar(string nome, string? descricao = null)
    {
        GarantirDisponivel();
        var nova = new ColecaoPersonalizada(Guid.NewGuid(), NormalizarNome(nome), NormalizarDescricao(descricao), Array.Empty<Guid>());
        Confirmar(_colecoes.Append(nova).ToArray());
        return nova.Id;
    }

    public void Editar(Guid id, string nome, string? descricao = null)
    {
        var atual = Encontrar(id);
        var editada = atual with { Nome = NormalizarNome(nome), Descricao = NormalizarDescricao(descricao) };
        Confirmar(_colecoes.Select(c => c.Id == id ? editada : c).ToArray());
    }

    public void Excluir(Guid id)
    {
        Encontrar(id);
        // Somente o arquivo de coleções muda. A biblioteca e suas obras são independentes.
        Confirmar(_colecoes.Where(c => c.Id != id).ToArray());
    }

    public bool AdicionarObra(Guid colecaoId, ItemBiblioteca obra)
    {
        var colecao = Encontrar(colecaoId);
        var id = _biblioteca.GarantirIdentidade(obra);
        if (colecao.Obras.Contains(id)) return false;
        var atualizada = colecao with { Obras = Array.AsReadOnly(colecao.Obras.Append(id).ToArray()) };
        Confirmar(_colecoes.Select(c => c.Id == colecaoId ? atualizada : c).ToArray());
        return true;
    }

    public bool RemoverObra(Guid colecaoId, Guid obraId)
    {
        var colecao = Encontrar(colecaoId);
        if (!colecao.Obras.Contains(obraId)) return false;
        var atualizada = colecao with { Obras = Array.AsReadOnly(colecao.Obras.Where(id => id != obraId).ToArray()) };
        Confirmar(_colecoes.Select(c => c.Id == colecaoId ? atualizada : c).ToArray());
        return true;
    }

    public IReadOnlyList<ItemBiblioteca> ObterObras(Guid colecaoId)
    {
        var colecao = Encontrar(colecaoId);
        var porId = _biblioteca.Itens.Where(i => i.IdInterno != null).ToDictionary(i => i.IdInterno!.Value);
        // IDs de obras excluídas não aparecem nem são reassociados por título/ID de catálogo.
        return colecao.Obras.Where(porId.ContainsKey).Select(id => porId[id]).ToArray();
    }

    public int ContarReferenciasAusentes(Guid colecaoId) => Encontrar(colecaoId).Obras.Count - ObterObras(colecaoId).Count;

    private ColecaoPersonalizada Encontrar(Guid id)
    {
        GarantirDisponivel();
        return _colecoes.FirstOrDefault(c => c.Id == id) ?? throw new ArgumentException("Selecione uma coleção existente.");
    }

    private void GarantirDisponivel()
    {
        if (!Carregadas || !_biblioteca.Carregada)
            throw new InvalidOperationException("Carregue a biblioteca e as coleções com sucesso antes de alterar associações.");
    }

    private void Confirmar(IReadOnlyList<ColecaoPersonalizada> candidatas)
    {
        RegrasColecoes.Validar(candidatas);
        var copia = Copiar(candidatas);
        _armazenamento.Salvar(copia);
        _colecoes = copia;
    }

    private static IReadOnlyList<ColecaoPersonalizada> Copiar(IReadOnlyList<ColecaoPersonalizada> colecoes) =>
        Array.AsReadOnly(colecoes.Select(c => c with { Obras = Array.AsReadOnly(c.Obras.ToArray()) }).ToArray());

    private static string NormalizarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome) || nome.Trim().Length > 120 || nome.Any(char.IsControl))
            throw new ArgumentException("Use um nome de coleção entre 1 e 120 caracteres, sem quebras de linha.");
        return nome.Trim();
    }

    private static string NormalizarDescricao(string? descricao)
    {
        var texto = descricao?.Trim() ?? string.Empty;
        if (texto.Length > 1000) throw new ArgumentException("Use até 1000 caracteres na descrição da coleção.");
        return texto;
    }
}

internal static class RegrasColecoes
{
    internal static void Validar(IReadOnlyList<ColecaoPersonalizada> colecoes)
    {
        ArgumentNullException.ThrowIfNull(colecoes);
        var ids = new HashSet<Guid>();
        var nomes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var c in colecoes)
        {
            if (c == null || c.Id == Guid.Empty || !ids.Add(c.Id) || string.IsNullOrWhiteSpace(c.Nome) ||
                c.Nome.Trim().Length > 120 || c.Nome.Any(char.IsControl) || c.Descricao == null || c.Descricao.Length > 1000 ||
                c.Obras == null || c.Obras.Any(id => id == Guid.Empty) || c.Obras.Distinct().Count() != c.Obras.Count)
                throw new InvalidDataException("As coleções contêm dados inválidos. Nenhum dado foi descartado.");
            if (!nomes.Add(c.Nome.Trim())) throw new ArgumentException("Já existe uma coleção com esse nome.");
        }
    }
}
