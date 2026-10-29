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
    [Fact(DisplayName = "VH06 | Abreviado | Utilitário interno recusa formato inválido")]
    public void Cenario06() { Assert.False(CoresAparencia.TentarNormalizar("#abc",out _));Assert.Throws<ArgumentException>(()=>CoresAparencia.Normalizar("#abc")); }
    [Fact(DisplayName = "VH07 | Transparência | Utilitário interno recusa formato inválido")]
    public void Cenario07() { Assert.False(CoresAparencia.TentarNormalizar("#FF112233",out _));Assert.Throws<ArgumentException>(()=>CoresAparencia.Normalizar("#FF112233")); }
    [Fact(DisplayName = "VH08 | Nome de cor | Utilitário interno recusa formato inválido")]
    public void Cenario08() { Assert.False(CoresAparencia.TentarNormalizar("red",out _));Assert.Throws<ArgumentException>(()=>CoresAparencia.Normalizar("red")); }
    [Fact(DisplayName = "VH09 | Vazio | Utilitário interno recusa formato inválido")]
    public void Cenario09() { Assert.False(CoresAparencia.TentarNormalizar("",out _));Assert.Throws<ArgumentException>(()=>CoresAparencia.Normalizar("")); }
    [Fact(DisplayName = "VH10 | Sem cerquilha | Utilitário interno recusa formato inválido")]
    public void Cenario10() { Assert.False(CoresAparencia.TentarNormalizar("3366CC",out _));Assert.Throws<ArgumentException>(()=>CoresAparencia.Normalizar("3366CC")); }
    [Fact(DisplayName = "VH11 | Dígito fora da faixa | Utilitário interno recusa formato inválido")]
    public void Cenario11() { Assert.False(CoresAparencia.TentarNormalizar("#12GG34",out _));Assert.Throws<ArgumentException>(()=>CoresAparencia.Normalizar("#12GG34")); }
    [Fact(DisplayName = "VH12 | Incompleto | Utilitário interno recusa formato inválido")]
    public void Cenario12() { Assert.False(CoresAparencia.TentarNormalizar("#12345",out _));Assert.Throws<ArgumentException>(()=>CoresAparencia.Normalizar("#12345")); }
    [Fact(DisplayName = "VH13 | Sete dígitos | Utilitário interno recusa formato inválido")]
    public void Cenario13() { Assert.False(CoresAparencia.TentarNormalizar("#1234567",out _));Assert.Throws<ArgumentException>(()=>CoresAparencia.Normalizar("#1234567")); }
    [Fact(DisplayName = "VH14 | Espaço interno | Utilitário interno recusa formato inválido")]
    public void Cenario14() { Assert.False(CoresAparencia.TentarNormalizar("#12 456",out _));Assert.Throws<ArgumentException>(()=>CoresAparencia.Normalizar("#12 456")); }
    [Fact(DisplayName = "VH15 | Pontuação | Utilitário interno recusa formato inválido")]
    public void Cenario15() { Assert.False(CoresAparencia.TentarNormalizar("#12345!",out _));Assert.Throws<ArgumentException>(()=>CoresAparencia.Normalizar("#12345!")); }
    [Fact(DisplayName = "VH16 | Cerquilha não ASCII | Utilitário interno recusa formato inválido")]
    public void Cenario16() { Assert.False(CoresAparencia.TentarNormalizar("＃123456",out _));Assert.Throws<ArgumentException>(()=>CoresAparencia.Normalizar("＃123456")); }
    [Fact(DisplayName = "VH17 | Dígitos não ASCII | Utilitário interno recusa formato inválido")]
    public void Cenario17() { Assert.False(CoresAparencia.TentarNormalizar("#１２３４５６",out _));Assert.Throws<ArgumentException>(()=>CoresAparencia.Normalizar("#１２３４５６")); }
    [Fact(DisplayName = "VH18 | Nulo | Não é cor válida")]
    public void Cenario18() { Assert.False(CoresAparencia.TentarNormalizar(null,out _)); }
}
