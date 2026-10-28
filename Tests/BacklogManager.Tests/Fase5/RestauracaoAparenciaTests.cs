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

public sealed class RestauracaoAparenciaTests
{
    [Fact(DisplayName = "VR01 | Restaurar conjunto ativo | Cores originais reaparecem sem trocar tema nem modo")]
    public void Cenario01() { var s=new AparenciaSimulada().Criar();s.Selecionar(TemaVisual.Acolhedor,ModoVisual.Escuro);s.Personalizar(ElementoCor.FundoPrincipal,"#123456");s.RestaurarAtivo();Assert.Equal(TemaVisual.Acolhedor,s.Preferencias.Tema);Assert.Equal(ModoVisual.Escuro,s.Preferencias.Modo);Assert.Equal(TemasPredefinidos.Obter(TemaVisual.Acolhedor,ModoVisual.Escuro)[ElementoCor.FundoPrincipal],ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas[ElementoCor.FundoPrincipal]); }
    [Fact(DisplayName = "VR02 | Restaurar ativo com outro tema personalizado | Outro tema mantém sua cor")]
    public void Cenario02() { var s=new AparenciaSimulada().Criar();s.Personalizar(ElementoCor.FundoPrincipal,"#123456");s.Selecionar(TemaVisual.Tecnologico,ModoVisual.Claro);s.Personalizar(ElementoCor.FundoPrincipal,"#654321");s.RestaurarAtivo();s.Selecionar(TemaVisual.Moderno,ModoVisual.Claro);Assert.Equal("#123456",ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas[ElementoCor.FundoPrincipal]); }
    [Fact(DisplayName = "VR03 | Restaurar ativo com outro modo personalizado | Outro modo mantém sua cor")]
    public void Cenario03() { var s=new AparenciaSimulada().Criar();s.Personalizar(ElementoCor.FundoPrincipal,"#123456");s.Selecionar(TemaVisual.Moderno,ModoVisual.Escuro);s.Personalizar(ElementoCor.FundoPrincipal,"#654321");s.RestaurarAtivo();s.Selecionar(TemaVisual.Moderno,ModoVisual.Claro);Assert.Equal("#123456",ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas[ElementoCor.FundoPrincipal]); }
    [Fact(DisplayName = "VR04 | Restaurar todas as cores do ativo | Treze cores voltam à paleta original")]
    public void Cenario04() { var s=new AparenciaSimulada().Criar();foreach(var e in Enum.GetValues<ElementoCor>())s.Personalizar(e,"#123456");s.RestaurarAtivo();Assert.Equal(TemasPredefinidos.Obter(TemaVisual.Moderno,ModoVisual.Claro),ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas); }
    [Fact(DisplayName = "VR05 | Restaurar conjunto sem personalização | Ação idempotente preserva padrão")]
    public void Cenario05() { var s=new AparenciaSimulada().Criar();s.RestaurarAtivo();s.RestaurarAtivo();Assert.Empty(s.Preferencias.Personalizacoes); }
    [Fact(DisplayName = "VR06 | Restaurar toda a aparência | Moderno claro e nenhum conjunto personalizado")]
    public void Cenario06() { var s=new AparenciaSimulada().Criar();foreach(var t in Enum.GetValues<TemaVisual>())foreach(var m in Enum.GetValues<ModoVisual>()){s.Selecionar(t,m);s.Personalizar(ElementoCor.FundoPrincipal,"#123456");}s.RestaurarTudo();Assert.Empty(s.Preferencias.Personalizacoes);Assert.Equal(TemaVisual.Moderno,s.Preferencias.Tema);Assert.Equal(ModoVisual.Claro,s.Preferencias.Modo); }
    [Fact(DisplayName = "VR07 | Repetir restauração global | Não introduz personalização nem falha")]
    public void Cenario07() { var a=new AparenciaSimulada();var s=a.Criar();s.RestaurarTudo();s.RestaurarTudo();Assert.Empty(s.Preferencias.Personalizacoes);Assert.False(s.PendenteSalvar); }
    [Fact(DisplayName = "VR08 | Restauração ativa persiste | Reabrir recupera paleta original do conjunto")]
    public void Cenario08() { using var p=new PastaTeste();var s=new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo));s.Carregar();s.Personalizar(ElementoCor.FundoPrincipal,"#123456");s.RestaurarAtivo();Assert.Empty(new ArmazenamentoAparenciaJson(p.Arquivo).Carregar().Preferencias.Personalizacoes); }
    [Fact(DisplayName = "VR09 | Restauração global persiste | Reabrir recupera Moderno claro sem cores pessoais")]
    public void Cenario09() { using var p=new PastaTeste();var s=new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo));s.Carregar();s.Selecionar(TemaVisual.Acolhedor,ModoVisual.Escuro);s.Personalizar(ElementoCor.FundoPrincipal,"#123456");s.RestaurarTudo();var l=new ArmazenamentoAparenciaJson(p.Arquivo).Carregar();Assert.Equal(TemaVisual.Moderno,l.Preferencias.Tema);Assert.Equal(ModoVisual.Claro,l.Preferencias.Modo);Assert.Empty(l.Preferencias.Personalizacoes); }
    [Fact(DisplayName = "VR10 | Falha ao persistir restauração | Prévia usa padrão e avisa gravação pendente")]
    public void Cenario10() { var a=new AparenciaSimulada();var s=a.Criar();s.Personalizar(ElementoCor.FundoPrincipal,"#123456");a.FalhaSalvar=new IOException();s.RestaurarTudo();Assert.Empty(s.Preferencias.Personalizacoes);Assert.True(s.PendenteSalvar);Assert.NotNull(s.Aviso);Assert.NotEmpty(a.Preferencias.Personalizacoes); }
    [Fact(DisplayName = "VR11 | Restaurar conserva metadados desconhecidos | Arquivo não é apagado integralmente")]
    public void Cenario11() { using var p=new PastaTeste();File.WriteAllText(p.Arquivo,"{\"OutroRecurso\":42}");var s=new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo));s.Carregar();s.RestaurarTudo();Assert.Equal(42,(int)JsonNode.Parse(File.ReadAllText(p.Arquivo))!["OutroRecurso"]!); }
    [Fact(DisplayName = "VR12 | Restaurar remove somente ajustes de contraste do ativo | Aviso desaparece e cor original retorna")]
    public void Cenario12() { var s=new AparenciaSimulada().Criar();s.Personalizar(ElementoCor.TextoPrincipal,"#F3F4FA");Assert.NotEmpty(ResolvedorAparencia.Resolver(s.Preferencias).Avisos);s.RestaurarAtivo();Assert.Empty(ResolvedorAparencia.Resolver(s.Preferencias).Avisos); }
}
