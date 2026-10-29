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

public sealed class EstatisticasTests
{
    [Fact(DisplayName = "DE01 | Contar Na Fila | Estado atual independente do período")]
    public void Caso01() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(200,"Na Fila")},7);Assert.Equal(1,r.NaFila);Assert.Equal(0,r.Adicionadas); }
    [Fact(DisplayName = "DE02 | Contar Jogando | Estado atual independente do período")]
    public void Caso02() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(200,"Jogando")},7);Assert.Equal(1,r.EmAndamento);Assert.Equal(0,r.Adicionadas); }
    [Fact(DisplayName = "DE03 | Contar Zerado | Estado atual independente do período")]
    public void Caso03() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(200,"Zerado")},7);Assert.Equal(1,r.Concluidas);Assert.Equal(0,r.Adicionadas); }
    [Fact(DisplayName = "DE04 | Distribuir Jogo | Contagem por mídia correta")]
    public void Caso04() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(tipo:TipoMidia.Jogo)});Assert.Equal(1,r.Total);Assert.Equal(1,r.PorMidia.Single(e=>e.Nome==TextosMidia.Plural(TipoMidia.Jogo)).Quantidade);Assert.Equal(1,r.PorMidia.Sum(e=>e.Quantidade)); }
    [Fact(DisplayName = "DE05 | Distribuir Anime | Contagem por mídia correta")]
    public void Caso05() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(tipo:TipoMidia.Anime)});Assert.Equal(1,r.Total);Assert.Equal(1,r.PorMidia.Single(e=>e.Nome==TextosMidia.Plural(TipoMidia.Anime)).Quantidade);Assert.Equal(1,r.PorMidia.Sum(e=>e.Quantidade)); }
    [Fact(DisplayName = "DE06 | Distribuir Serie | Contagem por mídia correta")]
    public void Caso06() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(tipo:TipoMidia.Serie)});Assert.Equal(1,r.Total);Assert.Equal(1,r.PorMidia.Single(e=>e.Nome==TextosMidia.Plural(TipoMidia.Serie)).Quantidade);Assert.Equal(1,r.PorMidia.Sum(e=>e.Quantidade)); }
    [Fact(DisplayName = "DE07 | Distribuir Filme | Contagem por mídia correta")]
    public void Caso07() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(tipo:TipoMidia.Filme)});Assert.Equal(1,r.Total);Assert.Equal(1,r.PorMidia.Single(e=>e.Nome==TextosMidia.Plural(TipoMidia.Filme)).Quantidade);Assert.Equal(1,r.PorMidia.Sum(e=>e.Quantidade)); }
    [Fact(DisplayName = "DE08 | Distribuir Manga | Contagem por mídia correta")]
    public void Caso08() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(tipo:TipoMidia.Manga)});Assert.Equal(1,r.Total);Assert.Equal(1,r.PorMidia.Single(e=>e.Nome==TextosMidia.Plural(TipoMidia.Manga)).Quantidade);Assert.Equal(1,r.PorMidia.Sum(e=>e.Quantidade)); }
    [Fact(DisplayName = "DE09 | Distribuir Livro | Contagem por mídia correta")]
    public void Caso09() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(tipo:TipoMidia.Livro)});Assert.Equal(1,r.Total);Assert.Equal(1,r.PorMidia.Single(e=>e.Nome==TextosMidia.Plural(TipoMidia.Livro)).Quantidade);Assert.Equal(1,r.PorMidia.Sum(e=>e.Quantidade)); }
    [Fact(DisplayName = "DE10 | Vazia | Zeros finitos sem divisão inválida")]
    public void Caso10() { var r=ApoioDashboard.Servico.Calcular(Array.Empty<ItemBiblioteca>());Assert.Equal(0,r.Total);Assert.Equal(0,r.PercentualConcluidas);Assert.All(r.PorMidia,d=>Assert.Equal(0,d.Percentual)); }
    [Fact(DisplayName = "DE11 | Status mistos | Todas as distribuições fecham o total")]
    public void Caso11() { var r=ApoioDashboard.Servico.Calcular(new[]{"Na Fila","Jogando","Zerado","Pausado","Abandonado","Outro"}.Select(s=>ApoioDashboard.Item(status:s)).ToArray());Assert.Equal(6,r.Total);Assert.Equal(6,r.PorStatus.Sum(d=>d.Quantidade));Assert.Equal(1,r.PorStatus.Last().Quantidade); }
    [Fact(DisplayName = "DE12 | Status desconhecido | Não normaliza o dado pessoal")]
    public void Caso12() { var i=ApoioDashboard.Item(status:"jogando");var r=ApoioDashboard.Servico.Calcular(new[]{i});Assert.Equal(0,r.EmAndamento);Assert.Equal(1,r.PorStatus.Last().Quantidade);Assert.Equal("jogando",i.Pessoal.Status); }
    [Fact(DisplayName = "DE13 | Percentual concluído | Proporção por registros não soma unidades")]
    public void Caso13() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(status:"Zerado"),ApoioDashboard.Item(status:"Jogando")});Assert.Equal(50,r.PercentualConcluidas); }
    [Fact(DisplayName = "DE14 | Sem data | Ainda conta no resumo atual")]
    public void Caso14() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(status:"Zerado") with {DataAdicao=null}});Assert.Equal(1,r.Concluidas);Assert.Equal(1,r.Total); }
    [Fact(DisplayName = "DE15 | Futura | Conta no estado atual e não na atividade")]
    public void Caso15() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item(-2,"Zerado")});Assert.Equal(1,r.Concluidas);Assert.Equal(0,r.Adicionadas); }
    [Fact(DisplayName = "DE16 | Snapshots | Resultado anterior não é alterado por nova consulta")]
    public void Caso16() { var i=ApoioDashboard.Item();var a=ApoioDashboard.Servico.Calcular(new[]{i});ApoioDashboard.Servico.Calcular(Array.Empty<ItemBiblioteca>());Assert.Equal(1,a.Total); }
    [Fact(DisplayName = "DE17 | Não grava IDs | Leitura não modifica registros fornecidos")]
    public void Caso17() { var i=ApoioDashboard.Item() with{IdInterno=null};ApoioDashboard.Servico.Calcular(new[]{i});Assert.Null(i.IdInterno); }
    [Fact(DisplayName = "DE18 | Na fila não é desejo | Resumo usa estado real sem propriedade de desejos")]
    public void Caso18() { var r=ApoioDashboard.Servico.Calcular(new[]{ApoioDashboard.Item()});Assert.Equal(1,r.NaFila);Assert.Equal(0,r.Concluidas); }
}
