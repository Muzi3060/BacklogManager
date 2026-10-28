using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Avalonia.Automation.Peers;
using Avalonia.Automation.Provider;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Tests.Fase2;
using Xunit;
namespace BacklogManager.Tests.Revisao2;

public sealed class GradeRefinadaTests
{
    private static TelaTeste Tela(bool completo = false)
    {
        var i = ApoioMultimidia.Item(TipoMidia.Livro, completo ? new() { Pagina = 10 } : null);
        var t = new TelaTeste(new[] { ConversorBibliotecaJson.ParaRegistro(i) }); t.Campo<ComboBox>("CmbTipoBiblioteca").SelectedIndex = (int)TipoMidia.Livro; t.Clicar("NavBacklog"); Renderizar(t); return t;
    }
    private static void Renderizar(TelaTeste t) { Dispatcher.UIThread.RunJobs(); t.Janela.UpdateLayout(); }
    private static ListBoxItem Item(TelaTeste t) => t.Campo<ListBox>("ListaBacklog").GetVisualDescendants().OfType<ListBoxItem>().First();
    private static Border Cartao(TelaTeste t) => Item(t).GetVisualDescendants().OfType<Border>().First(b => b.Classes.Contains("gradeCard"));
    private static TextBlock Selecao(TelaTeste t) => Item(t).GetVisualDescendants().OfType<TextBlock>().First(b => b.Classes.Contains("textoSelecao"));
    [AvaloniaFact(DisplayName = "CG01 | Grade sem hover | Cartão mantém cantos discretos sem traço externo")]
    public void SemContorno() { using var t = Tela(); Assert.Equal(new Thickness(0), Cartao(t).BorderThickness); Assert.Equal(new CornerRadius(8), Cartao(t).CornerRadius); }
    [AvaloniaFact(DisplayName = "CG02 | Hover real na grade | Contorno continua ausente")]
    public void Hover() { using var t = Tela(); t.Janela.MouseMove(Item(t).TranslatePoint(new Point(20, 20), t.Janela)!.Value, RawInputModifiers.None); Renderizar(t); Assert.True(Item(t).IsPointerOver); Assert.Equal(new Thickness(0), Cartao(t).BorderThickness); }
    [AvaloniaFact(DisplayName = "CG03 | Selecionar cartão | Marca interna e texto indicam seleção sem contorno")]
    public void Selecionar() { using var t = Tela(); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; Renderizar(t); Assert.True(Selecao(t).IsVisible); Assert.Equal("✓ Selecionado", Selecao(t).Text); Assert.Equal(new Thickness(0), Cartao(t).BorderThickness); }
    [AvaloniaFact(DisplayName = "CG04 | Desfazer seleção | Marca de seleção desaparece sem apagar dados")]
    public void Deselecionar() { using var t = Tela(); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; t.Campo<ListBox>("ListaBacklog").SelectedIndex = -1; Renderizar(t); Assert.False(Selecao(t).IsVisible); Assert.Single(t.Salvos); }
    [AvaloniaFact(DisplayName = "CG05 | Foco por teclado | Texto de foco visível sem adorno externo")]
    public void Foco() { using var t = Tela(); Item(t).Focus(NavigationMethod.Tab); Renderizar(t); var foco = Item(t).GetVisualDescendants().OfType<TextBlock>().First(b => b.Classes.Contains("textoFoco")); Assert.True(foco.IsVisible); Assert.Null(Item(t).FocusAdorner); Assert.Equal(new Thickness(0), Cartao(t).BorderThickness); }
    [AvaloniaFact(DisplayName = "CG06 | Acessibilidade da seleção | Provider informa seleção verdadeira após selecionar")]
    public void Acessibilidade() { using var t = Tela(); var peer = ControlAutomationPeer.CreatePeerForElement(Item(t)); var provider = peer!.GetProvider<ISelectionItemProvider>(); Assert.NotNull(provider); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; Assert.True(provider.IsSelected); }
    [AvaloniaFact(DisplayName = "CG07 | Altura do cartão comum | Altura automática evita a reserva fixa de 352 pixels")]
    public void Altura() { using var t = Tela(); Assert.True(double.IsNaN(Cartao(t).Height)); Assert.True(Cartao(t).Bounds.Height < 352); Assert.True(Cartao(t).Bounds.Height > 174); }
    [AvaloniaFact(DisplayName = "CG08 | Janela mínima | Cartão compacto também usa altura de conteúdo")]
    public void Compacto() { using var t = Tela(); t.Janela.Width = 900; t.Janela.Height = 660; Renderizar(t); Assert.True(double.IsNaN(Cartao(t).Height)); Assert.True(Cartao(t).Bounds.Height < 270); Assert.Equal(new Thickness(0), Cartao(t).BorderThickness); }
    [AvaloniaFact(DisplayName = "CG09 | Cartão com progresso | Altura permanece igual aos cartões sem progresso")]
    public void Conteudo() { using var comum = Tela(); var altura = Cartao(comum).Bounds.Height; using var completo = Tela(true); Assert.Equal(altura, Cartao(completo).Bounds.Height); Assert.Contains(Cartao(completo).GetVisualDescendants().OfType<TextBlock>(), b => b.Text != null && b.Text.Contains("Página 10")); }
    [AvaloniaFact(DisplayName = "CG10 | Seleção e foco juntos | Ações de edição continuam habilitadas e ID não muda")]
    public void Acoes() { using var t = Tela(); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; Item(t).Focus(NavigationMethod.Tab); Assert.True(t.Campo<Button>("BtnEditar").IsEnabled); Assert.True(t.Campo<Button>("BtnRemover").IsEnabled); t.Clicar("BtnEditar"); Assert.Equal((int)TipoMidia.Livro, t.Campo<ComboBox>("CmbTipoCadastro").SelectedIndex); }
    [AvaloniaFact(DisplayName = "CG11 | Trocar visualização com seleção | Seleção permanece ao retornar à grade sem contorno")]
    public void Trocar() { using var t = Tela(); var lista = t.Campo<ListBox>("ListaBacklog"); lista.SelectedIndex = 0; var item = lista.SelectedItem; t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex = 1; t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex = 0; Renderizar(t); Assert.Same(item, lista.SelectedItem); Assert.True(Selecao(t).IsVisible); Assert.Equal(new Thickness(0), Cartao(t).BorderThickness); }
    [AvaloniaFact(DisplayName = "CG12 | Captura de grade selecionada no mínimo | Marca interna e conteúdo renderizam sem contorno externo")]
    public void Captura() { using var t = Tela(true); t.Janela.Width = 900; t.Janela.Height = 660; t.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; Item(t).Focus(NavigationMethod.Tab); Renderizar(t); using var imagem = t.Janela.CaptureRenderedFrame(); Assert.NotNull(imagem); var pasta = Path.Combine(Path.GetTempPath(), "backlogmanager-revisao3-resultados", "imagens"); Directory.CreateDirectory(pasta); imagem.Save(Path.Combine(pasta, "grade-selecionada-foco.png"), PngBitmapEncoderOptions.Default); }
    private static byte[] Pixel(Bitmap imagem, Point p)
    {
        var memoria = System.Runtime.InteropServices.Marshal.AllocHGlobal(4);
        try { imagem.CopyPixels(new PixelRect((int)p.X, (int)p.Y, 1, 1), memoria, 4, 4); var bytes = new byte[4]; System.Runtime.InteropServices.Marshal.Copy(memoria, bytes, 0, 4); return bytes; }
        finally { System.Runtime.InteropServices.Marshal.FreeHGlobal(memoria); }
    }
    private static void MargemSemCamada(TelaTeste t)
    {
        using var imagem = t.Janela.CaptureRenderedFrame(); Assert.NotNull(imagem);
        var margem = Cartao(t).TranslatePoint(new Point(Cartao(t).Bounds.Width + 5, 30), t.Janela)!.Value;
        var fundo = Cartao(t).TranslatePoint(new Point(Cartao(t).Bounds.Width + 35, 30), t.Janela)!.Value;
        Assert.Equal(Pixel(imagem, fundo), Pixel(imagem, margem));
    }
    [AvaloniaFact(DisplayName = "CG13 | Margem externa com seleção e foco | Pixels fora do cartão não recebem camada do tema")]
    public void MargemSelecionada() { using var t = Tela(); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; Item(t).Focus(NavigationMethod.Tab); Renderizar(t); MargemSemCamada(t); }
    [AvaloniaFact(DisplayName = "CG14 | Margem externa durante hover | Fundo além do cartão permanece igual ao fundo da página")]
    public void MargemHover() { using var t = Tela(); t.Janela.MouseMove(Item(t).TranslatePoint(new Point(20, 20), t.Janela)!.Value, RawInputModifiers.None); Renderizar(t); MargemSemCamada(t); }
}
