using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BacklogManager.Models;

// Registro do formato JSON existente. O domínio e a interface usam ItemBiblioteca.
public class BacklogItem
{
    public string Titulo { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public double Nota { get; set; }

    public string Review { get; set; } = string.Empty;

    public int? SteamAppId { get; set; }

    // IDs legados não têm origem confiável; somente novas seleções preenchem IgdbId.
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? IgdbId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DadosMultimidiaJson? Multimidia { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? DadosAdicionais { get; set; }

    public BacklogItem()
    {

    }

    public BacklogItem(string titulo, string categoria, string status, double nota, string review, int? steamAppID)
    {
        Titulo = titulo;
        Categoria = categoria;
        Status = status;
        Nota = nota;
        Review = review;
        SteamAppId = steamAppID;
    }

    public override string ToString()
    {
        return $"{Titulo} - {Categoria} - {Status} - {Nota} - {Review}";
    }
}
