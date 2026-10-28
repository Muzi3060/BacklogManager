using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Avalonia.VisualTree;
using BacklogManager.Controls;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Tests.Fase2;
using Xunit;
namespace BacklogManager.Tests.AjustesFase3;

public sealed class CapasGradeTests
{
    private static TelaTeste Abrir(int modo = 0)
    {
        var t = new TelaTeste(new[] { ConversorBibliotecaJson.ParaRegistro(ApoioMultimidia.Item(TipoMidia.Livro)) });
        t.Campo<ComboBox>("CmbTipoBiblioteca").SelectedIndex = (int)TipoMidia.Livro;
        t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex = modo; t.Clicar("NavBacklog"); Renderizar(t); return t;
    }
    private static void Renderizar(TelaTeste t) { Dispatcher.UIThread.RunJobs(); t.Janela.UpdateLayout(); }
    private static CapaObra Capa(TelaTeste t) => t.Campo<ListBox>("ListaBacklog").GetVisualDescendants().OfType<CapaObra>().First();
    private static ListBoxItem Cartao(TelaTeste t) => t.Campo<ListBox>("ListaBacklog").GetVisualDescendants().OfType<ListBoxItem>().First();
    private static void Hover(TelaTeste t) { var pos = Cartao(t).TranslatePoint(new Point(20, 20), t.Janela)!.Value; t.Janela.MouseMove(pos, RawInputModifiers.None); Renderizar(t); Assert.True(Cartao(t).IsPointerOver); }
    [AvaloniaFact(DisplayName = "HV01 | Capa na grade | Preenchimento proporcional cobre a área do cartão")]
    public void Preencher() { using var t = Abrir(); Assert.Equal(Stretch.UniformToFill, Capa(t).Preenchimento); Assert.Equal(Stretch.UniformToFill, Capa(t).GetVisualDescendants().OfType<Image>().Single().Stretch); }
    [AvaloniaFact(DisplayName = "HV02 | Capa na lista | Modo reduzido preenche proporcionalmente a miniatura")]
    public void Lista() { using var t = Abrir(1); Assert.Equal(Stretch.UniformToFill, Capa(t).Preenchimento); }
    [AvaloniaFact(DisplayName = "HV03 | Alterar preenchimento da capa | Imagem existente muda sem novo carregamento")]
    public async Task AlterarPreenchimento() { using var t = Abrir(); var capa = Capa(t); await capa.CarregamentoPendente; var imagem = capa.GetVisualDescendants().OfType<Image>().Single(); var fonte = imagem.Source; capa.Preenchimento = Stretch.Uniform; Assert.Same(fonte, imagem.Source); Assert.Equal(Stretch.Uniform, imagem.Stretch); }
    [AvaloniaFact(DisplayName = "HV04 | Hover no cartão | Capa não é substituída, ocultada ou escurecida por opacidade")]
    public async Task HoverImagem() { using var t = Abrir(); var capa = Capa(t); await capa.CarregamentoPendente; var i = capa.GetVisualDescendants().OfType<Image>().Single(); var fonte = i.Source; Hover(t); Assert.Same(fonte, i.Source); Assert.True(i.IsVisible); Assert.Equal(1, i.Opacity); Assert.Equal(1, capa.Opacity); }
    [AvaloniaFact(DisplayName = "HV05 | Hover em item não selecionado | Realce não seleciona ou exclui obra")]
    public void HoverSemSelecao() { using var t = Abrir(); Hover(t); Assert.Equal(-1, t.Campo<ListBox>("ListaBacklog").SelectedIndex); Assert.Single(t.Salvos); }
    [AvaloniaFact(DisplayName = "HV06 | Hover em item selecionado | Seleção e ações de edição permanecem")]
    public void HoverSelecionado() { using var t = Abrir(); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; var selecionado = t.Campo<ListBox>("ListaBacklog").SelectedItem; Hover(t); Assert.Same(selecionado, t.Campo<ListBox>("ListaBacklog").SelectedItem); Assert.True(t.Campo<Button>("BtnEditar").IsEnabled); }
    [AvaloniaFact(DisplayName = "HV07 | Sair do hover | Cartão conserva capa e dados pessoais")]
    public async Task Sair() { using var t = Abrir(); await Capa(t).CarregamentoPendente; Hover(t); t.Janela.MouseMove(new Point(10, 10), RawInputModifiers.None); Renderizar(t); Assert.False(Cartao(t).IsPointerOver); Assert.Contains(Capa(t).GetVisualDescendants().OfType<Image>(), i => i.Source != null && i.IsVisible); Assert.Single(t.Exibidos); }
    [AvaloniaFact(DisplayName = "HV08 | Grade compacta | Preenchimento mantido ao reduzir para 900 por 660")]
    public void Compacta() { using var t = Abrir(); t.Janela.Width = 900; t.Janela.Height = 660; Renderizar(t); Assert.Equal(104, Capa(t).Bounds.Height); Assert.Equal(Stretch.UniformToFill, Capa(t).Preenchimento); }
    [AvaloniaFact(DisplayName = "HV09 | Alternar grade e lista | Ambos os templates preenchem a área proporcionalmente")]
    public void Alternar() { using var t = Abrir(); t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex = 1; Renderizar(t); Assert.Equal(Stretch.UniformToFill, Capa(t).Preenchimento); t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex = 0; Renderizar(t); Assert.Equal(Stretch.UniformToFill, Capa(t).Preenchimento); }
    [AvaloniaFact(DisplayName = "HV10 | Comparar pixels antes e durante hover | Centro da capa mantém os mesmos pixels sem sobreposição")]
    public async Task Pixels()
    {
        using var t = Abrir(); await Capa(t).CarregamentoPendente; Renderizar(t); t.Janela.MouseMove(new Point(10, 10), RawInputModifiers.None); Renderizar(t);
        using var antes = t.Janela.CaptureRenderedFrame(); Assert.NotNull(antes); var pos = Capa(t).TranslatePoint(new Point(40, 40), t.Janela)!.Value;
        var bytesAntes = Pixel(antes, pos);
        Hover(t); using var depois = t.Janela.CaptureRenderedFrame(); Assert.NotNull(depois); var bytesDepois = Pixel(depois, pos); Assert.Equal(bytesAntes, bytesDepois);
        var pasta = Path.Combine(Path.GetTempPath(), "backlogmanager-revisao2-resultados", "imagens"); Directory.CreateDirectory(pasta); antes.Save(Path.Combine(pasta, "grade-sem-hover.png"), PngBitmapEncoderOptions.Default); depois.Save(Path.Combine(pasta, "grade-hover.png"), PngBitmapEncoderOptions.Default);
    }
    private static byte[] Pixel(Bitmap imagem, Point ponto)
    {
        var memoria = System.Runtime.InteropServices.Marshal.AllocHGlobal(4);
        try { imagem.CopyPixels(new PixelRect((int)ponto.X, (int)ponto.Y, 1, 1), memoria, 4, 4); var bytes = new byte[4]; System.Runtime.InteropServices.Marshal.Copy(memoria, bytes, 0, 4); return bytes; }
        finally { System.Runtime.InteropServices.Marshal.FreeHGlobal(memoria); }
    }
}
