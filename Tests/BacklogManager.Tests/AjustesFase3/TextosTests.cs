using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Avalonia.VisualTree;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Tests.Fase2;
using Xunit;
namespace BacklogManager.Tests.AjustesFase3;

public sealed class TextosTests
{
    private static string[] Textos(TelaTeste t) => t.Campo<ScrollViewer>("PaginaInicio").GetVisualDescendants().OfType<TextBlock>().Select(b => b.Text ?? "").ToArray();
    [AvaloniaFact(DisplayName = "TX01 | Página inicial vazia | Mensagens falam de obras e biblioteca")]
    public void Vazia() { using var t = new TelaTeste(); Dispatcher.UIThread.RunJobs(); Assert.Contains("Bem-vindo à sua biblioteca", Textos(t)); Assert.Contains("Sua coleção começa aqui. Adicione sua primeira obra!", Textos(t)); Assert.Contains("primeira obra", t.Campo<TextBlock>("TxtResumo").Text!); }
    [AvaloniaFact(DisplayName = "TX02 | Chamada para cadastro na página inicial | Convite é genérico para todas as mídias")]
    public void Convite() { using var t = new TelaTeste(); Dispatcher.UIThread.RunJobs(); Assert.Contains("Qual será sua próxima obra?", Textos(t)); Assert.Contains("Progresso da biblioteca", Textos(t)); }
    [AvaloniaFact(DisplayName = "TX03 | Biblioteca com livro concluído | Resumo usa obras e mantém contagem correta")]
    public void Livro() { var i = ApoioMultimidia.Item(TipoMidia.Livro); i = i with { Pessoal = i.Pessoal with { Status = "Zerado" } }; using var t = new TelaTeste(new[] { ConversorBibliotecaJson.ParaRegistro(i) }); Assert.Equal("1 de 1 obras concluídas · 0 na fila.", t.Campo<TextBlock>("TxtResumo").Text); Assert.Equal("100% concluído", t.Campo<TextBlock>("TxtProgresso").Text); }
    [AvaloniaFact(DisplayName = "TX04 | Resumo de seis mídias | Contadores locais incluem todas as categorias")]
    public void Mista() { using var t = new TelaTeste(Enum.GetValues<TipoMidia>().Select(tipo => ConversorBibliotecaJson.ParaRegistro(ApoioMultimidia.Item(tipo)))); Assert.Equal("6", t.Campo<TextBlock>("TxtTotal").Text); Assert.Contains("6 obras", t.Campo<TextBlock>("TxtResumo").Text!); }
    [AvaloniaFact(DisplayName = "TX05 | Recente de livro na página inicial | Tooltip diz obra e edição abre o tipo correto")]
    public void Recente() { using var t = new TelaTeste(new[] { ConversorBibliotecaJson.ParaRegistro(ApoioMultimidia.Item(TipoMidia.Livro)) }); Dispatcher.UIThread.RunJobs(); t.Janela.UpdateLayout(); var botao = t.Campo<ItemsControl>("ListaRecentes").GetVisualDescendants().OfType<Button>().First(); Assert.Equal("Abrir esta obra para editar", ToolTip.GetTip(botao)); botao.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Button.ClickEvent)); Assert.Equal((int)TipoMidia.Livro, t.Campo<ComboBox>("CmbTipoCadastro").SelectedIndex); }
    [AvaloniaFact(DisplayName = "TX06 | Placeholder de review no cadastro | Texto genérico sem referência exclusiva a jogo")]
    public void ReviewNova() { using var t = new TelaTeste(); var texto = t.Campo<TextBox>("TxtReview").PlaceholderText; Assert.Contains("obra", texto!); Assert.DoesNotContain("jogo", texto!); }
    [AvaloniaFact(DisplayName = "TX07 | Trocar mídia no cadastro | Placeholder de review continua adequado às seis mídias")]
    public void ReviewMidias() { using var t = new TelaTeste(); foreach (var tipo in Enum.GetValues<TipoMidia>()) { t.Campo<ComboBox>("CmbTipoCadastro").SelectedIndex = (int)tipo; Assert.Contains("obra", t.Campo<TextBox>("TxtReview").PlaceholderText!); } }
    [AvaloniaFact(DisplayName = "TX08 | Editar review legada | Novo placeholder não modifica o conteúdo pessoal")]
    public void ReviewAntiga() { var r = PastaTeste.Jogo(); r.Review = "Minha história\ncom acentos 🎮"; using var t = new TelaTeste(new[] { r }); t.Editar(); Assert.Equal(r.Review, t.Campo<TextBox>("TxtReview").Text); t.Clicar("BtnAtualizar"); Assert.Equal(r.Review, Assert.Single(t.Salvos).Review); }
    [AvaloniaFact(DisplayName = "TX09 | Excluir última obra | Mensagem vazia volta a ser genérica")]
    public void RemoverUltima() { using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; t.Clicar("BtnRemover"); Assert.Contains("primeira obra", t.Campo<TextBlock>("TxtResumo").Text!); Assert.True(t.Campo<Border>("EstadoInicioVazio").IsVisible); }
    [AvaloniaFact(DisplayName = "TX10 | Página inicial no tamanho mínimo | Textos multimídia renderizam e navegação permanece acessível")]
    public void Minimo() { using var t = new TelaTeste(new[] { ConversorBibliotecaJson.ParaRegistro(ApoioMultimidia.Item(TipoMidia.Anime)) }); t.Janela.Width = 900; t.Janela.Height = 660; Dispatcher.UIThread.RunJobs(); t.Janela.UpdateLayout(); using var imagem = t.Janela.CaptureRenderedFrame(); Assert.NotNull(imagem); var pasta = Path.Combine(Path.GetTempPath(), "backlogmanager-revisao3-resultados", "imagens"); Directory.CreateDirectory(pasta); imagem.Save(Path.Combine(pasta, "inicio-multimidia.png"), PngBitmapEncoderOptions.Default); Assert.True(t.Campo<Button>("NavBacklog").Bounds.Height > 0); }
    [AvaloniaFact(DisplayName = "TX11 | Anime em andamento e filme concluído | Rótulos e contagens da página inicial abrangem mídias diferentes")]
    public void EstadosMistos() { var a = ApoioMultimidia.Item(TipoMidia.Anime); a = a with { Pessoal = a.Pessoal with { Status = "Jogando" } }; var f = ApoioMultimidia.Item(TipoMidia.Filme); f = f with { Pessoal = f.Pessoal with { Status = "Zerado" } }; using var t = new TelaTeste(new[] { ConversorBibliotecaJson.ParaRegistro(a), ConversorBibliotecaJson.ParaRegistro(f) }); Dispatcher.UIThread.RunJobs(); Assert.Equal("1", t.Campo<TextBlock>("TxtJogando").Text); Assert.Equal("1", t.Campo<TextBlock>("TxtZerados").Text); Assert.Contains("Em andamento", Textos(t)); Assert.Contains("Concluídas", Textos(t)); }
    [AvaloniaFact(DisplayName = "TX12 | Recentes de leitura e reprodução | Status dos cartões continuam adaptados à mídia com tooltip genérico")]
    public void RecentesMistos() { var l = ApoioMultimidia.Item(TipoMidia.Livro); l = l with { Pessoal = l.Pessoal with { Status = "Jogando" } }; var a = ApoioMultimidia.Item(TipoMidia.Anime); a = a with { Pessoal = a.Pessoal with { Status = "Jogando" } }; using var t = new TelaTeste(new[] { ConversorBibliotecaJson.ParaRegistro(l), ConversorBibliotecaJson.ParaRegistro(a) }); Dispatcher.UIThread.RunJobs(); t.Janela.UpdateLayout(); var recentes = t.Campo<ItemsControl>("ListaRecentes"); var textos = recentes.GetVisualDescendants().OfType<TextBlock>().Select(b => b.Text).ToArray(); Assert.Contains("Lendo", textos); Assert.Contains("Assistindo", textos); Assert.All(recentes.GetVisualDescendants().OfType<Button>(), b => Assert.Equal("Abrir esta obra para editar", ToolTip.GetTip(b))); }
    [AvaloniaFact(DisplayName = "TX13 | Auditoria dos textos estáticos da página inicial | Nenhuma mensagem exige que a biblioteca seja de jogos")]
    public void SemTextoExclusivo() { using var t = new TelaTeste(); Dispatcher.UIThread.RunJobs(); Assert.DoesNotContain(Textos(t), texto => System.Text.RegularExpressions.Regex.IsMatch(texto, @"\bjogos?\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase)); }
}
