using System.Text.Json.Serialization;

namespace BacklogManager.Models;

// Bloco opcional do formato existente. Versões/campos desconhecidos bloqueiam gravação.
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed class DadosMultimidiaJson
{
    public required int Versao { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter<TipoMidia>))]
    public required TipoMidia Tipo { get; init; }
    public string? Origem { get; init; }
    public string? IdentificadorExterno { get; init; }
    public string? CapaUrl { get; init; }
    public string[] Generos { get; init; } = System.Array.Empty<string>();
    public bool Simulado { get; init; }
    public ProgressoPessoal? Progresso { get; init; }
}
