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







    [Fact(DisplayName = "VC14 | Contraste matemático conhecido | Preto e branco têm razão 21 e mesma cor tem razão 1")]
    public void Cenario14() { Assert.Equal(21,CoresAparencia.Contraste("#000000","#FFFFFF"),6);Assert.Equal(1,CoresAparencia.Contraste("#123456","#123456"),6); }


    [Fact(DisplayName = "VC17 | Sucesso aviso e erro nos dois modos | Mensagens semânticas têm texto legível")]
    public void Cenario17() { foreach(var m in Enum.GetValues<ModoVisual>()){var r=ResolvedorAparencia.Resolver(new(){Modo=m}).Recursos;foreach(var tipo in new[]{"Success","Warning","Error"})ApoioAparencia.Par(r[tipo+"Brush"],r[tipo+"Background"]);} }




}
