using System.Net;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Headless;
using Avalonia.Threading;
using Xunit;

namespace BacklogManager.Tests;

public sealed class BuscaJanelaTests
{
    private static async Task Digitar(TelaTeste t, string texto)
    {
        t.Campo<TextBox>("TxtTitulo").Text = texto;
        await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Background);
    }
    private static Task Concluir(TelaTeste t) => t.Janela.BuscaPendente.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);
    private static TaskCompletionSource Sinal() => new(TaskCreationOptions.RunContinuationsAsynchronously);

    [AvaloniaFact(DisplayName = "B01 | Digitação rápida | Somente o termo final chega ao HTTP")]
    public async Task Debounce()
    {
        using var t = new TelaTeste(); await Digitar(t, "Primeiro"); var anterior = t.Janela.BuscaPendente;
        await Digitar(t, "Segundo"); await Concluir(t); await anterior; Assert.Equal(1, t.Http.Chamadas);
    }
    [AvaloniaFact(DisplayName = "B02 | Limpar antes da espera | Nenhuma requisição enviada")]
    public async Task LimparAntes()
    {
        using var t = new TelaTeste(); await Digitar(t, "Jogo"); var anterior = t.Janela.BuscaPendente;
        await Digitar(t, ""); await anterior; Assert.Equal(0, t.Http.Chamadas); Assert.False(t.Campo<ListBox>("ListaSugestoes").IsVisible);
    }
    [AvaloniaFact(DisplayName = "B03 | Trocar texto com HTTP ativo | Cancelamento chega ao transporte")]
    public async Task CancelamentoHttp()
    {
        using var t = new TelaTeste(); var iniciou = Sinal(); var cancelou = Sinal();
        t.Http.Responder = async (_, token) => { iniciou.TrySetResult(); try { await Task.Delay(Timeout.Infinite, token); } catch (OperationCanceledException) { cancelou.TrySetResult(); throw; } return HttpSimulado.Json("[]"); };
        await Digitar(t, "Primeiro"); var anterior = t.Janela.BuscaPendente; await iniciou.Task.WaitAsync(TimeSpan.FromSeconds(3), TestContext.Current.CancellationToken);
        await Digitar(t, ""); await cancelou.Task.WaitAsync(TimeSpan.FromSeconds(3), TestContext.Current.CancellationToken); await anterior;
        Assert.False(t.Campo<ListBox>("ListaSugestoes").IsVisible);
    }
    [AvaloniaFact(DisplayName = "B04 | Resposta antiga chega depois | Resultado antigo não reaparece")]
    public async Task RespostaAntiga()
    {
        using var t = new TelaTeste(); var iniciou = Sinal(); var liberar = Sinal();
        t.Http.Responder = async (_, _) => { iniciou.TrySetResult(); await liberar.Task; return HttpSimulado.Json("[{\"id\":1,\"name\":\"Antigo\"}]"); };
        await Digitar(t, "Antigo"); var anterior = t.Janela.BuscaPendente; await iniciou.Task.WaitAsync(TimeSpan.FromSeconds(3), TestContext.Current.CancellationToken);
        await Digitar(t, ""); liberar.SetResult(); await anterior; Assert.False(t.Campo<ListBox>("ListaSugestoes").IsVisible); Assert.Empty(t.Campo<ListBox>("ListaSugestoes").Items);
    }
    [AvaloniaFact(DisplayName = "B05 | Erro antigo após nova pesquisa | Aviso atual não é substituído")]
    public async Task ErroAntigo()
    {
        using var t = new TelaTeste(); var iniciou = Sinal(); var liberar = Sinal();
        t.Http.Responder = async (_, _) => { iniciou.TrySetResult(); await liberar.Task; throw new HttpRequestException("Falha antiga"); };
        await Digitar(t, "Antigo"); var anterior = t.Janela.BuscaPendente; await iniciou.Task.WaitAsync(TimeSpan.FromSeconds(3), TestContext.Current.CancellationToken);
        await Digitar(t, ""); var aviso = t.Aviso; liberar.SetResult(); await anterior; Assert.Equal(aviso, t.Aviso);
    }
    [AvaloniaFact(DisplayName = "B06 | Entrar em edição com busca ativa | Busca cancelada sem sugestões na edição")]
    public async Task EditarCancela()
    {
        using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); await Digitar(t, "Pesquisa"); var anterior = t.Janela.BuscaPendente;
        t.Editar(); await anterior; await Concluir(t); Assert.Equal(0, t.Http.Chamadas); Assert.True(t.Campo<TextBox>("TxtTitulo").IsReadOnly); Assert.False(t.Campo<ListBox>("ListaSugestoes").IsVisible);
    }
    [AvaloniaFact(DisplayName = "B07 | Navegar para início durante busca | Pesquisa cancelada")]
    public async Task NavegarCancela()
    {
        using var t = new TelaTeste(); await Digitar(t, "Jogo"); var anterior = t.Janela.BuscaPendente;
        t.Clicar("NavInicio"); await anterior; Assert.Equal(0, t.Http.Chamadas); Assert.True(t.Campo<ScrollViewer>("PaginaInicio").IsVisible);
    }
    [AvaloniaFact(DisplayName = "B08 | Novo jogo durante pesquisa | Busca anterior cancelada e formulário limpo")]
    public async Task NovoCancela()
    {
        using var t = new TelaTeste(); await Digitar(t, "Jogo"); var anterior = t.Janela.BuscaPendente;
        // O botão de navegação abre o cadastro; NovoJogo é acionado por seu evento real via reflexão de teste.
        typeof(MainWindow).GetMethod("NovoJogo_Click", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
            .Invoke(t.Janela, new object?[] { null, new Avalonia.Interactivity.RoutedEventArgs() });
        await anterior; await Concluir(t); Assert.Equal(0, t.Http.Chamadas); Assert.Equal("", t.Campo<TextBox>("TxtTitulo").Text);
    }
    [AvaloniaFact(DisplayName = "B09 | Fechar com HTTP ativo | Requisição cancelada sem atualizar janela fechada")]
    public async Task FecharCancela()
    {
        using var t = new TelaTeste(); var iniciou = Sinal(); var cancelou = Sinal();
        t.Http.Responder = async (_, token) => { iniciou.TrySetResult(); try { await Task.Delay(Timeout.Infinite, token); } catch (OperationCanceledException) { cancelou.TrySetResult(); throw; } return HttpSimulado.Json("[]"); };
        await Digitar(t, "Jogo"); var busca = t.Janela.BuscaPendente; await iniciou.Task.WaitAsync(TimeSpan.FromSeconds(3), TestContext.Current.CancellationToken);
        t.Janela.Close(); await cancelou.Task.WaitAsync(TimeSpan.FromSeconds(3), TestContext.Current.CancellationToken); await busca;
    }
    [AvaloniaFact(DisplayName = "B10 | Timeout na janela | Aviso específico e coleção continua acessível")]
    public async Task TimeoutAvisado()
    {
        using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Http.Responder = (_, _) => throw new TaskCanceledException();
        await Digitar(t, "Jogo"); await Concluir(t); Assert.Contains("demorou", t.Aviso); Assert.Single(t.Exibidos);
    }
    [AvaloniaFact(DisplayName = "B11 | Falha de rede na janela | Aviso sem encerramento")]
    public async Task RedeAvisada()
    {
        using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Http.Responder = (_, _) => throw new HttpRequestException();
        await Digitar(t, "Jogo"); await Concluir(t); Assert.Contains("conexão", t.Aviso); Assert.Single(t.Exibidos);
    }
    [AvaloniaFact(DisplayName = "B12 | Resposta inválida na janela | Aviso sem lista de sugestões enganosa")]
    public async Task JsonAvisado()
    {
        using var t = new TelaTeste(); t.Http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json("{"));
        await Digitar(t, "Jogo"); await Concluir(t); Assert.Contains("inválida", t.Aviso); Assert.False(t.Campo<ListBox>("ListaSugestoes").IsVisible);
    }
    [AvaloniaFact(DisplayName = "B13 | Buscar novamente após erro | Sugestões válidas apresentadas")]
    public async Task RecuperarBusca()
    {
        using var t = new TelaTeste(); t.Http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json("{}", HttpStatusCode.ServiceUnavailable));
        await Digitar(t, "Primeiro"); await Concluir(t); Assert.Contains("indisponível", t.Aviso);
        t.Http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json("[{\"id\":42,\"name\":\"Segundo\"}]"));
        await Digitar(t, "Segundo"); await Concluir(t); Assert.True(t.Campo<ListBox>("ListaSugestoes").IsVisible); Assert.Single(t.Campo<ListBox>("ListaSugestoes").Items);
    }
    [AvaloniaFact(DisplayName = "B14 | Pesquisa válida sem resultados | Mensagem distinta de erro")]
    public async Task Vazia()
    {
        using var t = new TelaTeste(); await Digitar(t, "Ausente"); await Concluir(t);
        Assert.Equal("Nenhum resultado", t.Campo<TextBlock>("TxtAvisoTitulo").Text); Assert.DoesNotContain("erro", t.Campo<Border>("AvisoOperacao").Classes);
    }
    [AvaloniaFact(DisplayName = "B15 | Sem credenciais | Coleção abre e edição local funciona")]
    public async Task SemCredenciais()
    {
        using var t = new TelaTeste(new[] { PastaTeste.Jogo() }, semServico: true); Assert.Single(t.Exibidos);
        await Digitar(t, "Jogo"); await Concluir(t); Assert.Equal(0, t.Http.Chamadas); Assert.Contains("credenciais", t.Aviso);
        t.Editar(); t.Campo<NumericUpDown>("NumNota").Value = 2; t.Clicar("BtnAtualizar"); Assert.Equal(2, Assert.Single(t.Salvos).Nota);
    }
    [AvaloniaFact(DisplayName = "B16 | Alterar texto após selecionar | Seleção antiga não pode cadastrar outro título")]
    public async Task InvalidarSelecao()
    {
        using var t = new TelaTeste(); t.Selecionar(); await Digitar(t, "Outro título"); t.Clicar("BtnAdicionar");
        Assert.Empty(t.Exibidos); Assert.False(File.Exists(t.Pasta.Arquivo)); await Digitar(t, "");
    }
    [AvaloniaFact(DisplayName = "B17 | Títulos iguais com IDs diferentes | ID da sugestão escolhida é utilizado")]
    public void MesmoTitulo()
    {
        using var t = new TelaTeste(); var jogos = new[] { new BacklogManager.Models.ObraCatalogo { Titulo = "Mesmo", Origem = "IGDB", IdentificadorExterno = "1" }, new BacklogManager.Models.ObraCatalogo { Titulo = "Mesmo", Origem = "IGDB", IdentificadorExterno = "2" } };
        t.Campo<ListBox>("ListaSugestoes").ItemsSource = jogos; t.Campo<ListBox>("ListaSugestoes").SelectedIndex = 1;
        t.Clicar("BtnAdicionar"); Assert.Equal(2, Assert.Single(t.Salvos).IgdbId);
    }
    [AvaloniaFact(DisplayName = "B18 | Título de um caractere | Pesquisa existente continua aceita")]
    public async Task UmCaractere() { using var t = new TelaTeste(); await Digitar(t, "A"); await Concluir(t); Assert.Equal(1, t.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "B19 | Abrir edição | Nenhuma pesquisa HTTP automática")]
    public async Task EdicaoNaoPesquisa() { using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Editar(); await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Background); await Concluir(t); Assert.Equal(0, t.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "B20 | Exceção inesperada do transporte | Barreira do evento evita encerramento e dados intactos")]
    public async Task FalhaInesperada()
    {
        using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); var bytes = File.ReadAllBytes(t.Pasta.Arquivo);
        t.Http.Responder = (_, _) => throw new InvalidOperationException("segredo-simulado"); await Digitar(t, "Jogo"); await Concluir(t);
        Assert.Contains("Não foi possível", t.Aviso); Assert.DoesNotContain("segredo", t.Aviso); Assert.Equal(bytes, File.ReadAllBytes(t.Pasta.Arquivo));
    }
    [AvaloniaFact(DisplayName = "B21 | Busca volta a funcionar | Aviso antigo de falha removido")]
    public async Task LimparErroAntigo()
    {
        using var t = new TelaTeste(); t.Http.Responder = (_, _) => throw new HttpRequestException();
        await Digitar(t, "Primeiro"); await Concluir(t);
        t.Http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json("[{\"id\":42,\"name\":\"Segundo\"}]"));
        await Digitar(t, "Segundo"); await Concluir(t);
        Assert.False(t.Campo<Border>("AvisoOperacao").IsVisible);
    }
    [AvaloniaFact(DisplayName = "B22 | Texto alterado e cadastro imediato | Seleção anterior recusada antes do evento enfileirado")]
    public void AlteracaoImediata()
    {
        using var t = new TelaTeste(); t.Selecionar(); t.Campo<TextBox>("TxtTitulo").Text = "Outro";
        t.Clicar("BtnAdicionar"); Assert.Empty(t.Exibidos); Assert.False(File.Exists(t.Pasta.Arquivo));
    }

    [AvaloniaFact(DisplayName = "B23 | Renderizar telas com dados simulados | Quadros de início, lista e erro capturados")]
    public void RenderizarTelas()
    {
        using var t = new TelaTeste(Enumerable.Range(1, 11).Select(n => PastaTeste.Jogo($"Jogo simulado {n}")), semServico: true);
        var pasta = Path.Combine(Path.GetTempPath(), "backlogmanager-fase1-resultados", "imagens");
        Directory.CreateDirectory(pasta);
        void Capturar(string nome)
        {
            Dispatcher.UIThread.RunJobs();
            t.Janela.UpdateLayout();
            using var imagem = t.Janela.CaptureRenderedFrame();
            Assert.NotNull(imagem);
            Assert.True(imagem.PixelSize.Width >= 900);
            imagem.Save(Path.Combine(pasta, nome + ".png"), Avalonia.Media.Imaging.PngBitmapEncoderOptions.Default);
        }
        Capturar("inicio-sem-credenciais");
        t.Clicar("NavBacklog");
        Capturar("colecao-local");
        t.Janela.Width = 900;
        t.Janela.Height = 660;
        t.Editar();
        Directory.CreateDirectory(t.Pasta.Arquivo + ".bak");
        t.Clicar("BtnAtualizar");
        Capturar("falha-gravacao-janela-minima");
    }

}
