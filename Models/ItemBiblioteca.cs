using System.Collections.Generic;
using System.Text.Json;

namespace BacklogManager.Models;

public sealed record ItemBiblioteca(ObraCatalogo Catalogo, DadosPessoaisBiblioteca Pessoal)
{
    public bool TemProgresso => Pessoal.Progresso is { Vazio: false };
    public string ProgressoExibido => TextosMidia.Progresso(Catalogo.Tipo, Pessoal.Progresso);
    public string StatusExibido => TextosMidia.Status(Catalogo.Tipo, Pessoal.Status);

    // Metadados de compatibilidade: não são usados para identificar uma obra no catálogo.
    public int? SteamAppIdLegado { get; init; }
    public IReadOnlyDictionary<string, JsonElement>? DadosAdicionais { get; init; }
}
