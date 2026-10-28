using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BacklogManager.Contracts;
using BacklogManager.Models;

namespace BacklogManager.Services;

public sealed class PesquisaJogosIgdb : IPesquisaCatalogo
{
    private readonly IgdbService _servico;
    public PesquisaJogosIgdb(IgdbService servico)
    {
        ArgumentNullException.ThrowIfNull(servico);
        _servico = servico;
    }

    public async Task<IReadOnlyList<ObraCatalogo>> BuscarAsync(string termo, CancellationToken cancellationToken = default)
    {
        var jogos = await _servico.BuscarJogosAsync(termo, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        return jogos.Select(jogo => new ObraCatalogo
        {
            Tipo = TipoMidia.Jogo,
            Titulo = jogo.Name,
            Origem = "IGDB",
            IdentificadorExterno = jogo.Id.ToString(CultureInfo.InvariantCulture)
        }).ToArray();
    }
}
