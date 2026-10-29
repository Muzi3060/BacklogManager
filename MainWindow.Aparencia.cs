using System;
using System.Linq;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BacklogManager.Aparencia;
using BacklogManager.Controls;

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
        AtualizarAparencia(true);
    }
    private void AbrirAparencia()
    {
        if (EditoresCores.Children.Count > 0) return;
        foreach (var elemento in Enum.GetValues<ElementoCor>())
        {
            var editor = new EditorCor(elemento);
            editor.CorAlterada += (e, cor) => { _aparencia.Personalizar(e, cor); AtualizarAparencia(false); };
            EditoresCores.Children.Add(editor);
        }
        AtualizarAparencia(true);
    }
    private void AtualizarAparencia(bool preencher)
    {
        AplicadorAparencia.Aplicar(_aparencia.Preferencias);
        var resolvida = ResolvedorAparencia.Resolver(_aparencia.Preferencias);
        _preenchendoAparencia = true;
        try
        {
            CmbTema.SelectedIndex = (int)_aparencia.Preferencias.Tema;
            CmbModo.SelectedIndex = (int)_aparencia.Preferencias.Modo;
            foreach (var editor in EditoresCores.Children.OfType<EditorCor>())
            {
                if (preencher) editor.Preencher(resolvida.Solicitadas[editor.Elemento]);
                editor.MostrarAjuste(resolvida.Avisos.GetValueOrDefault(editor.Elemento));
            }
        }
        finally { _preenchendoAparencia = false; }
        TxtEstadoAparencia.Text = _aparencia.Aviso ?? "Aparência em uso. As alterações válidas são salvas automaticamente; cada tema e modo preserva suas próprias cores.";
        BtnRepetirSalvarAparencia.IsVisible = _aparencia.PendenteSalvar;
    }
    private void TemaAparencia_Changed(object? sender, SelectionChangedEventArgs e)
    {
        if (!_interfacePronta || _preenchendoAparencia || CmbTema.SelectedIndex < 0 || CmbModo.SelectedIndex < 0) return;
        _aparencia.Selecionar((TemaVisual)CmbTema.SelectedIndex, (ModoVisual)CmbModo.SelectedIndex);
        ConfirmacaoRestaurarAparencia.IsVisible = false;
        AtualizarAparencia(true);
    }
    private void RestaurarCores_Click(object? sender, RoutedEventArgs e) { _aparencia.RestaurarAtivo(); AtualizarAparencia(true); }
    private void PedirRestauracao_Click(object? sender, RoutedEventArgs e) => ConfirmacaoRestaurarAparencia.IsVisible = true;
    private void CancelarRestauracao_Click(object? sender, RoutedEventArgs e) => ConfirmacaoRestaurarAparencia.IsVisible = false;
    private void ConfirmarRestauracao_Click(object? sender, RoutedEventArgs e)
    {
        if (!ConfirmacaoRestaurarAparencia.IsVisible) return;
        _aparencia.RestaurarTudo(); ConfirmacaoRestaurarAparencia.IsVisible = false; AtualizarAparencia(true);
    }
    private void RepetirSalvarAparencia_Click(object? sender, RoutedEventArgs e) { _aparencia.TentarSalvarNovamente(); AtualizarAparencia(false); }
}
