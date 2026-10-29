using Xunit;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Presenters;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.VisualTree;
using BacklogManager.Aparencia;
using BacklogManager.Controls;
namespace BacklogManager.Tests.Fase5;

public sealed class EstadosVisuaisTests
{
    private static ContentPresenter Apresentador(Button b) => b.GetVisualDescendants().OfType<ContentPresenter>().First(p => p.Name == "PART_ContentPresenter");
    private static void Contraste(ContentPresenter c) => ApoioAparencia.Par(ApoioAparencia.Cor(c.Foreground), ApoioAparencia.Cor(c.Background));




    [AvaloniaFact(DisplayName="VE05 | Botão desabilitado | Texto identificável e controle permanece desabilitado")]
    public void Desabilitado() { using var t=new TelaTeste();t.Clicar("NavColecoes");ApoioAparencia.Render(t);var b=t.Campo<Button>("BtnAdicionarObraColecao");Assert.False(b.IsEnabled);Contraste(Apresentador(b)); }
    [AvaloniaFact(DisplayName="VE06 | Seleção de texto no formulário | Texto selecionado contrasta com destaque")]
    public void TextoSelecionado() { using var t=new TelaTeste();t.Clicar("NavCadastro");t.Campo<TextBox>("TxtReview").Text="Texto simulado";var c=t.Campo<TextBox>("TxtReview");c.SelectAll();ApoioAparencia.Render(t);ApoioAparencia.Par(ApoioAparencia.Cor(c.SelectionForegroundBrush),ApoioAparencia.Cor(c.SelectionBrush)); }

    [AvaloniaFact(DisplayName="VE08 | Estado vazio da biblioteca | Orientação continua legível nos dois modos")]
    public void Vazio() { using var t=new TelaTeste();foreach(var m in Enum.GetValues<ModoVisual>()){ApoioAparencia.Tema(t,TemaVisual.Acolhedor,m);t.Clicar("NavBacklog");ApoioAparencia.Render(t);Assert.True(t.Campo<StackPanel>("EstadoListaVazia").IsVisible);var c=t.Campo<TextBlock>("TxtDicaListaVazia");ApoioAparencia.Par(ApoioAparencia.Cor(c.Foreground),ApoioAparencia.Cor(t.Janela.Background));} }

    [AvaloniaFact(DisplayName="VE10 | Aviso de sucesso | Mensagem mantém contraste com superfície semântica")]
    public void Sucesso() { using var t=new TelaTeste();t.Clicar("NavColecoes");t.Campo<TextBox>("TxtNomeColecao").Text="Coleção simulada";t.Clicar("BtnCriarColecao");ApoioAparencia.Render(t);var aviso=t.Campo<Border>("AvisoOperacao");ApoioAparencia.Par(ApoioAparencia.Cor(t.Campo<TextBlock>("TxtAvisoMensagem").Foreground),ApoioAparencia.Cor(aviso.Background));Assert.Equal("Coleção criada",t.Campo<TextBlock>("TxtAvisoTitulo").Text); }
    [AvaloniaFact(DisplayName="VE11 | Aviso de falha | Ícone texto e mensagem de erro continuam presentes")]
    public void Falha() { using var t=new TelaTeste(semServico:true);ApoioAparencia.Render(t);var aviso=t.Campo<Border>("AvisoOperacao");Assert.Contains("erro", aviso.Classes);ApoioAparencia.Par(ApoioAparencia.Cor(t.Campo<TextBlock>("TxtAvisoMensagem").Foreground),ApoioAparencia.Cor(aviso.Background));Assert.NotEmpty(t.Aviso); }
    [AvaloniaFact(DisplayName="VE12 | Menu contextual existente | Menu aberto recebe superfície e texto do tema")]
    public void Menu() { using var t=new TelaTeste();ApoioAparencia.Tema(t,TemaVisual.Tecnologico,ModoVisual.Escuro);t.Clicar("NavCadastro");var campo=t.Campo<TextBox>("TxtReview");ApoioAparencia.Render(t);var flyout=Assert.IsType<MenuFlyout>(campo.ContextFlyout);flyout.ShowAt(campo);ApoioAparencia.Render(t);Assert.True(flyout.IsOpen);var r=Application.Current!.Resources;ApoioAparencia.Par(ApoioAparencia.Cor(r["MenuFlyoutItemForeground"]),ApoioAparencia.Cor(r["MenuFlyoutPresenterBackground"]));flyout.Hide(); }








}
