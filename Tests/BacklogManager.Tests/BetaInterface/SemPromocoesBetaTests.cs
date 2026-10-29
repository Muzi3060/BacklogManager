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

public sealed class SemPromocoesBetaTests
{
    [AvaloniaFact(DisplayName = "BZ01 | Início com obras sem promoções | Nenhum anúncio preço oferta ou navegação comercial")]
    public void Caso01()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item()); t.Clicar("NavInicio"); ApoioBeta.Render(t); ApoioBeta.SemPromocoes(t.Janela); Assert.DoesNotContain(t.Janela.GetVisualDescendants().OfType<Button>(), b => b.Tag?.ToString() == "promocoes");
    }

    [AvaloniaFact(DisplayName = "BZ02 | Biblioteca em grade sem promoções | Nenhum anúncio preço oferta ou navegação comercial")]
    public void Caso02()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item()); t.Clicar("NavBacklog"); ApoioBeta.Render(t); ApoioBeta.SemPromocoes(t.Janela); Assert.DoesNotContain(t.Janela.GetVisualDescendants().OfType<Button>(), b => b.Tag?.ToString() == "promocoes");
    }

    [AvaloniaFact(DisplayName = "BZ03 | Coleções sem promoções | Nenhum anúncio preço oferta ou navegação comercial")]
    public void Caso03()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item()); t.Clicar("NavColecoes"); ApoioBeta.Render(t); ApoioBeta.SemPromocoes(t.Janela); Assert.DoesNotContain(t.Janela.GetVisualDescendants().OfType<Button>(), b => b.Tag?.ToString() == "promocoes");
    }

    [AvaloniaFact(DisplayName = "BZ04 | Cadastro sem promoções | Nenhum anúncio preço oferta ou navegação comercial")]
    public void Caso04()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item()); t.Clicar("NavCadastro"); ApoioBeta.Render(t); ApoioBeta.SemPromocoes(t.Janela); Assert.DoesNotContain(t.Janela.GetVisualDescendants().OfType<Button>(), b => b.Tag?.ToString() == "promocoes");
    }

    [AvaloniaFact(DisplayName = "BZ05 | Configurações sem promoções | Nenhum anúncio preço oferta ou navegação comercial")]
    public void Caso05()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item()); t.Clicar("NavConfiguracoes"); ApoioBeta.Render(t); ApoioBeta.SemPromocoes(t.Janela); Assert.DoesNotContain(t.Janela.GetVisualDescendants().OfType<Button>(), b => b.Tag?.ToString() == "promocoes");
    }

    [AvaloniaFact(DisplayName = "BZ06 | Biblioteca em lista | Visualização alternativa sem conteúdo comercial")]
    public void Caso06()
    {
        using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Clicar("NavBacklog"); t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex = 1; ApoioBeta.Render(t); ApoioBeta.SemPromocoes(t.Janela); Assert.Single(t.Exibidos);
    }

    [AvaloniaFact(DisplayName = "BZ07 | Página inicial vazia | Convites locais sem reserva de promoções")]
    public void Caso07()
    {
        using var t = ApoioBeta.Tela(); ApoioBeta.Render(t); ApoioBeta.SemPromocoes(t.Janela); Assert.True(t.Campo<Border>("EstadoInicioVazio").IsVisible);
    }

    [AvaloniaFact(DisplayName = "BZ08 | Sem credenciais | Aviso IGDB preservado sem promoções")]
    public void Caso08()
    {
        using var t = new TelaTeste(new[] { PastaTeste.Jogo() }, semServico: true); ApoioBeta.Render(t); ApoioBeta.SemPromocoes(t.Janela); Assert.Contains("IGDB", t.Aviso); Assert.Equal(0, t.Http.Chamadas);
    }

    [AvaloniaFact(DisplayName = "BZ09 | Edição pessoal | Formulário não expõe preços nem altera a obra ao abrir")]
    public void Caso09()
    {
        using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); var antes = File.ReadAllBytes(t.Pasta.Arquivo); t.Editar(); ApoioBeta.Render(t); ApoioBeta.SemPromocoes(t.Janela); Assert.Equal(antes, File.ReadAllBytes(t.Pasta.Arquivo));
    }

    [AvaloniaFact(DisplayName = "BZ10 | Filtro de status Na Fila | Fila continua status local sem lista de desejos")]
    public void Caso10()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item(status: "Na Fila")); Assert.Equal("1", t.Campo<TextBlock>("TxtNaFilaDashboard").Text); ApoioBeta.Render(t); ApoioBeta.SemPromocoes(t.Janela);
    }

    [AvaloniaFact(DisplayName = "BZ11 | Backup local | Prévia e confirmação sem serviços comerciais")]
    public async Task Caso11()
    {
        using var a = new AmbienteBackup(); using var t = new TelaBackup(a); a.Importar(); await t.Selecionar(a.Importado); t.Render(); ApoioBeta.SemPromocoes(t.Janela); Assert.True(t.Campo<Border>("PreviaBackup").IsVisible); Assert.Equal(0, t.Http.Chamadas);
    }

    [AvaloniaFact(DisplayName = "BZ12 | Biblioteca indisponível | Erro local sem ofertas simuladas ou requisições")]
    public void Caso12()
    {
        using var t = new TelaTeste(json: "{"); ApoioBeta.Render(t); ApoioBeta.SemPromocoes(t.Janela); Assert.True(t.Campo<Border>("FalhaDashboard").IsVisible); Assert.Equal(0, t.Http.Chamadas);
    }

}
