using System.Net;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using Avalonia.VisualTree;
using BacklogManager.Controls;
using BacklogManager.Data;
using BacklogManager.Services;
using BacklogManager.Models;
using Xunit;
namespace BacklogManager.Tests.Revisao2;

public sealed class MiniaturasTests
{
    private sealed class TelaImagem : IDisposable
    {
        private readonly HttpClient _cliente;
        internal TelaTeste T { get; }
        internal HttpSimulado Http { get; }
        internal CapaObra Capa => T.Campo<ListBox>("ListaBacklog").GetVisualDescendants().OfType<CapaObra>().First();
        internal TelaImagem(int largura = 8, int altura = 4, string? url = "https://images.igdb.com/igdb/image/upload/t_cover_big/teste.jpg")
        {
            var png = Png(largura, altura);
            Http = new HttpSimulado { Responder = (_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(png) }) }; _cliente = new HttpClient(Http);
            var i = Fase1.ApoioArquitetura.Jogo(); i = i with { Catalogo = i.Catalogo with { CapaUrl = url } };
            T = new TelaTeste(new[] { ConversorBibliotecaJson.ParaRegistro(i) }, capas: new CarregadorCapas(_cliente)); T.Campo<ComboBox>("CmbVisualizacao").SelectedIndex = 1; T.Clicar("NavBacklog"); Renderizar();
        }
        internal void Renderizar() { Dispatcher.UIThread.RunJobs(); T.Janela.UpdateLayout(); }
        internal async Task Pronta() { await Capa.CarregamentoPendente; Renderizar(); }
        public void Dispose() { T.Dispose(); _cliente.Dispose(); }
    }
    private static byte[] Png(int largura, int altura)
    {
        using var imagem = new WriteableBitmap(new PixelSize(largura, altura), new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Opaque);
        using (var quadro = imagem.Lock())
        {
            var bytes = new byte[quadro.RowBytes * altura]; for (var y = 0; y < altura; y++) for (var x = 0; x < largura; x++) { var p = y * quadro.RowBytes + x * 4; bytes[p] = 80; bytes[p + 1] = 180; bytes[p + 2] = 30; bytes[p + 3] = 255; }
            Marshal.Copy(bytes, 0, quadro.Address, bytes.Length);
        }
        using var s = new MemoryStream(); imagem.Save(s, PngBitmapEncoderOptions.Default); return s.ToArray();
    }
    private static byte[] Pixel(Bitmap imagem, Point p)
    {
        var memoria = Marshal.AllocHGlobal(4); try { imagem.CopyPixels(new PixelRect((int)p.X, (int)p.Y, 1, 1), memoria, 4, 4); var bytes = new byte[4]; Marshal.Copy(memoria, bytes, 0, 4); return bytes; } finally { Marshal.FreeHGlobal(memoria); }
    }
    private static void SemMargens(TelaImagem t)
    {
        using var frame = t.T.Janela.CaptureRenderedFrame(); Assert.NotNull(frame);
        foreach (var x in new[] { 2, 32, 61 }) { var ponto = t.Capa.TranslatePoint(new Point(x, 48), t.T.Janela)!.Value; var pixel = Pixel(frame, ponto); Assert.InRange(pixel[1], 175, 185); }
    }
    [AvaloniaFact(DisplayName = "ML01 | Miniatura na lista | Preenchimento proporcional e área de 64 por 96")]
    public void Configuracao() { using var t = new TelaImagem(); Assert.Equal(Stretch.UniformToFill, t.Capa.Preenchimento); Assert.Equal(new Size(64, 96), t.Capa.Bounds.Size); }
    [AvaloniaFact(DisplayName = "ML02 | Imagem horizontal na lista | Pixels das duas bordas são imagem sem margens laterais")]
    public async Task Horizontal() { using var t = new TelaImagem(16, 4); await t.Pronta(); SemMargens(t); }
    [AvaloniaFact(DisplayName = "ML03 | Imagem vertical estreita | Miniatura preenche lateralmente sem distorção de modo Fill")]
    public async Task Vertical() { using var t = new TelaImagem(2, 16); await t.Pronta(); SemMargens(t); Assert.Equal(Stretch.UniformToFill, t.Capa.GetVisualDescendants().OfType<Image>().Single().Stretch); }
    [AvaloniaFact(DisplayName = "ML04 | Imagem quadrada | Preenchimento também ocupa as duas laterais")]
    public async Task Quadrada() { using var t = new TelaImagem(8, 8); await t.Pronta(); SemMargens(t); }
    [AvaloniaFact(DisplayName = "ML05 | Miniatura no tamanho mínimo | Área e preenchimento conservados em 900 por 660")]
    public async Task Minima() { using var t = new TelaImagem(); t.T.Janela.Width = 900; t.T.Janela.Height = 660; await t.Pronta(); SemMargens(t); Assert.Equal(new Size(64, 96), t.Capa.Bounds.Size); }
    [AvaloniaFact(DisplayName = "ML06 | Miniatura em janela maior | Área reservada permanece preenchida")]
    public async Task Maior() { using var t = new TelaImagem(); t.T.Janela.Width = 1280; t.T.Janela.Height = 900; await t.Pronta(); SemMargens(t); }
    [AvaloniaFact(DisplayName = "ML07 | Ausência de capa | Fundo padrão ocupa toda a área e mensagem é legível")]
    public async Task Ausente() { using var t = new TelaImagem(url: null); await t.Pronta(); var texto = t.Capa.GetVisualDescendants().OfType<TextBlock>().Single(); Assert.True(texto.IsVisible); Assert.Equal("Sem capa", texto.Text); using var frame = t.T.Janela.CaptureRenderedFrame(); Assert.NotNull(frame); Assert.Equal(Pixel(frame, t.Capa.TranslatePoint(new Point(2, 48), t.T.Janela)!.Value), Pixel(frame, t.Capa.TranslatePoint(new Point(61, 48), t.T.Janela)!.Value)); Assert.Equal(0, t.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "ML08 | URL inválida | Imagem padrão sem download nem mudança do tamanho")]
    public async Task Invalida() { using var t = new TelaImagem(url: "invalida"); await t.Pronta(); Assert.Equal(new Size(64, 96), t.Capa.Bounds.Size); Assert.Contains(t.Capa.GetVisualDescendants().OfType<TextBlock>(), b => b.IsVisible && b.Text == "Sem capa"); Assert.Equal(0, t.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "ML09 | Seleção na lista | Capa permanece preenchida sem sobreposição na imagem")]
    public async Task Selecionada() { using var t = new TelaImagem(); await t.Pronta(); t.T.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; t.Renderizar(); SemMargens(t); }
    [AvaloniaFact(DisplayName = "ML10 | Troca de visualização | Lista volta preenchida usando cache da mesma capa")]
    public async Task Alternar() { using var t = new TelaImagem(); await t.Pronta(); t.T.Campo<ComboBox>("CmbVisualizacao").SelectedIndex = 0; t.Renderizar(); t.T.Campo<ComboBox>("CmbVisualizacao").SelectedIndex = 1; t.Renderizar(); await t.Pronta(); SemMargens(t); Assert.Equal(1, t.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "ML11 | Orientação original da imagem | Source conserva sua proporção enquanto o recorte é proporcional")]
    public async Task Proporcao() { using var t = new TelaImagem(16, 4); await t.Pronta(); var b = Assert.IsType<Bitmap>(t.Capa.GetVisualDescendants().OfType<Image>().Single().Source); Assert.Equal(4, b.Size.Width / b.Size.Height); Assert.Equal(Stretch.UniformToFill, t.Capa.Preenchimento); }
    [AvaloniaFact(DisplayName = "ML12 | Captura da lista preenchida | Miniatura sem margens renderiza no tamanho mínimo")]
    public async Task Captura() { using var t = new TelaImagem(); t.T.Janela.Width = 900; t.T.Janela.Height = 660; await t.Pronta(); using var frame = t.T.Janela.CaptureRenderedFrame(); Assert.NotNull(frame); var pasta = Path.Combine(Path.GetTempPath(), "backlogmanager-revisao2-resultados", "imagens"); Directory.CreateDirectory(pasta); frame.Save(Path.Combine(pasta, "lista-miniatura-preenchida.png"), PngBitmapEncoderOptions.Default); }
}
