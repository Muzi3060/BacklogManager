using System.Net;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;
using BacklogManager.Controls;
using BacklogManager.Models;
using Xunit;
namespace BacklogManager.Tests.Revisao3;

public sealed class RecentesCapasTests
{
    [AvaloniaFact(DisplayName = "CR01 | Recente com URL de capa | Imagem do catálogo aparece no lugar do ícone fixo")]
    public async Task Capa() { using var a = new ApoioVisual(); await a.Prontas(); Assert.Contains(Assert.Single(a.Recentes).GetVisualDescendants().OfType<Image>(), i => i.Source is Bitmap && i.IsVisible); }
    [AvaloniaFact(DisplayName = "CR02 | Recente sem capa | Fallback Sem capa permanece visível")]
    public async Task SemCapa() { using var a = new ApoioVisual(new[] { ApoioVisual.Jogo(capa: null) }); await a.Prontas(); Assert.Contains(a.Recentes[0].GetVisualDescendants().OfType<TextBlock>(), t => t.IsVisible && t.Text == "Sem capa"); Assert.Equal(0, a.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "CR03 | Recente com URL inválida | Fallback sem download externo")]
    public async Task Invalida() { using var a = new ApoioVisual(new[] { ApoioVisual.Jogo(capa: "invalida") }); await a.Prontas(); Assert.Contains(a.Recentes[0].GetVisualDescendants().OfType<TextBlock>(), t => t.IsVisible && t.Text == "Sem capa"); Assert.Equal(0, a.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "CR04 | CDN retorna 404 no recente | Imagem indisponível não interrompe página inicial")]
    public async Task NaoEncontrada() { using var a = new ApoioVisual(status: HttpStatusCode.NotFound); await a.Prontas(); Assert.Contains(a.Recentes[0].GetVisualDescendants().OfType<TextBlock>(), t => t.IsVisible && t.Text == "Sem capa"); Assert.Single(a.Tela.Salvos); }
    [AvaloniaFact(DisplayName = "CR05 | Falha de conexão da capa recente | Uso local e fallback preservados")]
    public async Task Offline() { using var a = new ApoioVisual(responder: (_, _) => throw new HttpRequestException("offline simulado")); await a.Prontas(); Assert.Contains(a.Recentes[0].GetVisualDescendants().OfType<TextBlock>(), t => t.IsVisible && t.Text == "Sem capa"); Assert.Equal(0, a.Tela.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "CR06 | Recente de catálogo simulado | Desenho local carrega sem HTTP")]
    public async Task Simulada() { using var a = new ApoioVisual(new[] { Fase2.ApoioMultimidia.Item(TipoMidia.Livro) }); await a.Prontas(); Assert.Contains(a.Recentes[0].GetVisualDescendants().OfType<Image>(), i => i.Source is DrawingImage && i.IsVisible); Assert.Equal(0, a.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "CR07 | Alternar início e biblioteca | Mesma capa é compartilhada pelo cache da sessão")]
    public async Task Cache() { using var a = new ApoioVisual(); await a.Prontas(); var fonte = a.Recentes[0].GetVisualDescendants().OfType<Image>().Single().Source; a.Biblioteca(); await a.Prontas(); Assert.Same(fonte, a.Cartoes[0].GetVisualDescendants().OfType<Image>().Single().Source); a.Tela.Clicar("NavInicio"); await a.Prontas(); Assert.Equal(1, a.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "CR08 | Abrir obra pelo recente com capa | Botão continua abrindo edição do registro correto")]
    public async Task Editar() { using var a = new ApoioVisual(); await a.Prontas(); var b = a.Tela.Campo<ItemsControl>("ListaRecentes").GetVisualDescendants().OfType<Button>().First(); b.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); Assert.Equal("Jogo com capa", a.Tela.Campo<TextBox>("TxtTitulo").Text); Assert.Equal(0, a.Tela.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "CR09 | Carregar e navegar com capa recente | Arquivo não é regravado ou enriquecido automaticamente")]
    public async Task SemGravacao() { using var a = new ApoioVisual(); var bytes = File.ReadAllBytes(a.Tela.Pasta.Arquivo); await a.Prontas(); a.Biblioteca(); a.Tela.Clicar("NavInicio"); await a.Prontas(); Assert.Equal(bytes, File.ReadAllBytes(a.Tela.Pasta.Arquivo)); }
    [AvaloniaFact(DisplayName = "CR10 | Quatro obras na coleção | Recentes continuam limitados às três últimas na ordem anterior")]
    public async Task Tres() { using var a = new ApoioVisual(Enumerable.Range(1, 4).Select(n => ApoioVisual.Jogo("Obra " + n))); await a.Prontas(); var itens = a.Tela.Campo<ItemsControl>("ListaRecentes").Items.Cast<ItemBiblioteca>().ToArray(); Assert.Equal(new[] { "Obra 4", "Obra 3", "Obra 2" }, itens.Select(i => i.Catalogo.Titulo)); Assert.Equal(3, a.Recentes.Length); }
    [AvaloniaFact(DisplayName = "CR11 | Recentes na janela mínima | Cabeçalho com capa mantém altura e texto legível")]
    public async Task Minima() { using var a = new ApoioVisual(); a.Tela.Janela.Width = 900; a.Tela.Janela.Height = 660; await a.Prontas(); Assert.Equal(72, a.Recentes[0].Bounds.Height); Assert.True(a.Recentes[0].Bounds.Width > 100); a.Capturar("inicio-capas-minimo"); }
    [AvaloniaFact(DisplayName = "CR12 | Download recente pendente | Fallback inicial é substituído pela capa ao terminar")]
    public async Task Assincrona() { var entrou = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously); var liberar = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously); using var a = new ApoioVisual(responder: async (_, ct) => { entrou.TrySetResult(); await liberar.Task.WaitAsync(ct); return ApoioVisual.Resposta(); }); await entrou.Task.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken); Assert.Contains(a.Recentes[0].GetVisualDescendants().OfType<TextBlock>(), t => t.Text == "Sem capa" && t.IsVisible); liberar.SetResult(); await a.Prontas(); Assert.Contains(a.Recentes[0].GetVisualDescendants().OfType<Image>(), i => i.Source != null && i.IsVisible); }
    [AvaloniaFact(DisplayName = "CR13 | Bytes inválidos no recente | Fallback controlado sem encerramento")]
    public async Task ConteudoInvalido() { using var a = new ApoioVisual(responder: (_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(new byte[] { 1, 2, 3 }) })); await a.Prontas(); Assert.Contains(a.Recentes[0].GetVisualDescendants().OfType<TextBlock>(), t => t.Text == "Sem capa" && t.IsVisible); }
    [AvaloniaFact(DisplayName = "CR14 | Biblioteca vazia | Nenhum recente fictício é criado para exibir capa")]
    public async Task Vazia() { using var a = new ApoioVisual(Array.Empty<ItemBiblioteca>()); await a.Prontas(); Assert.Empty(a.Recentes); Assert.True(a.Tela.Campo<Border>("EstadoInicioVazio").IsVisible); Assert.Equal(0, a.Http.Chamadas); }
}
