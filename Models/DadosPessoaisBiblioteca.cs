namespace BacklogManager.Models;

public sealed record DadosPessoaisBiblioteca
{
    // Categoria ainda é o valor escolhido pelo usuário; não vira gênero da API.
    public string Categoria { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public double Nota { get; init; }
    public string Review { get; init; } = string.Empty;
}
