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

public sealed class ContinuarTests
{
    [Fact(DisplayName = "DC01 | Andamento de Jogo | Preserva referência status e progresso por mídia")]
    public void Caso01() { var i=ApoioDashboard.Item(status:"Jogando",tipo:TipoMidia.Jogo);var r=ApoioDashboard.Servico.Calcular(new[]{i});Assert.Same(i,Assert.Single(r.Continuar).Obra);Assert.Equal(i.StatusExibido,r.Continuar[0].Obra.StatusExibido);Assert.Equal(i.TemProgresso?i.ProgressoExibido:"Progresso não informado",r.Continuar[0].Progresso); }
    [Fact(DisplayName = "DC02 | Andamento de Anime | Preserva referência status e progresso por mídia")]
    public void Caso02() { var i=ApoioDashboard.Item(status:"Jogando",tipo:TipoMidia.Anime);var r=ApoioDashboard.Servico.Calcular(new[]{i});Assert.Same(i,Assert.Single(r.Continuar).Obra);Assert.Equal(i.StatusExibido,r.Continuar[0].Obra.StatusExibido);Assert.Equal(i.TemProgresso?i.ProgressoExibido:"Progresso não informado",r.Continuar[0].Progresso); }
    [Fact(DisplayName = "DC03 | Andamento de Serie | Preserva referência status e progresso por mídia")]
    public void Caso03() { var i=ApoioDashboard.Item(status:"Jogando",tipo:TipoMidia.Serie);var r=ApoioDashboard.Servico.Calcular(new[]{i});Assert.Same(i,Assert.Single(r.Continuar).Obra);Assert.Equal(i.StatusExibido,r.Continuar[0].Obra.StatusExibido);Assert.Equal(i.TemProgresso?i.ProgressoExibido:"Progresso não informado",r.Continuar[0].Progresso); }
    [Fact(DisplayName = "DC04 | Andamento de Filme | Preserva referência status e progresso por mídia")]
    public void Caso04() { var i=ApoioDashboard.Item(status:"Jogando",tipo:TipoMidia.Filme);var r=ApoioDashboard.Servico.Calcular(new[]{i});Assert.Same(i,Assert.Single(r.Continuar).Obra);Assert.Equal(i.StatusExibido,r.Continuar[0].Obra.StatusExibido);Assert.Equal(i.TemProgresso?i.ProgressoExibido:"Progresso não informado",r.Continuar[0].Progresso); }
    [Fact(DisplayName = "DC05 | Andamento de Manga | Preserva referência status e progresso por mídia")]
    public void Caso05() { var i=ApoioDashboard.Item(status:"Jogando",tipo:TipoMidia.Manga);var r=ApoioDashboard.Servico.Calcular(new[]{i});Assert.Same(i,Assert.Single(r.Continuar).Obra);Assert.Equal(i.StatusExibido,r.Continuar[0].Obra.StatusExibido);Assert.Equal(i.TemProgresso?i.ProgressoExibido:"Progresso não informado",r.Continuar[0].Progresso); }
    [Fact(DisplayName = "DC06 | Andamento de Livro | Preserva referência status e progresso por mídia")]
    public void Caso06() { var i=ApoioDashboard.Item(status:"Jogando",tipo:TipoMidia.Livro);var r=ApoioDashboard.Servico.Calcular(new[]{i});Assert.Same(i,Assert.Single(r.Continuar).Obra);Assert.Equal(i.StatusExibido,r.Continuar[0].Obra.StatusExibido);Assert.Equal(i.TemProgresso?i.ProgressoExibido:"Progresso não informado",r.Continuar[0].Progresso); }
    [Fact(DisplayName = "DC07 | Sem progresso | Exibe orientação sem percentual inventado")]
    public void Caso07() { var i=ApoioDashboard.Item(status:"Jogando");i=i with {Pessoal=i.Pessoal with {Progresso=null}};Assert.Equal("Progresso não informado",ApoioDashboard.Servico.Calcular(new[]{i}).Continuar[0].Progresso); }
    [Fact(DisplayName = "DC08 | Outros estados | Não aparecem como andamento")]
    public void Caso08() { var r=ApoioDashboard.Servico.Calcular(new[]{"Na Fila","Zerado","Pausado","Abandonado","Personalizado"}.Select(s=>ApoioDashboard.Item(status:s)).ToArray());Assert.Empty(r.Continuar); }
    [Fact(DisplayName = "DC09 | Legado sem data nem ID | Continua editável sem gerar identidade")]
    public void Caso09() { var i=ApoioDashboard.Item(status:"Jogando") with{DataAdicao=null,IdInterno=null};Assert.Same(i,ApoioDashboard.Servico.Calcular(new[]{i}).Continuar[0].Obra);Assert.Null(i.IdInterno); }
    [Fact(DisplayName = "DC10 | Ordem e limite de andamento | Conhecidos antes dos sem data e limite seis")]
    public void Caso10() { var a=ApoioDashboard.Item(status:"Jogando") with{DataAdicao=null};var itens=new[]{a}.Concat(Enumerable.Range(0,8).Select(n=>ApoioDashboard.Item(n,"Jogando"))).ToArray();var r=ApoioDashboard.Servico.Calcular(itens);Assert.Equal(9,r.EmAndamento);Assert.Equal(6,r.Continuar.Count);Assert.DoesNotContain(r.Continuar,e=>ReferenceEquals(e.Obra,a)); }
    [Fact(DisplayName = "DC11 | Duplicatas em andamento | Mantém as duas instâncias")]
    public void Caso11() { var a=ApoioDashboard.Item(status:"Jogando");Assert.Equal(2,ApoioDashboard.Servico.Calcular(new[]{a,a with{}}).Continuar.Count); }
    [Fact(DisplayName = "DC12 | Zero legado | Não altera progresso zero ao consultar")]
    public void Caso12() { var i=ApoioDashboard.Item(status:"Jogando",tipo:TipoMidia.Livro);i=i with{Pessoal=i.Pessoal with{Progresso=new(){Pagina=0}}};var r=ApoioDashboard.Servico.Calcular(new[]{i});Assert.Equal(0,i.Pessoal.Progresso!.Pagina);Assert.Contains("0",r.Continuar[0].Progresso); }
}
