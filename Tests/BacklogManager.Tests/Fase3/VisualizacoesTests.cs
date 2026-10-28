using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Avalonia.VisualTree;
using BacklogManager.Controls;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Services;
using BacklogManager.Tests.Fase2;
using Xunit;

namespace BacklogManager.Tests.Fase3;

public sealed class VisualizacoesTests
{
    private static TelaTeste Abrir(int modo, params BacklogItem[] registros)
    {
        var t = new TelaTeste(registros);
        t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex = modo;
        t.Clicar("NavBacklog"); Renderizar(t); return t;
    }
    private static void Renderizar(TelaTeste t) { Dispatcher.UIThread.RunJobs(); t.Janela.UpdateLayout(); }
    private static void Painel(int modo)
    {
        using var t = Abrir(modo, PastaTeste.Jogo());
        Assert.Equal(modo, t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex);
        var painel = t.Campo<ListBox>("ListaBacklog").ItemsPanel!.Build();
        if (modo == 0) Assert.IsType<WrapPanel>(painel); else Assert.IsType<VirtualizingStackPanel>(painel);
    }
    private static void Trocar(int modo)
    {
        var um = PastaTeste.Jogo(); using var t = Abrir(modo, um, um);
        var lista = t.Campo<ListBox>("ListaBacklog"); lista.SelectedIndex = 1; var selecionado = lista.SelectedItem;
        t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex = 1 - modo; Renderizar(t);
        Assert.Same(selecionado, lista.SelectedItem); Assert.Equal(1, lista.SelectedIndex); Assert.True(t.Campo<Button>("BtnEditar").IsEnabled);
    }
    private static void Adicionar(int modo)
    {
        using var t = Abrir(modo); t.Selecionar(); t.Clicar("BtnAdicionar"); t.Clicar("NavBacklog");
        Assert.Single(t.Exibidos); Assert.Single(t.Salvos); Assert.NotNull(t.Salvos[0].Multimidia!.DataAdicao);
        Assert.Equal(modo, t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex);
    }
    private static async Task Simulado(int modo)
    {
        using var t = Abrir(modo); t.Campo<ComboBox>("CmbTipoCadastro").SelectedIndex = (int)TipoMidia.Livro;
        t.Campo<TextBox>("TxtTitulo").Text = "simulado"; await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Background);
        await t.Janela.BuscaPendente.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
        t.Campo<ListBox>("ListaSugestoes").SelectedIndex = 0; t.Campo<NumericUpDown>("NumPagina").Value = 17; t.Clicar("BtnAdicionar");
        t.Campo<ComboBox>("CmbTipoBiblioteca").SelectedIndex = (int)TipoMidia.Livro; t.Clicar("NavBacklog"); Renderizar(t);
        Assert.Equal(17, Assert.Single(t.Salvos).Multimidia!.Progresso!.Pagina); Assert.Single(t.Exibidos); Assert.Equal(0, t.Http.Chamadas);
    }
    private static void EditarLegado(int modo)
    {
        var r = PastaTeste.Jogo(); r.SteamAppId = 361855; using var t = Abrir(modo, r); t.Editar();
        t.Campo<NumericUpDown>("NumNota").Value = 3; t.Campo<TextBox>("TxtReview").Text = "Review modificada"; t.Clicar("BtnAtualizar");
        var salvo = Assert.Single(t.Salvos); Assert.Equal(3, salvo.Nota); Assert.Equal("Review modificada", salvo.Review); Assert.Equal(361855, salvo.SteamAppId); Assert.Null(salvo.IgdbId);
    }
    private static void EditarProgresso(int modo)
    {
        using var t = Abrir(modo, ConversorBibliotecaJson.ParaRegistro(ApoioMultimidia.Item(TipoMidia.Anime, new() { EpisodiosAssistidos = 2 })));
        t.Campo<ComboBox>("CmbTipoBiblioteca").SelectedIndex = (int)TipoMidia.Anime; t.Editar(); t.Campo<NumericUpDown>("NumEpisodiosAssistidos").Value = 8; t.Clicar("BtnAtualizar");
        Assert.Equal(8, Assert.Single(t.Salvos).Multimidia!.Progresso!.EpisodiosAssistidos);
    }
    private static void RemoverDuplicata(int modo)
    {
        var um = PastaTeste.Jogo(); using var t = Abrir(modo, um, um); var lista = t.Campo<ListBox>("ListaBacklog"); var primeira = lista.Items[0]; lista.SelectedIndex = 1; t.Clicar("BtnRemover");
        Assert.Single(t.Exibidos); Assert.Same(primeira, lista.Items[0]); Assert.Single(t.Salvos);
    }
    private static void Filtrar(int modo)
    {
        var a = ConsultasTests.Item(); var b = ConsultasTests.Item("Beta", 2, "Aventura", "Switch");
        using var t = Abrir(modo, ConversorBibliotecaJson.ParaRegistro(a), ConversorBibliotecaJson.ParaRegistro(b));
        t.Campo<ComboBox>("CmbGenero").SelectedItem = "RPG"; t.Campo<NumericUpDown>("NumNotaMinima").Value = 4;
        Assert.Equal("Alfa", Assert.Single(t.Exibidos).Titulo); t.Clicar("BtnLimparFiltros"); Assert.Equal(2, t.Exibidos.Length);
    }
    private static void Ordenar(int modo)
    {
        using var t = Abrir(modo, PastaTeste.Jogo("Zeta"), PastaTeste.Jogo("Alfa")); t.Campo<ComboBox>("CmbOrdenacao").SelectedIndex = 1;
        Assert.Equal(new[] { "Alfa", "Zeta" }, t.Exibidos.Select(i => i.Titulo)); Assert.Equal("Zeta", t.Salvos[0].Titulo);
    }
    private static void FalhaGravacao(int modo)
    {
        using var t = Abrir(modo, PastaTeste.Jogo()); var original = File.ReadAllBytes(t.Pasta.Arquivo); t.Editar();
        File.AppendAllText(t.Pasta.Arquivo, " "); t.Campo<TextBox>("TxtReview").Text = "Não salvar"; t.Clicar("BtnAtualizar");
        Assert.Equal("Review simulada", t.Exibidos[0].Review); Assert.Contains("mudou fora", t.Aviso); Assert.Equal(original.Concat(new byte[] { 32 }), File.ReadAllBytes(t.Pasta.Arquivo));
    }
    private static void Vazio(int modo)
    {
        using var t = Abrir(modo); Assert.Empty(t.Exibidos); Assert.True(t.Campo<StackPanel>("EstadoListaVazia").IsVisible);
        Assert.False(t.Campo<Button>("BtnEditar").IsEnabled); Assert.False(t.Campo<Button>("BtnRemover").IsEnabled);
    }
    private static async Task Capturar(int modo)
    {
        var registros = Enum.GetValues<TipoMidia>().Select(tipo => ConversorBibliotecaJson.ParaRegistro(ApoioMultimidia.Item(tipo, tipo == TipoMidia.Livro ? new() { Pagina = 17 } : null))).ToArray();
        using var t = Abrir(modo, registros); t.Janela.Width = 900; t.Janela.Height = 660;
        var pasta = Path.Combine(Path.GetTempPath(), "backlogmanager-revisao2-resultados", "visualizacoes"); Directory.CreateDirectory(pasta);
        foreach (var tipo in Enum.GetValues<TipoMidia>())
        {
            t.Campo<ComboBox>("CmbTipoBiblioteca").SelectedIndex = (int)tipo; Renderizar(t);
            var capas = t.Janela.GetVisualDescendants().OfType<CapaObra>().Where(c => c.IsVisible).ToArray();
            await Task.WhenAll(capas.Select(c => c.CarregamentoPendente)); Renderizar(t);
            Assert.Single(t.Exibidos); Assert.True(t.Campo<ListBox>("ListaBacklog").Bounds.Height > 100);
            using var imagem = t.Janela.CaptureRenderedFrame(); Assert.NotNull(imagem); imagem.Save(Path.Combine(pasta, $"{tipo}-{(modo == 0 ? "grade" : "lista")}.png"), PngBitmapEncoderOptions.Default);
        }
    }
    private static void Conteudo(int modo)
    {
        var item = ApoioMultimidia.Item(TipoMidia.Livro, new() { Pagina = 17 }); using var t = Abrir(modo, ConversorBibliotecaJson.ParaRegistro(item));
        t.Campo<ComboBox>("CmbTipoBiblioteca").SelectedIndex = (int)TipoMidia.Livro; Renderizar(t);
        var textos = t.Campo<ListBox>("ListaBacklog").GetVisualDescendants().OfType<TextBlock>().Select(b => b.Text).ToArray();
        Assert.Contains(item.Catalogo.Titulo, textos); Assert.Contains(item.Pessoal.Review, textos); Assert.Contains(item.StatusExibido, textos); Assert.Contains(item.ProgressoExibido, textos); Assert.Contains(textos, s => s != null && s.Contains('★'));
    }
    [AvaloniaFact(DisplayName = "VG01 | Painel de visualização em grade | Painel correto e seleção de modo aplicada")]
    public void PainelVG() => Painel(0);
    [AvaloniaFact(DisplayName = "VG02 | Alternar com duplicata selecionada em grade | Mesma instância e índice permanecem selecionados")]
    public void TrocarVG() => Trocar(0);
    [AvaloniaFact(DisplayName = "VG03 | Adicionar jogo em grade | Cadastro persistido com data real e modo preservado")]
    public void AdicionarVG() => Adicionar(0);
    [AvaloniaFact(DisplayName = "VG04 | Adicionar livro simulado em grade | Progresso persiste sem HTTP real")]
    public Task SimuladoVG() => Simulado(0);
    [AvaloniaFact(DisplayName = "VG05 | Editar nota e review legadas em grade | Steam original preservado e IgdbId ausente")]
    public void EditarLegadoVG() => EditarLegado(0);
    [AvaloniaFact(DisplayName = "VG06 | Editar progresso de anime em grade | Episódios atualizados e persistidos")]
    public void EditarProgressoVG() => EditarProgresso(0);
    [AvaloniaFact(DisplayName = "VG07 | Remover segunda cópia igual em grade | Primeira instância permanece sem deduplicação")]
    public void RemoverDuplicataVG() => RemoverDuplicata(0);
    [AvaloniaFact(DisplayName = "VG08 | Filtrar e limpar em grade | Gênero e nota combinam e limpeza restaura itens")]
    public void FiltrarVG() => Filtrar(0);
    [AvaloniaFact(DisplayName = "VG09 | Ordenar títulos em grade | Ordem visual muda sem regravar biblioteca")]
    public void OrdenarVG() => Ordenar(0);
    [AvaloniaFact(DisplayName = "VG10 | Arquivo alterado externamente em grade | Gravação bloqueada e memória anterior preservada")]
    public void FalhaGravacaoVG() => FalhaGravacao(0);
    [AvaloniaFact(DisplayName = "VG11 | Biblioteca vazia em grade | Estado vazio e ações desabilitadas")]
    public void VazioVG() => Vazio(0);
    [AvaloniaFact(DisplayName = "VG12 | Seis mídias no tamanho mínimo em grade | Telas renderizadas com lista utilizável e capas")]
    public Task CapturarVG() => Capturar(0);
    [AvaloniaFact(DisplayName = "VG13 | Dados pessoais no cartão em grade | Status, nota, review e progresso renderizam")]
    public void ConteudoVG() => Conteudo(0);
    [AvaloniaFact(DisplayName = "VL01 | Painel de visualização em lista | Painel correto e seleção de modo aplicada")]
    public void PainelVL() => Painel(1);
    [AvaloniaFact(DisplayName = "VL02 | Alternar com duplicata selecionada em lista | Mesma instância e índice permanecem selecionados")]
    public void TrocarVL() => Trocar(1);
    [AvaloniaFact(DisplayName = "VL03 | Adicionar jogo em lista | Cadastro persistido com data real e modo preservado")]
    public void AdicionarVL() => Adicionar(1);
    [AvaloniaFact(DisplayName = "VL04 | Adicionar livro simulado em lista | Progresso persiste sem HTTP real")]
    public Task SimuladoVL() => Simulado(1);
    [AvaloniaFact(DisplayName = "VL05 | Editar nota e review legadas em lista | Steam original preservado e IgdbId ausente")]
    public void EditarLegadoVL() => EditarLegado(1);
    [AvaloniaFact(DisplayName = "VL06 | Editar progresso de anime em lista | Episódios atualizados e persistidos")]
    public void EditarProgressoVL() => EditarProgresso(1);
    [AvaloniaFact(DisplayName = "VL07 | Remover segunda cópia igual em lista | Primeira instância permanece sem deduplicação")]
    public void RemoverDuplicataVL() => RemoverDuplicata(1);
    [AvaloniaFact(DisplayName = "VL08 | Filtrar e limpar em lista | Gênero e nota combinam e limpeza restaura itens")]
    public void FiltrarVL() => Filtrar(1);
    [AvaloniaFact(DisplayName = "VL09 | Ordenar títulos em lista | Ordem visual muda sem regravar biblioteca")]
    public void OrdenarVL() => Ordenar(1);
    [AvaloniaFact(DisplayName = "VL10 | Arquivo alterado externamente em lista | Gravação bloqueada e memória anterior preservada")]
    public void FalhaGravacaoVL() => FalhaGravacao(1);
    [AvaloniaFact(DisplayName = "VL11 | Biblioteca vazia em lista | Estado vazio e ações desabilitadas")]
    public void VazioVL() => Vazio(1);
    [AvaloniaFact(DisplayName = "VL12 | Seis mídias no tamanho mínimo em lista | Telas renderizadas com lista utilizável e capas")]
    public Task CapturarVL() => Capturar(1);
    [AvaloniaFact(DisplayName = "VL13 | Dados pessoais no cartão em lista | Status, nota, review e progresso renderizam")]
    public void ConteudoVL() => Conteudo(1);
}
