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

public sealed class PersonalizacaoTests
{
    [Fact(DisplayName = "VI01 | Personalizar FundoPrincipal | Somente a escolha solicitada muda e demais escolhas permanecem")]
    public void Cenario01() { var s=new AparenciaSimulada().Criar();var antes=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;s.Personalizar(ElementoCor.FundoPrincipal,"#3366CC");var depois=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;Assert.Equal("#3366CC",depois[ElementoCor.FundoPrincipal]);foreach(var p in antes)if(p.Key!=ElementoCor.FundoPrincipal)Assert.Equal(p.Value,depois[p.Key]); }
    [Fact(DisplayName = "VI02 | Personalizar FundoSidebar | Somente a escolha solicitada muda e demais escolhas permanecem")]
    public void Cenario02() { var s=new AparenciaSimulada().Criar();var antes=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;s.Personalizar(ElementoCor.FundoSidebar,"#3366CC");var depois=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;Assert.Equal("#3366CC",depois[ElementoCor.FundoSidebar]);foreach(var p in antes)if(p.Key!=ElementoCor.FundoSidebar)Assert.Equal(p.Value,depois[p.Key]); }
    [Fact(DisplayName = "VI03 | Personalizar TextoSidebar | Somente a escolha solicitada muda e demais escolhas permanecem")]
    public void Cenario03() { var s=new AparenciaSimulada().Criar();var antes=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;s.Personalizar(ElementoCor.TextoSidebar,"#3366CC");var depois=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;Assert.Equal("#3366CC",depois[ElementoCor.TextoSidebar]);foreach(var p in antes)if(p.Key!=ElementoCor.TextoSidebar)Assert.Equal(p.Value,depois[p.Key]); }
    [Fact(DisplayName = "VI04 | Personalizar FundoPainel | Somente a escolha solicitada muda e demais escolhas permanecem")]
    public void Cenario04() { var s=new AparenciaSimulada().Criar();var antes=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;s.Personalizar(ElementoCor.FundoPainel,"#3366CC");var depois=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;Assert.Equal("#3366CC",depois[ElementoCor.FundoPainel]);foreach(var p in antes)if(p.Key!=ElementoCor.FundoPainel)Assert.Equal(p.Value,depois[p.Key]); }
    [Fact(DisplayName = "VI05 | Personalizar TextoPainel | Somente a escolha solicitada muda e demais escolhas permanecem")]
    public void Cenario05() { var s=new AparenciaSimulada().Criar();var antes=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;s.Personalizar(ElementoCor.TextoPainel,"#3366CC");var depois=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;Assert.Equal("#3366CC",depois[ElementoCor.TextoPainel]);foreach(var p in antes)if(p.Key!=ElementoCor.TextoPainel)Assert.Equal(p.Value,depois[p.Key]); }
    [Fact(DisplayName = "VI06 | Personalizar FundoCartao | Somente a escolha solicitada muda e demais escolhas permanecem")]
    public void Cenario06() { var s=new AparenciaSimulada().Criar();var antes=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;s.Personalizar(ElementoCor.FundoCartao,"#3366CC");var depois=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;Assert.Equal("#3366CC",depois[ElementoCor.FundoCartao]);foreach(var p in antes)if(p.Key!=ElementoCor.FundoCartao)Assert.Equal(p.Value,depois[p.Key]); }
    [Fact(DisplayName = "VI07 | Personalizar TextoCartao | Somente a escolha solicitada muda e demais escolhas permanecem")]
    public void Cenario07() { var s=new AparenciaSimulada().Criar();var antes=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;s.Personalizar(ElementoCor.TextoCartao,"#3366CC");var depois=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;Assert.Equal("#3366CC",depois[ElementoCor.TextoCartao]);foreach(var p in antes)if(p.Key!=ElementoCor.TextoCartao)Assert.Equal(p.Value,depois[p.Key]); }
    [Fact(DisplayName = "VI08 | Personalizar FundoBotao | Somente a escolha solicitada muda e demais escolhas permanecem")]
    public void Cenario08() { var s=new AparenciaSimulada().Criar();var antes=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;s.Personalizar(ElementoCor.FundoBotao,"#3366CC");var depois=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;Assert.Equal("#3366CC",depois[ElementoCor.FundoBotao]);foreach(var p in antes)if(p.Key!=ElementoCor.FundoBotao)Assert.Equal(p.Value,depois[p.Key]); }
    [Fact(DisplayName = "VI09 | Personalizar TextoBotao | Somente a escolha solicitada muda e demais escolhas permanecem")]
    public void Cenario09() { var s=new AparenciaSimulada().Criar();var antes=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;s.Personalizar(ElementoCor.TextoBotao,"#3366CC");var depois=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;Assert.Equal("#3366CC",depois[ElementoCor.TextoBotao]);foreach(var p in antes)if(p.Key!=ElementoCor.TextoBotao)Assert.Equal(p.Value,depois[p.Key]); }
    [Fact(DisplayName = "VI10 | Personalizar Destaque | Somente a escolha solicitada muda e demais escolhas permanecem")]
    public void Cenario10() { var s=new AparenciaSimulada().Criar();var antes=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;s.Personalizar(ElementoCor.Destaque,"#3366CC");var depois=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;Assert.Equal("#3366CC",depois[ElementoCor.Destaque]);foreach(var p in antes)if(p.Key!=ElementoCor.Destaque)Assert.Equal(p.Value,depois[p.Key]); }
    [Fact(DisplayName = "VI11 | Personalizar TextoDestaque | Somente a escolha solicitada muda e demais escolhas permanecem")]
    public void Cenario11() { var s=new AparenciaSimulada().Criar();var antes=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;s.Personalizar(ElementoCor.TextoDestaque,"#3366CC");var depois=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;Assert.Equal("#3366CC",depois[ElementoCor.TextoDestaque]);foreach(var p in antes)if(p.Key!=ElementoCor.TextoDestaque)Assert.Equal(p.Value,depois[p.Key]); }
    [Fact(DisplayName = "VI12 | Personalizar TextoPrincipal | Somente a escolha solicitada muda e demais escolhas permanecem")]
    public void Cenario12() { var s=new AparenciaSimulada().Criar();var antes=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;s.Personalizar(ElementoCor.TextoPrincipal,"#3366CC");var depois=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;Assert.Equal("#3366CC",depois[ElementoCor.TextoPrincipal]);foreach(var p in antes)if(p.Key!=ElementoCor.TextoPrincipal)Assert.Equal(p.Value,depois[p.Key]); }
    [Fact(DisplayName = "VI13 | Personalizar TextoSecundario | Somente a escolha solicitada muda e demais escolhas permanecem")]
    public void Cenario13() { var s=new AparenciaSimulada().Criar();var antes=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;s.Personalizar(ElementoCor.TextoSecundario,"#3366CC");var depois=ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas;Assert.Equal("#3366CC",depois[ElementoCor.TextoSecundario]);foreach(var p in antes)if(p.Key!=ElementoCor.TextoSecundario)Assert.Equal(p.Value,depois[p.Key]); }
    [Fact(DisplayName = "VI14 | Isolar modo claro do escuro | Personalização clara reaparece ao voltar")]
    public void Cenario14() { var s=new AparenciaSimulada().Criar();s.Personalizar(ElementoCor.FundoPrincipal,"#FFAA22");s.Selecionar(TemaVisual.Moderno,ModoVisual.Escuro);Assert.NotEqual("#FFAA22",ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas[ElementoCor.FundoPrincipal]);s.Selecionar(TemaVisual.Moderno,ModoVisual.Claro);Assert.Equal("#FFAA22",ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas[ElementoCor.FundoPrincipal]); }
    [Fact(DisplayName = "VI15 | Isolar os seis conjuntos | Cada conjunto recupera sua própria escolha")]
    public void Cenario15() { var s=new AparenciaSimulada().Criar();var n=0;foreach(var t in Enum.GetValues<TemaVisual>())foreach(var m in Enum.GetValues<ModoVisual>()){s.Selecionar(t,m);s.Personalizar(ElementoCor.FundoPrincipal,$"#12{++n:X2}34");}n=0;foreach(var t in Enum.GetValues<TemaVisual>())foreach(var m in Enum.GetValues<ModoVisual>()){s.Selecionar(t,m);Assert.Equal($"#12{++n:X2}34",ResolvedorAparencia.Resolver(s.Preferencias).Solicitadas[ElementoCor.FundoPrincipal]);} }
    [Fact(DisplayName = "VI16 | Reeditar um grupo após outro | Última escolha substitui apenas o próprio grupo")]
    public void Cenario16() { var s=new AparenciaSimulada().Criar();s.Personalizar(ElementoCor.FundoPrincipal,"#123456");s.Personalizar(ElementoCor.FundoBotao,"#ABCDEF");s.Personalizar(ElementoCor.FundoPrincipal,"#654321");var r=ResolvedorAparencia.Resolver(s.Preferencias);Assert.Equal("#654321",r.Solicitadas[ElementoCor.FundoPrincipal]);Assert.Equal("#ABCDEF",r.Solicitadas[ElementoCor.FundoBotao]); }
    [Fact(DisplayName = "VI17 | Cor arbitrária fora da paleta | Hexadecimal válido é aceito sem limitação temática")]
    public void Cenario17() { var s=new AparenciaSimulada().Criar();s.Personalizar(ElementoCor.FundoPrincipal,"#F7A1B3");Assert.Equal("#F7A1B3",ResolvedorAparencia.Resolver(s.Preferencias).Recursos["PageBackground"]); }
    [Fact(DisplayName = "VI18 | Grupo inexistente | Entrada inválida não grava")]
    public void Cenario18() { var a=new AparenciaSimulada();var s=a.Criar();Assert.Throws<ArgumentOutOfRangeException>(()=>s.Personalizar((ElementoCor)99,"#123456"));Assert.Equal(0,a.Gravacoes); }
    [Fact(DisplayName = "VI19 | Snapshot anterior de personalizações | Alteração posterior não modifica snapshot anterior")]
    public void Cenario19() { var s=new AparenciaSimulada().Criar();s.Personalizar(ElementoCor.FundoPrincipal,"#123456");var anterior=s.Preferencias;s.Personalizar(ElementoCor.FundoPrincipal,"#654321");Assert.Equal("#123456",anterior.Personalizacoes["Moderno/Claro"][ElementoCor.FundoPrincipal]); }
}
