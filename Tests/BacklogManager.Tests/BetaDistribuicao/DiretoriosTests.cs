using BacklogManager.Configuracao;
using Xunit;

namespace BacklogManager.Tests.BetaDistribuicao;

public sealed class DiretoriosTests
{
    [Theory]
    [InlineData("D01",true,null,"/local/MinhasRealizacoes/Dados")]
    [InlineData("D02",false,null,"/usuario/.local/share/minhas-realizacoes/dados")]
    [InlineData("D03",false,"/xdg","/xdg/minhas-realizacoes/dados")]
    [InlineData("D04",false,"","/usuario/.local/share/minhas-realizacoes/dados")]
    [InlineData("D05",false,"relativo","/usuario/.local/share/minhas-realizacoes/dados")]
    public void Padrao(string id,bool windows,string? xdg,string esperado)
    { Assert.NotEmpty(id); Assert.Equal(esperado,ResolvedorDiretorios.Resolver([],windows,"/local","/usuario",xdg)); }

    [Theory]
    [InlineData("D06","/perfil")][InlineData("D08","/perfil com espaço")][InlineData("D09","/ação 日本")]
    [InlineData("D12","/outra-pasta")][InlineData("D13","/binarios-movidos")]
    public void PerfilExplicito(string id,string pasta)
    { Assert.NotEmpty(id);Assert.Equal(pasta,ResolvedorDiretorios.Resolver(["--data-dir",pasta],false,"/local","/usuario")); }

    [Theory]
    [InlineData("D07",new[]{"--data-dir","relativo"})]
    [InlineData("D21",new[]{"--data-dir"})]
    [InlineData("D22",new[]{"--data-dir","/a","--data-dir","/b"})]
    public void ArgumentoInvalido(string id,string[] args)
    { Assert.NotEmpty(id); Assert.Throws<ArgumentException>(()=>ResolvedorDiretorios.Resolver(args,false,"/local","/usuario")); }

    [Theory]
    [InlineData("D10")][InlineData("D15")][InlineData("D18")]
    public void PastaNovaIsolada(string id)
    {
        using var a=new AmbienteDistribuicao();
        var c=ResolvedorDiretorios.Preparar(a.Destino);
        Assert.True(Directory.Exists(a.Destino));Assert.False(ResolvedorDiretorios.TemDados(c));
        Assert.Empty(Directory.GetFiles(a.Destino));Assert.NotEqual(c.Pasta,a.Origem);Assert.NotEmpty(id);
    }
}
internal sealed class AmbienteDistribuicao:IDisposable
{
    internal string Pasta {get;}=Path.Combine(Path.GetTempPath(),"MR.Beta.Tests",Guid.NewGuid().ToString("N"));
    internal string Origem=>Path.Combine(Pasta,"origem");
    internal string Destino=>Path.Combine(Pasta,"destino");
    internal string Seguranca=>Path.Combine(Pasta,"seguranca");
    internal AmbienteDistribuicao(){Directory.CreateDirectory(Origem);Directory.CreateDirectory(Destino);}
    public void Dispose(){if(Directory.Exists(Pasta)) Directory.Delete(Pasta,true);}
}
