using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using BacklogManager.Models;

namespace BacklogManager.Services;

// A sessão compartilha pedidos, limita tamanho/concorrência e nunca envia credenciais.
public sealed class CarregadorCapas : IDisposable
{
    internal const int LimiteBytes = 5 * 1024 * 1024;
    private const int LimiteCache = 128;
    private readonly HttpClient _cliente;
    private readonly bool _possuiCliente;
    private readonly TimeSpan _tempoLimite;
    private readonly SemaphoreSlim _vagas = new(4);
    private readonly CancellationTokenSource _encerramento = new();
    private readonly Dictionary<string, Task<IImage?>> _cache = new();
    private readonly List<WeakReference<Bitmap>> _imagens = new();
    private readonly object _sincronia = new();
    private bool _descartado;

    public CarregadorCapas(HttpClient? cliente = null, TimeSpan? tempoLimite = null)
    {
        _tempoLimite = tempoLimite ?? TimeSpan.FromSeconds(10);
        if (_tempoLimite <= TimeSpan.Zero || _tempoLimite.TotalMilliseconds > uint.MaxValue - 1)
            throw new ArgumentOutOfRangeException(nameof(tempoLimite));
        _possuiCliente = cliente == null;
        _cliente = cliente ?? new HttpClient(new SocketsHttpHandler { AllowAutoRedirect = false })
        { Timeout = TimeSpan.FromSeconds(10) };
    }

    internal static Uri? EnderecoRemoto(string? endereco)
    {
        if (string.IsNullOrWhiteSpace(endereco)) return null;
        if (endereco.StartsWith("//", StringComparison.Ordinal)) endereco = "https:" + endereco;
        return Uri.TryCreate(endereco, UriKind.Absolute, out var uri) && uri.Scheme == "https" &&
            uri.Host.Equals("images.igdb.com", StringComparison.OrdinalIgnoreCase) && uri.IsDefaultPort &&
            string.IsNullOrEmpty(uri.UserInfo) && uri.AbsolutePath.StartsWith("/igdb/image/upload/", StringComparison.Ordinal)
            ? uri : null;
    }

    public async Task<IImage?> CarregarAsync(string? endereco, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (string.IsNullOrWhiteSpace(endereco)) return null;
        endereco = EnderecoRemoto(endereco)?.AbsoluteUri ?? endereco;
        Task<IImage?> tarefa;
        lock (_sincronia)
        {
            if (_descartado) return null;
            if (!_cache.TryGetValue(endereco, out tarefa!))
            {
                if (_cache.Count >= LimiteCache)
                {
                    var chave = _cache.FirstOrDefault(par => par.Value.IsCompleted).Key;
                    if (chave == null) return null;
                    // A imagem pode continuar em uso por um cartão. O GC libera as evictadas.
                    _cache.Remove(chave);
                }
                tarefa = Task.Run(() => CarregarInternoAsync(endereco, _encerramento.Token));
                _cache.Add(endereco, tarefa);
            }
        }
        // Cancelar um cartão não cancela o pedido compartilhado por outro cartão.
        return await tarefa.WaitAsync(cancellationToken);
    }

    private async Task<IImage?> CarregarInternoAsync(string endereco, CancellationToken token)
    {
        // ResponseHeadersRead encerra o timeout do HttpClient ao receber os headers.
        // Este prazo também cobre espera na fila e leitura do corpo da imagem.
        using var prazo = CancellationTokenSource.CreateLinkedTokenSource(token);
        prazo.CancelAfter(_tempoLimite);
        token = prazo.Token;
        try
        {
            if (endereco.StartsWith("avares://", StringComparison.Ordinal))
                return await Dispatcher.UIThread.InvokeAsync(() => CapaSimulada(endereco));
            var uri = EnderecoRemoto(endereco);
            if (uri == null) return null;
            await _vagas.WaitAsync(token);
            try
            {
                using var resposta = await _cliente.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, token);
                if (!resposta.IsSuccessStatusCode || resposta.Content.Headers.ContentLength > LimiteBytes) return null;
                using var origem = await resposta.Content.ReadAsStreamAsync(token);
                using var destino = new MemoryStream();
                var buffer = new byte[8192];
                int lidos;
                while ((lidos = await origem.ReadAsync(buffer, token)) > 0)
                {
                    if (destino.Length + lidos > LimiteBytes) return null;
                    destino.Write(buffer, 0, lidos);
                }
                token.ThrowIfCancellationRequested();
                destino.Position = 0;
                Bitmap bitmap;
                try { bitmap = Bitmap.DecodeToWidth(destino, 320); }
                // O decodificador Skia desta versão lança NullReferenceException para bytes inválidos.
                // O tratamento fica restrito à fronteira de decodificação de conteúdo externo.
                catch (NullReferenceException) { return null; }
                if (bitmap.Size.Height > 4096) { bitmap.Dispose(); return null; }
                lock (_sincronia)
                {
                    if (_descartado) { bitmap.Dispose(); return null; }
                    _imagens.RemoveAll(referencia => !referencia.TryGetTarget(out _));
                    _imagens.Add(new WeakReference<Bitmap>(bitmap));
                }
                return bitmap;
            }
            finally { _vagas.Release(); }
        }
        catch (Exception erro) when (erro is HttpRequestException or OperationCanceledException or IOException or
            ArgumentException or InvalidOperationException or NotSupportedException)
        {
            return null; // Sem capa é um estado normal; pesquisa e biblioteca permanecem utilizáveis.
        }
    }

    private static IImage? CapaSimulada(string endereco)
    {
        var tipo = Enum.GetValues<TipoMidia>().FirstOrDefault(t => t != TipoMidia.Jogo &&
            endereco == $"avares://BacklogManager/Assets/CapasSimuladas/{t}.svg");
        if (tipo == TipoMidia.Jogo) return null;
        var grupo = new DrawingGroup();
        var cor = tipo switch
        {
            TipoMidia.Anime => "#635080",
            TipoMidia.Serie => "#3F6879",
            TipoMidia.Filme => "#805255",
            TipoMidia.Manga => "#6D6442",
            _ => "#3F7165"
        };
        grupo.Children.Add(new GeometryDrawing { Brush = Brush.Parse(cor), Geometry = new RectangleGeometry(new Rect(0, 0, 180, 260)) });
        grupo.Children.Add(new GeometryDrawing { Pen = new Pen(Brushes.White, 3), Geometry = new RectangleGeometry(new Rect(42, 40, 96, 116)) });
        for (var y = 65; y < 140; y += 23)
            grupo.Children.Add(new GeometryDrawing { Brush = Brushes.White, Geometry = new RectangleGeometry(new Rect(57, y, 66, 3)) });
        var texto = new FormattedText($"{TextosMidia.Plural(tipo)}\nSIMULADO", CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight, Typeface.Default, 15, Brushes.White);
        grupo.Children.Add(new GeometryDrawing { Brush = Brushes.White, Geometry = texto.BuildGeometry(new Point(18, 195)) });
        return new DrawingImage { Drawing = grupo };
    }

    public void Dispose()
    {
        lock (_sincronia)
        {
            if (_descartado) return;
            _descartado = true;
            _encerramento.Cancel();
            foreach (var referencia in _imagens)
                if (referencia.TryGetTarget(out var bitmap)) bitmap.Dispose();
            _imagens.Clear();
            _cache.Clear();
        }
        if (_possuiCliente) _cliente.Dispose();
        // Pedidos em encerramento ainda podem liberar o semáforo; não descartá-lo antes deles.
    }
}
