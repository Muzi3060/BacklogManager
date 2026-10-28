using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using BacklogManager.Models;

namespace BacklogManager.Data;

internal static class ConversorBibliotecaJson
{
    internal static ItemBiblioteca ParaModelo(BacklogItem registro)
    {
        ArgumentNullException.ThrowIfNull(registro);
        if (registro.Multimidia != null) ValidarMultimidia(registro);
        var extra = registro.Multimidia;
        return new ItemBiblioteca(new ObraCatalogo
        {
            Tipo = extra?.Tipo ?? TipoMidia.Jogo,
            Titulo = registro.Titulo,
            Origem = extra?.Origem ?? (registro.IgdbId.HasValue ? "IGDB" : null),
            IdentificadorExterno = extra?.IdentificadorExterno ?? registro.IgdbId?.ToString(CultureInfo.InvariantCulture),
            CapaUrl = extra?.CapaUrl,
            Generos = extra?.Generos.ToArray() ?? Array.Empty<string>(),
            Simulado = extra?.Simulado ?? false
        }, new DadosPessoaisBiblioteca
        {
            Categoria = registro.Categoria,
            Status = registro.Status,
            Nota = registro.Nota,
            Review = registro.Review,
            Progresso = extra?.Progresso
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
        if (!Enum.IsDefined(catalogo.Tipo) || catalogo.Generos == null || catalogo.Generos.Any(string.IsNullOrWhiteSpace))
            throw new NotSupportedException("Tipo de mídia ou gêneros inválidos.");
        RegrasProgresso.Validar(catalogo.Tipo, item.Pessoal.Progresso);
        ValidarReferencia(catalogo.Tipo, catalogo.Origem, catalogo.IdentificadorExterno, catalogo.Simulado);

        int? igdbId = catalogo.Tipo == TipoMidia.Jogo && catalogo.IdentificadorExterno != null
            ? int.Parse(catalogo.IdentificadorExterno, CultureInfo.InvariantCulture) : null;
        var precisaBloco = catalogo.Tipo != TipoMidia.Jogo || catalogo.CapaUrl != null || catalogo.Generos.Count > 0 ||
            item.Pessoal.Progresso is { Vazio: false };
        return new BacklogItem(catalogo.Titulo, item.Pessoal.Categoria, item.Pessoal.Status,
            item.Pessoal.Nota, item.Pessoal.Review, item.SteamAppIdLegado)
        {
            IgdbId = igdbId,
            DadosAdicionais = CopiarExtras(item.DadosAdicionais),
            Multimidia = !precisaBloco ? null : new DadosMultimidiaJson
            {
                Versao = 1,
                Tipo = catalogo.Tipo,
                Origem = catalogo.Origem,
                IdentificadorExterno = catalogo.IdentificadorExterno,
                CapaUrl = catalogo.CapaUrl,
                Generos = catalogo.Generos.ToArray(),
                Simulado = catalogo.Simulado,
                Progresso = item.Pessoal.Progresso is { Vazio: false } ? item.Pessoal.Progresso : null
            }
        };
    }

    internal static void ValidarMultimidia(BacklogItem registro)
    {
        var dados = registro.Multimidia!;
        if (dados.Versao != 1 || !Enum.IsDefined(dados.Tipo) || dados.Generos == null || dados.Generos.Any(string.IsNullOrWhiteSpace))
            throw new InvalidDataException("Formato multimídia desconhecido ou inválido. O original foi preservado.");
        try { ValidarReferencia(dados.Tipo, dados.Origem, dados.IdentificadorExterno, dados.Simulado); }
        catch (NotSupportedException) { throw new InvalidDataException("Referência de catálogo multimídia inválida."); }
        var idEsperado = dados.Tipo == TipoMidia.Jogo && dados.IdentificadorExterno != null
            ? int.Parse(dados.IdentificadorExterno, CultureInfo.InvariantCulture) : (int?)null;
        if (registro.IgdbId != idEsperado)
            throw new InvalidDataException("Identificadores conflitantes no registro multimídia. Nenhum foi convertido.");
        RegrasProgresso.Validar(dados.Tipo, dados.Progresso);
    }

    private static void ValidarReferencia(TipoMidia tipo, string? origem, string? id, bool simulado)
    {
        if ((origem == null) != (id == null) || origem != null && (string.IsNullOrWhiteSpace(origem) || string.IsNullOrWhiteSpace(id)))
            throw new NotSupportedException("A referência de catálogo está incompleta.");
        if (tipo == TipoMidia.Jogo && (simulado || origem != null &&
            (origem != "IGDB" || !int.TryParse(id, NumberStyles.None, CultureInfo.InvariantCulture, out var numero) ||
             numero <= 0 || numero.ToString(CultureInfo.InvariantCulture) != id)))
            throw new NotSupportedException("Jogos precisam de uma referência IGDB válida ou de um registro legado sem origem confirmada.");
        if (simulado && (origem != "Simulado" || id == null) || origem == "Simulado" && !simulado)
            throw new NotSupportedException("A origem simulada precisa estar explicitamente identificada.");
    }

    private static Dictionary<string, JsonElement>? CopiarExtras(IReadOnlyDictionary<string, JsonElement>? extras) =>
        extras?.ToDictionary(par => par.Key, par => par.Value.Clone());
}
