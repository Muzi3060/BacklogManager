using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using BacklogManager.Data;
using BacklogManager.Models;
using Xunit;
using static BacklogManager.Tests.Fase2.ApoioMultimidia;

namespace BacklogManager.Tests.Fase2;

public sealed class InterfaceMultimidiaTests
{
    private static void TipoCadastro(TelaTeste t, TipoMidia tipo) => t.Campo<ComboBox>("CmbTipoCadastro").SelectedIndex = (int)tipo;
    private static void TipoBiblioteca(TelaTeste t, TipoMidia tipo) => t.Campo<ComboBox>("CmbTipoBiblioteca").SelectedIndex = (int)tipo;
    private static async Task Buscar(TelaTeste t, string texto = "simulado")
    {
        t.Campo<TextBox>("TxtTitulo").Text = texto;
        await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Background);
        await t.Janela.BuscaPendente.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
    }
    private static async Task Selecionar(TelaTeste t, TipoMidia tipo)
    {
        TipoCadastro(t, tipo); await Buscar(t); t.Campo<ListBox>("ListaSugestoes").SelectedIndex = 0;
    }
    private static async Task Cadastrar(TelaTeste t, TipoMidia tipo, string campo, decimal valor)
    {
        await Selecionar(t, tipo); t.Campo<NumericUpDown>(campo).Value = valor; t.Clicar("BtnAdicionar");
    }
    private static void ConferirCadastro(TelaTeste t, TipoMidia tipo)
    {
        var r = Assert.Single(t.Salvos); Assert.Equal(tipo, r.Multimidia!.Tipo); Assert.True(r.Multimidia.Simulado); Assert.Null(r.IgdbId);
        TipoBiblioteca(t, tipo); Assert.Single(t.Exibidos); Assert.Equal(0, t.Http.Chamadas);
    }

    [AvaloniaFact(DisplayName = "UM01 | Anime pela interface sem credenciais | Cadastro e episódios persistem com aviso de simulação")]
    public async Task Anime() { using var t = new TelaTeste(semServico: true); await Cadastrar(t, TipoMidia.Anime, "NumEpisodiosAssistidos", 12); ConferirCadastro(t, TipoMidia.Anime); Assert.Equal(12, t.Salvos[0].Multimidia!.Progresso!.EpisodiosAssistidos); }
    [AvaloniaFact(DisplayName = "UM02 | Série pela interface sem credenciais | Cadastro e temporada persistem")]
    public async Task Serie() { using var t = new TelaTeste(semServico: true); await Cadastrar(t, TipoMidia.Serie, "NumTemporada", 2); ConferirCadastro(t, TipoMidia.Serie); Assert.Equal(2, t.Salvos[0].Multimidia!.Progresso!.Temporada); }
    [AvaloniaFact(DisplayName = "UM03 | Filme pela interface sem credenciais | Cadastro e minutos persistem")]
    public async Task Filme() { using var t = new TelaTeste(semServico: true); await Cadastrar(t, TipoMidia.Filme, "NumMinutosAssistidos", 25.5m); ConferirCadastro(t, TipoMidia.Filme); Assert.Equal(25.5m, t.Salvos[0].Multimidia!.Progresso!.MinutosAssistidos); }
    [AvaloniaFact(DisplayName = "UM04 | Mangá pela interface sem credenciais | Cadastro e capítulo persistem")]
    public async Task Manga() { using var t = new TelaTeste(semServico: true); await Cadastrar(t, TipoMidia.Manga, "NumCapituloManga", 10); ConferirCadastro(t, TipoMidia.Manga); Assert.Equal(10, t.Salvos[0].Multimidia!.Progresso!.Capitulo); }
    [AvaloniaFact(DisplayName = "UM05 | Livro pela interface sem credenciais | Cadastro e página persistem")]
    public async Task Livro() { using var t = new TelaTeste(semServico: true); await Cadastrar(t, TipoMidia.Livro, "NumPagina", 25); ConferirCadastro(t, TipoMidia.Livro); Assert.Equal(25, t.Salvos[0].Multimidia!.Progresso!.Pagina); }
    [AvaloniaFact(DisplayName = "UM06 | Biblioteca mista | Cada categoria exibe somente sua mídia")]
    public void Categorias()
    {
        var registros = Enum.GetValues<TipoMidia>().Select(tipo => ConversorBibliotecaJson.ParaRegistro(Item(tipo))).ToArray(); using var t = new TelaTeste(registros);
        foreach (var tipo in Enum.GetValues<TipoMidia>()) { TipoBiblioteca(t, tipo); var r = Assert.Single(t.Exibidos); Assert.Equal(tipo, r.Multimidia?.Tipo ?? TipoMidia.Jogo); }
    }
    [AvaloniaFact(DisplayName = "UM07 | Trocar tipo durante espera da busca | Sugestões da categoria antiga desaparecem")]
    public async Task TrocaBusca()
    {
        using var t = new TelaTeste(); TipoCadastro(t, TipoMidia.Anime); t.Campo<TextBox>("TxtTitulo").Text = "simulado";
        await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Background); var antiga = t.Janela.BuscaPendente;
        TipoCadastro(t, TipoMidia.Livro); await antiga; Assert.Empty(t.Campo<ListBox>("ListaSugestoes").Items); Assert.Equal("", t.Campo<TextBox>("TxtTitulo").Text);
    }
    [AvaloniaFact(DisplayName = "UM08 | Trocar tipo após seleção | Seleção antiga não pode cadastrar em outra categoria")]
    public async Task TrocaSelecao() { using var t = new TelaTeste(); await Selecionar(t, TipoMidia.Anime); TipoCadastro(t, TipoMidia.Livro); t.Clicar("BtnAdicionar"); Assert.Empty(t.Salvos); }
    [AvaloniaFact(DisplayName = "UM09 | Cadastro simulado sem progresso | Nenhum zero automático é gravado")]
    public async Task SemProgresso() { using var t = new TelaTeste(); await Selecionar(t, TipoMidia.Livro); t.Clicar("BtnAdicionar"); Assert.Null(Assert.Single(t.Salvos).Multimidia!.Progresso); }
    [AvaloniaFact(DisplayName = "UM10 | Campo inteiro recebe fração | Aviso de progresso inválido sem truncamento ou encerramento")]
    public async Task FracaoInteira() { using var t = new TelaTeste(); await Selecionar(t, TipoMidia.Livro); t.Campo<NumericUpDown>("NumPagina").Value = 1.5m; t.Clicar("BtnAdicionar"); Assert.Empty(t.Salvos); Assert.Equal("Progresso inválido", t.Campo<TextBlock>("TxtAvisoTitulo").Text); }
    [AvaloniaFact(DisplayName = "UM11 | Editar livro e reiniciar | Página e origem simulada permanecem")]
    public async Task EditarReiniciar()
    {
        using var t = new TelaTeste(); await Cadastrar(t, TipoMidia.Livro, "NumPagina", 10); TipoBiblioteca(t, TipoMidia.Livro); t.Editar();
        Assert.False(t.Campo<ComboBox>("CmbTipoCadastro").IsEnabled); Assert.Equal(10, t.Campo<NumericUpDown>("NumPagina").Value);
        t.Campo<NumericUpDown>("NumPagina").Value = 20; t.Clicar("BtnAtualizar");
        using var nova = new TelaTeste(t.Salvos); TipoBiblioteca(nova, TipoMidia.Livro); nova.Editar(); Assert.Equal(20, nova.Campo<NumericUpDown>("NumPagina").Value); Assert.True(nova.Salvos[0].Multimidia!.Simulado);
    }
    [AvaloniaFact(DisplayName = "UM12 | Remover anime em coleção mista | Livro e jogo permanecem intactos")]
    public void Remover()
    {
        using var t = new TelaTeste(Enum.GetValues<TipoMidia>().Select(tipo => ConversorBibliotecaJson.ParaRegistro(Item(tipo)))); TipoBiblioteca(t, TipoMidia.Anime); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; t.Clicar("BtnRemover"); Assert.Equal(5, t.Salvos.Count); Assert.DoesNotContain(t.Salvos, r => r.Multimidia?.Tipo == TipoMidia.Anime);
    }
    [AvaloniaFact(DisplayName = "UM13 | Falha de gravação em cadastro de livro | Coleção e formulário preservados sem falso sucesso")]
    public async Task FalhaCadastro()
    {
        using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); Directory.CreateDirectory(t.Pasta.Arquivo + ".bak"); await Cadastrar(t, TipoMidia.Livro, "NumPagina", 12); Assert.Single(t.Salvos); Assert.Equal(12, t.Campo<NumericUpDown>("NumPagina").Value); Assert.Contains("não foram aplicadas", t.Aviso);
    }
    [AvaloniaFact(DisplayName = "UM14 | Progresso de jogo legado | SteamAppId e ausência de IgdbId preservados ao editar horas")]
    public void JogoLegado()
    {
        using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Editar(); t.Campo<NumericUpDown>("NumHorasJogadas").Value = 7.5m; t.Clicar("BtnAtualizar"); var r = Assert.Single(t.Salvos); Assert.Equal(1010, r.SteamAppId); Assert.Null(r.IgdbId); Assert.Equal(7.5m, r.Multimidia!.Progresso!.HorasJogadas);
    }
    [AvaloniaFact(DisplayName = "UM15 | Alternar tipos do formulário | Campos de progresso anteriores limpos e painéis adequados")]
    public void LimparCampos()
    {
        using var t = new TelaTeste(); t.Campo<NumericUpDown>("NumHorasJogadas").Value = 10; TipoCadastro(t, TipoMidia.Livro); Assert.Null(t.Campo<NumericUpDown>("NumHorasJogadas").Value); Assert.True(t.Campo<StackPanel>("ProgressoLivro").IsVisible); Assert.False(t.Campo<StackPanel>("ProgressoJogo").IsVisible);
    }
    [AvaloniaFact(DisplayName = "UM16 | Livro usa texto de status apropriado | Rótulo Lendo mantém estado interno Jogando")]
    public async Task StatusLivro()
    {
        using var t = new TelaTeste(); await Selecionar(t, TipoMidia.Livro); t.Campo<ComboBox>("CmbStatus").SelectedIndex = 1;
        Assert.Equal("Lendo", (t.Campo<ComboBox>("CmbStatus").SelectedItem as ComboBoxItem)!.Content); t.Clicar("BtnAdicionar"); Assert.Equal("Jogando", Assert.Single(t.Salvos).Status);
    }
    [AvaloniaFact(DisplayName = "UM17 | Filtrar status em categoria livro | Rótulos adaptados filtram pelo estado interno correto")]
    public void FiltroLivro()
    {
        var um = Item(TipoMidia.Livro); var dois = um with { Catalogo = um.Catalogo with { Titulo = "Outro simulado" }, Pessoal = um.Pessoal with { Status = "Jogando" } };
        using var t = new TelaTeste(new[] { ConversorBibliotecaJson.ParaRegistro(um), ConversorBibliotecaJson.ParaRegistro(dois) }); TipoBiblioteca(t, TipoMidia.Livro); t.Campo<ComboBox>("CmbFiltroStatus").SelectedIndex = 2; Assert.Equal("Outro simulado", Assert.Single(t.Exibidos).Titulo);
    }
    [AvaloniaFact(DisplayName = "UM18 | Abrir categoria sem obras | Estado vazio claro sem excluir outras mídias")]
    public void CategoriaVazia() { using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); TipoBiblioteca(t, TipoMidia.Livro); Assert.Empty(t.Exibidos); Assert.Single(t.Salvos); Assert.Contains("categoria", t.Campo<TextBlock>("TxtListaVazia").Text!); }
    [AvaloniaFact(DisplayName = "UM19 | Novo cadastro após editar outra mídia | Tipo volta a ser selecionável")]
    public void NovoDepoisEdicao()
    {
        using var t = new TelaTeste(new[] { ConversorBibliotecaJson.ParaRegistro(Item(TipoMidia.Livro)) }); TipoBiblioteca(t, TipoMidia.Livro); t.Editar();
        typeof(MainWindow).GetMethod("LimparFormulario_Click", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.Invoke(t.Janela, new object?[] { null, new Avalonia.Interactivity.RoutedEventArgs() }); Assert.True(t.Campo<ComboBox>("CmbTipoCadastro").IsEnabled); Assert.Null(t.Campo<NumericUpDown>("NumPagina").Value);
    }
    [AvaloniaFact(DisplayName = "UM20 | Capturar telas de seis mídias | Categorias, avisos e progresso renderizam no tamanho mínimo")]
    public async Task Renderizar()
    {
        using var t = new TelaTeste(Enum.GetValues<TipoMidia>().Select(tipo => ConversorBibliotecaJson.ParaRegistro(Item(tipo))), semServico: true);
        t.Janela.Width = 900; t.Janela.Height = 660; var pasta = Path.Combine(Path.GetTempPath(), "backlogmanager-fase2-resultados", "imagens"); Directory.CreateDirectory(pasta);
        foreach (var tipo in Enum.GetValues<TipoMidia>())
        {
            TipoBiblioteca(t, tipo); t.Clicar("NavBacklog"); Dispatcher.UIThread.RunJobs(); t.Janela.UpdateLayout();
            using (var imagem = t.Janela.CaptureRenderedFrame()) { Assert.NotNull(imagem); imagem.Save(Path.Combine(pasta, tipo + "-biblioteca.png"), PngBitmapEncoderOptions.Default); }
            t.Editar(); Dispatcher.UIThread.RunJobs(); t.Janela.UpdateLayout();
            using var formulario = t.Janela.CaptureRenderedFrame(); Assert.NotNull(formulario); formulario.Save(Path.Combine(pasta, tipo + "-formulario.png"), PngBitmapEncoderOptions.Default);
            t.Campo<ScrollViewer>("PaginaCadastro").Offset = new Avalonia.Vector(0, 10000);
            Dispatcher.UIThread.RunJobs(); t.Janela.UpdateLayout();
            using var rodape = t.Janela.CaptureRenderedFrame(); Assert.NotNull(rodape); rodape.Save(Path.Combine(pasta, tipo + "-progresso.png"), PngBitmapEncoderOptions.Default);
            t.Campo<ScrollViewer>("PaginaCadastro").Offset = new Avalonia.Vector(0, 0);
        }
        await Task.CompletedTask;
    }
    [AvaloniaFact(DisplayName = "UM21 | Série com temporada e episódio | Ambos os campos opcionais persistem")]
    public async Task SerieCompleta()
    {
        using var t = new TelaTeste(); await Selecionar(t, TipoMidia.Serie);
        t.Campo<NumericUpDown>("NumTemporada").Value = 2; t.Campo<NumericUpDown>("NumEpisodio").Value = 7; t.Clicar("BtnAdicionar");
        var p = Assert.Single(t.Salvos).Multimidia!.Progresso!; Assert.Equal(2, p.Temporada); Assert.Equal(7, p.Episodio);
    }
    [AvaloniaFact(DisplayName = "UM22 | Mangá com capítulo e volume | Campos próprios preservados incluindo zero")]
    public async Task MangaCompleto()
    {
        using var t = new TelaTeste(); await Selecionar(t, TipoMidia.Manga);
        t.Campo<NumericUpDown>("NumCapituloManga").Value = 0; t.Campo<NumericUpDown>("NumVolume").Value = 2; t.Clicar("BtnAdicionar");
        var p = Assert.Single(t.Salvos).Multimidia!.Progresso!; Assert.Equal(0, p.Capitulo); Assert.Equal(2, p.Volume);
    }
    [AvaloniaFact(DisplayName = "UM23 | Livro com capítulo sem página | Campo opcional vazio permanece nulo")]
    public async Task LivroCapitulo()
    {
        using var t = new TelaTeste(); await Selecionar(t, TipoMidia.Livro);
        t.Campo<NumericUpDown>("NumCapituloLivro").Value = 3; t.Clicar("BtnAdicionar");
        var p = Assert.Single(t.Salvos).Multimidia!.Progresso!; Assert.Equal(3, p.Capitulo); Assert.Null(p.Pagina);
    }
    [AvaloniaFact(DisplayName = "UM24 | Aviso de catálogo simulado | Cinco mídias têm indicação clara e jogos mantêm IGDB")]
    public void AvisoSimulado()
    {
        using var t = new TelaTeste();
        foreach (var tipo in Enum.GetValues<TipoMidia>()) { TipoCadastro(t, tipo); Assert.Equal(tipo != TipoMidia.Jogo, t.Campo<Border>("AvisoCatalogoSimulado").IsVisible); }
    }
    [AvaloniaFact(DisplayName = "UM25 | Limpar busca simulada antes de concluir | Sugestões não reaparecem após limpar")]
    public async Task LimparBusca()
    {
        using var t = new TelaTeste(); TipoCadastro(t, TipoMidia.Livro); t.Campo<TextBox>("TxtTitulo").Text = "simulado";
        await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Background); var busca = t.Janela.BuscaPendente;
        t.Campo<TextBox>("TxtTitulo").Text = ""; await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Background); await busca;
        Assert.False(t.Campo<ListBox>("ListaSugestoes").IsVisible); Assert.Empty(t.Campo<ListBox>("ListaSugestoes").Items);
    }
    [AvaloniaFact(DisplayName = "UM26 | Falha e nova tentativa de cadastro multimídia | Retomada salva uma vez sem duplicar o registro")]
    public async Task RecuperarCadastro()
    {
        using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); Directory.CreateDirectory(t.Pasta.Arquivo + ".bak");
        await Cadastrar(t, TipoMidia.Anime, "NumEpisodiosAssistidos", 5); Assert.Single(t.Salvos);
        Directory.Delete(t.Pasta.Arquivo + ".bak"); t.Clicar("BtnAdicionar"); Assert.Equal(2, t.Salvos.Count); Assert.Single(t.Salvos, r => r.Multimidia?.Tipo == TipoMidia.Anime);
    }

    [AvaloniaFact(DisplayName = "UM27 | Selecionar mídia simulada sem credenciais IGDB | Aviso de jogos indisponíveis não contradiz o catálogo local")]
    public void AvisoIgdbSomenteJogos()
    {
        using var t = new TelaTeste(semServico: true); Assert.True(t.Campo<Border>("AvisoOperacao").IsVisible);
        TipoCadastro(t, TipoMidia.Livro); Assert.False(t.Campo<Border>("AvisoOperacao").IsVisible); Assert.True(t.Campo<Border>("AvisoCatalogoSimulado").IsVisible);
    }

    [AvaloniaFact(DisplayName = "UM28 | Trocar para livro mantendo primeiro status | Campo fechado mostra Quero ler sem conservar texto antigo")]
    public void StatusSelecionadoVisivel()
    {
        using var t = new TelaTeste(); TipoCadastro(t, TipoMidia.Livro); Dispatcher.UIThread.RunJobs();
        Assert.Equal("Quero ler", t.Campo<ComboBox>("CmbStatus").SelectionBoxItem);
    }

}
