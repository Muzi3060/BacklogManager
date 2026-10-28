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

public sealed class HexadecimalTests
{
    [Fact(DisplayName = "VH01 | Preto | Hexadecimal completo normalizado")]
    public void Cenario01() { Assert.True(CoresAparencia.TentarNormalizar("#000000",out var cor));Assert.Equal("#000000",cor); }
    [Fact(DisplayName = "VH02 | Branco | Hexadecimal completo normalizado")]
    public void Cenario02() { Assert.True(CoresAparencia.TentarNormalizar("#FFFFFF",out var cor));Assert.Equal("#FFFFFF",cor); }
    [Fact(DisplayName = "VH03 | Misto | Hexadecimal completo normalizado")]
    public void Cenario03() { Assert.True(CoresAparencia.TentarNormalizar("#aBcDeF",out var cor));Assert.Equal("#ABCDEF",cor); }
    [Fact(DisplayName = "VH04 | Espaços externos | Hexadecimal completo normalizado")]
    public void Cenario04() { Assert.True(CoresAparencia.TentarNormalizar("  #3366cc  ",out var cor));Assert.Equal("#3366CC",cor); }
    [Fact(DisplayName = "VH05 | Fora da paleta | Hexadecimal completo normalizado")]
    public void Cenario05() { Assert.True(CoresAparencia.TentarNormalizar("#F7A1B3",out var cor));Assert.Equal("#F7A1B3",cor); }
    [Fact(DisplayName = "VH06 | Abreviado | Valor recusado e preferência anterior preservada")]
    public void Cenario06() { var a=new AparenciaSimulada();var s=a.Criar();Assert.False(CoresAparencia.TentarNormalizar("#abc",out _));Assert.Throws<ArgumentException>(()=>s.Personalizar(ElementoCor.FundoPrincipal,"#abc"));Assert.Equal(0,a.Gravacoes); }
    [Fact(DisplayName = "VH07 | Transparência | Valor recusado e preferência anterior preservada")]
    public void Cenario07() { var a=new AparenciaSimulada();var s=a.Criar();Assert.False(CoresAparencia.TentarNormalizar("#FF112233",out _));Assert.Throws<ArgumentException>(()=>s.Personalizar(ElementoCor.FundoPrincipal,"#FF112233"));Assert.Equal(0,a.Gravacoes); }
    [Fact(DisplayName = "VH08 | Nome de cor | Valor recusado e preferência anterior preservada")]
    public void Cenario08() { var a=new AparenciaSimulada();var s=a.Criar();Assert.False(CoresAparencia.TentarNormalizar("red",out _));Assert.Throws<ArgumentException>(()=>s.Personalizar(ElementoCor.FundoPrincipal,"red"));Assert.Equal(0,a.Gravacoes); }
    [Fact(DisplayName = "VH09 | Vazio | Valor recusado e preferência anterior preservada")]
    public void Cenario09() { var a=new AparenciaSimulada();var s=a.Criar();Assert.False(CoresAparencia.TentarNormalizar("",out _));Assert.Throws<ArgumentException>(()=>s.Personalizar(ElementoCor.FundoPrincipal,""));Assert.Equal(0,a.Gravacoes); }
    [Fact(DisplayName = "VH10 | Sem cerquilha | Valor recusado e preferência anterior preservada")]
    public void Cenario10() { var a=new AparenciaSimulada();var s=a.Criar();Assert.False(CoresAparencia.TentarNormalizar("3366CC",out _));Assert.Throws<ArgumentException>(()=>s.Personalizar(ElementoCor.FundoPrincipal,"3366CC"));Assert.Equal(0,a.Gravacoes); }
    [Fact(DisplayName = "VH11 | Dígito fora da faixa | Valor recusado e preferência anterior preservada")]
    public void Cenario11() { var a=new AparenciaSimulada();var s=a.Criar();Assert.False(CoresAparencia.TentarNormalizar("#12GG34",out _));Assert.Throws<ArgumentException>(()=>s.Personalizar(ElementoCor.FundoPrincipal,"#12GG34"));Assert.Equal(0,a.Gravacoes); }
    [Fact(DisplayName = "VH12 | Incompleto | Valor recusado e preferência anterior preservada")]
    public void Cenario12() { var a=new AparenciaSimulada();var s=a.Criar();Assert.False(CoresAparencia.TentarNormalizar("#12345",out _));Assert.Throws<ArgumentException>(()=>s.Personalizar(ElementoCor.FundoPrincipal,"#12345"));Assert.Equal(0,a.Gravacoes); }
    [Fact(DisplayName = "VH13 | Sete dígitos | Valor recusado e preferência anterior preservada")]
    public void Cenario13() { var a=new AparenciaSimulada();var s=a.Criar();Assert.False(CoresAparencia.TentarNormalizar("#1234567",out _));Assert.Throws<ArgumentException>(()=>s.Personalizar(ElementoCor.FundoPrincipal,"#1234567"));Assert.Equal(0,a.Gravacoes); }
    [Fact(DisplayName = "VH14 | Espaço interno | Valor recusado e preferência anterior preservada")]
    public void Cenario14() { var a=new AparenciaSimulada();var s=a.Criar();Assert.False(CoresAparencia.TentarNormalizar("#12 456",out _));Assert.Throws<ArgumentException>(()=>s.Personalizar(ElementoCor.FundoPrincipal,"#12 456"));Assert.Equal(0,a.Gravacoes); }
    [Fact(DisplayName = "VH15 | Pontuação | Valor recusado e preferência anterior preservada")]
    public void Cenario15() { var a=new AparenciaSimulada();var s=a.Criar();Assert.False(CoresAparencia.TentarNormalizar("#12345!",out _));Assert.Throws<ArgumentException>(()=>s.Personalizar(ElementoCor.FundoPrincipal,"#12345!"));Assert.Equal(0,a.Gravacoes); }
    [Fact(DisplayName = "VH16 | Cerquilha não ASCII | Valor recusado e preferência anterior preservada")]
    public void Cenario16() { var a=new AparenciaSimulada();var s=a.Criar();Assert.False(CoresAparencia.TentarNormalizar("＃123456",out _));Assert.Throws<ArgumentException>(()=>s.Personalizar(ElementoCor.FundoPrincipal,"＃123456"));Assert.Equal(0,a.Gravacoes); }
    [Fact(DisplayName = "VH17 | Dígitos não ASCII | Valor recusado e preferência anterior preservada")]
    public void Cenario17() { var a=new AparenciaSimulada();var s=a.Criar();Assert.False(CoresAparencia.TentarNormalizar("#１２３４５６",out _));Assert.Throws<ArgumentException>(()=>s.Personalizar(ElementoCor.FundoPrincipal,"#１２３４５６"));Assert.Equal(0,a.Gravacoes); }
    [Fact(DisplayName = "VH18 | Nulo | Não é cor válida")]
    public void Cenario18() { Assert.False(CoresAparencia.TentarNormalizar(null,out _)); }
}
