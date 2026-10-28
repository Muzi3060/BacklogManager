using System.Collections.Generic;
using System;
using System.Text.Json;

namespace BacklogManager.Models;

public sealed record ItemBiblioteca(ObraCatalogo Catalogo, DadosPessoaisBiblioteca Pessoal)
{
    // Ausente nos registros antigos: a data original não pode ser adivinhada.
    public DateTimeOffset? DataAdicao { get; init; }
    public bool TemProgresso => Pessoal.Progresso is { Vazio: false };
    public string ProgressoExibido => TextosMidia.Progresso(Catalogo.Tipo, Pessoal.Progresso);
    public string StatusExibido => TextosMidia.Status(Catalogo.Tipo, Pessoal.Status);
    public string GenerosExibidos => Catalogo.Generos.Count > 0 ? Catalogo.GenerosTexto :
        string.IsNullOrWhiteSpace(Pessoal.Categoria) ? "Gêneros não informados" : $"Categoria legada: {Pessoal.Categoria}";

    // Metadados de compatibilidade: não são usados para identificar uma obra no catálogo.
    public int? SteamAppIdLegado { get; init; }
    public IReadOnlyDictionary<string, JsonElement>? DadosAdicionais { get; init; }
}
