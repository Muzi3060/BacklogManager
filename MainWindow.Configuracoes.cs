using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using BacklogManager.Backup;

namespace BacklogManager;

public partial class MainWindow
{
    private readonly ServicoBackupLocal? _backup;
    private PreviaRestauracao? _previaBackup;
    private bool _selecionandoBackup;
    internal Func<bool, Task<string?>>? SelecionarArquivoBackup { get; set; }
    internal Task OperacaoBackupPendente { get; private set; } = Task.CompletedTask;
    private void InicializarConfiguracoes()
    {
        TxtCaminhosDados.Text = _backup == null ? "Armazenamento de teste sem backup configurado." :
            string.Join(Environment.NewLine, _backup.Caminhos.Arquivos.Select(p => p.Key + ": " + p.Value)) +
            Environment.NewLine + "Backup de segurança anterior à restauração: " + _backup.PastaSeguranca;
        BtnExportarBackup.IsEnabled = _backup != null && _backup.Pendencia == null && !_selecionandoBackup;
        BtnSelecionarBackup.IsEnabled = BtnExportarBackup.IsEnabled;
        PreviaBackup.IsEnabled = !_selecionandoBackup;
        BtnRecuperarBackup.IsVisible = _backup?.Pendencia != null || _recargaBackupPendente;
        PreferenciasVisuais.IsEnabled = _backup?.Pendencia == null;
        if (_backup?.Pendencia != null) TxtEstadoBackup.Text = _backup.Pendencia + " Nenhuma gravação será permitida até resolver a recuperação.";
    }
    private async Task<string?> EscolherArquivo(bool exportar)
    {
        if (SelecionarArquivoBackup != null) return await SelecionarArquivoBackup(exportar);
        var tipo = new FilePickerFileType("Backup JSON") { Patterns = new[] { "*.json" } };
        if (exportar)
        {
            var f = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Exportar biblioteca e coleções", SuggestedFileName = "minhas-realizacoes-backup.json",
                DefaultExtension = "json", FileTypeChoices = new[] { tipo }, ShowOverwritePrompt = true
            });
            return f?.TryGetLocalPath();
        }
        var arquivos = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        { Title = "Selecionar backup completo", AllowMultiple = false, FileTypeFilter = new[] { tipo } });
        return arquivos.FirstOrDefault()?.TryGetLocalPath();
    }
    private void ExportarBackup_Click(object? sender, RoutedEventArgs e) => OperacaoBackupPendente = ExecutarSeletor(true);
    private void SelecionarBackup_Click(object? sender, RoutedEventArgs e) => OperacaoBackupPendente = ExecutarSeletor(false);
    private async Task ExecutarSeletor(bool exportar)
    {
        if (_backup == null || _backup.Pendencia != null || !BtnSelecionarBackup.IsEnabled) return;
        _selecionandoBackup = true;
        InicializarConfiguracoes();
        if (!exportar) LimparPreviaBackup();
        try
        {
            var caminho = await EscolherArquivo(exportar);
            if (_fechada) return;
            if (caminho == null) { TxtEstadoBackup.Text = "Operação cancelada. Nenhum dado foi alterado."; return; }
            if (exportar)
            {
                _backup.Exportar(caminho, ChkIncluirPreferencias.IsChecked == true);
                TxtEstadoBackup.Text = "Backup exportado e verificado: " + caminho;
            }
            else
            {
                _previaBackup = _backup.Prever(caminho);
                var p = _previaBackup;
                TxtPreviaBackup.Text = $"Origem: {p.Arquivo}\nData declarada: {p.CriadoEm:g}\n" +
                    $"Obras: {Contagem(p.ObrasAtuais)} → {p.Obras}; coleções: {Contagem(p.ColecoesAtuais)} → {p.Colecoes}.\n" +
                    $"Vínculos: {p.Vinculos}; referências ausentes: {p.ReferenciasAusentes}.\n{p.ResumoPorMidia}\n" +
                    (p.TemPreferencias ? $"Preferências disponíveis: {p.PreferenciasDisponiveis}. " : "Sem preferências disponíveis. ") + p.Aviso +
                    "\nA restauração substitui a biblioteca e as coleções atuais. Não há mesclagem.";
                ChkRestaurarPreferencias.IsEnabled = p.TemPreferencias && p.PreferenciasValidas;
                PreviaBackup.IsVisible = true;
                TxtEstadoBackup.Text = "Backup validado. Revise a prévia antes de confirmar. Os dados atuais ainda não foram modificados.";
            }
        }
        catch (Exception erro) when (ServicoBackupLocal.Falha(erro))
        { TxtEstadoBackup.Text = "Operação não concluída: " + erro.Message; }
        finally
        {
            _selecionandoBackup = false;
            if (!_fechada) InicializarConfiguracoes();
        }
    }
    private static string Contagem(int valor) => valor < 0 ? "não legível" : valor.ToString();
    private void LimparPreviaBackup()
    {
        _previaBackup = null; PreviaBackup.IsVisible = false;
        ChkConfirmarSubstituicao.IsChecked = false; ChkRestaurarPreferencias.IsChecked = false;
        BtnConfirmarBackup.IsEnabled = false;
    }
    private void PreferenciasBackup_Changed(object? sender, RoutedEventArgs e)
    { if (_interfacePronta) ChkConfirmarSubstituicao.IsChecked = false; }
    private void ConfirmacaoBackup_Changed(object? sender, RoutedEventArgs e)
    { if (_interfacePronta) BtnConfirmarBackup.IsEnabled = _previaBackup != null && ChkConfirmarSubstituicao.IsChecked == true; }
    private void CancelarBackup_Click(object? sender, RoutedEventArgs e)
    { LimparPreviaBackup(); TxtEstadoBackup.Text = "Restauração cancelada. Nenhum dado foi alterado."; }
    private void ConfirmarBackup_Click(object? sender, RoutedEventArgs e)
    {
        if (_backup == null || _previaBackup == null || _selecionandoBackup || ChkConfirmarSubstituicao.IsChecked != true) return;
        try
        {
            var preferencias = ChkRestaurarPreferencias.IsChecked == true;
            var seguranca = _backup.Restaurar(_previaBackup, preferencias, true);
            // O serviço só retorna após verificar os arquivos. Falha de recarga não vira sucesso de interface.
            RecarregarAposBackup(preferencias && _previaBackup.TemAparencia, preferencias && _previaBackup.TemVisualizacao);
            TxtEstadoBackup.Text = "Restauração concluída e dados recarregados. Backup do estado anterior: " + seguranca;
        }
        catch (Exception erro) when (ServicoBackupLocal.Falha(erro))
        {
            TxtEstadoBackup.Text = "Restauração não concluída integralmente: " + erro.Message;
            if (_backup.Pendencia == null)
            {
                try { RecarregarAposBackup(false, false); }
                catch (Exception recarga) when (ServicoBackupLocal.Falha(recarga))
                { TxtEstadoBackup.Text += " Reabra o aplicativo: " + recarga.Message; _recargaBackupPendente = true; }
            }
        }
        finally { LimparPreviaBackup(); InicializarConfiguracoes(); }
    }
    private bool _recargaBackupPendente;
    private void RecarregarAposBackup(bool aparencia = true, bool visualizacao = true)
    {
        _recargaBackupPendente = true;
        CancelarBusca(); LimparFormulario();
        _biblioteca.Carregar(); _colecoes.Carregar();
        CmbColecoes.SelectedIndex = -1; CmbObraColecao.SelectedIndex = -1;
        TxtNomeColecao.Text = TxtDescricaoColecao.Text = string.Empty; CancelarExclusaoColecao();
        if (aparencia) { _aparencia.Carregar(); AtualizarAparencia(); }
        if (visualizacao) CarregarVisualizacao();
        _recargaBackupPendente = false;
        AtualizarColecao(); AtualizarDisponibilidadeCadastro();
        BtnAtualizar.IsEnabled = PersistenciaDisponivel;
        AvisoColecoesIndisponiveis.IsVisible = false;
        AvisoOperacao.IsVisible = false;
    }
    private void RecuperarBackup_Click(object? sender, RoutedEventArgs e)
    {
        if (_backup == null) return;
        if (!_backup.Recuperar()) TxtEstadoBackup.Text = _backup.Pendencia;
        else
        {
            try { RecarregarAposBackup(); TxtEstadoBackup.Text = "Recuperação verificada. Os dados foram recarregados."; }
            catch (Exception erro) when (ServicoBackupLocal.Falha(erro))
            { TxtEstadoBackup.Text = "Arquivos recuperados; reabra o aplicativo para recarregar os dados. " + erro.Message; }
        }
        InicializarConfiguracoes();
    }
}
