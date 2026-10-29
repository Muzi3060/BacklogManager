using Avalonia.Controls;
using BacklogManager.Models;
using BacklogManager.Services;

namespace BacklogManager;

public partial class MainWindow
{
    private ServicoDashboard _dashboard = new();
    private void AtualizarDashboard()
    {
        if (!_interfacePronta) return;
        var disponivel = PersistenciaDisponivel;
        ConteudoDashboard.IsVisible = disponivel;
        FalhaDashboard.IsVisible = !disponivel;
        if (!disponivel) { TxtContagemNav.Text = "—"; TxtTotal.Text = "—"; return; }
        var dias = CmbPeriodoDashboard.SelectedIndex == 0 ? 7 : 30;
        var resumo = _dashboard.Calcular(_biblioteca.Itens, dias);
        TxtTotal.Text = resumo.Total.ToString();
        TxtJogando.Text = resumo.EmAndamento.ToString();
        TxtZerados.Text = resumo.Concluidas.ToString();
        TxtNaFilaDashboard.Text = resumo.NaFila.ToString();
        TxtContagemNav.Text = resumo.Total.ToString();
        TxtProgresso.Text = $"{resumo.PercentualConcluidas:0}% concluído";
        ProgressoBacklog.Value = resumo.PercentualConcluidas;
        TxtResumo.Text = resumo.Total == 0
            ? "Adicione sua primeira obra e comece a acompanhar seu progresso."
            : $"{resumo.Concluidas} de {resumo.Total} obras concluídas · {resumo.NaFila} na fila.";
        DistribuicaoMidias.ItemsSource = resumo.PorMidia;
        DistribuicaoStatus.ItemsSource = resumo.PorStatus;
        TxtAdicionadas.Text = resumo.Adicionadas.ToString();
        TxtPeriodoDashboard.Text = $"De {resumo.InicioLocal:dd/MM/yyyy} até {resumo.FimLocal:dd/MM/yyyy} · {dias} dias, incluindo hoje.";
        ListaAtividade.ItemsSource = resumo.Atividade;
        ListaContinuar.ItemsSource = resumo.Continuar;
        TxtAtividadeVazia.IsVisible = resumo.Adicionadas == 0;
        TxtContinuarVazio.IsVisible = resumo.EmAndamento == 0;
        EstadoInicioVazio.IsVisible = resumo.Total == 0;
    }
    private void PeriodoDashboard_Changed(object? sender, SelectionChangedEventArgs e)
    { if (_interfacePronta) AtualizarDashboard(); }
    private void EditarDashboard_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is Button { Tag: EntradaDashboard entrada }) EditarJogo(entrada.Obra);
    }
}
