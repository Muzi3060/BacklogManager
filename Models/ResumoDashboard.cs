using System;
using System.Collections.Generic;

namespace BacklogManager.Models;

public sealed record DistribuicaoDashboard(string Nome, int Quantidade, double Percentual);
public sealed record EntradaDashboard(ItemBiblioteca Obra, string Data, string Progresso)
{
    public string Midia => TextosMidia.Plural(Obra.Catalogo.Tipo);
}
public sealed record ResumoDashboard(
    int Total, int EmAndamento, int Concluidas, int NaFila, int SemData, int DatasFuturas,
    int Adicionadas, double PercentualConcluidas, DateTimeOffset Agora, DateTime InicioLocal, DateTime FimLocal,
    IReadOnlyList<DistribuicaoDashboard> PorMidia, IReadOnlyList<DistribuicaoDashboard> PorStatus,
    IReadOnlyList<EntradaDashboard> Atividade, IReadOnlyList<EntradaDashboard> Continuar,
    IReadOnlyList<ItemBiblioteca> Legados);
