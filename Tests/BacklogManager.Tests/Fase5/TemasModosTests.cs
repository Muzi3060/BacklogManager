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

public sealed class TemasModosTests
{
    [Fact(DisplayName = "VT01 | Aplicar Moderno Claro | Seleção recuperada com paleta completa e recursos válidos")]
    public void Cenario01() { var a = new AparenciaSimulada(); var s = a.Criar(); s.Selecionar(TemaVisual.Moderno,ModoVisual.Claro); Assert.Equal(TemaVisual.Moderno, a.Preferencias.Tema); Assert.Equal(ModoVisual.Claro, a.Preferencias.Modo); var r = ResolvedorAparencia.Resolver(s.Preferencias); Assert.Equal(13,r.Solicitadas.Count); Assert.All(r.Recursos.Values,c=>Assert.True(CoresAparencia.TentarNormalizar(c,out _))); }
    [Fact(DisplayName = "VT02 | Aplicar Moderno Escuro | Seleção recuperada com paleta completa e recursos válidos")]
    public void Cenario02() { var a = new AparenciaSimulada(); var s = a.Criar(); s.Selecionar(TemaVisual.Moderno,ModoVisual.Escuro); Assert.Equal(TemaVisual.Moderno, a.Preferencias.Tema); Assert.Equal(ModoVisual.Escuro, a.Preferencias.Modo); var r = ResolvedorAparencia.Resolver(s.Preferencias); Assert.Equal(13,r.Solicitadas.Count); Assert.All(r.Recursos.Values,c=>Assert.True(CoresAparencia.TentarNormalizar(c,out _))); }
    [Fact(DisplayName = "VT03 | Aplicar Tecnologico Claro | Seleção recuperada com paleta completa e recursos válidos")]
    public void Cenario03() { var a = new AparenciaSimulada(); var s = a.Criar(); s.Selecionar(TemaVisual.Tecnologico,ModoVisual.Claro); Assert.Equal(TemaVisual.Tecnologico, a.Preferencias.Tema); Assert.Equal(ModoVisual.Claro, a.Preferencias.Modo); var r = ResolvedorAparencia.Resolver(s.Preferencias); Assert.Equal(13,r.Solicitadas.Count); Assert.All(r.Recursos.Values,c=>Assert.True(CoresAparencia.TentarNormalizar(c,out _))); }
    [Fact(DisplayName = "VT04 | Aplicar Tecnologico Escuro | Seleção recuperada com paleta completa e recursos válidos")]
    public void Cenario04() { var a = new AparenciaSimulada(); var s = a.Criar(); s.Selecionar(TemaVisual.Tecnologico,ModoVisual.Escuro); Assert.Equal(TemaVisual.Tecnologico, a.Preferencias.Tema); Assert.Equal(ModoVisual.Escuro, a.Preferencias.Modo); var r = ResolvedorAparencia.Resolver(s.Preferencias); Assert.Equal(13,r.Solicitadas.Count); Assert.All(r.Recursos.Values,c=>Assert.True(CoresAparencia.TentarNormalizar(c,out _))); }
    [Fact(DisplayName = "VT05 | Aplicar Acolhedor Claro | Seleção recuperada com paleta completa e recursos válidos")]
    public void Cenario05() { var a = new AparenciaSimulada(); var s = a.Criar(); s.Selecionar(TemaVisual.Acolhedor,ModoVisual.Claro); Assert.Equal(TemaVisual.Acolhedor, a.Preferencias.Tema); Assert.Equal(ModoVisual.Claro, a.Preferencias.Modo); var r = ResolvedorAparencia.Resolver(s.Preferencias); Assert.Equal(13,r.Solicitadas.Count); Assert.All(r.Recursos.Values,c=>Assert.True(CoresAparencia.TentarNormalizar(c,out _))); }
    [Fact(DisplayName = "VT06 | Aplicar Acolhedor Escuro | Seleção recuperada com paleta completa e recursos válidos")]
    public void Cenario06() { var a = new AparenciaSimulada(); var s = a.Criar(); s.Selecionar(TemaVisual.Acolhedor,ModoVisual.Escuro); Assert.Equal(TemaVisual.Acolhedor, a.Preferencias.Tema); Assert.Equal(ModoVisual.Escuro, a.Preferencias.Modo); var r = ResolvedorAparencia.Resolver(s.Preferencias); Assert.Equal(13,r.Solicitadas.Count); Assert.All(r.Recursos.Values,c=>Assert.True(CoresAparencia.TentarNormalizar(c,out _))); }
    [Fact(DisplayName = "VT07 | Primeira instalação | Moderno claro sem gravação automática")]
    public void Cenario07() { var a = new AparenciaSimulada(); var s = a.Criar(); Assert.Equal(TemaVisual.Moderno,s.Preferencias.Tema); Assert.Equal(ModoVisual.Claro,s.Preferencias.Modo); Assert.Equal(0,a.Gravacoes); }
    [Fact(DisplayName = "VT08 | Preferência anterior válida | Tema e modo carregados sem reinicializar escolhas")]
    public void Cenario08() { var a=new AparenciaSimulada { Preferencias=new() { Tema=TemaVisual.Acolhedor,Modo=ModoVisual.Escuro } }; var s=a.Criar(); Assert.Equal(TemaVisual.Acolhedor,s.Preferencias.Tema); Assert.Equal(ModoVisual.Escuro,s.Preferencias.Modo); }
    [Fact(DisplayName = "VT09 | Alternância repetida | Cada seleção válida permanece coerente")]
    public void Cenario09() { var a=new AparenciaSimulada();var s=a.Criar();for(var i=0;i<12;i++){var t=(TemaVisual)(i%3);var m=(ModoVisual)(i%2);s.Selecionar(t,m);Assert.Equal(t,s.Preferencias.Tema);Assert.Equal(m,s.Preferencias.Modo);} }
    [Fact(DisplayName = "VT10 | Tema inválido | Não aplica nem grava enum desconhecido")]
    public void Cenario10() { var a=new AparenciaSimulada();var s=a.Criar();Assert.Throws<ArgumentOutOfRangeException>(()=>s.Selecionar((TemaVisual)99,ModoVisual.Claro));Assert.Equal(0,a.Gravacoes); }
    [Fact(DisplayName = "VT11 | Modo inválido | Não altera tema nem grava modo desconhecido")]
    public void Cenario11() { var a=new AparenciaSimulada();var s=a.Criar();Assert.Throws<ArgumentOutOfRangeException>(()=>s.Selecionar(TemaVisual.Acolhedor,(ModoVisual)99));Assert.Equal(TemaVisual.Moderno,s.Preferencias.Tema);Assert.Equal(0,a.Gravacoes); }
    [Fact(DisplayName = "VT12 | Falha de gravação durante seleção | Prévia mantém tema e informa persistência pendente")]
    public void Cenario12() { var a=new AparenciaSimulada{FalhaSalvar=new IOException()};var s=a.Criar();s.Selecionar(TemaVisual.Tecnologico,ModoVisual.Escuro);Assert.Equal(TemaVisual.Tecnologico,s.Preferencias.Tema);Assert.True(s.PendenteSalvar);Assert.NotNull(s.Aviso); }
}
