using Xunit;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Avalonia.VisualTree;
using BacklogManager.Controls;
using BacklogManager.Models;
using BacklogManager.Tests.Revisao3;
namespace BacklogManager.Tests.Fase6;

public sealed class CapasDashboardTests
{
    private static ApoioVisual Tela(string? url=ApoioVisual.Url,System.Net.HttpStatusCode status=System.Net.HttpStatusCode.OK)
        =>new(new[]{ApoioVisual.Jogo(capa:url) with {DataAdicao=DateTimeOffset.UtcNow}},status);
    private static CapaObra Capa(ApoioVisual a)=>a.Tela.Campo<ItemsControl>("ListaAtividade").GetVisualDescendants().OfType<CapaObra>().Single();
    [AvaloniaFact(DisplayName="DK01 | Capa disponível | Miniatura assíncrona exibe imagem")]
    public async Task Disponivel(){using var a=Tela();await a.Prontas();Assert.True(Capa(a).GetVisualDescendants().OfType<Image>().Single().IsVisible);}
    [AvaloniaFact(DisplayName="DK02 | URL ausente | Fallback sem requisição")]
    public async Task Ausente(){using var a=Tela(null);await a.Prontas();Assert.False(Capa(a).GetVisualDescendants().OfType<Image>().Single().IsVisible);Assert.Equal(0,a.Http.Chamadas);}
    [AvaloniaFact(DisplayName="DK03 | URL inválida | Fallback não encerra interface")]
    public async Task Invalida(){using var a=Tela("não é URL");await a.Prontas();Assert.False(Capa(a).GetVisualDescendants().OfType<Image>().Single().IsVisible);}
    [AvaloniaFact(DisplayName="DK04 | HTTP indisponível | Fallback preserva contagens")]
    public async Task Falha(){using var a=Tela(status:System.Net.HttpStatusCode.ServiceUnavailable);await a.Prontas();Assert.False(Capa(a).GetVisualDescendants().OfType<Image>().Single().IsVisible);Assert.Equal("1",a.Tela.Campo<TextBlock>("TxtTotal").Text);}
    [AvaloniaFact(DisplayName="DK05 | Imagem horizontal | UniformToFill preserva proporção")]
    public async Task Preencher(){using var a=Tela();await a.Prontas();Assert.Equal(Stretch.UniformToFill,Capa(a).Preenchimento);}
    [AvaloniaFact(DisplayName="DK06 | Miniatura discreta | Limite de tamanho não domina estatísticas")]
    public async Task Pequena(){using var a=Tela();await a.Prontas();Assert.Equal(64,Capa(a).Height);Assert.Equal(60,Capa(a).Width);}
    [AvaloniaFact(DisplayName="DK07 | Cantos das miniaturas | Mantém recorte arredondado")]
    public async Task Cantos(){using var a=Tela();await a.Prontas();Assert.Equal(8,Assert.IsType<RectangleGeometry>(Capa(a).Clip).RadiusX);}
    [AvaloniaFact(DisplayName="DK08 | Navegar e voltar | Reutiliza cache sem novo download")]
    public async Task Cache(){using var a=Tela();await a.Prontas();var n=a.Http.Chamadas;a.Tela.Clicar("NavBacklog");a.Tela.Clicar("NavInicio");await a.Prontas();Assert.Equal(n,a.Http.Chamadas);}
    [AvaloniaFact(DisplayName="DK09 | Capa inválida e edição | Fallback não bloqueia abertura da obra")]
    public async Task Editar(){using var a=Tela(null);await a.Prontas();var b=a.Tela.Campo<ItemsControl>("ListaAtividade").GetVisualDescendants().OfType<Button>().First();b.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Button.ClickEvent));Assert.True(a.Tela.Campo<ScrollViewer>("PaginaCadastro").IsVisible);}
    [AvaloniaFact(DisplayName="DK10 | Título longo | Tooltip conserva título completo")]
    public async Task Titulo(){var i=ApoioVisual.Jogo(new string('X',200)) with{DataAdicao=DateTimeOffset.UtcNow};using var a=new ApoioVisual(new[]{i});await a.Prontas();Assert.Contains(a.Tela.Campo<ItemsControl>("ListaAtividade").GetVisualDescendants().OfType<TextBlock>(),b=>b.Text==i.Catalogo.Titulo&&ToolTip.GetTip(b)?.ToString()==i.Catalogo.Titulo);}
    [AvaloniaFact(DisplayName="DK11 | Sem data e com capa | Registro preservado sem bloco legado no Dashboard")]
    public async Task Legado(){using var a=new ApoioVisual();await a.Prontas();Assert.Empty(a.Tela.Campo<ItemsControl>("ListaAtividade").Items);Assert.Null(a.Tela.Janela.FindControl<ItemsControl>("ListaRecentes"));Assert.Single(a.Tela.Salvos);}
    [AvaloniaFact(DisplayName="DK12 | Fechar com carregador ativo | Cancelamento não grava biblioteca")]
    public void Fechar(){using var a=Tela();var antes=File.ReadAllBytes(a.Tela.Pasta.Arquivo);a.Tela.Janela.Close();Assert.Equal(antes,File.ReadAllBytes(a.Tela.Pasta.Arquivo));}
}
