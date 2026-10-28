using System.Net;
using Avalonia;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using BacklogManager.Models;
using BacklogManager.Services;
using Xunit;

namespace BacklogManager.Tests.Fase3;

public sealed class CapasTests
{
    private const string Url = "https://images.igdb.com/igdb/image/upload/t_cover_big/teste.jpg";
    internal static byte[] Png()
    {
        using var bitmap = new WriteableBitmap(new PixelSize(2, 3), new Vector(96, 96), PixelFormat.Rgba8888, AlphaFormat.Unpremul);
        using var fluxo = new MemoryStream(); bitmap.Save(fluxo, PngBitmapEncoderOptions.Default); return fluxo.ToArray();
    }
    private static HttpResponseMessage Imagem(byte[] png) => new(HttpStatusCode.OK) { Content = new ByteArrayContent(png) };
    private static async Task SemHttp(string? url)
    {
        using var h = new HttpSimulado(); using var c = new HttpClient(h); using var a = new CarregadorCapas(c);
        Assert.Null(await a.CarregarAsync(url, TestContext.Current.CancellationToken)); Assert.Equal(0, h.Chamadas);
    }
    [AvaloniaFact(DisplayName = "CP01 | Capa nula | Imagem padrão sem HTTP")]
    public Task Nula() => SemHttp(null);
    [AvaloniaFact(DisplayName = "CP02 | Capa vazia | Imagem padrão sem HTTP")]
    public Task Vazia() => SemHttp(" ");
    [AvaloniaFact(DisplayName = "CP03 | URL malformada | Imagem padrão sem exceção")]
    public Task Malformada() => SemHttp("://invalida");
    [AvaloniaFact(DisplayName = "CP04 | HTTP sem TLS | URL recusada")]
    public Task Http() => SemHttp(Url.Replace("https:", "http:"));
    [AvaloniaFact(DisplayName = "CP05 | Caminho pessoal local | Arquivo não é aberto como capa")]
    public Task Arquivo() => SemHttp("file:///tmp/segredo.json");
    [AvaloniaFact(DisplayName = "CP06 | Host parecido com CDN | Domínio falso recusado")]
    public Task HostFalso() => SemHttp(Url.Replace("images.igdb.com", "images.igdb.com.outro.test"));
    [AvaloniaFact(DisplayName = "CP07 | Porta alternativa | URL recusada sem rede")]
    public Task Porta() => SemHttp(Url.Replace("images.igdb.com", "images.igdb.com:444"));
    [AvaloniaFact(DisplayName = "CP08 | Credenciais em URL | Userinfo recusado")]
    public Task UserInfo() => SemHttp(Url.Replace("https://", "https://usuario:senha@"));
    [AvaloniaFact(DisplayName = "CP09 | Caminho remoto fora das imagens | Recurso recusado")]
    public Task Caminho() => SemHttp("https://images.igdb.com/segredo.json");
    [AvaloniaFact(DisplayName = "CP10 | Recurso local desconhecido | Sem tentativa de arquivo ou HTTP")]
    public Task LocalDesconhecido() => SemHttp("avares://BacklogManager/Assets/CapasSimuladas/Outro.svg");
    [AvaloniaFact(DisplayName = "CP11 | Cinco capas simuladas | Desenhos locais válidos e nenhuma rede")]
    public async Task Simuladas() { using var h = new HttpSimulado(); using var c = new HttpClient(h); using var a = new CarregadorCapas(c); foreach (var t in Enum.GetValues<TipoMidia>().Skip(1)) { var i = await a.CarregarAsync($"avares://BacklogManager/Assets/CapasSimuladas/{t}.svg", TestContext.Current.CancellationToken); Assert.IsType<DrawingImage>(i); Assert.Equal(new Size(180, 260), i.Size); } Assert.Equal(0, h.Chamadas); }
    [AvaloniaFact(DisplayName = "CP12 | Imagem remota válida | Bitmap decodificado com largura limitada")]
    public async Task Sucesso() { var png = Png(); using var h = new HttpSimulado { Responder = (_, _) => Task.FromResult(Imagem(png)) }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); var i = await a.CarregarAsync(Url, TestContext.Current.CancellationToken); Assert.IsType<Bitmap>(i); Assert.True(i.Size.Width <= 320); Assert.Equal(1, h.Chamadas); }
    [AvaloniaFact(DisplayName = "CP13 | Repetir a mesma capa | Instância reutilizada e um único download")]
    public async Task Cache() { var png = Png(); using var h = new HttpSimulado { Responder = (_, _) => Task.FromResult(Imagem(png)) }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); var i = await a.CarregarAsync(Url, TestContext.Current.CancellationToken); Assert.Same(i, await a.CarregarAsync(Url, TestContext.Current.CancellationToken)); Assert.Equal(1, h.Chamadas); }
    [AvaloniaFact(DisplayName = "CP14 | Pedidos simultâneos da mesma capa | Download compartilhado pelos consumidores")]
    public async Task Compartilhada() { var png = Png(); var liberar = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously); using var h = new HttpSimulado { Responder = async (_, ct) => { await liberar.Task.WaitAsync(ct); return Imagem(png); } }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); var um = a.CarregarAsync(Url); var dois = a.CarregarAsync(Url); liberar.SetResult(); await Task.WhenAll(um, dois); Assert.Same(um.Result, dois.Result); Assert.NotNull(um.Result); Assert.Equal(1, h.Chamadas); }
    [AvaloniaFact(DisplayName = "CP15 | Capa inexistente 404 | Imagem padrão sem encerramento")]
    public async Task NaoEncontrada() { using var h = new HttpSimulado { Responder = (_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)) }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); Assert.Null(await a.CarregarAsync(Url, TestContext.Current.CancellationToken)); }
    [AvaloniaFact(DisplayName = "CP16 | Servidor de imagens 500 | Falha isolada do catálogo")]
    public async Task Servidor() { using var h = new HttpSimulado { Responder = (_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)) }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); Assert.Null(await a.CarregarAsync(Url, TestContext.Current.CancellationToken)); }
    [AvaloniaFact(DisplayName = "CP17 | Bytes não representam imagem | Conteúdo inválido vira imagem padrão")]
    public async Task BytesInvalidos() { using var h = new HttpSimulado { Responder = (_, _) => Task.FromResult(Imagem(new byte[] { 1, 2, 3 })) }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); Assert.Null(await a.CarregarAsync(Url, TestContext.Current.CancellationToken)); }
    [AvaloniaFact(DisplayName = "CP18 | Content-Length acima do limite | Imagem grande é recusada")]
    public async Task GrandeHeader() { using var h = new HttpSimulado { Responder = (_, _) => { var r = Imagem(Array.Empty<byte>()); r.Content.Headers.ContentLength = CarregadorCapas.LimiteBytes + 1; return Task.FromResult(r); } }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); Assert.Null(await a.CarregarAsync(Url, TestContext.Current.CancellationToken)); }
    [AvaloniaFact(DisplayName = "CP19 | Fluxo sem tamanho supera limite | Leitura é interrompida antes da decodificação")]
    public async Task GrandeFluxo() { using var h = new HttpSimulado { Responder = (_, _) => { var r = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(new FluxoSemTamanho(new byte[CarregadorCapas.LimiteBytes + 1])) }; return Task.FromResult(r); } }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); Assert.Null(await a.CarregarAsync(Url, TestContext.Current.CancellationToken)); }
    [AvaloniaFact(DisplayName = "CP20 | Falha de conexão da capa | Biblioteca pode continuar sem imagem")]
    public async Task Rede() { using var h = new HttpSimulado { Responder = (_, _) => throw new HttpRequestException("offline") }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); Assert.Null(await a.CarregarAsync(Url, TestContext.Current.CancellationToken)); }
    [AvaloniaFact(DisplayName = "CP21 | Timeout da imagem | Resultado ausente sem exceção não tratada")]
    public async Task Timeout() { using var h = new HttpSimulado { Responder = (_, _) => throw new TaskCanceledException() }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); Assert.Null(await a.CarregarAsync(Url, TestContext.Current.CancellationToken)); }
    [AvaloniaFact(DisplayName = "CP22 | Cancelamento antes de carregar | Nenhum pedido é enviado")]
    public async Task Cancelada() { using var h = new HttpSimulado(); using var c = new HttpClient(h); using var a = new CarregadorCapas(c); using var cts = new CancellationTokenSource(); cts.Cancel(); await Assert.ThrowsAnyAsync<OperationCanceledException>(() => a.CarregarAsync(Url, cts.Token)); Assert.Equal(0, h.Chamadas); }
    [AvaloniaFact(DisplayName = "CP23 | Um cartão cancela pedido compartilhado | Outro consumidor ainda recebe a capa")]
    public async Task CancelarUm() { var png = Png(); var liberar = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously); using var h = new HttpSimulado { Responder = async (_, ct) => { await liberar.Task.WaitAsync(ct); return Imagem(png); } }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); using var cts = new CancellationTokenSource(); var um = a.CarregarAsync(Url, cts.Token); var dois = a.CarregarAsync(Url); cts.Cancel(); liberar.SetResult(); await Assert.ThrowsAnyAsync<OperationCanceledException>(() => um); Assert.NotNull(await dois); Assert.Equal(1, h.Chamadas); }
    [AvaloniaFact(DisplayName = "CP24 | Encerrar com pedido em andamento | Pedido é cancelado e carregador não aceita novos downloads")]
    public async Task Encerrar() { var entrou = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously); using var h = new HttpSimulado { Responder = async (_, ct) => { entrou.SetResult(); await Task.Delay(System.Threading.Timeout.InfiniteTimeSpan, ct); return Imagem(Array.Empty<byte>()); } }; using var c = new HttpClient(h); var a = new CarregadorCapas(c); var t = a.CarregarAsync(Url); await entrou.Task.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken); a.Dispose(); Assert.Null(await t); Assert.Null(await a.CarregarAsync(Url)); a.Dispose(); }
    [AvaloniaFact(DisplayName = "CP25 | URL sem protocolo | Pedido normalizado para HTTPS")]
    public async Task SemProtocolo() { var png = Png(); using var h = new HttpSimulado { Responder = (r, _) => { Assert.Equal("https", r.RequestUri!.Scheme); return Task.FromResult(Imagem(png)); } }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); Assert.NotNull(await a.CarregarAsync(Url[6..], TestContext.Current.CancellationToken)); }
    [AvaloniaFact(DisplayName = "CP26 | Redirecionamento do CDN | Resposta não é tratada como imagem")]
    public async Task Redirecionamento() { using var h = new HttpSimulado { Responder = (_, _) => { var r = new HttpResponseMessage(HttpStatusCode.Redirect); r.Headers.Location = new Uri("https://outro.test/imagem"); return Task.FromResult(r); } }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); Assert.Null(await a.CarregarAsync(Url, TestContext.Current.CancellationToken)); Assert.Equal(1, h.Chamadas); }
    [AvaloniaFact(DisplayName = "CP27 | Download de imagem | Nenhuma credencial IGDB é enviada")]
    public async Task SemCredenciais() { var png = Png(); using var h = new HttpSimulado { Responder = (r, _) => { Assert.Null(r.Headers.Authorization); Assert.False(r.Headers.Contains("Client-ID")); return Task.FromResult(Imagem(png)); } }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); Assert.NotNull(await a.CarregarAsync(Url, TestContext.Current.CancellationToken)); }
    [AvaloniaFact(DisplayName = "CP28 | Muitas imagens ao mesmo tempo | No máximo quatro downloads simultâneos")]
    public async Task Concorrencia() { var png = Png(); var liberar = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously); var quatro = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously); var ativos = 0; var maximo = 0; using var h = new HttpSimulado { Responder = async (_, ct) => { var n = Interlocked.Increment(ref ativos); InterlockedExtensions.Max(ref maximo, n); if (n == 4) quatro.TrySetResult(); await liberar.Task.WaitAsync(ct); Interlocked.Decrement(ref ativos); return Imagem(png); } }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); var tarefas = Enumerable.Range(0, 8).Select(n => a.CarregarAsync(Url + "?n=" + n)).ToArray(); await quatro.Task.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken); liberar.SetResult(); Assert.All(await Task.WhenAll(tarefas), Assert.NotNull); Assert.InRange(maximo, 1, 4); }
    [AvaloniaFact(DisplayName = "CP29 | Mesma URL com e sem protocolo | Cache normalizado evita download duplicado")]
    public async Task CacheNormalizado() { var png = Png(); using var h = new HttpSimulado { Responder = (_, _) => Task.FromResult(Imagem(png)) }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); var i = await a.CarregarAsync(Url, TestContext.Current.CancellationToken); Assert.Same(i, await a.CarregarAsync(Url[6..], TestContext.Current.CancellationToken)); Assert.Equal(1, h.Chamadas); }
    [AvaloniaFact(DisplayName = "CP30 | Falha de uma capa seguida de outra válida | Erro não bloqueia novos endereços")]
    public async Task RecuperarOutra() { var png = Png(); using var h = new HttpSimulado { Responder = (r, _) => Task.FromResult(r.RequestUri!.Query == "?falha" ? new HttpResponseMessage(HttpStatusCode.NotFound) : Imagem(png)) }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); Assert.Null(await a.CarregarAsync(Url + "?falha", TestContext.Current.CancellationToken)); Assert.NotNull(await a.CarregarAsync(Url, TestContext.Current.CancellationToken)); }
    [AvaloniaFact(DisplayName = "CP31 | Endereços diferentes | Cache não confunde capas distintas")]
    public async Task EnderecosDistintos() { var png = Png(); using var h = new HttpSimulado { Responder = (_, _) => Task.FromResult(Imagem(png)) }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); var um = await a.CarregarAsync(Url, TestContext.Current.CancellationToken); var dois = await a.CarregarAsync(Url + "?dois", TestContext.Current.CancellationToken); Assert.NotSame(um, dois); Assert.Equal(2, h.Chamadas); }
    [AvaloniaFact(DisplayName = "CP32 | Cache excede 128 endereços | Entradas concluídas antigas são substituídas com segurança")]
    public async Task CacheLimitado() { var chamadas = 0; using var h = new HttpSimulado { Responder = (_, _) => { Interlocked.Increment(ref chamadas); return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)); } }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c); for (var n = 0; n < 129; n++) Assert.Null(await a.CarregarAsync(Url + "?n=" + n, TestContext.Current.CancellationToken)); Assert.Null(await a.CarregarAsync(Url + "?n=0", TestContext.Current.CancellationToken)); Assert.Equal(130, chamadas); }
    [AvaloniaFact(DisplayName = "CP33 | Headers recebidos mas corpo não termina | Prazo completo cancela a leitura da imagem")]
    public async Task PrazoCorpo() { using var h = new HttpSimulado { Responder = (_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(new FluxoLento()) }) }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c, TimeSpan.FromMilliseconds(80)); Assert.Null(await a.CarregarAsync(Url).WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken)); }
    [AvaloniaFact(DisplayName = "CP34 | Servidor não envia headers | Prazo completo cancela o pedido HTTP")]
    public async Task PrazoHeaders() { using var h = new HttpSimulado { Responder = async (_, ct) => { await Task.Delay(System.Threading.Timeout.InfiniteTimeSpan, ct); return new HttpResponseMessage(); } }; using var c = new HttpClient(h); using var a = new CarregadorCapas(c, TimeSpan.FromMilliseconds(80)); Assert.Null(await a.CarregarAsync(Url).WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken)); }
    [AvaloniaFact(DisplayName = "CP35 | Prazo de carregamento inválido | Configuração rejeitada antes de criar pedidos")]
    public void PrazoInvalido() => Assert.Throws<ArgumentOutOfRangeException>(() => new CarregadorCapas(tempoLimite: TimeSpan.Zero));
    private sealed class FluxoLento : MemoryStream
    {
        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) { await Task.Delay(System.Threading.Timeout.InfiniteTimeSpan, cancellationToken); return 0; }
    }
    private sealed class FluxoSemTamanho(byte[] dados) : MemoryStream(dados) { public override bool CanSeek => false; }
    private static class InterlockedExtensions
    {
        internal static void Max(ref int alvo, int valor) { int anterior; do { anterior = alvo; if (anterior >= valor) return; } while (Interlocked.CompareExchange(ref alvo, valor, anterior) != anterior); }
    }
}
