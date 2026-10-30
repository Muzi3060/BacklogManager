using System.Net;
using BacklogManager.Configuracao;
using BacklogManager.Services;
using Xunit;
namespace BacklogManager.Tests.BetaDistribuicao;
public sealed class CredenciaisBetaTests
{
    [Theory]
    [InlineData("I05",null,null,null,true,"beta")]
    [InlineData("I06","ambiente","novo",null,false,"ambiente")]
    [InlineData("I07","ambiente",null,"legado",false,"ambiente")]
    [InlineData("I08","ambiente","novo",null,false,"ambiente")]
    [InlineData("I09","ambiente","novo","legado",false,"ambiente")]
    [InlineData("I10","ambiente"," ","legado",false,"ambiente")]
    [InlineData("I11","ambiente",null,null,false,null)]
    [InlineData("I12",null,"invalido\r\n",null,false,null)]
    [InlineData("I13",null,null,null,true,"beta")]
    [InlineData("I20",null,null,null,true,"beta")]
    [InlineData("I21",null,"novo",null,false,null)]
    [InlineData("I22","cliente com espaço","novo",null,false,null)]
    public void PrecedenciaSemMistura(string id,string? cliente,string? novo,string? legado,bool daBeta,string? esperado)
    {
        var ambiente=new Dictionary<string,string?>{{"IGDB_CLIENT_ID",cliente},{"IGDB_ACCESS_TOKEN",novo},{"IGDB_ACESS_TOKEN",legado}};
        var c=ProvedorCredenciaisIgdb.Obter(n=>ambiente.GetValueOrDefault(n),()=>new("beta",id=="I20"?"token-substituido":"token-beta",true));
        Assert.Equal(esperado,c?.ClientId);if(c!=null){Assert.Equal(daBeta,c.DaBeta);Assert.Equal(daBeta?(id=="I20"?"token-substituido":"token-beta"):string.IsNullOrWhiteSpace(novo)?legado:novo,c.AccessToken);}
    }
    [Theory]
    [InlineData("I14",401)][InlineData("I15",403)][InlineData("I16",429)][InlineData("I17",503)]
    public async Task ErrosSeguros(string id,int codigo)
    {
        var h=new HttpSimulado{Responder=(_,_)=>Task.FromResult(new HttpResponseMessage((HttpStatusCode)codigo))};using var http=new HttpClient(h);
        var s=new IgdbService(http,"cliente-simulado","token-simulado",true);
        var erro=await Assert.ThrowsAsync<PesquisaIgdbException>(()=>s.BuscarJogosAsync("Batman"));
        Assert.DoesNotContain("token-simulado",erro.Message);if(codigo is 401 or 403)Assert.Contains("pode ter expirado",erro.Message);Assert.Equal(1,h.Chamadas);Assert.NotEmpty(id);
    }
    [Fact]public void DesenvolvimentoSemRecursoNaoExigeSegredos()=>Assert.Null(ProvedorCredenciaisIgdb.Obter(_=>null,()=>null));
    [Fact]public void RecursoInvalidoNaoMistura()=>Assert.Null(ProvedorCredenciaisIgdb.Obter(_=>null,()=>new("beta","token invalido",true)));
}
