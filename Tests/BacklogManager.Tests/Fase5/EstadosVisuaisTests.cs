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
    [AvaloniaFact(DisplayName="VE01 | Botão normal | Texto efetivo contrasta com fundo personalizado")]
    public void Normal() { using var t=new TelaTeste();ApoioAparencia.Tema(t,TemaVisual.Moderno,ModoVisual.Claro);ApoioAparencia.Editor(t,ElementoCor.FundoBotao).Campo.Text="#777777";ApoioAparencia.Editor(t,ElementoCor.TextoBotao).Campo.Text="#777777";ApoioAparencia.Render(t);Contraste(Apresentador(t.Campo<Button>("BtnRestaurarCores"))); }
    [AvaloniaFact(DisplayName="VE02 | Botão com hover | Estado muda e texto mantém contraste")]
    public void Hover() { using var t=new TelaTeste();ApoioAparencia.Tema(t,TemaVisual.Tecnologico,ModoVisual.Escuro);var b=t.Campo<Button>("BtnRestaurarCores");t.Janela.MouseMove(b.TranslatePoint(new Point(10,10),t.Janela)!.Value);ApoioAparencia.Render(t);var p=Apresentador(b);Assert.Equal(ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias).Recursos["ButtonHover"],ApoioAparencia.Cor(p.Background));Contraste(p); }
    [AvaloniaFact(DisplayName="VE03 | Botão pressionado | Estado pressionado mantém contraste")]
    public void Pressionado() { using var t=new TelaTeste();ApoioAparencia.Tema(t,TemaVisual.Acolhedor,ModoVisual.Claro);var b=t.Campo<Button>("BtnRestaurarCores");var ponto=b.TranslatePoint(new Point(10,10),t.Janela)!.Value;t.Janela.MouseMove(ponto);t.Janela.MouseDown(ponto,MouseButton.Left);ApoioAparencia.Render(t);Contraste(Apresentador(b));Assert.Equal(ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias).Recursos["ButtonPressed"],ApoioAparencia.Cor(Apresentador(b).Background));t.Janela.MouseUp(ponto,MouseButton.Left); }
    [AvaloniaFact(DisplayName="VE04 | Foco por teclado no botão | Indicador e texto permanecem visíveis")]
    public void Foco() { using var t=new TelaTeste();t.Clicar("NavAparencia");ApoioAparencia.Render(t);var b=t.Campo<Button>("BtnRestaurarCores");b.Focus(NavigationMethod.Tab);ApoioAparencia.Render(t);var p=Apresentador(b);Assert.True(p.BorderThickness.Left>=2);ApoioAparencia.Par(ApoioAparencia.Cor(p.BorderBrush),ApoioAparencia.Cor(p.Background),3); }
    [AvaloniaFact(DisplayName="VE05 | Botão desabilitado | Texto identificável e controle permanece desabilitado")]
    public void Desabilitado() { using var t=new TelaTeste();t.Clicar("NavColecoes");ApoioAparencia.Render(t);var b=t.Campo<Button>("BtnAdicionarObraColecao");Assert.False(b.IsEnabled);Contraste(Apresentador(b)); }
    [AvaloniaFact(DisplayName="VE06 | Seleção de texto no formulário | Texto selecionado contrasta com destaque")]
    public void TextoSelecionado() { using var t=new TelaTeste();t.Clicar("NavCadastro");t.Campo<TextBox>("TxtReview").Text="Texto simulado";var c=t.Campo<TextBox>("TxtReview");c.SelectAll();ApoioAparencia.Render(t);ApoioAparencia.Par(ApoioAparencia.Cor(c.SelectionForegroundBrush),ApoioAparencia.Cor(c.SelectionBrush)); }
    [AvaloniaFact(DisplayName="VE07 | Erro hexadecimal | Mensagem textual e preview anterior preservados")]
    public void Erro() { using var t=new TelaTeste();t.Clicar("NavAparencia");var e=ApoioAparencia.Editor(t,ElementoCor.FundoPrincipal);var antes=ApoioAparencia.Cor(e.Amostra.Background);e.Campo.Text="#123";Assert.Contains("Use #RRGGBB",e.Mensagem.Text!);Assert.Equal(antes,ApoioAparencia.Cor(e.Amostra.Background)); }
    [AvaloniaFact(DisplayName="VE08 | Estado vazio da biblioteca | Orientação continua legível nos dois modos")]
    public void Vazio() { using var t=new TelaTeste();foreach(var m in Enum.GetValues<ModoVisual>()){ApoioAparencia.Tema(t,TemaVisual.Acolhedor,m);t.Clicar("NavBacklog");ApoioAparencia.Render(t);Assert.True(t.Campo<StackPanel>("EstadoListaVazia").IsVisible);var c=t.Campo<TextBlock>("TxtDicaListaVazia");ApoioAparencia.Par(ApoioAparencia.Cor(c.Foreground),ApoioAparencia.Cor(t.Janela.Background));} }
    [AvaloniaFact(DisplayName="VE09 | Seleção com destaque igual ao cartão | Barra permanece visível sobre o cartão")]
    public void Selecao() { using var t=new TelaTeste(new[]{PastaTeste.Jogo()});t.Clicar("NavAparencia");ApoioAparencia.Editor(t,ElementoCor.FundoCartao).Campo.Text="#123456";ApoioAparencia.Editor(t,ElementoCor.Destaque).Campo.Text="#123456";t.Clicar("NavBacklog");t.Campo<ListBox>("ListaBacklog").SelectedIndex=0;ApoioAparencia.Render(t);var barra=t.Campo<ListBox>("ListaBacklog").GetVisualDescendants().OfType<Border>().Single(b=>b.Classes.Contains("indicadorSelecao"));Assert.True(barra.IsVisible);ApoioAparencia.Par(ApoioAparencia.Cor(barra.Background),"#123456",3); }
    [AvaloniaFact(DisplayName="VE10 | Aviso de sucesso | Mensagem mantém contraste com superfície semântica")]
    public void Sucesso() { using var t=new TelaTeste();t.Clicar("NavColecoes");t.Campo<TextBox>("TxtNomeColecao").Text="Coleção simulada";t.Clicar("BtnCriarColecao");ApoioAparencia.Render(t);var aviso=t.Campo<Border>("AvisoOperacao");ApoioAparencia.Par(ApoioAparencia.Cor(t.Campo<TextBlock>("TxtAvisoMensagem").Foreground),ApoioAparencia.Cor(aviso.Background));Assert.Equal("Coleção criada",t.Campo<TextBlock>("TxtAvisoTitulo").Text); }
    [AvaloniaFact(DisplayName="VE11 | Aviso de falha | Ícone texto e mensagem de erro continuam presentes")]
    public void Falha() { using var t=new TelaTeste(semServico:true);ApoioAparencia.Render(t);var aviso=t.Campo<Border>("AvisoOperacao");Assert.Contains("erro", aviso.Classes);ApoioAparencia.Par(ApoioAparencia.Cor(t.Campo<TextBlock>("TxtAvisoMensagem").Foreground),ApoioAparencia.Cor(aviso.Background));Assert.NotEmpty(t.Aviso); }
    [AvaloniaFact(DisplayName="VE12 | Menu contextual existente | Menu aberto recebe superfície e texto do tema")]
    public void Menu() { using var t=new TelaTeste();ApoioAparencia.Tema(t,TemaVisual.Tecnologico,ModoVisual.Escuro);t.Clicar("NavCadastro");var campo=t.Campo<TextBox>("TxtReview");ApoioAparencia.Render(t);var flyout=Assert.IsType<MenuFlyout>(campo.ContextFlyout);flyout.ShowAt(campo);ApoioAparencia.Render(t);Assert.True(flyout.IsOpen);var r=Application.Current!.Resources;ApoioAparencia.Par(ApoioAparencia.Cor(r["MenuFlyoutItemForeground"]),ApoioAparencia.Cor(r["MenuFlyoutPresenterBackground"]));flyout.Hide(); }
    [AvaloniaFact(DisplayName="VE13 | Abrir editores sem editar | Exibir padrões não provoca gravações")]
    public void SomenteAbrir() { var a=new AparenciaSimulada();using var t=new TelaTeste(aparencia:a.Criar());t.Clicar("NavAparencia");ApoioAparencia.Render(t);Assert.Equal(0,a.Gravacoes);Assert.All(t.Campo<WrapPanel>("EditoresCores").Children.OfType<EditorCor>(),e=>Assert.Equal(12,e.GetVisualDescendants().OfType<Button>().Count())); }
    [AvaloniaFact(DisplayName="VE14 | Texto do cartão com fundo independente | Não herda indevidamente o texto do painel")]
    public void CartaoIndependente() { using var t=new TelaTeste(new[]{PastaTeste.Jogo()});t.Clicar("NavAparencia");ApoioAparencia.Editor(t,ElementoCor.FundoPainel).Campo.Text="#000000";ApoioAparencia.Editor(t,ElementoCor.TextoPainel).Campo.Text="#FFFFFF";ApoioAparencia.Editor(t,ElementoCor.FundoCartao).Campo.Text="#FFFFFF";ApoioAparencia.Editor(t,ElementoCor.TextoCartao).Campo.Text="#000000";t.Clicar("NavBacklog");ApoioAparencia.Render(t);var cartao=t.Campo<ListBox>("ListaBacklog").GetVisualDescendants().OfType<Border>().Single(b=>b.Classes.Contains("gradeCard"));Assert.Equal("#FFFFFF",ApoioAparencia.Cor(cartao.Background));var titulo=cartao.GetVisualDescendants().OfType<TextBlock>().First(x=>x.Text=="Jogo simulado");Assert.Equal("#000000",ApoioAparencia.Cor(titulo.Foreground)); }

    [AvaloniaFact(DisplayName="VE15 | Corrigir entrada voltando à cor anterior | Mensagem inválida desaparece sem gravação redundante")]
    public void CorrigirMesmaCor() { var a=new AparenciaSimulada();using var t=new TelaTeste(aparencia:a.Criar());t.Clicar("NavAparencia");var e=ApoioAparencia.Editor(t,ElementoCor.FundoPrincipal);e.Campo.Text="#123456";var n=a.Gravacoes;e.Campo.Text="#12";Assert.True(e.Mensagem.IsVisible);e.Campo.Text="#123456";Assert.False(e.Mensagem.IsVisible);Assert.Equal(n,a.Gravacoes); }

    [AvaloniaFact(DisplayName="VE16 | Foco de teclado com mouse sobre botão | Hover e foco simultâneos mantêm indicador visível")]
    public void FocoHover() { using var t=new TelaTeste();t.Clicar("NavAparencia");ApoioAparencia.Editor(t,ElementoCor.FundoBotao).Campo.Text="#FFFFFF";ApoioAparencia.Editor(t,ElementoCor.TextoBotao).Campo.Text="#000000";ApoioAparencia.Editor(t,ElementoCor.Destaque).Campo.Text="#949494";ApoioAparencia.Render(t);var b=t.Campo<Button>("BtnRestaurarCores");t.Janela.MouseMove(b.TranslatePoint(new Point(10,10),t.Janela)!.Value);b.Focus(NavigationMethod.Tab);ApoioAparencia.Render(t);var p=Apresentador(b);Assert.True(p.BorderThickness.Left>=2);ApoioAparencia.Par(ApoioAparencia.Cor(p.BorderBrush),ApoioAparencia.Cor(p.Background),3);}

    [AvaloniaFact(DisplayName="VE17 | Texto de seleção com destaque limítrofe | Texto pequeno atende a 4.5 mesmo quando indicador aceita 3")]
    public void TextoSelecaoPequeno() { using var t=new TelaTeste(new[]{PastaTeste.Jogo()});t.Clicar("NavAparencia");ApoioAparencia.Editor(t,ElementoCor.FundoCartao).Campo.Text="#FFFFFF";ApoioAparencia.Editor(t,ElementoCor.Destaque).Campo.Text="#949494";t.Clicar("NavBacklog");t.Campo<ListBox>("ListaBacklog").SelectedIndex=0;ApoioAparencia.Render(t);var texto=t.Campo<ListBox>("ListaBacklog").GetVisualDescendants().OfType<TextBlock>().Single(b=>b.Classes.Contains("textoSelecao"));Assert.True(texto.IsVisible);ApoioAparencia.Par(ApoioAparencia.Cor(texto.Foreground),"#FFFFFF");}
}
