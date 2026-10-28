using System.Net;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using Avalonia.VisualTree;
using BacklogManager.Controls;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Services;
namespace BacklogManager.Tests.Revisao3;

internal sealed class ApoioVisual : IDisposable
{
    internal const string Url = "https://images.igdb.com/igdb/image/upload/t_cover_big/teste.jpg";
    private readonly HttpClient _cliente;
    internal HttpSimulado Http { get; }
    internal TelaTeste Tela { get; }
    internal ApoioVisual(IEnumerable<ItemBiblioteca>? itens = null, HttpStatusCode status = HttpStatusCode.OK,
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>? responder = null)
    {
        Http = new HttpSimulado { Responder = responder ?? ((r, _) => Task.FromResult(Resposta(r.RequestUri!.AbsolutePath.Contains("estreita") ? 2 : 16, r.RequestUri.AbsolutePath.Contains("estreita") ? 16 : 4, status))) };
        _cliente = new HttpClient(Http);
        Tela = new TelaTeste((itens ?? new[] { Jogo() }).Select(ConversorBibliotecaJson.ParaRegistro), capas: new CarregadorCapas(_cliente)); Renderizar();
    }
    internal static ItemBiblioteca Jogo(string titulo = "Jogo com capa", string? capa = Url) => Fase1.ApoioArquitetura.Jogo(titulo) with
    { Catalogo = Fase1.ApoioArquitetura.Jogo(titulo).Catalogo with { CapaUrl = capa, Generos = new[] { "RPG" } } };
    internal static ItemBiblioteca[] Mistos()
    {
        var legado = ConversorBibliotecaJson.ParaModelo(PastaTeste.Jogo("Legado sem capa"));
        var completo = Jogo("Catálogo completo", Url.Replace("teste.jpg", "estreita.jpg"));
        completo = completo with { Pessoal = completo.Pessoal with { Progresso = new() { HorasJogadas = 10 }, Review = "Uma review extensa " + new string('x', 250) } };
        return new[] { legado, Jogo("Capa horizontal"), completo };
    }
    internal void Biblioteca(TipoMidia tipo = TipoMidia.Jogo, int modo = 0)
    { Tela.Campo<ComboBox>("CmbTipoBiblioteca").SelectedIndex = (int)tipo; Tela.Campo<ComboBox>("CmbVisualizacao").SelectedIndex = modo; Tela.Clicar("NavBacklog"); Renderizar(); }
    internal void Renderizar() { Dispatcher.UIThread.RunJobs(); Tela.Janela.UpdateLayout(); }
    internal async Task Prontas()
    { Renderizar(); await Task.WhenAll(Tela.Janela.GetVisualDescendants().OfType<CapaObra>().Select(c => c.CarregamentoPendente)); Renderizar(); }
    internal CapaObra[] Recentes => Tela.Campo<ItemsControl>("ListaRecentes").GetVisualDescendants().OfType<CapaObra>().ToArray();
    internal Border[] Cartoes => Tela.Campo<ListBox>("ListaBacklog").GetVisualDescendants().OfType<Border>().Where(b => b.Classes.Contains("gradeCard")).ToArray();
    internal static byte[] Pixel(Bitmap imagem, Point ponto)
    {
        var memoria = Marshal.AllocHGlobal(4);
        try { imagem.CopyPixels(new PixelRect((int)ponto.X, (int)ponto.Y, 1, 1), memoria, 4, 4); var bytes = new byte[4]; Marshal.Copy(memoria, bytes, 0, 4); return bytes; }
        finally { Marshal.FreeHGlobal(memoria); }
    }
    internal static HttpResponseMessage Resposta(int largura = 16, int altura = 4, HttpStatusCode status = HttpStatusCode.OK)
    {
        using var bitmap = new WriteableBitmap(new PixelSize(largura, altura), new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Opaque);
        using (var quadro = bitmap.Lock())
        {
            var bytes = new byte[quadro.RowBytes * altura]; for (var y = 0; y < altura; y++) for (var x = 0; x < largura; x++) { var p = y * quadro.RowBytes + x * 4; bytes[p] = 80; bytes[p + 1] = 180; bytes[p + 2] = 30; bytes[p + 3] = 255; }
            Marshal.Copy(bytes, 0, quadro.Address, bytes.Length);
        }
        using var s = new MemoryStream(); bitmap.Save(s, PngBitmapEncoderOptions.Default);
        return new HttpResponseMessage(status) { Content = new ByteArrayContent(s.ToArray()) };
    }
    internal void Capturar(string nome)
    { Renderizar(); using var imagem = Tela.Janela.CaptureRenderedFrame(); var pasta = Path.Combine(Path.GetTempPath(), "backlogmanager-revisao3-resultados", "imagens"); Directory.CreateDirectory(pasta); imagem!.Save(Path.Combine(pasta, nome + ".png"), PngBitmapEncoderOptions.Default); }
    public void Dispose() { Tela.Dispose(); _cliente.Dispose(); }
}
