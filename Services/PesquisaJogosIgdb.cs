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
            IdentificadorExterno = jogo.Id.ToString(CultureInfo.InvariantCulture),
            CapaUrl = Capa(jogo.Cover),
            Generos = Nomes(jogo.Genres),
            Plataformas = Nomes(jogo.Platforms)
        }).ToArray();
    }

    private static string[] Nomes(IgdbNome[]? nomes) => nomes?
        .Where(n => !string.IsNullOrWhiteSpace(n?.Name)).Select(n => n.Name!.Trim())
        .Distinct(StringComparer.OrdinalIgnoreCase).ToArray() ?? Array.Empty<string>();

    private static string? Capa(IgdbCover? capa)
    {
        var id = capa?.ImageId;
        if (!string.IsNullOrEmpty(id) && id.Length <= 100 && id.All(c => char.IsAsciiLetterOrDigit(c) || c == '_'))
            return $"https://images.igdb.com/igdb/image/upload/t_cover_big/{id}.jpg";
        return CarregadorCapas.EnderecoRemoto(capa?.Url)?.AbsoluteUri;
    }
}
