using System.Collections.Generic;
using BacklogManager.Models;

namespace BacklogManager.Contracts;

public interface IArmazenamentoBiblioteca
{
    string DescricaoOrigem { get; }
    IReadOnlyList<ItemBiblioteca> Carregar();
    void Salvar(IReadOnlyList<ItemBiblioteca> itens);
}
