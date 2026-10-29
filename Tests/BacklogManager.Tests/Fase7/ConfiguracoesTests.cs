using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using BacklogManager.Backup;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Services;
using BacklogManager.Aparencia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Xunit;
namespace BacklogManager.Tests.Fase7;
public sealed class ConfiguracoesTests
{
    [AvaloniaFact(DisplayName="QN01 | Configurações Moderno Claro | Conteúdo renderizado com contraste e rolagem")]
    public void Caso01()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); t.Clicar("NavAparencia"); t.Campo<ComboBox>("CmbTema").SelectedIndex=(int)TemaVisual.Moderno; t.Campo<ComboBox>("CmbModo").SelectedIndex=(int)ModoVisual.Claro; t.Clicar("NavConfiguracoes"); t.Janela.Width=900;t.Janela.Height=660;t.Render(); Assert.True(t.Campo<ScrollViewer>("PaginaConfiguracoes").IsVisible); Assert.True(t.Campo<Button>("NavConfiguracoes").Bounds.Height>0); Capturar(t,"Moderno-Claro"); var r=ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias); Fase5.ApoioAparencia.Par(r.Recursos["PageForeground"],r.Recursos["PageBackground"]);
    }
    [AvaloniaFact(DisplayName="QN02 | Configurações Moderno Escuro | Conteúdo renderizado com contraste e rolagem")]
    public void Caso02()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); t.Clicar("NavAparencia"); t.Campo<ComboBox>("CmbTema").SelectedIndex=(int)TemaVisual.Moderno; t.Campo<ComboBox>("CmbModo").SelectedIndex=(int)ModoVisual.Escuro; t.Clicar("NavConfiguracoes"); t.Janela.Width=900;t.Janela.Height=660;t.Render(); Assert.True(t.Campo<ScrollViewer>("PaginaConfiguracoes").IsVisible); Assert.True(t.Campo<Button>("NavConfiguracoes").Bounds.Height>0); Capturar(t,"Moderno-Escuro"); var r=ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias); Fase5.ApoioAparencia.Par(r.Recursos["PageForeground"],r.Recursos["PageBackground"]);
    }
    [AvaloniaFact(DisplayName="QN03 | Configurações Tecnologico Claro | Conteúdo renderizado com contraste e rolagem")]
    public void Caso03()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); t.Clicar("NavAparencia"); t.Campo<ComboBox>("CmbTema").SelectedIndex=(int)TemaVisual.Tecnologico; t.Campo<ComboBox>("CmbModo").SelectedIndex=(int)ModoVisual.Claro; t.Clicar("NavConfiguracoes"); t.Janela.Width=900;t.Janela.Height=660;t.Render(); Assert.True(t.Campo<ScrollViewer>("PaginaConfiguracoes").IsVisible); Assert.True(t.Campo<Button>("NavConfiguracoes").Bounds.Height>0); Capturar(t,"Tecnologico-Claro"); var r=ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias); Fase5.ApoioAparencia.Par(r.Recursos["PageForeground"],r.Recursos["PageBackground"]);
    }
    [AvaloniaFact(DisplayName="QN04 | Configurações Tecnologico Escuro | Conteúdo renderizado com contraste e rolagem")]
    public void Caso04()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); t.Clicar("NavAparencia"); t.Campo<ComboBox>("CmbTema").SelectedIndex=(int)TemaVisual.Tecnologico; t.Campo<ComboBox>("CmbModo").SelectedIndex=(int)ModoVisual.Escuro; t.Clicar("NavConfiguracoes"); t.Janela.Width=900;t.Janela.Height=660;t.Render(); Assert.True(t.Campo<ScrollViewer>("PaginaConfiguracoes").IsVisible); Assert.True(t.Campo<Button>("NavConfiguracoes").Bounds.Height>0); Capturar(t,"Tecnologico-Escuro"); var r=ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias); Fase5.ApoioAparencia.Par(r.Recursos["PageForeground"],r.Recursos["PageBackground"]);
    }
    [AvaloniaFact(DisplayName="QN05 | Configurações Acolhedor Claro | Conteúdo renderizado com contraste e rolagem")]
    public void Caso05()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); t.Clicar("NavAparencia"); t.Campo<ComboBox>("CmbTema").SelectedIndex=(int)TemaVisual.Acolhedor; t.Campo<ComboBox>("CmbModo").SelectedIndex=(int)ModoVisual.Claro; t.Clicar("NavConfiguracoes"); t.Janela.Width=900;t.Janela.Height=660;t.Render(); Assert.True(t.Campo<ScrollViewer>("PaginaConfiguracoes").IsVisible); Assert.True(t.Campo<Button>("NavConfiguracoes").Bounds.Height>0); Capturar(t,"Acolhedor-Claro"); var r=ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias); Fase5.ApoioAparencia.Par(r.Recursos["PageForeground"],r.Recursos["PageBackground"]);
    }
    [AvaloniaFact(DisplayName="QN06 | Configurações Acolhedor Escuro | Conteúdo renderizado com contraste e rolagem")]
    public void Caso06()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); t.Clicar("NavAparencia"); t.Campo<ComboBox>("CmbTema").SelectedIndex=(int)TemaVisual.Acolhedor; t.Campo<ComboBox>("CmbModo").SelectedIndex=(int)ModoVisual.Escuro; t.Clicar("NavConfiguracoes"); t.Janela.Width=900;t.Janela.Height=660;t.Render(); Assert.True(t.Campo<ScrollViewer>("PaginaConfiguracoes").IsVisible); Assert.True(t.Campo<Button>("NavConfiguracoes").Bounds.Height>0); Capturar(t,"Acolhedor-Escuro"); var r=ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias); Fase5.ApoioAparencia.Par(r.Recursos["PageForeground"],r.Recursos["PageBackground"]);
    }
    [AvaloniaFact(DisplayName="QN07 | Atalho de aparência | Página original abre e configurações fecha")]
    public void Caso07()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); t.Clicar("BtnAparenciaConfiguracoes"); Assert.True(t.Campo<ScrollViewer>("PaginaAparencia").IsVisible); Assert.False(t.Campo<ScrollViewer>("PaginaConfiguracoes").IsVisible);
    }
    [AvaloniaFact(DisplayName="QN08 | Identificação local | Caminhos reais e modo local apresentados")]
    public void Caso08()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); Assert.Contains(a.Caminhos.Arquivos["Biblioteca"],t.Campo<TextBlock>("TxtCaminhosDados").Text); Assert.Contains(a.Servico.PastaSeguranca,t.Campo<TextBlock>("TxtCaminhosDados").Text);
    }
    [AvaloniaFact(DisplayName="QN09 | Nuvem e Drive futuros | Mensagem sem requisições remotas")]
    public void Caso09()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); Assert.Contains("ainda não estão disponíveis",t.Campo<TextBlock>("TxtRecursosFuturos").Text); Assert.Equal(0,t.Http.Chamadas);
    }
    [AvaloniaFact(DisplayName="QN10 | Teclado na navegação | Foco acessível sem mouse")]
    public void Caso10()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); t.Render(); Assert.True(t.Campo<Button>("NavConfiguracoes").Focus(Avalonia.Input.NavigationMethod.Tab)); Assert.True(t.Campo<Button>("NavConfiguracoes").IsFocused);
    }
    [AvaloniaFact(DisplayName="QN11 | Teclado no backup | Ação exportar recebe foco")]
    public void Caso11()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); t.Render(); Assert.True(t.Campo<Button>("BtnExportarBackup").Focus(Avalonia.Input.NavigationMethod.Tab)); Assert.True(t.Campo<Button>("BtnExportarBackup").IsFocused);
    }
    [AvaloniaFact(DisplayName="QN12 | Rolagem mínima | Última seção pode ser alcançada")]
    public void Caso12()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); t.Janela.Width=900;t.Janela.Height=660;t.Render(); var p=t.Campo<ScrollViewer>("PaginaConfiguracoes"); Assert.True(p.Extent.Height>p.Viewport.Height); p.Offset=new Avalonia.Vector(0,10000); t.Render(); Assert.True(p.Offset.Y>0); Capturar(t,"minimo-fim");
    }
    [AvaloniaFact(DisplayName="QN13 | Cores próprias | Painel aplica personalização existente")]
    public void Caso13()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); t.Clicar("NavAparencia"); var e=t.Campo<WrapPanel>("EditoresCores").Children.OfType<BacklogManager.Controls.EditorCor>().Single(c=>c.Elemento==ElementoCor.FundoPrincipal); e.Campo.Text="#163A63"; t.Clicar("NavConfiguracoes"); t.Render(); Assert.Equal("#163A63",ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias).Solicitadas[ElementoCor.FundoPrincipal]); Capturar(t,"personalizado");
    }
    [AvaloniaFact(DisplayName="QN14 | Preferência sincronizada | Seletores da biblioteca e configurações acompanham a mesma escolha")]
    public void Caso14()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); t.Campo<ComboBox>("CmbVisualizacaoConfiguracoes").SelectedIndex=1; Assert.Equal(1,t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex); t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex=0; Assert.Equal(0,t.Campo<ComboBox>("CmbVisualizacaoConfiguracoes").SelectedIndex);
    }
    [AvaloniaFact(DisplayName="QN15 | Cancelamento de exportação | Nenhum backup ou dado criado")]
    public async Task Caso15()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); var antes=a.Snapshot(); await t.Exportar(null); a.Igual(antes); Assert.False(File.Exists(a.Saida)); Assert.Contains("cancelada",t.Estado);
    }
    [AvaloniaFact(DisplayName="QN16 | Erro no seletor | Mensagem clara e ações reabilitadas")]
    public async Task Caso16()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); t.Janela.SelecionarArquivoBackup=_=>throw new IOException("seletor indisponível");t.Clicar("BtnSelecionarBackup");await t.Janela.OperacaoBackupPendente; Assert.Contains("não concluída",t.Estado); Assert.True(t.Campo<Button>("BtnExportarBackup").IsEnabled);
    }
    private static void Capturar(TelaBackup t,string nome)
    {
        t.Render(); using var imagem=Avalonia.Headless.HeadlessWindowExtensions.CaptureRenderedFrame(t.Janela);
        Assert.NotNull(imagem); var pasta=Path.Combine(Path.GetTempPath(),"backlogmanager-fase7-resultados","imagens");
        Directory.CreateDirectory(pasta); imagem.Save(Path.Combine(pasta,nome+".png"),Avalonia.Media.Imaging.PngBitmapEncoderOptions.Default);
    }
}
