using Xunit;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;
using BacklogManager.Aparencia;
using BacklogManager.Controls;
using BacklogManager.Data;
using BacklogManager.Models;
using System.Text.Json.Nodes;
namespace BacklogManager.Tests.Fase5;

public sealed class ContrasteTests
{
    [Fact(DisplayName = "VC01 | Contraste pronto Moderno Claro | Todos os pares de texto e indicadores atendem aos mínimos")]
    public void Cenario01() { var r=ResolvedorAparencia.Resolver(new(){Tema=TemaVisual.Moderno,Modo=ModoVisual.Claro});foreach(var superficie in new[]{"Page","Sidebar","Panel","Card"}){ApoioAparencia.Par(r.Recursos[superficie+"Foreground"],r.Recursos[superficie+"Background"]);ApoioAparencia.Par(r.Recursos[superficie+"Muted"],r.Recursos[superficie+"Background"]);ApoioAparencia.Par(r.Recursos[superficie+"Border"],r.Recursos[superficie+"Background"],3);}Assert.Empty(r.Avisos); }
    [Fact(DisplayName = "VC02 | Contraste pronto Moderno Escuro | Todos os pares de texto e indicadores atendem aos mínimos")]
    public void Cenario02() { var r=ResolvedorAparencia.Resolver(new(){Tema=TemaVisual.Moderno,Modo=ModoVisual.Escuro});foreach(var superficie in new[]{"Page","Sidebar","Panel","Card"}){ApoioAparencia.Par(r.Recursos[superficie+"Foreground"],r.Recursos[superficie+"Background"]);ApoioAparencia.Par(r.Recursos[superficie+"Muted"],r.Recursos[superficie+"Background"]);ApoioAparencia.Par(r.Recursos[superficie+"Border"],r.Recursos[superficie+"Background"],3);}Assert.Empty(r.Avisos); }
    [Fact(DisplayName = "VC03 | Contraste pronto Tecnologico Claro | Todos os pares de texto e indicadores atendem aos mínimos")]
    public void Cenario03() { var r=ResolvedorAparencia.Resolver(new(){Tema=TemaVisual.Tecnologico,Modo=ModoVisual.Claro});foreach(var superficie in new[]{"Page","Sidebar","Panel","Card"}){ApoioAparencia.Par(r.Recursos[superficie+"Foreground"],r.Recursos[superficie+"Background"]);ApoioAparencia.Par(r.Recursos[superficie+"Muted"],r.Recursos[superficie+"Background"]);ApoioAparencia.Par(r.Recursos[superficie+"Border"],r.Recursos[superficie+"Background"],3);}Assert.Empty(r.Avisos); }
    [Fact(DisplayName = "VC04 | Contraste pronto Tecnologico Escuro | Todos os pares de texto e indicadores atendem aos mínimos")]
    public void Cenario04() { var r=ResolvedorAparencia.Resolver(new(){Tema=TemaVisual.Tecnologico,Modo=ModoVisual.Escuro});foreach(var superficie in new[]{"Page","Sidebar","Panel","Card"}){ApoioAparencia.Par(r.Recursos[superficie+"Foreground"],r.Recursos[superficie+"Background"]);ApoioAparencia.Par(r.Recursos[superficie+"Muted"],r.Recursos[superficie+"Background"]);ApoioAparencia.Par(r.Recursos[superficie+"Border"],r.Recursos[superficie+"Background"],3);}Assert.Empty(r.Avisos); }
    [Fact(DisplayName = "VC05 | Contraste pronto Acolhedor Claro | Todos os pares de texto e indicadores atendem aos mínimos")]
    public void Cenario05() { var r=ResolvedorAparencia.Resolver(new(){Tema=TemaVisual.Acolhedor,Modo=ModoVisual.Claro});foreach(var superficie in new[]{"Page","Sidebar","Panel","Card"}){ApoioAparencia.Par(r.Recursos[superficie+"Foreground"],r.Recursos[superficie+"Background"]);ApoioAparencia.Par(r.Recursos[superficie+"Muted"],r.Recursos[superficie+"Background"]);ApoioAparencia.Par(r.Recursos[superficie+"Border"],r.Recursos[superficie+"Background"],3);}Assert.Empty(r.Avisos); }
    [Fact(DisplayName = "VC06 | Contraste pronto Acolhedor Escuro | Todos os pares de texto e indicadores atendem aos mínimos")]
    public void Cenario06() { var r=ResolvedorAparencia.Resolver(new(){Tema=TemaVisual.Acolhedor,Modo=ModoVisual.Escuro});foreach(var superficie in new[]{"Page","Sidebar","Panel","Card"}){ApoioAparencia.Par(r.Recursos[superficie+"Foreground"],r.Recursos[superficie+"Background"]);ApoioAparencia.Par(r.Recursos[superficie+"Muted"],r.Recursos[superficie+"Background"]);ApoioAparencia.Par(r.Recursos[superficie+"Border"],r.Recursos[superficie+"Background"],3);}Assert.Empty(r.Avisos); }
    [Fact(DisplayName = "VC07 | Branco sobre branco | Texto efetivo legível e preferência solicitada intacta")]
    public void Cenario07() { var s=new AparenciaSimulada().Criar();s.Personalizar(ElementoCor.FundoPrincipal,"#FFFFFF");s.Personalizar(ElementoCor.TextoPrincipal,"#FFFFFF");var r=ResolvedorAparencia.Resolver(s.Preferencias);ApoioAparencia.Par(r.Recursos["PageForeground"],r.Recursos["PageBackground"]);Assert.Equal("#FFFFFF",r.Solicitadas[ElementoCor.TextoPrincipal]); }
    [Fact(DisplayName = "VC08 | Preto sobre preto | Texto efetivo legível e preferência solicitada intacta")]
    public void Cenario08() { var s=new AparenciaSimulada().Criar();s.Personalizar(ElementoCor.FundoPrincipal,"#000000");s.Personalizar(ElementoCor.TextoPrincipal,"#000000");var r=ResolvedorAparencia.Resolver(s.Preferencias);ApoioAparencia.Par(r.Recursos["PageForeground"],r.Recursos["PageBackground"]);Assert.Equal("#000000",r.Solicitadas[ElementoCor.TextoPrincipal]); }
    [Fact(DisplayName = "VC09 | Azul sobre azul | Texto efetivo legível e preferência solicitada intacta")]
    public void Cenario09() { var s=new AparenciaSimulada().Criar();s.Personalizar(ElementoCor.FundoPrincipal,"#3366CC");s.Personalizar(ElementoCor.TextoPrincipal,"#3366CC");var r=ResolvedorAparencia.Resolver(s.Preferencias);ApoioAparencia.Par(r.Recursos["PageForeground"],r.Recursos["PageBackground"]);Assert.Equal("#3366CC",r.Solicitadas[ElementoCor.TextoPrincipal]); }
    [Fact(DisplayName = "VC10 | Dourado sobre rosa | Texto efetivo legível e preferência solicitada intacta")]
    public void Cenario10() { var s=new AparenciaSimulada().Criar();s.Personalizar(ElementoCor.FundoPrincipal,"#F5DCE6");s.Personalizar(ElementoCor.TextoPrincipal,"#DAAF59");var r=ResolvedorAparencia.Resolver(s.Preferencias);ApoioAparencia.Par(r.Recursos["PageForeground"],r.Recursos["PageBackground"]);Assert.Equal("#DAAF59",r.Solicitadas[ElementoCor.TextoPrincipal]); }
    [Fact(DisplayName = "VC11 | Cinzas iguais | Texto efetivo legível e preferência solicitada intacta")]
    public void Cenario11() { var s=new AparenciaSimulada().Criar();s.Personalizar(ElementoCor.FundoPrincipal,"#777777");s.Personalizar(ElementoCor.TextoPrincipal,"#777777");var r=ResolvedorAparencia.Resolver(s.Preferencias);ApoioAparencia.Par(r.Recursos["PageForeground"],r.Recursos["PageBackground"]);Assert.Equal("#777777",r.Solicitadas[ElementoCor.TextoPrincipal]); }
    [Fact(DisplayName = "VC12 | Par próximo do limite válido | Texto efetivo legível e preferência solicitada intacta")]
    public void Cenario12() { var s=new AparenciaSimulada().Criar();s.Personalizar(ElementoCor.FundoPrincipal,"#FFFFFF");s.Personalizar(ElementoCor.TextoPrincipal,"#767676");var r=ResolvedorAparencia.Resolver(s.Preferencias);ApoioAparencia.Par(r.Recursos["PageForeground"],r.Recursos["PageBackground"]);Assert.Equal("#767676",r.Solicitadas[ElementoCor.TextoPrincipal]); }
    [Fact(DisplayName = "VC13 | Fundo volta a permitir o texto escolhido | Cor solicitada reaplicada sem perdê-la")]
    public void Cenario13() { var s=new AparenciaSimulada().Criar();s.Personalizar(ElementoCor.TextoPrincipal,"#FFFFFF");Assert.NotEqual("#FFFFFF",ResolvedorAparencia.Resolver(s.Preferencias).Recursos["PageForeground"]);s.Personalizar(ElementoCor.FundoPrincipal,"#000000");Assert.Equal("#FFFFFF",ResolvedorAparencia.Resolver(s.Preferencias).Recursos["PageForeground"]); }
    [Fact(DisplayName = "VC14 | Contraste matemático conhecido | Preto e branco têm razão 21 e mesma cor tem razão 1")]
    public void Cenario14() { Assert.Equal(21,CoresAparencia.Contraste("#000000","#FFFFFF"),6);Assert.Equal(1,CoresAparencia.Contraste("#123456","#123456"),6); }
    [Fact(DisplayName = "VC15 | Hover e pressionado de botão customizado | Cada estado tem texto legível")]
    public void Cenario15() { var s=new AparenciaSimulada().Criar();s.Personalizar(ElementoCor.FundoBotao,"#777777");s.Personalizar(ElementoCor.TextoBotao,"#888888");var r=ResolvedorAparencia.Resolver(s.Preferencias).Recursos;ApoioAparencia.Par(r["ButtonForeground"],r["ButtonBackground"]);ApoioAparencia.Par(r["ButtonHoverText"],r["ButtonHover"]);ApoioAparencia.Par(r["ButtonPressedText"],r["ButtonPressed"]); }
    [Fact(DisplayName = "VC16 | Destaque igual ao cartão | Indicador derivado contrasta sem apagar a escolha")]
    public void Cenario16() { var s=new AparenciaSimulada().Criar();s.Personalizar(ElementoCor.FundoCartao,"#123456");s.Personalizar(ElementoCor.Destaque,"#123456");var r=ResolvedorAparencia.Resolver(s.Preferencias);ApoioAparencia.Par(r.Recursos["SelectionBrush"],r.Recursos["CardBackground"],3);Assert.Equal("#123456",r.Solicitadas[ElementoCor.Destaque]); }
    [Fact(DisplayName = "VC17 | Sucesso aviso e erro nos dois modos | Mensagens semânticas têm texto legível")]
    public void Cenario17() { foreach(var m in Enum.GetValues<ModoVisual>()){var r=ResolvedorAparencia.Resolver(new(){Modo=m}).Recursos;foreach(var tipo in new[]{"Success","Warning","Error"})ApoioAparencia.Par(r[tipo+"Brush"],r[tipo+"Background"]);} }
    [Fact(DisplayName = "VC18 | Texto secundário de baixo contraste | Aviso explícito e texto seguro")]
    public void Cenario18() { var s=new AparenciaSimulada().Criar();s.Personalizar(ElementoCor.TextoSecundario,"#F3F4FA");var r=ResolvedorAparencia.Resolver(s.Preferencias);Assert.Contains(ElementoCor.TextoSecundario,r.Avisos.Keys);ApoioAparencia.Par(r.Recursos["MutedForeground"],r.Recursos["PageBackground"]); }
    [Fact(DisplayName = "VC19 | Todos os fundos customizados de uma só cor | Textos gerais e de grupos continuam legíveis")]
    public void Cenario19() { var s=new AparenciaSimulada().Criar();foreach(var e in Enum.GetValues<ElementoCor>())s.Personalizar(e,"#884422");var r=ResolvedorAparencia.Resolver(s.Preferencias).Recursos;foreach(var grupo in new[]{"Page","Sidebar","Panel","Card","Button"})ApoioAparencia.Par(r[grupo+"Foreground"],r[grupo+"Background"]);ApoioAparencia.Par(r["AccentForeground"],r["AccentBrush"]); }

    [Fact(DisplayName="VC20 | Foco com destaque limítrofe e botão branco | Indicador contrasta nos estados normal hover e pressionado")]
    public void FocoEstados() { var s=new AparenciaSimulada().Criar();s.Personalizar(ElementoCor.FundoBotao,"#FFFFFF");s.Personalizar(ElementoCor.TextoBotao,"#000000");s.Personalizar(ElementoCor.Destaque,"#949494");var r=ResolvedorAparencia.Resolver(s.Preferencias).Recursos;foreach(var fundo in new[]{"ButtonBackground","ButtonHover","ButtonPressed"})ApoioAparencia.Par(r["ButtonFocus"],r[fundo],3);Assert.Equal("#949494",ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas[ElementoCor.Destaque]);}
}
