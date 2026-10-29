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

public sealed class AtividadeTests
{
    [Fact(DisplayName = "DA01 | Hoje | Intervalo civil conta corretamente")]
    public void Caso01() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(0)},7);Assert.Equal(1,r.Adicionadas); }
    [Fact(DisplayName = "DA02 | Sétimo dia | Intervalo civil conta corretamente")]
    public void Caso02() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(6)},7);Assert.Equal(1,r.Adicionadas); }
    [Fact(DisplayName = "DA03 | Fora de sete dias | Intervalo civil conta corretamente")]
    public void Caso03() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(7)},7);Assert.Equal(0,r.Adicionadas); }
    [Fact(DisplayName = "DA04 | Trigésimo dia | Intervalo civil conta corretamente")]
    public void Caso04() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(29)},30);Assert.Equal(1,r.Adicionadas); }
    [Fact(DisplayName = "DA05 | Fora de trinta dias | Intervalo civil conta corretamente")]
    public void Caso05() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(30)},30);Assert.Equal(0,r.Adicionadas); }
    [Fact(DisplayName = "DA06 | Futura | Intervalo civil conta corretamente")]
    public void Caso06() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(-1)},30);Assert.Equal(0,r.Adicionadas); }
    [Fact(DisplayName = "DA07 | Antiga para sete dias | Intervalo civil conta corretamente")]
    public void Caso07() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(10)},7);Assert.Equal(0,r.Adicionadas); }
    [Fact(DisplayName = "DA08 | Mesma data em trinta dias | Intervalo civil conta corretamente")]
    public void Caso08() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(10)},30);Assert.Equal(1,r.Adicionadas); }
    [Fact(DisplayName = "DA09 | Data ausente | Preserva ausência e separa do período")]
    public void Caso09() { var i=ApoioDashboard.Item() with {DataAdicao=null};var r=ApoioDashboard.Servico.Calcular(new[]{i});Assert.Equal(0,r.Adicionadas);Assert.Equal(1,r.SemData);Assert.Same(i,Assert.Single(r.Legados));Assert.Null(i.DataAdicao); }
    [Fact(DisplayName = "DA10 | Início exato do período | Meia noite do sétimo dia é incluída")]
    public void Caso10() { var i=ApoioDashboard.Item() with {DataAdicao=new DateTimeOffset(2026,10,23,0,0,0,TimeSpan.Zero)};Assert.Single(ApoioDashboard.Servico.Calcular(new[]{i},7).Atividade); }
    [Fact(DisplayName = "DA11 | Antes da fronteira | Instante anterior não entra no período")]
    public void Caso11() { var i=ApoioDashboard.Item() with {DataAdicao=new DateTimeOffset(2026,10,22,23,59,59,TimeSpan.Zero)};Assert.Empty(ApoioDashboard.Servico.Calcular(new[]{i},7).Atividade); }
    [Fact(DisplayName = "DA12 | Mais tarde hoje | Exclui horário futuro mesmo no dia atual")]
    public void Caso12() { var i=ApoioDashboard.Item() with {DataAdicao=ApoioDashboard.Agora.AddMinutes(1)};var r=ApoioDashboard.Servico.Calcular(new[]{i});Assert.Equal(0,r.Adicionadas);Assert.Equal(1,r.DatasFuturas); }
    [Fact(DisplayName = "DA13 | Offsets diferentes mesmo instante | Contagem considera o instante real")]
    public void Caso13() { var i=ApoioDashboard.Item() with {DataAdicao=ApoioDashboard.Agora.ToOffset(TimeSpan.FromHours(-3))};Assert.Equal(1,ApoioDashboard.Servico.Calcular(new[]{i}).Adicionadas); }
    [Fact(DisplayName = "DA14 | Fuso com virada civil | Fronteira usa o fuso informado")]
    public void Caso14() { var f=TimeZoneInfo.CreateCustomTimeZone("Teste",TimeSpan.FromHours(-3),"Teste","Teste");var agora=new DateTimeOffset(2026,10,29,1,0,0,TimeSpan.Zero);var s=new ServicoDashboard(()=>agora,f);Assert.Equal(new DateTime(2026,10,22),s.Calcular(Array.Empty<ItemBiblioteca>(),7).InicioLocal); }
    [Fact(DisplayName = "DA15 | Ordem por data | Mais recente precede antigo")]
    public void Caso15() { var a=ApoioDashboard.Item(2);var b=ApoioDashboard.Item();Assert.Same(b,ApoioDashboard.Servico.Calcular(new[]{a,b}).Atividade[0].Obra); }
    [Fact(DisplayName = "DA16 | Empate estável | Instâncias iguais não são deduplicadas")]
    public void Caso16() { var a=ApoioDashboard.Item();var b=a with{};var r=ApoioDashboard.Servico.Calcular(new[]{a,b});Assert.Equal(2,r.Adicionadas);Assert.Same(a,r.Atividade[0].Obra);Assert.Same(b,r.Atividade[1].Obra); }
    [Fact(DisplayName = "DA17 | Limite visual | Conta todas mas mostra seis")]
    public void Caso17() { var r=ApoioDashboard.Servico.Calcular(Enumerable.Range(0,12).Select(n=>ApoioDashboard.Item(n)).ToArray());Assert.Equal(12,r.Adicionadas);Assert.Equal(6,r.Atividade.Count); }
    [Fact(DisplayName = "DA18 | Período inválido | Recusa opção não suportada")]
    public void Caso18() { Assert.Throws<ArgumentOutOfRangeException>(()=>ApoioDashboard.Servico.Calcular(Array.Empty<ItemBiblioteca>(),14)); }
    [Fact(DisplayName = "DA19 | Entrada nula | Recusa coleção inexistente")]
    public void Caso19() { Assert.Throws<ArgumentNullException>(()=>ApoioDashboard.Servico.Calcular(null!)); }
    [Fact(DisplayName = "DA20 | Relógio avança | Resultado muda sem modificar registros")]
    public void Caso20() { var agora=ApoioDashboard.Agora;var s=new ServicoDashboard(()=>agora,TimeZoneInfo.Utc);var i=ApoioDashboard.Item(6);Assert.Equal(1,s.Calcular(new[]{i},7).Adicionadas);agora=agora.AddDays(1);Assert.Equal(0,s.Calcular(new[]{i},7).Adicionadas); }

    [Fact(DisplayName = "DA21 | Fuso com meia noite inexistente | Fronteira civil funciona durante avanço do horário")]
    public void HorarioVerao()
    {
        var inicio=TimeZoneInfo.TransitionTime.CreateFixedDateRule(new DateTime(1,1,1,0,0,0),3,1);
        var fim=TimeZoneInfo.TransitionTime.CreateFixedDateRule(new DateTime(1,1,1,0,0,0),10,1);
        var regra=TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(new DateTime(2026,1,1),new DateTime(2026,12,31),TimeSpan.FromHours(1),inicio,fim);
        var fuso=TimeZoneInfo.CreateCustomTimeZone("Fuso de teste",TimeSpan.FromHours(-3),"Teste","Normal","Verão",new[]{regra});
        var s=new ServicoDashboard(()=>new DateTimeOffset(2026,3,7,12,0,0,TimeSpan.Zero),fuso);
        var antes=ApoioDashboard.Item() with{DataAdicao=new DateTimeOffset(2026,3,1,2,59,59,TimeSpan.Zero)};
        var depois=antes with{DataAdicao=new DateTimeOffset(2026,3,1,3,0,0,TimeSpan.Zero)};
        var r=s.Calcular(new[]{antes,depois},7);Assert.Equal(1,r.Adicionadas);Assert.Same(depois,Assert.Single(r.Atividade).Obra);
    }
}
