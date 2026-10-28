using System;
using System.Collections.Generic;

namespace BacklogManager.Models;

public sealed record ObraCatalogo
{
    public TipoMidia Tipo { get; init; } = TipoMidia.Jogo;
    public string Titulo { get; init; } = string.Empty;
    public string? Origem { get; init; }
    public string? IdentificadorExterno { get; init; }
    public bool Simulado { get; init; }
    public string GenerosTexto => string.Join(", ", Generos);
    public string? CapaUrl { get; init; }
    public IReadOnlyList<string> Generos { get; init; } = Array.Empty<string>();
}
