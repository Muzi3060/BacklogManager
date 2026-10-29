using System.Text;
using System.Text.Json.Nodes;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Styling;
using Avalonia.VisualTree;
using BacklogManager.Aparencia;
using BacklogManager.Backup;
using BacklogManager.Controls;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Tests.Fase5;
using BacklogManager.Tests.Fase6;
using BacklogManager.Tests.Fase7;
using Xunit;
namespace BacklogManager.Tests.BetaInterface;

public sealed class NavegacaoBetaTests
{
    [AvaloniaFact(DisplayName = "BN01 | Início na navegação | Página correta e seleção ativa")]
    public void Caso01()
    {
        using var t = new TelaTeste(); t.Clicar("NavInicio"); ApoioBeta.Render(t); Assert.True(t.Campo<ScrollViewer>("PaginaInicio").IsVisible); Assert.Contains("active", t.Campo<Button>("NavInicio").Classes); Assert.DoesNotContain("active", t.Campo<Button>("NavConfiguracoes").Classes);
    }

    [AvaloniaFact(DisplayName = "BN02 | Biblioteca na navegação | Página correta e seleção ativa")]
    public void Caso02()
    {
        using var t = new TelaTeste(); t.Clicar("NavBacklog"); ApoioBeta.Render(t); Assert.True(t.Campo<Grid>("PaginaBacklog").IsVisible); Assert.Contains("active", t.Campo<Button>("NavBacklog").Classes); Assert.DoesNotContain("active", t.Campo<Button>("NavConfiguracoes").Classes);
    }

    [AvaloniaFact(DisplayName = "BN03 | Coleções na navegação | Página correta e seleção ativa")]
    public void Caso03()
    {
        using var t = new TelaTeste(); t.Clicar("NavColecoes"); ApoioBeta.Render(t); Assert.True(t.Campo<ScrollViewer>("PaginaColecoes").IsVisible); Assert.Contains("active", t.Campo<Button>("NavColecoes").Classes); Assert.DoesNotContain("active", t.Campo<Button>("NavConfiguracoes").Classes);
    }

    [AvaloniaFact(DisplayName = "BN04 | Adicionar editar na navegação | Página correta e seleção ativa")]
    public void Caso04()
    {
        using var t = new TelaTeste(); t.Clicar("NavCadastro"); ApoioBeta.Render(t); Assert.True(t.Campo<ScrollViewer>("PaginaCadastro").IsVisible); Assert.Contains("active", t.Campo<Button>("NavCadastro").Classes); Assert.DoesNotContain("active", t.Campo<Button>("NavConfiguracoes").Classes);
    }

    [AvaloniaFact(DisplayName = "BN05 | Configurações no rodapé | Botão ancorado abaixo dos links principais")]
    public void Caso05()
    {
        using var t = new TelaTeste(); ApoioBeta.Render(t); var b = t.Campo<Button>("NavConfiguracoes"); var pai = Assert.IsType<Border>(b.Parent); Assert.Equal(3, Grid.GetRow(pai)); Assert.True(b.TranslatePoint(default, t.Janela)!.Value.Y > t.Campo<Button>("NavCadastro").TranslatePoint(default, t.Janela)!.Value.Y);
    }

    [AvaloniaFact(DisplayName = "BN06 | Rodapé em janela mínima | Configurações visíveis dentro da janela")]
    public void Caso06()
    {
        using var t = new TelaTeste(); t.Janela.Width = 900; t.Janela.Height = 660; ApoioBeta.Render(t); var b = t.Campo<Button>("NavConfiguracoes"); var p = b.TranslatePoint(default, t.Janela)!.Value; Assert.InRange(p.Y, 0, 660 - b.Bounds.Height); Assert.True(b.IsEffectivelyVisible);
    }

    [AvaloniaFact(DisplayName = "BN07 | Teclado no rodapé | Foco por Tab chega a Configurações")]
    public void Caso07()
    {
        using var t = new TelaTeste(); ApoioBeta.Render(t); var b = t.Campo<Button>("NavConfiguracoes"); Assert.True(b.Focus(NavigationMethod.Tab)); Assert.True(b.IsFocused);
    }

    [AvaloniaFact(DisplayName = "BN08 | Navegar e voltar | Somente a página escolhida permanece visível")]
    public void Caso08()
    {
        using var t = new TelaTeste(); t.Clicar("NavConfiguracoes"); Assert.True(t.Campo<ScrollViewer>("PaginaConfiguracoes").IsVisible); t.Clicar("NavInicio"); Assert.False(t.Campo<ScrollViewer>("PaginaConfiguracoes").IsVisible); Assert.True(t.Campo<ScrollViewer>("PaginaInicio").IsVisible);
    }

    [AvaloniaFact(DisplayName = "BN09 | Remoção do link Aparência | Nenhuma rota ou editor independente")]
    public void Caso09()
    {
        using var t = new TelaTeste(); ApoioBeta.SemEditor(t.Janela); Assert.DoesNotContain(t.Janela.GetVisualDescendants().OfType<Button>(), b => b.Tag?.ToString() == "aparencia");
    }

    [AvaloniaFact(DisplayName = "BN10 | Rodapé sem indicador antigo | Dados neste computador e Armazenamento local ausentes")]
    public void Caso10()
    {
        using var t = new TelaTeste(); ApoioBeta.Render(t); Assert.DoesNotContain(ApoioBeta.Textos(t.Janela), s => s is "Dados neste computador" or "Armazenamento local");
    }

    [AvaloniaFact(DisplayName = "BN11 | Rascunho durante navegação | Review editada preservada ao abrir configurações")]
    public void Caso11()
    {
        using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Editar(); t.Campo<TextBox>("TxtReview").Text = "Rascunho isolado"; t.Clicar("NavConfiguracoes"); t.Clicar("NavCadastro"); Assert.Equal("Rascunho isolado", t.Campo<TextBox>("TxtReview").Text);
    }

    [AvaloniaFact(DisplayName = "BN12 | Navegação local sem IGDB | Biblioteca e configurações disponíveis sem credenciais")]
    public void Caso12()
    {
        using var t = new TelaTeste(new[] { PastaTeste.Jogo() }, semServico: true); var bytes = File.ReadAllBytes(t.Pasta.Arquivo); t.Clicar("NavBacklog"); t.Clicar("NavColecoes"); t.Clicar("NavConfiguracoes"); t.Clicar("NavInicio"); Assert.Equal(bytes, File.ReadAllBytes(t.Pasta.Arquivo)); Assert.Equal(0, t.Http.Chamadas);
    }

}
