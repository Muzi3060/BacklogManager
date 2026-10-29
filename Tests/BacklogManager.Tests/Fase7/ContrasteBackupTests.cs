using Avalonia.Controls;
using Avalonia;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using BacklogManager.Aparencia;
using Xunit;
namespace BacklogManager.Tests.Fase7;
public sealed class ContrasteBackupTests
{
    [AvaloniaFact(DisplayName="QH01 | Checkbox Moderno Claro marcado false | Texto do consentimento e da opção atendem contraste 4.5")]
    public async Task Caso01()
    {
        using var a=new AmbienteBackup();using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);
        t.Clicar("NavConfiguracoes");t.Campo<ComboBox>("CmbTema").SelectedIndex=(int)TemaVisual.Moderno;t.Campo<ComboBox>("CmbModo").SelectedIndex=(int)ModoVisual.Claro;
        t.Clicar("NavConfiguracoes");
        foreach(var nome in new[]{"ChkConfirmarSubstituicao","ChkIncluirPreferencias"})
        {
            var c=t.Campo<CheckBox>(nome);c.IsChecked=false;t.Render();
            var texto=c.GetVisualDescendants().OfType<TextBlock>().First(x=>!string.IsNullOrWhiteSpace(x.Text));
            Fase5.ApoioAparencia.Par(Fase5.ApoioAparencia.Cor(texto.Foreground),Fase5.ApoioAparencia.Cor(c.Background));
        }
    }
    [AvaloniaFact(DisplayName="QH02 | Checkbox Moderno Claro marcado true | Texto do consentimento e da opção atendem contraste 4.5")]
    public async Task Caso02()
    {
        using var a=new AmbienteBackup();using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);
        t.Clicar("NavConfiguracoes");t.Campo<ComboBox>("CmbTema").SelectedIndex=(int)TemaVisual.Moderno;t.Campo<ComboBox>("CmbModo").SelectedIndex=(int)ModoVisual.Claro;
        t.Clicar("NavConfiguracoes");
        foreach(var nome in new[]{"ChkConfirmarSubstituicao","ChkIncluirPreferencias"})
        {
            var c=t.Campo<CheckBox>(nome);c.IsChecked=true;t.Render();
            var texto=c.GetVisualDescendants().OfType<TextBlock>().First(x=>!string.IsNullOrWhiteSpace(x.Text));
            Fase5.ApoioAparencia.Par(Fase5.ApoioAparencia.Cor(texto.Foreground),Fase5.ApoioAparencia.Cor(c.Background));
        }
    }
    [AvaloniaFact(DisplayName="QH03 | Checkbox Moderno Escuro marcado false | Texto do consentimento e da opção atendem contraste 4.5")]
    public async Task Caso03()
    {
        using var a=new AmbienteBackup();using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);
        t.Clicar("NavConfiguracoes");t.Campo<ComboBox>("CmbTema").SelectedIndex=(int)TemaVisual.Moderno;t.Campo<ComboBox>("CmbModo").SelectedIndex=(int)ModoVisual.Escuro;
        t.Clicar("NavConfiguracoes");
        foreach(var nome in new[]{"ChkConfirmarSubstituicao","ChkIncluirPreferencias"})
        {
            var c=t.Campo<CheckBox>(nome);c.IsChecked=false;t.Render();
            var texto=c.GetVisualDescendants().OfType<TextBlock>().First(x=>!string.IsNullOrWhiteSpace(x.Text));
            Fase5.ApoioAparencia.Par(Fase5.ApoioAparencia.Cor(texto.Foreground),Fase5.ApoioAparencia.Cor(c.Background));
        }
    }
    [AvaloniaFact(DisplayName="QH04 | Checkbox Moderno Escuro marcado true | Texto do consentimento e da opção atendem contraste 4.5")]
    public async Task Caso04()
    {
        using var a=new AmbienteBackup();using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);
        t.Clicar("NavConfiguracoes");t.Campo<ComboBox>("CmbTema").SelectedIndex=(int)TemaVisual.Moderno;t.Campo<ComboBox>("CmbModo").SelectedIndex=(int)ModoVisual.Escuro;
        t.Clicar("NavConfiguracoes");
        foreach(var nome in new[]{"ChkConfirmarSubstituicao","ChkIncluirPreferencias"})
        {
            var c=t.Campo<CheckBox>(nome);c.IsChecked=true;t.Render();
            var texto=c.GetVisualDescendants().OfType<TextBlock>().First(x=>!string.IsNullOrWhiteSpace(x.Text));
            Fase5.ApoioAparencia.Par(Fase5.ApoioAparencia.Cor(texto.Foreground),Fase5.ApoioAparencia.Cor(c.Background));
        }
    }
    [AvaloniaFact(DisplayName="QH05 | Checkbox Tecnologico Claro marcado false | Texto do consentimento e da opção atendem contraste 4.5")]
    public async Task Caso05()
    {
        using var a=new AmbienteBackup();using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);
        t.Clicar("NavConfiguracoes");t.Campo<ComboBox>("CmbTema").SelectedIndex=(int)TemaVisual.Tecnologico;t.Campo<ComboBox>("CmbModo").SelectedIndex=(int)ModoVisual.Claro;
        t.Clicar("NavConfiguracoes");
        foreach(var nome in new[]{"ChkConfirmarSubstituicao","ChkIncluirPreferencias"})
        {
            var c=t.Campo<CheckBox>(nome);c.IsChecked=false;t.Render();
            var texto=c.GetVisualDescendants().OfType<TextBlock>().First(x=>!string.IsNullOrWhiteSpace(x.Text));
            Fase5.ApoioAparencia.Par(Fase5.ApoioAparencia.Cor(texto.Foreground),Fase5.ApoioAparencia.Cor(c.Background));
        }
    }
    [AvaloniaFact(DisplayName="QH06 | Checkbox Tecnologico Claro marcado true | Texto do consentimento e da opção atendem contraste 4.5")]
    public async Task Caso06()
    {
        using var a=new AmbienteBackup();using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);
        t.Clicar("NavConfiguracoes");t.Campo<ComboBox>("CmbTema").SelectedIndex=(int)TemaVisual.Tecnologico;t.Campo<ComboBox>("CmbModo").SelectedIndex=(int)ModoVisual.Claro;
        t.Clicar("NavConfiguracoes");
        foreach(var nome in new[]{"ChkConfirmarSubstituicao","ChkIncluirPreferencias"})
        {
            var c=t.Campo<CheckBox>(nome);c.IsChecked=true;t.Render();
            var texto=c.GetVisualDescendants().OfType<TextBlock>().First(x=>!string.IsNullOrWhiteSpace(x.Text));
            Fase5.ApoioAparencia.Par(Fase5.ApoioAparencia.Cor(texto.Foreground),Fase5.ApoioAparencia.Cor(c.Background));
        }
    }
    [AvaloniaFact(DisplayName="QH07 | Checkbox Tecnologico Escuro marcado false | Texto do consentimento e da opção atendem contraste 4.5")]
    public async Task Caso07()
    {
        using var a=new AmbienteBackup();using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);
        t.Clicar("NavConfiguracoes");t.Campo<ComboBox>("CmbTema").SelectedIndex=(int)TemaVisual.Tecnologico;t.Campo<ComboBox>("CmbModo").SelectedIndex=(int)ModoVisual.Escuro;
        t.Clicar("NavConfiguracoes");
        foreach(var nome in new[]{"ChkConfirmarSubstituicao","ChkIncluirPreferencias"})
        {
            var c=t.Campo<CheckBox>(nome);c.IsChecked=false;t.Render();
            var texto=c.GetVisualDescendants().OfType<TextBlock>().First(x=>!string.IsNullOrWhiteSpace(x.Text));
            Fase5.ApoioAparencia.Par(Fase5.ApoioAparencia.Cor(texto.Foreground),Fase5.ApoioAparencia.Cor(c.Background));
        }
    }
    [AvaloniaFact(DisplayName="QH08 | Checkbox Tecnologico Escuro marcado true | Texto do consentimento e da opção atendem contraste 4.5")]
    public async Task Caso08()
    {
        using var a=new AmbienteBackup();using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);
        t.Clicar("NavConfiguracoes");t.Campo<ComboBox>("CmbTema").SelectedIndex=(int)TemaVisual.Tecnologico;t.Campo<ComboBox>("CmbModo").SelectedIndex=(int)ModoVisual.Escuro;
        t.Clicar("NavConfiguracoes");
        foreach(var nome in new[]{"ChkConfirmarSubstituicao","ChkIncluirPreferencias"})
        {
            var c=t.Campo<CheckBox>(nome);c.IsChecked=true;t.Render();
            var texto=c.GetVisualDescendants().OfType<TextBlock>().First(x=>!string.IsNullOrWhiteSpace(x.Text));
            Fase5.ApoioAparencia.Par(Fase5.ApoioAparencia.Cor(texto.Foreground),Fase5.ApoioAparencia.Cor(c.Background));
        }
    }
    [AvaloniaFact(DisplayName="QH09 | Checkbox Acolhedor Claro marcado false | Texto do consentimento e da opção atendem contraste 4.5")]
    public async Task Caso09()
    {
        using var a=new AmbienteBackup();using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);
        t.Clicar("NavConfiguracoes");t.Campo<ComboBox>("CmbTema").SelectedIndex=(int)TemaVisual.Acolhedor;t.Campo<ComboBox>("CmbModo").SelectedIndex=(int)ModoVisual.Claro;
        t.Clicar("NavConfiguracoes");
        foreach(var nome in new[]{"ChkConfirmarSubstituicao","ChkIncluirPreferencias"})
        {
            var c=t.Campo<CheckBox>(nome);c.IsChecked=false;t.Render();
            var texto=c.GetVisualDescendants().OfType<TextBlock>().First(x=>!string.IsNullOrWhiteSpace(x.Text));
            Fase5.ApoioAparencia.Par(Fase5.ApoioAparencia.Cor(texto.Foreground),Fase5.ApoioAparencia.Cor(c.Background));
        }
    }
    [AvaloniaFact(DisplayName="QH10 | Checkbox Acolhedor Claro marcado true | Texto do consentimento e da opção atendem contraste 4.5")]
    public async Task Caso10()
    {
        using var a=new AmbienteBackup();using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);
        t.Clicar("NavConfiguracoes");t.Campo<ComboBox>("CmbTema").SelectedIndex=(int)TemaVisual.Acolhedor;t.Campo<ComboBox>("CmbModo").SelectedIndex=(int)ModoVisual.Claro;
        t.Clicar("NavConfiguracoes");
        foreach(var nome in new[]{"ChkConfirmarSubstituicao","ChkIncluirPreferencias"})
        {
            var c=t.Campo<CheckBox>(nome);c.IsChecked=true;t.Render();
            var texto=c.GetVisualDescendants().OfType<TextBlock>().First(x=>!string.IsNullOrWhiteSpace(x.Text));
            Fase5.ApoioAparencia.Par(Fase5.ApoioAparencia.Cor(texto.Foreground),Fase5.ApoioAparencia.Cor(c.Background));
        }
    }
    [AvaloniaFact(DisplayName="QH11 | Checkbox Acolhedor Escuro marcado false | Texto do consentimento e da opção atendem contraste 4.5")]
    public async Task Caso11()
    {
        using var a=new AmbienteBackup();using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);
        t.Clicar("NavConfiguracoes");t.Campo<ComboBox>("CmbTema").SelectedIndex=(int)TemaVisual.Acolhedor;t.Campo<ComboBox>("CmbModo").SelectedIndex=(int)ModoVisual.Escuro;
        t.Clicar("NavConfiguracoes");
        foreach(var nome in new[]{"ChkConfirmarSubstituicao","ChkIncluirPreferencias"})
        {
            var c=t.Campo<CheckBox>(nome);c.IsChecked=false;t.Render();
            var texto=c.GetVisualDescendants().OfType<TextBlock>().First(x=>!string.IsNullOrWhiteSpace(x.Text));
            Fase5.ApoioAparencia.Par(Fase5.ApoioAparencia.Cor(texto.Foreground),Fase5.ApoioAparencia.Cor(c.Background));
        }
    }
    [AvaloniaFact(DisplayName="QH12 | Checkbox Acolhedor Escuro marcado true | Texto do consentimento e da opção atendem contraste 4.5")]
    public async Task Caso12()
    {
        using var a=new AmbienteBackup();using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);
        t.Clicar("NavConfiguracoes");t.Campo<ComboBox>("CmbTema").SelectedIndex=(int)TemaVisual.Acolhedor;t.Campo<ComboBox>("CmbModo").SelectedIndex=(int)ModoVisual.Escuro;
        t.Clicar("NavConfiguracoes");
        foreach(var nome in new[]{"ChkConfirmarSubstituicao","ChkIncluirPreferencias"})
        {
            var c=t.Campo<CheckBox>(nome);c.IsChecked=true;t.Render();
            var texto=c.GetVisualDescendants().OfType<TextBlock>().First(x=>!string.IsNullOrWhiteSpace(x.Text));
            Fase5.ApoioAparencia.Par(Fase5.ApoioAparencia.Cor(texto.Foreground),Fase5.ApoioAparencia.Cor(c.Background));
        }
    }
    [AvaloniaFact(DisplayName="QH13 | Foco e hover no consentimento | Texto mantém contraste e checkbox recebe foco por teclado")]
    public async Task FocoHover()
    {
        using var a=new AmbienteBackup();using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);
        var c=t.Campo<CheckBox>("ChkConfirmarSubstituicao");c.BringIntoView();t.Render();Assert.True(c.Focus(Avalonia.Input.NavigationMethod.Tab));
        Avalonia.Headless.HeadlessWindowExtensions.MouseMove(t.Janela,c.TranslatePoint(new Avalonia.Point(8,8),t.Janela)!.Value);c.IsChecked=true;t.Render();
        var texto=c.GetVisualDescendants().OfType<TextBlock>().First(x=>!string.IsNullOrWhiteSpace(x.Text));
        Fase5.ApoioAparencia.Par(Fase5.ApoioAparencia.Cor(texto.Foreground),Fase5.ApoioAparencia.Cor(c.Background));Assert.True(c.IsFocused);
    }
    [AvaloniaFact(DisplayName="QH14 | Checkbox Dark Mode | Superfície e texto seguem tokens seguros")]
    public void Personalizado()
    {
        using var a=new AmbienteBackup();using var t=new TelaBackup(a);
        t.Janela.AparenciaAtual.Selecionar(TemaVisual.DarkMode,ModoVisual.Escuro);
        AplicadorAparencia.Aplicar(t.Janela.AparenciaAtual.Preferencias);t.Render();var c=t.Campo<CheckBox>("ChkIncluirPreferencias");
        var texto=c.GetVisualDescendants().OfType<TextBlock>().First(x=>!string.IsNullOrWhiteSpace(x.Text));
        Assert.Equal("#202020",Fase5.ApoioAparencia.Cor(c.Background));Fase5.ApoioAparencia.Par(Fase5.ApoioAparencia.Cor(texto.Foreground),"#202020");
    }
}
