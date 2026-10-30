using System.Text;
using BacklogManager.Backup;
using BacklogManager.Services;
using Xunit;
namespace BacklogManager.Tests.BetaDistribuicao;
public sealed class MigracaoTests
{
    private static void Biblioteca(AmbienteDistribuicao a,string conteudo="[]")=>File.WriteAllText(Path.Combine(a.Origem,"backlog.json"),conteudo);
    [Theory]
    [InlineData("M01","[]")]
    [InlineData("M07","[{\"Categoria\":\"Ação\",\"Status\":\"Na Fila\",\"Titulo\":\"Legado\",\"ExtensaoTeste\":{\"x\":1}}]")]
    [InlineData("M08","[{\"Categoria\":\"Ação\",\"Status\":\"Na Fila\",\"Titulo\":\"Batman\",\"SteamAppId\":361855,\"IgdbId\":123}]")]
    [InlineData("M09","[{\"Categoria\":\"Ação\",\"Status\":\"Na Fila\",\"Titulo\":\"A\"},{\"Categoria\":\"Ação\",\"Status\":\"Na Fila\",\"Titulo\":\"A\"}]")]
    [InlineData("M10","[{\"Categoria\":\"Ação\",\"Status\":\"Na Fila\",\"Titulo\":\"Legado\",\"PaginaAtual\":0}]")]
    public void PreservaBytes(string id,string json)
    {
        using var a=new AmbienteDistribuicao();Biblioteca(a,json);
        var s=new ServicoMigracaoLocal(new(Path.Combine(a.Destino,"backlog.json")),a.Seguranca);
        var antes=File.ReadAllBytes(Path.Combine(a.Origem,"backlog.json"));
        var p=s.Prever(a.Origem);var b=s.Migrar(p,true,true);
        Assert.True(Directory.Exists(b));Assert.Equal(antes,File.ReadAllBytes(Path.Combine(a.Destino,"backlog.json")));
        Assert.Equal(antes,File.ReadAllBytes(Path.Combine(a.Origem,"backlog.json")));Assert.NotEmpty(id);
    }
    [Theory]
    [InlineData("M03",false,false)][InlineData("M04",false,true)][InlineData("M05",true,false)][InlineData("M06",true,true)]
    public void PreferenciasOpcionais(string id,bool aparencia,bool visualizacao)
    {
        using var a=new AmbienteDistribuicao();Biblioteca(a);
        if(aparencia)File.WriteAllText(Path.Combine(a.Origem,"aparencia.preferencias.json"),id=="M06"?"{\"Versao\":2,\"Tema\":\"DarkMode\",\"Modo\":\"Escuro\",\"Personalizacoes\":{\"legado\":true}}":"{\"Versao\":1,\"Tema\":\"Moderno\",\"Modo\":\"Claro\"}");
        if(visualizacao)File.WriteAllText(Path.Combine(a.Origem,"biblioteca.preferencias.json"),"{\"Versao\":1,\"Visualizacao\":0}");
        var s=new ServicoMigracaoLocal(new(Path.Combine(a.Destino,"backlog.json")),a.Seguranca);s.Migrar(s.Prever(a.Origem),true,true);
        Assert.Equal(aparencia,File.Exists(Path.Combine(a.Destino,"aparencia.preferencias.json")));Assert.NotEmpty(id);
    }
    [Theory]
    [InlineData("M13","{")][InlineData("M14","{\"Versao\":999}")]
    public void OrigemInvalida(string id,string json)
    {using var a=new AmbienteDistribuicao();Biblioteca(a,json);var s=new ServicoMigracaoLocal(new(Path.Combine(a.Destino,"backlog.json")),a.Seguranca);Assert.ThrowsAny<Exception>(()=>s.Prever(a.Origem));Assert.Empty(Directory.GetFiles(a.Destino));Assert.NotEmpty(id);}
    [Theory]
    [InlineData("M02")][InlineData("M11")][InlineData("M12")][InlineData("M15")][InlineData("M23")][InlineData("M26")][InlineData("M28")][InlineData("M31")]
    public void ConjuntoEFalhas(string id)
    {
        using var a=new AmbienteDistribuicao();Biblioteca(a);
        var c=new CaminhosDadosLocais(Path.Combine(a.Destino,"backlog.json"));var s=new ServicoMigracaoLocal(c,a.Seguranca);
        if(id=="M23")
        { var outro=Path.Combine(a.Pasta,"outro.json");File.WriteAllText(outro,"[]");File.Delete(Path.Combine(a.Origem,"backlog.json"));File.CreateSymbolicLink(Path.Combine(a.Origem,"backlog.json"),outro);Assert.Throws<IOException>(()=>s.Prever(a.Origem));return; }
        if(id is "M02" or "M11" or "M12")
        {
            using var b=new BacklogManager.Tests.Fase7.AmbienteBackup();b.Preferencias();
            var jogo=Guid.NewGuid();
            if(id is "M11" or "M12")
            {b.Biblioteca(new BacklogManager.Models.BacklogItem("Jogo","RPG","Na Fila",0,"",361855){IdInterno=jogo});
             b.Colecoes(new BacklogManager.Models.ColecaoPersonalizada(Guid.NewGuid(),"Favoritos","",new[]{id=="M12"?Guid.NewGuid():jogo}));}
            foreach(var arquivo in b.Caminhos.Arquivos.Values)if(File.Exists(arquivo))File.Copy(arquivo,Path.Combine(a.Origem,Path.GetFileName(arquivo)),true);
        }
        if(id=="M15")File.WriteAllText(Path.Combine(a.Origem,"aparencia.preferencias.json"),"{\"Versao\":999}");
        var p=s.Prever(a.Origem);
        if(id=="M15"){Assert.False(p.PreferenciasValidas);Assert.Throws<IOException>(()=>s.Migrar(p,true,true));s.Migrar(p,false,true);Assert.False(File.Exists(c.Arquivos["Aparencia"]));return;}
        if(id=="M26")s.Etapa=e=>{if(e=="arquivos-preparados")throw new IOException("disco simulado");};
        if(id=="M28")s.Etapa=e=>{if(e is "depois-Biblioteca" or "rollback-Biblioteca")throw new IOException("falha de recuperação simulada");};
        if(id=="M31")s.Etapa=e=>{if(e=="antes-confirmacao")Biblioteca(a,"[ ]");};
        if(id is "M26" or "M28" or "M31")
        {Assert.Throws<IOException>(()=>s.Migrar(p,true,true));Assert.Equal(id=="M28",File.Exists(c.Diario));if(id=="M28")Assert.True(new ServicoBackupLocal(c,a.Seguranca).Recuperar());return;}
        s.Migrar(p,true,true);
        foreach(var arquivo in new CaminhosDadosLocais(Path.Combine(a.Origem,"backlog.json")).Arquivos.Values)
            if(File.Exists(arquivo))Assert.Equal(File.ReadAllBytes(arquivo),File.ReadAllBytes(Path.Combine(a.Destino,Path.GetFileName(arquivo))));
    }
    [Fact]public async Task M32MigracoesConcorrentesNaoSobrescrevem()
    {
        using var a=new AmbienteDistribuicao();Biblioteca(a);
        var c=new CaminhosDadosLocais(Path.Combine(a.Destino,"backlog.json"));
        var s1=new ServicoMigracaoLocal(c,a.Seguranca);var s2=new ServicoMigracaoLocal(c,a.Seguranca);
        var p1=s1.Prever(a.Origem);var p2=s2.Prever(a.Origem);
        async Task<bool> Tentar(ServicoMigracaoLocal s,PreviaMigracao p)=>await Task.Run(()=>{try{s.Migrar(p,true,true);return true;}catch(IOException){return false;}});
        var resultados=await Task.WhenAll(Tentar(s1,p1),Tentar(s2,p2));Assert.Single(resultados,x=>x);
        Assert.Equal("[]",File.ReadAllText(c.Arquivos["Biblioteca"]));Assert.False(File.Exists(c.Diario));
    }
    [Theory]
    [InlineData("M16")][InlineData("M17")][InlineData("M18")][InlineData("M19")][InlineData("M20")][InlineData("M21")][InlineData("M22")][InlineData("M24")][InlineData("M25")][InlineData("M27")][InlineData("M29")][InlineData("M30")]
    public void Protecoes(string id)
    {
        using var a=new AmbienteDistribuicao();Biblioteca(a);
        var c=new CaminhosDadosLocais(Path.Combine(a.Destino,"backlog.json"));
        var s=new ServicoMigracaoLocal(c,a.Seguranca);var p=s.Prever(a.Origem);
        switch(id)
        {
            case "M16":Assert.Throws<InvalidOperationException>(()=>s.Migrar(p,true,false));break;
            case "M17":case "M19":File.WriteAllText(c.Arquivos["Biblioteca"],"[]");Assert.Throws<IOException>(()=>s.Migrar(p,true,true));break;
            case "M18":Biblioteca(a,"[ ]");Assert.Throws<IOException>(()=>s.Migrar(p,true,true));break;
            case "M20":File.WriteAllText(Path.Combine(a.Origem,".mr-restauracao.json"),"{}");Assert.Throws<IOException>(()=>s.Migrar(p,true,true));break;
            case "M21":File.WriteAllText(c.Diario,"{}");Assert.Throws<IOException>(()=>s.Migrar(p,true,true));break;
            case "M22":Assert.Throws<IOException>(()=>s.Prever(a.Destino));break;
            case "M24":File.WriteAllText(a.Seguranca,"bloqueio");Assert.ThrowsAny<IOException>(()=>s.Migrar(p,true,true));break;
            case "M25":s.Etapa=e=>{if(e=="copia-gravada")File.WriteAllText(Directory.GetFiles(a.Seguranca,"backlog.json",SearchOption.AllDirectories).Single(),"alterado");};Assert.Throws<IOException>(()=>s.Migrar(p,true,true));break;
            case "M27":s.Etapa=e=>{if(e=="depois-Biblioteca")throw new IOException("falha simulada");};Assert.Throws<IOException>(()=>s.Migrar(p,true,true));Assert.False(File.Exists(c.Arquivos["Biblioteca"]));Assert.False(File.Exists(c.Diario));break;
            case "M29":s.Migrar(p,true,true);Assert.Throws<IOException>(()=>s.Migrar(p,true,true));break;
            case "M30":File.WriteAllText(Path.Combine(a.Origem,"backlog.json.bak"),"backup original");p=s.Prever(a.Origem);var pasta=s.Migrar(p,true,true);Assert.Equal("backup original",File.ReadAllText(Path.Combine(pasta,"backlog.json.bak")));break;
        }
    }
}
