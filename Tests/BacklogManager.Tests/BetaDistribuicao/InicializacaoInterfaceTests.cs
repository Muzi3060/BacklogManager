using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Interactivity;
using BacklogManager.Backup;
using Xunit;
namespace BacklogManager.Tests.BetaDistribuicao;
public sealed class InicializacaoInterfaceTests
{
    [AvaloniaTheory]
    [InlineData("A02")][InlineData("A03")][InlineData("A04")][InlineData("A05")][InlineData("A06")][InlineData("A07")][InlineData("A08")][InlineData("A09")][InlineData("A10")][InlineData("A11")][InlineData("A12")][InlineData("A13")]
    public async Task FluxoConsentido(string id)
    {
        using var a=new AmbienteDistribuicao();File.WriteAllText(Path.Combine(a.Origem,"backlog.json"),"[]");
        var c=new CaminhosDadosLocais(Path.Combine(a.Destino,"backlog.json"));
        var w=new JanelaInicializacaoDados(c,a.Origem,a.Seguranca);var continuou=false;w.Continuar=()=>continuou=true;
        w.Show();
        T Campo<T>(string n)where T:Control=>w.FindControl<T>(n)!;
        void Click(string n)=>Campo<Button>(n).RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        try
        {
            Assert.False(File.Exists(c.Arquivos["Biblioteca"]));
            if(id=="A03"){w.SelecionarPasta=()=>Task.FromResult<string?>(a.Origem);Click("BtnEscolher");Assert.Equal(a.Origem,Campo<TextBox>("TxtOrigem").Text);return;}
            if(id=="A07")File.WriteAllText(c.Arquivos["Biblioteca"],"[]");
            if(id=="A09")File.WriteAllText(c.Diario,"{}");
            if(id=="A13")Campo<TextBox>("TxtOrigem").Text=a.Pasta;
            Click("BtnPrever");
            if(id is "A07" or "A09" or "A13"){Assert.False(Campo<Button>("BtnMigrar").IsEnabled);Assert.Contains("indisponível",Campo<TextBlock>("TxtEstado").Text);return;}
            Assert.True(Campo<Button>("BtnMigrar").IsEnabled);
            Assert.Contains("Obras: 0",Campo<TextBlock>("TxtEstado").Text);
            if(id=="A05"){Click("BtnVazia");Assert.True(continuou);Assert.False(File.Exists(c.Arquivos["Biblioteca"]));return;}
            if(id=="A02"){Click("BtnMigrar");await w.OperacaoPendente;Assert.False(continuou);Assert.False(File.Exists(c.Arquivos["Biblioteca"]));return;}
            if(id=="A11"){w.Width=900;w.Height=660;w.UpdateLayout();Assert.True(Campo<Button>("BtnMigrar").Focusable);return;}
            if(id=="A12"){Assert.NotNull(w.Background);Assert.Equal(900,w.MinWidth);return;}
            if(id=="A08")File.WriteAllText(a.Seguranca,"sem acesso");
            Campo<CheckBox>("ChkConfirmar").IsChecked=true;Click("BtnMigrar");await w.OperacaoPendente;
            Assert.Equal(id!="A08",continuou);
            if(id=="A08")Assert.Contains("não concluída",Campo<TextBlock>("TxtEstado").Text);
            else Assert.Equal("[]",File.ReadAllText(c.Arquivos["Biblioteca"]));
        }
        finally{w.Close();}
    }
}
