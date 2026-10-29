using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using Avalonia.Automation.Peers;
using Avalonia.Automation.Provider;
using BacklogManager.Controls;
using Xunit;
namespace BacklogManager.Tests.Revisao3;

public sealed class ArredondamentoTests
{
    private static CapaObra Capa(Border c) => c.GetVisualDescendants().OfType<CapaObra>().Single();
    private static ListBoxItem Item(ApoioVisual a) => a.Tela.Campo<ListBox>("ListaBacklog").GetVisualDescendants().OfType<ListBoxItem>().First();
    [AvaloniaFact(DisplayName = "RD01 | Grade arredondada | Raio discreto oito com espessura de borda zero")]
    public async Task Grade() { using var a = new ApoioVisual(); a.Biblioteca(); await a.Prontas(); Assert.Equal(new CornerRadius(8), a.Cartoes[0].CornerRadius); Assert.Equal(new Thickness(0), a.Cartoes[0].BorderThickness); }
    [AvaloniaFact(DisplayName = "RD02 | Lista arredondada | Mesmo raio discreto dos cartões da grade")]
    public async Task Lista() { using var a = new ApoioVisual(); a.Biblioteca(modo: 1); await a.Prontas(); var c = a.Tela.Campo<ListBox>("ListaBacklog").GetVisualDescendants().OfType<Border>().Single(b => b.Classes.Contains("gameCard")); Assert.Equal(new CornerRadius(8), c.CornerRadius); }

    [AvaloniaFact(DisplayName = "RD04 | Bitmap preenchido nos cantos | Imagem é realmente recortada e não invade os cantos arredondados")]
    public async Task ClipImagem() { using var a = new ApoioVisual(); a.Biblioteca(); await a.Prontas(); var c = Capa(a.Cartoes[0]); using var frame = a.Tela.Janela.CaptureRenderedFrame(); Assert.NotNull(frame); var canto = ApoioVisual.Pixel(frame, c.TranslatePoint(new Point(1, 1), a.Tela.Janela)!.Value); var centro = ApoioVisual.Pixel(frame, c.TranslatePoint(new Point(20, 20), a.Tela.Janela)!.Value); Assert.NotEqual(centro, canto); Assert.InRange(centro[1], 175, 185); Assert.Equal(8, Assert.IsType<RectangleGeometry>(c.Clip).RadiusY); }
    [AvaloniaFact(DisplayName = "RD05 | Imagem padrão arredondada | Fallback também usa recorte consistente sem virar imagem quadrada")]
    public async Task ClipPadrao() { using var a = new ApoioVisual(new[] { ApoioVisual.Jogo(capa: null) }); a.Biblioteca(); await a.Prontas(); var c = Capa(a.Cartoes[0]); Assert.Equal(8, Assert.IsType<RectangleGeometry>(c.Clip).RadiusX); Assert.Contains(c.GetVisualDescendants().OfType<TextBlock>(), t => t.Text == "Sem capa" && t.IsVisible); }
    [AvaloniaFact(DisplayName = "RD06 | Hover arredondado | Cantos não reativam traço nem faixa externa do tema")]
    public async Task Hover() { using var a = new ApoioVisual(); a.Biblioteca(); await a.Prontas(); a.Tela.Janela.MouseMove(Item(a).TranslatePoint(new Point(20, 20), a.Tela.Janela)!.Value, RawInputModifiers.None); a.Renderizar(); Assert.True(Item(a).IsPointerOver); Assert.Equal(new Thickness(0), a.Cartoes[0].BorderThickness); using var frame = a.Tela.Janela.CaptureRenderedFrame(); Assert.NotNull(frame); var c = a.Cartoes[0]; Assert.Equal(ApoioVisual.Pixel(frame, c.TranslatePoint(new Point(185, 30), a.Tela.Janela)!.Value), ApoioVisual.Pixel(frame, c.TranslatePoint(new Point(215, 30), a.Tela.Janela)!.Value)); }
    [AvaloniaFact(DisplayName = "RD07 | Seleção arredondada | Marca interna permanece visível sem borda externa")]
    public async Task Selecao() { using var a = new ApoioVisual(); a.Biblioteca(); await a.Prontas(); a.Tela.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; a.Renderizar(); Assert.Contains(a.Cartoes[0].GetVisualDescendants().OfType<TextBlock>(), t => t.Text == "✓ Selecionado" && t.IsVisible); Assert.Equal(new Thickness(0), a.Cartoes[0].BorderThickness); }
    [AvaloniaFact(DisplayName = "RD08 | Foco por teclado com cantos recuperados | Texto de foco continua visível sem adorno externo")]
    public async Task Foco() { using var a = new ApoioVisual(); a.Biblioteca(); await a.Prontas(); Item(a).Focus(NavigationMethod.Tab); a.Renderizar(); Assert.Contains(a.Cartoes[0].GetVisualDescendants().OfType<TextBlock>(), t => t.Text == "Foco" && t.IsVisible); Assert.Null(Item(a).FocusAdorner); }
    [AvaloniaFact(DisplayName = "RD09 | Acessibilidade após arredondar | Provider mantém seleção do item")]
    public async Task Acessibilidade() { using var a = new ApoioVisual(); a.Biblioteca(); await a.Prontas(); var p = ControlAutomationPeer.CreatePeerForElement(Item(a))!.GetProvider<ISelectionItemProvider>(); Assert.NotNull(p); a.Tela.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; Assert.True(p.IsSelected); }
    [AvaloniaFact(DisplayName = "RD10 | Alternância das visualizações | Novos controles de capa conservam recorte arredondado")]
    public async Task Alternar() { using var a = new ApoioVisual(); a.Biblioteca(); await a.Prontas(); Assert.Equal(8, Assert.IsType<RectangleGeometry>(Capa(a.Cartoes[0]).Clip).RadiusX); a.Biblioteca(modo: 1); await a.Prontas(); var c = a.Tela.Campo<ListBox>("ListaBacklog").GetVisualDescendants().OfType<CapaObra>().Single(); Assert.Equal(8, Assert.IsType<RectangleGeometry>(c.Clip).RadiusX); }

    [AvaloniaFact(DisplayName = "RD12 | Capturas no tamanho mínimo | Grade uniforme e cantos renderizam nas grade e lista")]
    public async Task Capturas() { using var a = new ApoioVisual(ApoioVisual.Mistos()); a.Tela.Janela.Width = 900; a.Tela.Janela.Height = 660; await a.Prontas(); a.Biblioteca(); await a.Prontas(); a.Tela.Campo<ListBox>("ListaBacklog").SelectedIndex = 1; a.Renderizar(); a.Capturar("grade-uniforme-arredondada"); a.Biblioteca(modo: 1); await a.Prontas(); a.Capturar("lista-arredondada"); }
}
