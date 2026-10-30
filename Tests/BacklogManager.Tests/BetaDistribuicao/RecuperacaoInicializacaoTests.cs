using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Interactivity;
using BacklogManager.Backup;
using BacklogManager.Configuracao;
using Xunit;
namespace BacklogManager.Tests.BetaDistribuicao;
public sealed class RecuperacaoInicializacaoTests
{
    [AvaloniaTheory]
    [InlineData("R01")][InlineData("R02")][InlineData("R03")][InlineData("R04")][InlineData("R05")][InlineData("R06")][InlineData("R07")][InlineData("R08")][InlineData("R09")][InlineData("R10")]
    public void InicializacaoNaoEliminaRecuperacao(string id)
    {
        using var a=new AmbienteDistribuicao();var c=new CaminhosDadosLocais(Path.Combine(a.Destino,"backlog.json"));
        if(id=="R02")File.WriteAllText(c.Arquivos["Biblioteca"],"[ ]");
        if(id=="R03")File.WriteAllText(c.Arquivos["Colecoes"],"{\"Versao\":1,\"Colecoes\":[]}");
        if(id=="R04")File.WriteAllText(c.Arquivos["Aparencia"],"{\"Versao\":2,\"Tema\":\"DarkMode\",\"Modo\":\"Escuro\"}");
        if(id=="R05")File.WriteAllText(c.Arquivos["Visualizacao"],"{\"Versao\":1,\"Visualizacao\":1}");
        var pendente=id is "R06" or "R07" or "R08" or "R09" or "R10";
        if(pendente)File.WriteAllText(c.Diario,id=="R07"?"{\"Versao\":999}":id=="R08"?"{":"{}");
        if(id=="R09")File.WriteAllText(c.Arquivos["Biblioteca"],"JSON danificado fictício");
        if(id=="R10")File.WriteAllText(c.Arquivos["Aparencia"],"JSON danificado fictício");
        var antes=c.Arquivos.Values.Append(c.Diario).Where(File.Exists).ToDictionary(p=>p,File.ReadAllBytes);
        var d=InicializacaoAplicativo.PrepararPrincipal(c,_=>null);
        Assert.Equal(pendente,d.Backup!.Pendencia!=null);
        foreach(var (p,b) in antes)Assert.Equal(b,File.ReadAllBytes(p));
        if(id=="R01")Assert.Equal("[]",File.ReadAllText(c.Arquivos["Biblioteca"]));
        if(id is "R03" or "R04" or "R05" or "R06" or "R07" or "R08" or "R10")Assert.False(File.Exists(c.Arquivos["Biblioteca"]));
        var w=new MainWindow(d);w.Show();
        try
        {
            w.FindControl<Button>("NavConfiguracoes")!.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Assert.Equal(pendente,w.FindControl<Button>("BtnRecuperarBackup")!.IsVisible);
            Assert.Equal(!pendente,w.FindControl<Button>("BtnExportarBackup")!.IsEnabled);
            if(pendente){w.FindControl<Button>("BtnRecuperarBackup")!.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));Assert.NotNull(d.Backup.Pendencia);}
            foreach(var (p,b) in antes)Assert.Equal(b,File.ReadAllBytes(p));
        }
        finally{w.Close();}
    }
}
