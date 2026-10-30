using BacklogManager.Configuracao;
using NuGet.Versioning;
using Xunit;
namespace BacklogManager.Tests.BetaDistribuicao;
public sealed class VersaoTests
{
    [Theory]
    [InlineData("V01","0.1.0-beta.1",true)][InlineData("V02","0.1.0-beta.2",true)]
    [InlineData("V10","v0.1.0-beta.1",true)][InlineData("V11","0.1.0+build.2",true)]
    [InlineData("V12","versao estranha",false)][InlineData("V13","1.0",false)]
    [InlineData("V14","1.0.0.1",false)][InlineData("V15","1.0.0-beta.01",false)]
    [InlineData("V17","01.0.0",false)][InlineData("V18","1.0.0-",false)]
    [InlineData("V19","1.0.0+",false)][InlineData("V20",null,false)]
    public void ValidacaoEstrita(string id,string? texto,bool valido){Assert.Equal(valido,VersaoAplicativo.TentarLer(texto,out _));Assert.NotEmpty(id);}
    [Theory]
    [InlineData("V03","0.1.0-beta.10","0.1.0-beta.2",1)]
    [InlineData("V04","0.1.0","0.1.0-beta.1",1)]
    [InlineData("V05","0.0.9","0.1.0-beta.1",-1)]
    [InlineData("V06","0.1.0-beta.1","0.1.0-beta.1",0)]
    [InlineData("V07","1.0.0","0.1.0",1)][InlineData("V08","0.2.0","0.1.0",1)]
    [InlineData("V09","0.1.1","0.1.0",1)][InlineData("V21","0.1.0+x","0.1.0+y",0)]
    public void Ordem(string id,string a,string b,int ordem)
    {Assert.True(VersaoAplicativo.TentarLer(a,out var va));Assert.True(VersaoAplicativo.TentarLer(b,out var vb));Assert.Equal(ordem,Math.Sign(VersionComparer.VersionRelease.Compare(va,vb)));Assert.NotEmpty(id);}
    [Fact]public void V16VersaoDoProduto()=>Assert.Equal("0.1.0-beta.1",VersaoAplicativo.Atual);
}
