using Xunit;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using BacklogManager.Aparencia;
using BacklogManager.Controls;
using BacklogManager.Data;
using BacklogManager.Models;
namespace BacklogManager.Tests.Fase5;

public sealed class CompatibilidadeVisualTests
{
    private static void Personalizar(TelaTeste t,string fundo,string sidebar,string painel,string cartao,string botao,string destaque,string texto)
    {
        t.Clicar("NavAparencia");
        var cores=new Dictionary<ElementoCor,string>
        { [ElementoCor.FundoPrincipal]=fundo,[ElementoCor.FundoSidebar]=sidebar,[ElementoCor.FundoPainel]=painel,[ElementoCor.FundoCartao]=cartao,[ElementoCor.FundoBotao]=botao,[ElementoCor.Destaque]=destaque,[ElementoCor.TextoPrincipal]=texto,[ElementoCor.TextoSidebar]=texto,[ElementoCor.TextoPainel]=texto,[ElementoCor.TextoCartao]=texto,[ElementoCor.TextoBotao]=texto,[ElementoCor.TextoDestaque]=texto,[ElementoCor.TextoSecundario]=texto };
        foreach(var (e,cor) in cores)ApoioAparencia.Editor(t,e).Campo.Text=cor;
        ApoioAparencia.Render(t);
        var r=ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias);
        foreach(var (e,cor) in cores){Assert.Equal(cor,r.Solicitadas[e]);Assert.Equal(cor,ApoioAparencia.Editor(t,e).Campo.Text);}
        Assert.Equal(fundo,ApoioAparencia.Cor(t.Janela.Background));
        var lateral=t.Janela.GetVisualDescendants().OfType<Border>().Single(b=>b.Classes.Contains("sidebar"));Assert.Equal(sidebar,ApoioAparencia.Cor(lateral.Background));
        t.Clicar("NavInicio");ApoioAparencia.Capturar(t,"personalizado-"+fundo.TrimStart('#')+"-inicio");
        t.Clicar("NavBacklog");ApoioAparencia.Capturar(t,"personalizado-"+fundo.TrimStart('#')+"-biblioteca");
        var card=t.Campo<ListBox>("ListaBacklog").GetVisualDescendants().OfType<Border>().Single(b=>b.Classes.Contains("gradeCard"));Assert.Equal(cartao,ApoioAparencia.Cor(card.Background));
        foreach(var superficie in new[]{"Page","Sidebar","Panel","Card","Button"})ApoioAparencia.Par(r.Recursos[superficie+"Foreground"],r.Recursos[superficie+"Background"]);
    }
    [AvaloniaFact(DisplayName="VF01 | Personalização azul em todas as superfícies | Cores independentes chegam à janela barra e cartões")]
    public void Azul(){using var t=new TelaTeste(new[]{PastaTeste.Jogo()});Personalizar(t,"#163A63","#0E263F","#214B73","#163A63","#2B579A","#82C5FF","#FFFFFF");}
    [AvaloniaFact(DisplayName="VF02 | Personalização inteiramente acinzentada | Interface funciona sem cores obrigatórias dos presets")]
    public void Cinza(){using var t=new TelaTeste(new[]{PastaTeste.Jogo()});Personalizar(t,"#E0E0E0","#CCCCCC","#F5F5F5","#FFFFFF","#444444","#777777","#111111");}
    [AvaloniaFact(DisplayName="VF03 | Personalização preta | Preto válido aplicado com textos seguros")]
    public void Preto(){using var t=new TelaTeste(new[]{PastaTeste.Jogo()});Personalizar(t,"#000000","#000000","#111111","#171717","#242424","#70D6CB","#FFFFFF");}
    [AvaloniaFact(DisplayName="VF04 | Personalização rosada fora dos temas | Escolhas arbitrárias preservadas com contraste")]
    public void Rosa(){using var t=new TelaTeste(new[]{PastaTeste.Jogo()});Personalizar(t,"#F7A1B3","#F5DCE6","#FFF1F5","#FFCFDF","#7B3049","#804860","#25101C");}
    [AvaloniaFact(DisplayName="VF05 | Personalização quente independente | Painéis e cartões usam fundos próprios")]
    public void Quente(){using var t=new TelaTeste(new[]{PastaTeste.Jogo()});Personalizar(t,"#FFF5CC","#EDCF9D","#FFFAEE","#FFE8AA","#694519","#B87316","#332211");}
    [AvaloniaFact(DisplayName="VF06 | Todas as escolhas com contraste insuficiente | Preferências mantidas e textos efetivos legíveis")]
    public void BaixoContraste(){using var t=new TelaTeste(new[]{PastaTeste.Jogo()});Personalizar(t,"#884422","#884422","#884422","#884422","#884422","#884422","#884422");Assert.Contains(t.Campo<WrapPanel>("EditoresCores").Children.OfType<EditorCor>(),e=>!string.IsNullOrEmpty(e.Ajuste.Text));}
    [AvaloniaFact(DisplayName="VF07 | Lista de sugestões com painel personalizado | Não combina texto da página com fundo do controle")]
    public void Lista(){using var t=new TelaTeste();t.Clicar("NavAparencia");ApoioAparencia.Editor(t,ElementoCor.FundoPainel).Campo.Text="#000000";ApoioAparencia.Editor(t,ElementoCor.TextoPainel).Campo.Text="#FFFFFF";t.Clicar("NavCadastro");var lista=t.Campo<ListBox>("ListaSugestoes");ApoioAparencia.Render(t);Assert.Equal("#000000",ApoioAparencia.Cor(lista.Background));Assert.Equal("#FFFFFF",ApoioAparencia.Cor(lista.Foreground));}
    [AvaloniaFact(DisplayName="VF08 | Menu suspenso com painel personalizado | Itens não selecionados recebem texto e fundo coerentes")]
    public void Combo(){using var t=new TelaTeste();t.Clicar("NavAparencia");ApoioAparencia.Editor(t,ElementoCor.FundoPainel).Campo.Text="#000000";ApoioAparencia.Editor(t,ElementoCor.TextoPainel).Campo.Text="#FFFFFF";var combo=t.Campo<ComboBox>("CmbTema");combo.IsDropDownOpen=true;ApoioAparencia.Render(t);var item=Assert.IsType<ComboBoxItem>(combo.ContainerFromIndex(1));Assert.Equal("#FFFFFF",ApoioAparencia.Cor(item.Foreground));combo.IsDropDownOpen=false;}
    [AvaloniaFact(DisplayName="VF09 | Seleção branca com texto branco solicitado | Seleção efetiva mantém contraste e preserva a solicitação")]
    public void SelecaoTexto(){using var t=new TelaTeste();t.Clicar("NavAparencia");ApoioAparencia.Editor(t,ElementoCor.Destaque).Campo.Text="#FFFFFF";ApoioAparencia.Editor(t,ElementoCor.TextoDestaque).Campo.Text="#FFFFFF";t.Clicar("NavCadastro");ApoioAparencia.Render(t);var c=t.Campo<TextBox>("TxtReview");Assert.Equal("#FFFFFF",ApoioAparencia.Cor(c.SelectionBrush));Assert.Equal("#000000",ApoioAparencia.Cor(c.SelectionForegroundBrush));}
    [AvaloniaFact(DisplayName="VF10 | Fallback de capa após personalizar cartão | Texto e fundo do fallback acompanham cartão")]
    public void Capa(){using var t=new TelaTeste(new[]{PastaTeste.Jogo()});t.Clicar("NavAparencia");ApoioAparencia.Editor(t,ElementoCor.FundoCartao).Campo.Text="#000000";ApoioAparencia.Editor(t,ElementoCor.TextoCartao).Campo.Text="#FFFFFF";t.Clicar("NavBacklog");ApoioAparencia.Render(t);var capa=t.Campo<ListBox>("ListaBacklog").GetVisualDescendants().OfType<CapaObra>().Single();var texto=capa.GetVisualDescendants().OfType<TextBlock>().Single(x=>x.Text=="Sem capa");Assert.Equal("#FFFFFF",ApoioAparencia.Cor(texto.Foreground));Assert.Equal("#000000",ApoioAparencia.Cor(((Border)capa.Content!).Background));}
    [AvaloniaFact(DisplayName="VF11 | Trocar presets sem editar | Preenchimento dos campos não cria personalizações")]
    public void ApenasTrocar(){using var t=new TelaTeste();foreach(var tema in Enum.GetValues<TemaVisual>())foreach(var modo in Enum.GetValues<ModoVisual>()){ApoioAparencia.Tema(t,tema,modo);Assert.Empty(t.Janela.AparenciaAtual.Preferencias.Personalizacoes);}}
    [AvaloniaFact(DisplayName="VF12 | Preferência anterior de lista | Aparência não altera configuração de visualização")]
    public void ListaAnterior(){using var p=new PastaTeste();var caminho=Path.Combine(p.Pasta,"biblioteca.preferencias.json");var pref=new ArmazenamentoPreferenciasBiblioteca(caminho);pref.Salvar(VisualizacaoBiblioteca.Lista);var antes=File.ReadAllBytes(caminho);using var t=new TelaTeste(new[]{PastaTeste.Jogo()},preferencias:pref);Assert.Equal(1,t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex);ApoioAparencia.Tema(t,TemaVisual.Acolhedor,ModoVisual.Escuro);t.Clicar("BtnRestaurarAparencia");t.Clicar("BtnConfirmarRestaurarAparencia");Assert.Equal(antes,File.ReadAllBytes(caminho));Assert.Equal(1,t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex);}
    [AvaloniaFact(DisplayName="VF13 | Preferência JSON aplicada em nova janela | Tema modo e código arbitrário reaparecem após fechar")]
    public void ReinicioReal(){using var p=new PastaTeste();var caminho=Path.Combine(p.Pasta,"aparencia.json");using(var t=new TelaTeste(aparencia:new ServicoAparencia(new ArmazenamentoAparenciaJson(caminho)))){ApoioAparencia.Tema(t,TemaVisual.Tecnologico,ModoVisual.Escuro);ApoioAparencia.Editor(t,ElementoCor.FundoPrincipal).Campo.Text="#163A63";}using var b=new TelaTeste(aparencia:new ServicoAparencia(new ArmazenamentoAparenciaJson(caminho)));Assert.Equal(TemaVisual.Tecnologico,b.Janela.AparenciaAtual.Preferencias.Tema);Assert.Equal(ModoVisual.Escuro,b.Janela.AparenciaAtual.Preferencias.Modo);Assert.Equal("#163A63",ApoioAparencia.Cor(b.Janela.Background));}
}
