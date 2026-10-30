using System.Net;
using System.Text.Json;
using BacklogManager.Services;
using Xunit;
namespace BacklogManager.Tests.BetaDistribuicao;
public sealed class AtualizacoesTests
{
    internal const string Repo="exemplo/projeto";
    internal static object Release(string versao="0.1.0-beta.2",string rid="linux-x64",bool draft=false,string? url=null,bool pacote=true)=>new
    {draft,tag_name="v"+versao,html_url=url??$"https://github.com/{Repo}/releases/tag/v{versao}",assets=pacote?new[]{new{name=$"MinhasRealizacoes-{versao}-{rid}"+(rid=="win-x64"?".zip":".tar.gz"),state="uploaded"}}:[]};
    internal static HttpResponseMessage Lista(params object[] releases)=>HttpSimulado.Json(JsonSerializer.Serialize(releases));
    [Theory]
    [InlineData("U01")][InlineData("U02")][InlineData("U03")][InlineData("U04")][InlineData("U05")][InlineData("U06")][InlineData("U07")][InlineData("U08")][InlineData("U09")][InlineData("U10")][InlineData("U11")][InlineData("U12")][InlineData("U13")][InlineData("U14")][InlineData("U15")][InlineData("U16")][InlineData("U17")][InlineData("U18")][InlineData("U19")][InlineData("U20")][InlineData("U21")][InlineData("U22")][InlineData("U23")][InlineData("U25")]
    public async Task ConsultaControlada(string id)
    {
        var h=new HttpSimulado();h.Responder=(r,ct)=>
        {
            Assert.Equal("api.github.com",r.RequestUri!.Host);Assert.Null(r.Headers.Authorization);Assert.False(r.Headers.Contains("Client-ID"));Assert.DoesNotContain("latest",r.RequestUri.AbsolutePath);
            Assert.NotEmpty(r.Headers.UserAgent);Assert.True(r.Headers.Contains("X-GitHub-Api-Version"));
            var resposta=id switch
            {
                "U02"=>Lista(Release("0.1.0")),"U03"=>Lista(Release(draft:true)),"U04" or "U05"=>Lista(),
                "U06"=>Lista(Release("0.1.0-beta.10"),Release("0.1.0-beta.2")),
                "U07"=>h.Chamadas==1?Lista(Release("0.1.0-beta.1")):Lista(Release()),
                "U08"=>Lista(Release(rid:"win-x64")),"U09"=>Lista(Release(pacote:false)),
                "U10"=>HttpSimulado.Json("{"),"U11"=>HttpSimulado.Json(""),"U12"=>HttpSimulado.Json("[{}]"),
                "U13"=>new(HttpStatusCode.NotFound),"U14"=>new(HttpStatusCode.Forbidden),"U15"=>new(HttpStatusCode.TooManyRequests),
                "U16"=>throw new TaskCanceledException(),"U18"=>throw new HttpRequestException(),
                "U19"=>HttpSimulado.Json(new string('x',1024*1024+1)),"U20"=>Lista(Release()),"U21"=>new(HttpStatusCode.NotModified),
                "U23"=>Lista(Release(url:"https://malicioso.invalid/exe")),"U25"=>Lista(Release()),_=>Lista(Release())
            };
            return Task.FromResult(resposta);
        };
        using var http=new HttpClient(h);var s=new ServicoAtualizacoesGitHub(http,Repo,paginas:id=="U20"?1:10,tamanhoPagina:id is "U07" or "U20"?1:30);
        using var cts=CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);if(id=="U17")cts.Cancel();
        if(id=="U17"){await Assert.ThrowsAnyAsync<OperationCanceledException>(()=>s.Consultar("0.1.0-beta.1","linux-x64",cts.Token));return;}
        if(id=="U07")h.Responder=(r,_)=>Task.FromResult(h.Chamadas==1?Lista(Release("0.1.0-beta.1")):h.Chamadas==2?Lista(Release()):Lista());
        var resultado=await s.Consultar(id=="U25"?"0.1.0":"0.1.0-beta.1","linux-x64",TestContext.Current.CancellationToken);
        var esperado=id switch
        {"U03" or "U04" or "U05" or "U25"=>EstadoAtualizacao.Atualizado,"U08" or "U09"=>EstadoAtualizacao.SemPacote,
         "U10" or "U11" or "U12" or "U18" or "U19" or "U20" or "U23"=>EstadoAtualizacao.Inconclusivo,
         "U13" or "U14" or "U15" or "U16" or "U21"=>EstadoAtualizacao.Indisponivel,_=>EstadoAtualizacao.Disponivel};
        Assert.Equal(esperado,resultado.Estado);if(id=="U06")Assert.Equal("0.1.0-beta.10",resultado.Versao);
        if(id is "U14" or "U15"){await s.Consultar("0.1.0-beta.1","linux-x64",TestContext.Current.CancellationToken);Assert.Equal(1,h.Chamadas);}
        if(resultado.Pagina!=null)Assert.True(s.PaginaOficial(resultado.Pagina));
    }
    [Theory]
    [InlineData("http://github.com/exemplo/projeto/releases/tag/v1")]
    [InlineData("https://github.com/exemplo/outro/releases/tag/v1")]
    [InlineData("https://github.com/exemplo/projeto/releases/tag/v1?url=malicioso")]
    [InlineData("https://github.com/exemplo/projeto/releases/tag/v1#x")]
    [InlineData("https://github.com:8443/exemplo/projeto/releases/tag/v1")]
    [InlineData("https://github.com/exemplo/projeto/releases/tag/v1/extra")]
    public void BloqueiaLinksEstranhos(string url)
    {using var http=new HttpClient(new HttpSimulado());Assert.False(new ServicoAtualizacoesGitHub(http,Repo).PaginaOficial(new Uri(url)));}
}
