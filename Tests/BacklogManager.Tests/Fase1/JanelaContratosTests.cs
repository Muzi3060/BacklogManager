using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Interactivity;
using Avalonia.Threading;
using BacklogManager.Contracts;
using BacklogManager.Models;
using BacklogManager.Services;
using Xunit;
using static BacklogManager.Tests.Fase1.ApoioArquitetura;

namespace BacklogManager.Tests.Fase1;

public sealed class JanelaContratosTests
{
    private sealed class PesquisaSimulada : IPesquisaCatalogo, IDisposable
    {
        public Func<CancellationToken, Task<IReadOnlyList<ObraCatalogo>>> Resposta { get; set; } = _ => Task.FromResult<IReadOnlyList<ObraCatalogo>>(new[] { Jogo().Catalogo });
        public int Descartes { get; private set; }
        public Task<IReadOnlyList<ObraCatalogo>> BuscarAsync(string termo, CancellationToken cancellationToken = default) => Resposta(cancellationToken);
        public void Dispose() => Descartes++;
    }
    private static T Campo<T>(MainWindow j, string nome) where T : Control => j.FindControl<T>(nome)!;
    private static void Clicar(MainWindow j, string nome) => Campo<Button>(j, nome).RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    private static MainWindow Abrir(ArmazenamentoSimulado a, IPesquisaCatalogo? pesquisa = null, IDisposable? recurso = null)
    {
        var j = new MainWindow(new ServicoBiblioteca(a), pesquisa, recurso); j.Show(); return j;
    }
    private static void Selecionar(MainWindow j)
    {
        var obra = Jogo().Catalogo;
        Campo<TextBox>(j, "TxtTitulo").Text = "Pesq";
        Campo<ListBox>(j, "ListaSugestoes").ItemsSource = new[] { obra };
        Campo<ListBox>(j, "ListaSugestoes").SelectedItem = obra;
    }
    private static async Task Buscar(MainWindow j)
    {
        Campo<TextBox>(j, "TxtTitulo").Text = "Pesq";
        await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Background);
        await j.BuscaPendente.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
    }

    [AvaloniaFact(DisplayName = "UI01 | Janela com armazenamento por contrato | Coleção abre sem FileManager ou arquivo JSON")]
    public void SemArquivo()
    {
        var a = new ArmazenamentoSimulado { Registros = new[] { Jogo() } }; var j = Abrir(a);
        try { Assert.Equal("1", Campo<TextBlock>(j, "TxtTotal").Text); Assert.Equal(0, a.Gravacoes); }
        finally { j.Close(); }
    }
    [AvaloniaFact(DisplayName = "UI02 | Cadastro pela janela e armazenamento por contrato | Item persistido com catálogo e dados pessoais separados")]
    public void Adicionar()
    {
        var a = new ArmazenamentoSimulado(); var j = Abrir(a);
        try { Selecionar(j); Clicar(j, "BtnAdicionar"); var item = Assert.Single(a.Registros); Assert.Equal("42", item.Catalogo.IdentificadorExterno); Assert.Equal("Na Fila", item.Pessoal.Status); }
        finally { j.Close(); }
    }
    [AvaloniaFact(DisplayName = "UI03 | Edição local por contrato | Nota atualizada e referência de catálogo preservada")]
    public void Editar()
    {
        var original = Jogo() with { SteamAppIdLegado = 361855 }; var a = new ArmazenamentoSimulado { Registros = new[] { original } }; var j = Abrir(a);
        try
        {
            Campo<ListBox>(j, "ListaBacklog").SelectedIndex = 0; Clicar(j, "BtnEditar"); Campo<NumericUpDown>(j, "NumNota").Value = 2; Clicar(j, "BtnAtualizar");
            Assert.Equal(2, Assert.Single(a.Registros).Pessoal.Nota); Assert.Same(original.Catalogo, Assert.Single(a.Registros).Catalogo); Assert.Equal(361855, Assert.Single(a.Registros).SteamAppIdLegado);
        }
        finally { j.Close(); }
    }
    [AvaloniaFact(DisplayName = "UI04 | Exclusão por contrato | Registro selecionado removido do armazenamento")]
    public void Excluir()
    {
        var a = new ArmazenamentoSimulado { Registros = new[] { Jogo() } }; var j = Abrir(a);
        try { Campo<ListBox>(j, "ListaBacklog").SelectedIndex = 0; Clicar(j, "BtnRemover"); Assert.Empty(a.Registros); Assert.Equal("0", Campo<TextBlock>(j, "TxtTotal").Text); }
        finally { j.Close(); }
    }
    [AvaloniaFact(DisplayName = "UI05 | Erro na leitura do armazenamento substituído | Janela abre com aviso e gravação desativada")]
    public void LeituraFalha()
    {
        var a = new ArmazenamentoSimulado { FalhaLeitura = new IOException() }; var j = Abrir(a);
        try { Assert.False(Campo<Button>(j, "BtnAdicionar").IsEnabled); Assert.Equal("Coleção não carregada", Campo<TextBlock>(j, "TxtAvisoTitulo").Text); }
        finally { j.Close(); }
    }
    [AvaloniaFact(DisplayName = "UI06 | Erro na gravação do armazenamento substituído | Formulário e coleção anteriores mantidos")]
    public void GravacaoFalha()
    {
        var a = new ArmazenamentoSimulado { FalhaGravacao = new IOException() }; var j = Abrir(a);
        try { Selecionar(j); Clicar(j, "BtnAdicionar"); Assert.Empty(a.Registros); Assert.Equal("0", Campo<TextBlock>(j, "TxtTotal").Text); Assert.Equal("Jogo de teste", Campo<TextBox>(j, "TxtTitulo").Text); }
        finally { j.Close(); }
    }
    [AvaloniaFact(DisplayName = "UI07 | Pesquisa substituída por contrato | Sugestões usam modelo comum sem depender da IGDB")]
    public async Task PesquisaPorContrato()
    {
        var a = new ArmazenamentoSimulado(); using var p = new PesquisaSimulada(); var j = Abrir(a, p);
        try { await Buscar(j); var obra = Assert.IsType<ObraCatalogo>(Assert.Single(Campo<ListBox>(j, "ListaSugestoes").Items)); Assert.Equal("42", obra.IdentificadorExterno); }
        finally { j.Close(); }
    }
    [AvaloniaFact(DisplayName = "UI08 | Erro inesperado em pesquisa substituída | Aviso seguro sem encerrar a janela")]
    public async Task ErroPesquisaContrato()
    {
        var a = new ArmazenamentoSimulado(); using var p = new PesquisaSimulada { Resposta = _ => throw new InvalidOperationException("detalhe-sensivel") }; var j = Abrir(a, p);
        try { await Buscar(j); Assert.Equal("Falha na pesquisa", Campo<TextBlock>(j, "TxtAvisoTitulo").Text); Assert.DoesNotContain("detalhe-sensivel", Campo<TextBlock>(j, "TxtAvisoMensagem").Text!); }
        finally { j.Close(); }
    }
    [AvaloniaFact(DisplayName = "UI09 | Recurso próprio entregue à janela | Fechamento descarta o recurso uma vez")]
    public void DescarteProprio()
    {
        var p = new PesquisaSimulada(); var j = Abrir(new ArmazenamentoSimulado(), p, p);
        j.Close(); j.Close(); Assert.Equal(1, p.Descartes);
    }
    [AvaloniaFact(DisplayName = "UI10 | Pesquisa sem transferência de propriedade | Janela não descarta recurso do chamador")]
    public void RecursoDoChamador()
    {
        using var p = new PesquisaSimulada(); var j = Abrir(new ArmazenamentoSimulado(), p);
        j.Close(); Assert.Equal(0, p.Descartes);
    }
    [AvaloniaFact(DisplayName = "UI11 | Editar segunda cópia com valores idênticos | Apenas o segundo registro recebe a edição")]
    public void EditarDuplicadosIguais()
    {
        var um = Jogo(); var dois = Jogo();
        var a = new ArmazenamentoSimulado { Registros = new[] { um, dois } }; var j = Abrir(a);
        try
        {
            Campo<ListBox>(j, "ListaBacklog").SelectedIndex = 1; Clicar(j, "BtnEditar");
            Campo<NumericUpDown>(j, "NumNota").Value = 1; Clicar(j, "BtnAtualizar");
            Assert.Same(um, a.Registros[0]); Assert.Equal(1, a.Registros[1].Pessoal.Nota);
        }
        finally { j.Close(); }
    }
    [AvaloniaFact(DisplayName = "UI12 | Filtrar mantendo duas cópias idênticas | Seleção da segunda instância preservada")]
    public void FiltrarDuplicadosIguais()
    {
        var um = Jogo(); var dois = Jogo();
        var a = new ArmazenamentoSimulado { Registros = new[] { um, dois } }; var j = Abrir(a);
        try
        {
            Campo<ListBox>(j, "ListaBacklog").SelectedIndex = 1;
            Campo<TextBox>(j, "TxtBusca").Text = "Jogo"; Dispatcher.UIThread.RunJobs();
            Assert.Same(dois, Campo<ListBox>(j, "ListaBacklog").SelectedItem);
        }
        finally { j.Close(); }
    }

}
