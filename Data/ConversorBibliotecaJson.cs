using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using BacklogManager.Models;

namespace BacklogManager.Data;

internal static class ConversorBibliotecaJson
{
    internal static ItemBiblioteca ParaModelo(BacklogItem registro)
    {
        ArgumentNullException.ThrowIfNull(registro);
        return new ItemBiblioteca(new ObraCatalogo
        {
            Titulo = registro.Titulo,
            Origem = registro.IgdbId.HasValue ? "IGDB" : null,
            IdentificadorExterno = registro.IgdbId?.ToString(CultureInfo.InvariantCulture)
        }, new DadosPessoaisBiblioteca
        {
            Categoria = registro.Categoria,
            Status = registro.Status,
            Nota = registro.Nota,
            Review = registro.Review
        })
        {
            SteamAppIdLegado = registro.SteamAppId,
            DadosAdicionais = registro.DadosAdicionais == null ? null :
                new ReadOnlyDictionary<string, JsonElement>(CopiarExtras(registro.DadosAdicionais)!)
        };
    }

    internal static BacklogItem ParaRegistro(ItemBiblioteca item)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(item.Catalogo);
        ArgumentNullException.ThrowIfNull(item.Pessoal);
        var catalogo = item.Catalogo;
        // O formato pessoal atual não comporta novas mídias/capas/gêneros.
        // Recusar é mais seguro do que aceitar e perder esses dados silenciosamente.
        if (catalogo.Tipo != TipoMidia.Jogo || catalogo.CapaUrl != null ||
            catalogo.Generos == null || catalogo.Generos.Count != 0)
            throw new NotSupportedException("O JSON atual suporta somente os dados de jogos já disponíveis. Nenhuma migração foi realizada.");

        int? igdbId = null;
        if (catalogo.Origem != null || catalogo.IdentificadorExterno != null)
        {
            if (catalogo.Origem != "IGDB" || !int.TryParse(catalogo.IdentificadorExterno, NumberStyles.None,
                    CultureInfo.InvariantCulture, out var id) || id <= 0 ||
                id.ToString(CultureInfo.InvariantCulture) != catalogo.IdentificadorExterno)
                throw new NotSupportedException("A referência de catálogo não cabe no formato JSON atual.");
            igdbId = id;
        }
        return new BacklogItem(catalogo.Titulo, item.Pessoal.Categoria, item.Pessoal.Status,
            item.Pessoal.Nota, item.Pessoal.Review, item.SteamAppIdLegado)
        {
            IgdbId = igdbId,
            DadosAdicionais = CopiarExtras(item.DadosAdicionais)
        };
    }

    private static Dictionary<string, JsonElement>? CopiarExtras(IReadOnlyDictionary<string, JsonElement>? extras) =>
        extras?.ToDictionary(par => par.Key, par => par.Value.Clone());
}
