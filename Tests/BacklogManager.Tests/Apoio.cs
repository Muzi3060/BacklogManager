using System.Net;
using Avalonia;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using BacklogManager.Models;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: AvaloniaTestApplication(typeof(BacklogManager.Tests.AplicacaoTeste))]

namespace BacklogManager.Tests;

public static class AplicacaoTeste
{
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
        .UseSkia()
        .WithInterFont()
        .UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false });
}

public sealed class PastaTeste : IDisposable
{
    public string Pasta { get; } = Path.Combine(Path.GetTempPath(), "BacklogManager.Tests", Guid.NewGuid().ToString("N"));
    public string Arquivo => Path.Combine(Pasta, "backlog.json");
    public PastaTeste() => Directory.CreateDirectory(Pasta);
    public void Dispose() => Directory.Delete(Pasta, recursive: true);
    public static BacklogItem Jogo(string titulo = "Jogo simulado") => new(titulo, "RPG", "Na Fila", 4.5, "Review simulada", 1010);
}

public sealed class HttpSimulado : HttpMessageHandler
{
    public Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> Responder { get; set; }
        = (_, _) => Task.FromResult(Json("[]"));
    public int Chamadas { get; private set; }
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Chamadas++;
        return Responder(request, cancellationToken);
    }
    public static HttpResponseMessage Json(string json, HttpStatusCode status = HttpStatusCode.OK) =>
        new(status) { Content = new StringContent(json) };
}
