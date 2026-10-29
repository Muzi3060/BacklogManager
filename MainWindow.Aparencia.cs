using Avalonia.Controls;
using Avalonia.Interactivity;
using BacklogManager.Aparencia;

namespace BacklogManager;

public partial class MainWindow
{
    private ServicoAparencia _aparencia = null!;
    private bool _preenchendoAparencia;
    internal ServicoAparencia AparenciaAtual => _aparencia;
    private void InicializarAparencia(ServicoAparencia? servico)
    {
        _aparencia = servico ?? new ServicoAparencia(new ArmazenamentoAparenciaMemoria());
        if (_backup?.Pendencia == null) _aparencia.Carregar();
        AtualizarAparencia();
    }
    private void AtualizarAparencia()
    {
        AplicadorAparencia.Aplicar(_aparencia.Preferencias);
        _preenchendoAparencia = true;
        try
        {
            CmbTema.SelectedIndex = (int)_aparencia.Preferencias.Tema;
            CmbModo.SelectedIndex = (int)_aparencia.Preferencias.Modo;
        }
        finally { _preenchendoAparencia = false; }
        TxtEstadoAparencia.Text = _aparencia.Aviso ?? "Tema e modo aplicados. As escolhas são salvas automaticamente.";
        BtnRepetirSalvarAparencia.IsVisible = _aparencia.PendenteSalvar;
    }
    private void TemaAparencia_Changed(object? sender, SelectionChangedEventArgs e)
    {
        if (!_interfacePronta || _preenchendoAparencia || _backup?.Pendencia != null || CmbTema.SelectedIndex < 0 || CmbModo.SelectedIndex < 0) return;
        _aparencia.Selecionar((TemaVisual)CmbTema.SelectedIndex, (ModoVisual)CmbModo.SelectedIndex);
        AtualizarAparencia();
    }
    private void RepetirSalvarAparencia_Click(object? sender, RoutedEventArgs e)
    {
        if (_backup?.Pendencia != null) return;
        _aparencia.TentarSalvarNovamente(); AtualizarAparencia();
    }
}
