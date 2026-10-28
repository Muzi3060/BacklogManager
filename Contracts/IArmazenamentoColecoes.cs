using System.Collections.Generic;
using BacklogManager.Models;

namespace BacklogManager.Contracts;

public interface IArmazenamentoColecoes
{
    string DescricaoOrigem { get; }
    IReadOnlyList<ColecaoPersonalizada> Carregar();
    void Salvar(IReadOnlyList<ColecaoPersonalizada> colecoes);
}
