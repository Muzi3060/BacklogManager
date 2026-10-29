using Xunit;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;
using BacklogManager.Models;
using BacklogManager.Services;
using BacklogManager.Data;
using BacklogManager.Controls;
using BacklogManager.Aparencia;
namespace BacklogManager.Tests.Fase6;

public sealed class InterfaceDashboardTests
{
    [AvaloniaFact(DisplayName = "DU01 | Dashboard Moderno Claro | Resumo distribuições e período renderizam com o tema")]
    public void Caso01() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item(),ApoioDashboard.Item(2,"Jogando"));Fase5.ApoioAparencia.Tema(t,TemaVisual.Moderno,ModoVisual.Claro);t.Clicar("NavInicio");ApoioDashboard.Render(t);Assert.Equal("2",t.Campo<TextBlock>("TxtTotal").Text);Assert.Equal(6,t.Campo<ItemsControl>("DistribuicaoMidias").Items.Count);var pasta=Path.Combine(Path.GetTempPath(),"backlogmanager-fase6-resultados","imagens");Directory.CreateDirectory(pasta);using var imagem=t.Janela.CaptureRenderedFrame();Assert.NotNull(imagem);imagem.Save(Path.Combine(pasta,"Moderno-Claro.png"),PngBitmapEncoderOptions.Default); }
    [AvaloniaFact(DisplayName = "DU02 | Dashboard Moderno Escuro | Resumo distribuições e período renderizam com o tema")]
    public void Caso02() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item(),ApoioDashboard.Item(2,"Jogando"));Fase5.ApoioAparencia.Tema(t,TemaVisual.Moderno,ModoVisual.Escuro);t.Clicar("NavInicio");ApoioDashboard.Render(t);Assert.Equal("2",t.Campo<TextBlock>("TxtTotal").Text);Assert.Equal(6,t.Campo<ItemsControl>("DistribuicaoMidias").Items.Count);var pasta=Path.Combine(Path.GetTempPath(),"backlogmanager-fase6-resultados","imagens");Directory.CreateDirectory(pasta);using var imagem=t.Janela.CaptureRenderedFrame();Assert.NotNull(imagem);imagem.Save(Path.Combine(pasta,"Moderno-Escuro.png"),PngBitmapEncoderOptions.Default); }
    [AvaloniaFact(DisplayName = "DU03 | Dashboard Tecnologico Claro | Resumo distribuições e período renderizam com o tema")]
    public void Caso03() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item(),ApoioDashboard.Item(2,"Jogando"));Fase5.ApoioAparencia.Tema(t,TemaVisual.Tecnologico,ModoVisual.Claro);t.Clicar("NavInicio");ApoioDashboard.Render(t);Assert.Equal("2",t.Campo<TextBlock>("TxtTotal").Text);Assert.Equal(6,t.Campo<ItemsControl>("DistribuicaoMidias").Items.Count);var pasta=Path.Combine(Path.GetTempPath(),"backlogmanager-fase6-resultados","imagens");Directory.CreateDirectory(pasta);using var imagem=t.Janela.CaptureRenderedFrame();Assert.NotNull(imagem);imagem.Save(Path.Combine(pasta,"Tecnologico-Claro.png"),PngBitmapEncoderOptions.Default); }
    [AvaloniaFact(DisplayName = "DU04 | Dashboard Tecnologico Escuro | Resumo distribuições e período renderizam com o tema")]
    public void Caso04() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item(),ApoioDashboard.Item(2,"Jogando"));Fase5.ApoioAparencia.Tema(t,TemaVisual.Tecnologico,ModoVisual.Escuro);t.Clicar("NavInicio");ApoioDashboard.Render(t);Assert.Equal("2",t.Campo<TextBlock>("TxtTotal").Text);Assert.Equal(6,t.Campo<ItemsControl>("DistribuicaoMidias").Items.Count);var pasta=Path.Combine(Path.GetTempPath(),"backlogmanager-fase6-resultados","imagens");Directory.CreateDirectory(pasta);using var imagem=t.Janela.CaptureRenderedFrame();Assert.NotNull(imagem);imagem.Save(Path.Combine(pasta,"Tecnologico-Escuro.png"),PngBitmapEncoderOptions.Default); }
    [AvaloniaFact(DisplayName = "DU05 | Dashboard Acolhedor Claro | Resumo distribuições e período renderizam com o tema")]
    public void Caso05() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item(),ApoioDashboard.Item(2,"Jogando"));Fase5.ApoioAparencia.Tema(t,TemaVisual.Acolhedor,ModoVisual.Claro);t.Clicar("NavInicio");ApoioDashboard.Render(t);Assert.Equal("2",t.Campo<TextBlock>("TxtTotal").Text);Assert.Equal(6,t.Campo<ItemsControl>("DistribuicaoMidias").Items.Count);var pasta=Path.Combine(Path.GetTempPath(),"backlogmanager-fase6-resultados","imagens");Directory.CreateDirectory(pasta);using var imagem=t.Janela.CaptureRenderedFrame();Assert.NotNull(imagem);imagem.Save(Path.Combine(pasta,"Acolhedor-Claro.png"),PngBitmapEncoderOptions.Default); }
    [AvaloniaFact(DisplayName = "DU06 | Dashboard Acolhedor Escuro | Resumo distribuições e período renderizam com o tema")]
    public void Caso06() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item(),ApoioDashboard.Item(2,"Jogando"));Fase5.ApoioAparencia.Tema(t,TemaVisual.Acolhedor,ModoVisual.Escuro);t.Clicar("NavInicio");ApoioDashboard.Render(t);Assert.Equal("2",t.Campo<TextBlock>("TxtTotal").Text);Assert.Equal(6,t.Campo<ItemsControl>("DistribuicaoMidias").Items.Count);var pasta=Path.Combine(Path.GetTempPath(),"backlogmanager-fase6-resultados","imagens");Directory.CreateDirectory(pasta);using var imagem=t.Janela.CaptureRenderedFrame();Assert.NotNull(imagem);imagem.Save(Path.Combine(pasta,"Acolhedor-Escuro.png"),PngBitmapEncoderOptions.Default); }
    [AvaloniaFact(DisplayName = "DU07 | Janela mínima | Navegação e conteúdo alcançáveis por rolagem")]
    public void Caso07() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item());t.Janela.Width=900;t.Janela.Height=660;ApoioDashboard.Render(t);Assert.True(t.Campo<Button>("NavInicio").Bounds.Height>0);var p=Path.Combine(Path.GetTempPath(),"backlogmanager-fase6-resultados","imagens");Directory.CreateDirectory(p);using(var img=t.Janela.CaptureRenderedFrame()){Assert.NotNull(img);img.Save(Path.Combine(p,"minimo-topo.png"),PngBitmapEncoderOptions.Default);}t.Campo<ScrollViewer>("PaginaInicio").Offset=new Vector(0,10000);ApoioDashboard.Render(t);using var fim=t.Janela.CaptureRenderedFrame();Assert.NotNull(fim);fim.Save(Path.Combine(p,"minimo-fim.png"),PngBitmapEncoderOptions.Default); }
    [AvaloniaFact(DisplayName = "DU08 | Biblioteca inválida | Estatísticas indisponíveis não simulam biblioteca vazia")]
    public void Caso08() { using var t=new TelaTeste(json:"{");Assert.True(t.Campo<Border>("FalhaDashboard").IsVisible);Assert.False(t.Campo<StackPanel>("ConteudoDashboard").IsVisible);Assert.Equal("—",t.Campo<TextBlock>("TxtTotal").Text);Assert.Equal("{",File.ReadAllText(t.Pasta.Arquivo)); }
    [AvaloniaFact(DisplayName = "DU09 | Biblioteca vazia | Estado vazio distinto de falha")]
    public void Caso09() { using var t=ApoioDashboard.Tela();Assert.True(t.Campo<Border>("EstadoInicioVazio").IsVisible);Assert.False(t.Campo<Border>("FalhaDashboard").IsVisible);Assert.Equal("0",t.Campo<TextBlock>("TxtTotal").Text); }
    [AvaloniaFact(DisplayName = "DU10 | Alternar 7 e 30 dias | Só a atividade temporal muda")]
    public void Caso10() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item(10,"Jogando"));Assert.Equal("1",t.Campo<TextBlock>("TxtAdicionadas").Text);t.Campo<ComboBox>("CmbPeriodoDashboard").SelectedIndex=0;Assert.Equal("0",t.Campo<TextBlock>("TxtAdicionadas").Text);Assert.Equal("1",t.Campo<TextBlock>("TxtJogando").Text); }
    [AvaloniaFact(DisplayName = "DU11 | Datas ausentes | Não aparecem na atividade recente")]
    public void Caso11() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item() with{DataAdicao=null});Assert.Empty(t.Campo<ItemsControl>("ListaAtividade").Items);Assert.Single(t.Campo<ItemsControl>("ListaRecentes").Items);Assert.Contains("1 obra sem data",t.Campo<TextBlock>("TxtDatasDashboard").Text!); }
    [AvaloniaFact(DisplayName = "DU12 | Sem progresso | Interface não gera percentual pessoal")]
    public void Caso12() { var i=ApoioDashboard.Item(status:"Jogando");i=i with{Pessoal=i.Pessoal with{Progresso=null}};using var t=ApoioDashboard.Tela(i);var e=Assert.IsType<EntradaDashboard>(t.Campo<ItemsControl>("ListaContinuar").Items[0]);Assert.Equal("Progresso não informado",e.Progresso); }
    [AvaloniaFact(DisplayName = "DU13 | Abertura da atividade | Abre edição existente sem duplicar obra")]
    public void Caso13() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item());ApoioDashboard.Render(t);var b=t.Campo<ItemsControl>("ListaAtividade").GetVisualDescendants().OfType<Button>().First();b.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Button.ClickEvent));Assert.True(t.Campo<ScrollViewer>("PaginaCadastro").IsVisible);Assert.Single(t.Salvos); }
    [AvaloniaFact(DisplayName = "DU14 | Edição atualiza Dashboard | Contagem muda após gravação confirmada")]
    public void Caso14() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item(status:"Jogando"));t.Editar();var c=t.Campo<ComboBox>("CmbStatus");c.SelectedIndex=2;t.Clicar("BtnAtualizar");Assert.Equal("1",t.Campo<TextBlock>("TxtZerados").Text);Assert.Empty(t.Campo<ItemsControl>("ListaContinuar").Items); }
    [AvaloniaFact(DisplayName = "DU15 | Exclusão atualiza Dashboard | Total e atividade removem registro salvo")]
    public void Caso15() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item());t.Campo<ListBox>("ListaBacklog").SelectedIndex=0;t.Clicar("BtnRemover");Assert.Equal("0",t.Campo<TextBlock>("TxtTotal").Text);Assert.Empty(t.Campo<ItemsControl>("ListaAtividade").Items); }
    [AvaloniaFact(DisplayName = "DU16 | Filtros não contaminam totais | Resumo reflete biblioteca completa")]
    public void Caso16() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item(),ApoioDashboard.Item(tipo:TipoMidia.Livro));t.Campo<ComboBox>("CmbTipoBiblioteca").SelectedIndex=(int)TipoMidia.Livro;t.Clicar("NavInicio");Assert.Equal("2",t.Campo<TextBlock>("TxtTotal").Text);Assert.Equal("2",t.Campo<TextBlock>("TxtAdicionadas").Text); }
    [AvaloniaFact(DisplayName = "DU17 | Leitura não grava arquivos | Bytes da biblioteca preservados ao trocar período")]
    public void Caso17() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item());var antes=File.ReadAllBytes(t.Pasta.Arquivo);for(var n=0;n<4;n++){t.Campo<ComboBox>("CmbPeriodoDashboard").SelectedIndex=n%2;t.Clicar("NavInicio");}Assert.Equal(antes,File.ReadAllBytes(t.Pasta.Arquivo));Assert.Equal(0,t.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "DU18 | Promoções e conclusões | Mensagens não inventam métricas nem ofertas")]
    public void Caso18() { using var t=ApoioDashboard.Tela();ApoioDashboard.Render(t);var textos=t.Campo<ScrollViewer>("PaginaInicio").GetVisualDescendants().OfType<TextBlock>().Select(x=>x.Text??"").ToArray();Assert.Contains(textos,s=>s.Contains("Nenhuma oferta"));Assert.Contains(textos,s=>s.Contains("não há datas de conclusão"));Assert.DoesNotContain(textos,s=>s.Contains("R$")||s.Contains("lista de desejos")); }
    [AvaloniaFact(DisplayName = "DU19 | Cores personalizadas | Recursos do painel acompanham cor escolhida")]
    public void Caso19() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item());t.Clicar("NavAparencia");Fase5.ApoioAparencia.Editor(t,ElementoCor.FundoPainel).Campo.Text="#163A63";Fase5.ApoioAparencia.Editor(t,ElementoCor.TextoPainel).Campo.Text="#FFFFFF";t.Clicar("NavInicio");ApoioDashboard.Render(t);var c=t.Campo<ItemsControl>("DistribuicaoMidias").GetVisualDescendants().OfType<Border>().First(b=>b.Classes.Contains("card"));Assert.Equal("#163A63",Fase5.ApoioAparencia.Cor(c.Background)); }
    [AvaloniaFact(DisplayName = "DU20 | Teclado no período | Seletor recebe foco de teclado")]
    public void Caso20() { using var t=ApoioDashboard.Tela();ApoioDashboard.Render(t);var c=t.Campo<ComboBox>("CmbPeriodoDashboard");Assert.True(c.Focus(Avalonia.Input.NavigationMethod.Tab));Assert.True(c.IsFocused); }
}
