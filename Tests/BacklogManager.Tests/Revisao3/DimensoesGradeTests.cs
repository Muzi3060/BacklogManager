using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using BacklogManager.Controls;
using BacklogManager.Models;
using Xunit;
namespace BacklogManager.Tests.Revisao3;

public sealed class DimensoesGradeTests
{
    private static void Uniformes(ApoioVisual a, double altura = 332)
    {
        Assert.NotEmpty(a.Cartoes); Assert.All(a.Cartoes, c => { Assert.Equal(180, c.Bounds.Width); Assert.Equal(altura, c.Bounds.Height); var capa = c.GetVisualDescendants().OfType<CapaObra>().Single(); Assert.Equal(new Size(160, altura == 332 ? 174 : 104), capa.Bounds.Size); });
    }
    private static double Topo(TextBlock t, Border c) => t.TranslatePoint(new Point(), c)!.Value.Y;
    [AvaloniaFact(DisplayName = "DG01 | Grade com capa larga, estreita e legado sem capa | Todos os cartões e áreas de capa têm medidas idênticas")]
    public async Task Mistos() { using var a = new ApoioVisual(ApoioVisual.Mistos()); a.Biblioteca(); await a.Prontas(); Uniformes(a); }
    [AvaloniaFact(DisplayName = "DG02 | Mesmos registros em 900 por 660 | Dimensões compactas padronizadas sem depender dos metadados")]
    public async Task Compacta() { using var a = new ApoioVisual(ApoioVisual.Mistos()); a.Tela.Janela.Width = 900; a.Tela.Janela.Height = 660; a.Biblioteca(); await a.Prontas(); Uniformes(a, 262); }
    [AvaloniaFact(DisplayName = "DG03 | Grade em 1280 por 900 | Cartões conservam dimensões normais e quebram linha pelo painel")]
    public async Task Maior() { using var a = new ApoioVisual(ApoioVisual.Mistos()); a.Tela.Janela.Width = 1280; a.Tela.Janela.Height = 900; a.Biblioteca(); await a.Prontas(); Uniformes(a); }
    [AvaloniaFact(DisplayName = "DG04 | Título, gêneros e review extensos | Textos não alargam ou aumentam cartões")]
    public async Task Extensos() { var i = ApoioVisual.Jogo(new string('T', 250)); i = i with { Catalogo = i.Catalogo with { Generos = new[] { new string('G', 250) } }, Pessoal = i.Pessoal with { Review = new string('R', 1000), Status = new string('S', 150) } }; using var a = new ApoioVisual(new[] { i, ApoioVisual.Jogo("Curto", null) }); a.Biblioteca(); await a.Prontas(); Uniformes(a); }
    [AvaloniaFact(DisplayName = "DG05 | Títulos e notas em registros mistos | Linhas visíveis ficam na mesma posição relativa ao cartão")]
    public async Task Alinhamento() { using var a = new ApoioVisual(ApoioVisual.Mistos()); a.Biblioteca(); await a.Prontas(); foreach (var linha in new[] { 2, 3, 4, 5, 7 }) { var posicoes = a.Cartoes.Select(c => Topo(c.GetVisualDescendants().OfType<TextBlock>().Single(t => Grid.GetRow(t) == linha), c)).ToArray(); Assert.All(posicoes, p => Assert.Equal(posicoes[0], p)); } }
    [AvaloniaFact(DisplayName = "DG06 | Progresso ausente ou presente | Espaço da linha não desaparece nos registros legados")]
    public async Task Progresso() { using var a = new ApoioVisual(ApoioVisual.Mistos()); a.Biblioteca(); await a.Prontas(); var alturas = a.Cartoes.Select(c => c.Child).Cast<Grid>().Select(g => g.RowDefinitions[6].ActualHeight).ToArray(); Assert.All(alturas, h => Assert.Equal(16, h)); Uniformes(a); }
    [AvaloniaFact(DisplayName = "DG07 | Fallback e bitmap | Área padrão ocupa o mesmo espaço da imagem carregada")]
    public async Task Fallback() { using var a = new ApoioVisual(ApoioVisual.Mistos()); a.Biblioteca(); await a.Prontas(); var capas = a.Cartoes.Select(c => c.GetVisualDescendants().OfType<CapaObra>().Single()).ToArray(); Assert.Equal(capas[0].Bounds.Size, capas[1].Bounds.Size); Assert.Contains(capas[0].GetVisualDescendants().OfType<TextBlock>(), t => t.Text == "Sem capa" && t.IsVisible); }
    [AvaloniaFact(DisplayName = "DG08 | Obra simulada e registro sem flag de simulação | Linha reservada mantém a mesma altura nas duas situações")]
    public async Task Simulacao() { var i = Fase2.ApoioMultimidia.Item(TipoMidia.Livro, new() { Pagina = 2 }); var l = i with { Catalogo = i.Catalogo with { Titulo = "Livro sem metadados", Origem = null, IdentificadorExterno = null, Simulado = false, CapaUrl = null, Generos = Array.Empty<string>() }, Pessoal = i.Pessoal with { Progresso = null } }; using var a = new ApoioVisual(new[] { i, l }); a.Biblioteca(TipoMidia.Livro); await a.Prontas(); Uniformes(a); }
    [AvaloniaFact(DisplayName = "DG09 | Capa termina de carregar | Dimensões anteriores ao download não mudam")]
    public async Task Assincrona() { var liberar = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously); using var a = new ApoioVisual(responder: async (_, ct) => { await liberar.Task.WaitAsync(ct); return ApoioVisual.Resposta(); }); a.Biblioteca(); var tamanho = a.Cartoes[0].Bounds.Size; liberar.SetResult(); await a.Prontas(); Assert.Equal(tamanho, a.Cartoes[0].Bounds.Size); Uniformes(a); }
    [AvaloniaFact(DisplayName = "DG10 | Lista e retorno à grade | Padronização não depende do template usado antes")]
    public async Task Alternar() { using var a = new ApoioVisual(ApoioVisual.Mistos()); a.Biblioteca(modo: 1); await a.Prontas(); a.Biblioteca(); await a.Prontas(); Uniformes(a); }
    [AvaloniaFact(DisplayName = "DG11 | Selecionar cartão | Marca interna não muda altura ou alinhamento")]
    public async Task Selecionar() { using var a = new ApoioVisual(ApoioVisual.Mistos()); a.Biblioteca(); await a.Prontas(); var antes = a.Cartoes.Select(c => c.Bounds.Size).ToArray(); a.Tela.Campo<ListBox>("ListaBacklog").SelectedIndex = 1; a.Renderizar(); Assert.Equal(antes, a.Cartoes.Select(c => c.Bounds.Size)); Uniformes(a); }
    [AvaloniaFact(DisplayName = "DG12 | Reduzir e ampliar a janela | Medidas mudam juntas e nenhum JSON é regravado")]
    public async Task Redimensionar() { using var a = new ApoioVisual(ApoioVisual.Mistos()); var bytes = File.ReadAllBytes(a.Tela.Pasta.Arquivo); a.Biblioteca(); await a.Prontas(); a.Tela.Janela.Height = 660; a.Renderizar(); Uniformes(a, 262); a.Tela.Janela.Height = 780; a.Renderizar(); Uniformes(a); Assert.Equal(bytes, File.ReadAllBytes(a.Tela.Pasta.Arquivo)); }
    [AvaloniaFact(DisplayName = "DG13 | Seis categorias com e sem progresso | Padronização aplicada a todas as mídias")]
    public async Task Categorias() { var itens = Enum.GetValues<TipoMidia>().SelectMany(tipo => { var i = Fase2.ApoioMultimidia.Item(tipo); return new[] { i, i with { Catalogo = i.Catalogo with { Titulo = "Outra obra", CapaUrl = null }, Pessoal = i.Pessoal with { Review = "" } } }; }).ToArray(); using var a = new ApoioVisual(itens); foreach (var tipo in Enum.GetValues<TipoMidia>()) { a.Biblioteca(tipo); await a.Prontas(); Assert.Equal(2, a.Cartoes.Length); Uniformes(a); } }
    [AvaloniaFact(DisplayName = "DG14 | Progresso numericamente extenso | Texto é abreviado sem deslocar as linhas seguintes")]
    public async Task ProgressoExtenso() { var i = ApoioVisual.Jogo(); i = i with { Pessoal = i.Pessoal with { Progresso = new() { HorasJogadas = decimal.MaxValue } } }; using var a = new ApoioVisual(new[] { i, ApoioVisual.Jogo("Sem progresso", null) }); a.Biblioteca(); await a.Prontas(); Uniformes(a); var t = a.Cartoes[0].GetVisualDescendants().OfType<TextBlock>().Single(b => Grid.GetRow(b) == 6); Assert.Equal(i.ProgressoExibido, ToolTip.GetTip(t)); }
}
