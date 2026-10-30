using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Input;
using Avalonia.Input.Raw;
using Avalonia.Headless.XUnit;
using BacklogManager.Services;
using Xunit;
namespace BacklogManager.Tests.BetaDistribuicao;
public sealed class AtualizacoesInterfaceTests
{
    [AvaloniaTheory]
    [InlineData("T01")][InlineData("T02")][InlineData("T03")][InlineData("T04")][InlineData("T05")][InlineData("T06")][InlineData("T07")][InlineData("T08")][InlineData("T09")][InlineData("T10")][InlineData("T11")][InlineData("T12")][InlineData("U24")]
    public async Task ConsultaNaoInstala(string id)
    {
        using var a=new BacklogManager.Tests.Fase7.AmbienteBackup();using var t=new BacklogManager.Tests.Fase7.TelaBackup(a);
        var h=new HttpSimulado();using var http=new HttpClient(h);
        var gatilho=new TaskCompletionSource<HttpResponseMessage>();
        h.Responder=(_,ct)=>id is "T03" or "T04"?gatilho.Task.WaitAsync(ct):Task.FromResult(id switch
        {"T05"=>AtualizacoesTests.Lista(),"T07"=>new(System.Net.HttpStatusCode.NotFound),"T08"=>AtualizacoesTests.Lista(AtualizacoesTests.Release(pacote:false)),_=>AtualizacoesTests.Lista(AtualizacoesTests.Release())});
        t.Janela.ConfigurarAtualizacoes(new(http,AtualizacoesTests.Repo));var abriu=false;
        t.Janela.AbrirPaginaAtualizacao=_=>{abriu=true;if(id=="U24")throw new InvalidOperationException();return Task.CompletedTask;};
        if(id=="T01"){Assert.Contains("0.1.0-beta.1",t.Campo<TextBlock>("TxtVersaoAplicativo").Text);return;}
        if(id=="T10"){t.Render();Assert.True(t.Campo<Button>("BtnVerificarAtualizacoes").Focus());t.Janela.KeyPress(Key.Space,RawInputModifiers.None,default,null);t.Janela.KeyRelease(Key.Space,RawInputModifiers.None,default,null);await t.Janela.ConsultaAtualizacaoPendente;Assert.Equal(1,h.Chamadas);return;}
        if(id=="T11"){t.Janela.Width=900;t.Janela.Height=660;t.Render();Assert.True(t.Campo<Button>("BtnVerificarAtualizacoes").IsVisible);return;}
        if(id=="T12")
        {foreach(var tema in Enum.GetValues<BacklogManager.Aparencia.TemaVisual>())foreach(var modo in Enum.GetValues<BacklogManager.Aparencia.ModoVisual>())
          {BacklogManager.Aparencia.AplicadorAparencia.Aplicar(new(){Tema=tema,Modo=modo});t.Render();Assert.NotNull(t.Campo<TextBlock>("TxtEstadoAtualizacao").Foreground);Assert.True(t.Campo<Button>("BtnVerificarAtualizacoes").Focusable);}
          BacklogManager.Aparencia.AplicadorAparencia.Aplicar(new());return;}
        t.Clicar("BtnVerificarAtualizacoes");
        if(id is "T03" or "T04")
        {Assert.False(t.Campo<Button>("BtnVerificarAtualizacoes").IsEnabled);Assert.Contains("Consultando",t.Campo<TextBlock>("TxtEstadoAtualizacao").Text);t.Clicar("BtnVerificarAtualizacoes");Assert.Equal(1,h.Chamadas);gatilho.SetResult(AtualizacoesTests.Lista());}
        await t.Janela.ConsultaAtualizacaoPendente;
        Assert.False(abriu);Assert.True(t.Campo<Button>("BtnVerificarAtualizacoes").IsEnabled);
        if(id=="T05")Assert.Contains("Nenhuma versão",t.Campo<TextBlock>("TxtEstadoAtualizacao").Text);
        if(id=="T07")Assert.Contains("públicas",t.Campo<TextBlock>("TxtEstadoAtualizacao").Text);
        if(id=="T08")Assert.Contains("sem pacote",t.Campo<TextBlock>("TxtEstadoAtualizacao").Text);
        if(id is "T06" or "T09" or "U24")
        {Assert.True(t.Campo<Button>("BtnAbrirAtualizacao").IsVisible);t.Clicar("BtnAbrirAtualizacao");await t.Janela.AberturaAtualizacaoPendente;Assert.True(abriu);if(id=="U24")Assert.Contains("Nenhuma atualização",t.Campo<TextBlock>("TxtEstadoAtualizacao").Text);}
    }
}
