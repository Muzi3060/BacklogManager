using System;

namespace BacklogManager.Models;

public enum OrdenacaoBiblioteca { Insercao, TituloAscendente, TituloDescendente, MaiorNota, MenorNota, AdicaoRecente, AdicaoAntiga }
public enum VisualizacaoBiblioteca { Grade, Lista }

public sealed record ConsultaBiblioteca
{
    public string? Termo { get; init; }
    public TipoMidia? Tipo { get; init; }
    public string? Status { get; init; }
    public string? Genero { get; init; }
    public string? Plataforma { get; init; }
    public double? NotaMinima { get; init; }
    public DateOnly? AdicionadaDe { get; init; }
    public DateOnly? AdicionadaAte { get; init; }
    public OrdenacaoBiblioteca Ordenacao { get; init; }
}
