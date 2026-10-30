using System.Diagnostics;
using BacklogManager.Backup;
using Xunit;
namespace BacklogManager.Tests.BetaDistribuicao;
public sealed class InterrupcaoMigracaoTests
{
    [Theory]
    [InlineData("antes-backup")][InlineData("backup-verificado")][InlineData("arquivos-preparados")]
    [InlineData("diario-gravado")][InlineData("depois-Biblioteca")][InlineData("depois-Colecoes")]
    [InlineData("depois-Visualizacao")][InlineData("depois-Aparencia")][InlineData("antes-confirmacao")][InlineData("confirmada")]
    public async Task M27ProcessoInterrompidoPreservaOrigemERecuperaDestino(string etapa)
    {
        using var a=new AmbienteDistribuicao();var origem=new CaminhosDadosLocais(Path.Combine(a.Origem,"backlog.json"));
        var conteudos=new Dictionary<string,string>{{"Biblioteca","[]"},{"Colecoes","{\"Versao\":1,\"Colecoes\":[]}"},
          {"Visualizacao","{\"Versao\":1,\"Visualizacao\":1}"},{"Aparencia","{\"Versao\":2,\"Tema\":\"DarkMode\",\"Modo\":\"Escuro\"}"}};
        foreach(var (nome,conteudo) in conteudos)File.WriteAllText(origem.Arquivos[nome],conteudo);
        var inicio=new ProcessStartInfo("dotnet"){UseShellExecute=false,RedirectStandardOutput=true,RedirectStandardError=true};
        inicio.ArgumentList.Add(Path.Combine(AppContext.BaseDirectory,"BackupWorker","BacklogManager.BackupWorker.dll"));
        foreach(var arg in new[]{"--migracao",a.Origem,a.Destino,a.Seguranca,etapa})inicio.ArgumentList.Add(arg);
        using var processo=Process.Start(inicio)!;
        try{await processo.WaitForExitAsync(TestContext.Current.CancellationToken).WaitAsync(TimeSpan.FromSeconds(30),TestContext.Current.CancellationToken);}
        finally{if(!processo.HasExited){processo.Kill(true);processo.WaitForExit();}}
        Assert.Equal(73,processo.ExitCode);
        var destino=new CaminhosDadosLocais(Path.Combine(a.Destino,"backlog.json"));var recuperacao=new ServicoBackupLocal(destino,a.Seguranca);
        Assert.True(recuperacao.Recuperar(),recuperacao.Pendencia);Assert.False(File.Exists(destino.Diario));
        foreach(var (nome,conteudo) in conteudos)
        {
            Assert.Equal(conteudo,File.ReadAllText(origem.Arquivos[nome]));
            if(etapa=="confirmada")Assert.Equal(conteudo,File.ReadAllText(destino.Arquivos[nome]));
            else Assert.False(File.Exists(destino.Arquivos[nome]));
        }
    }
}
