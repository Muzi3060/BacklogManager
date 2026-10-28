using System;
using System.Collections.Generic;
using BacklogManager.Contracts;
using BacklogManager.Models;

namespace BacklogManager.Services;

public sealed class RoteadorPesquisaCatalogo
{
    private readonly IReadOnlyDictionary<TipoMidia, IPesquisaCatalogo?> _servicos;
    public RoteadorPesquisaCatalogo(IPesquisaCatalogo? jogos)
    {
        var servicos = new Dictionary<TipoMidia, IPesquisaCatalogo?> { [TipoMidia.Jogo] = jogos };
        foreach (var tipo in Enum.GetValues<TipoMidia>())
            if (tipo != TipoMidia.Jogo) servicos[tipo] = new CatalogoSimulado(tipo);
        _servicos = servicos;
    }
    public IPesquisaCatalogo? Obter(TipoMidia tipo) => _servicos.TryGetValue(tipo, out var servico)
        ? servico : throw new ArgumentOutOfRangeException(nameof(tipo));
}
