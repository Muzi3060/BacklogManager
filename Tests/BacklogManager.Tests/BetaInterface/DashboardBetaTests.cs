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

public sealed class DashboardBetaTests
{
    [AvaloniaFact(DisplayName = "BD01 | Dashboard vazio | Totais zerados com convite útil sem blocos removidos")]
    public void Caso01()
    {
        using var t = ApoioBeta.Tela(); Assert.Equal("0", t.Campo<TextBlock>("TxtTotal").Text); Assert.True(t.Campo<Border>("EstadoInicioVazio").IsVisible); Assert.Contains("primeira obra", t.Campo<TextBlock>("TxtResumo").Text); Assert.Null(t.Janela.FindControl<StackPanel>("BlocoSemData"));
    }

    [AvaloniaFact(DisplayName = "BD02 | Sem data na biblioteca | Registro sem data contado e preservado sem seção extra")]
    public void Caso02()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item() with { DataAdicao = null }); var antes = File.ReadAllBytes(t.Pasta.Arquivo); Assert.Equal("1", t.Campo<TextBlock>("TxtTotal").Text); Assert.Empty(t.Campo<ItemsControl>("ListaAtividade").Items); Assert.Null(t.Janela.FindControl<ItemsControl>("ListaRecentes")); Assert.Null(Assert.Single(t.Salvos).Multimidia?.DataAdicao); Assert.Equal(antes, File.ReadAllBytes(t.Pasta.Arquivo));
    }

    [AvaloniaFact(DisplayName = "BD03 | Andamento sem data | Obra continua no acompanhamento sem inventar data")]
    public void Caso03()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item(status: "Jogando") with { DataAdicao = null }); var e = Assert.IsType<EntradaDashboard>(Assert.Single(t.Campo<ItemsControl>("ListaContinuar").Items)); Assert.Null(e.Obra.DataAdicao); Assert.Equal("Data não registrada", e.Data); Assert.Empty(t.Campo<ItemsControl>("ListaAtividade").Items);
    }

    [AvaloniaFact(DisplayName = "BD04 | Atividade sete dias | Adição antiga excluída sem afetar total")]
    public void Caso04()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item(2), ApoioDashboard.Item(20)); t.Campo<ComboBox>("CmbPeriodoDashboard").SelectedIndex = 0; Assert.Equal("1", t.Campo<TextBlock>("TxtAdicionadas").Text); Assert.Equal("2", t.Campo<TextBlock>("TxtTotal").Text);
    }

    [AvaloniaFact(DisplayName = "BD05 | Atividade trinta dias | Adições reais do intervalo exibidas")]
    public void Caso05()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item(2), ApoioDashboard.Item(20), ApoioDashboard.Item(40)); Assert.Equal("2", t.Campo<TextBlock>("TxtAdicionadas").Text); Assert.Equal(2, t.Campo<ItemsControl>("ListaAtividade").Items.Count);
    }

    [AvaloniaFact(DisplayName = "BD06 | Data futura | Fora da atividade sem apagar registro")]
    public void Caso06()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item(-2)); Assert.Equal("1", t.Campo<TextBlock>("TxtTotal").Text); Assert.Empty(t.Campo<ItemsControl>("ListaAtividade").Items); Assert.Single(t.Salvos);
    }

    [AvaloniaFact(DisplayName = "BD07 | Distribuição multimídia | Seis mídias mantêm suas quantidades")]
    public void Caso07()
    {
        using var t = ApoioBeta.Tela(Enum.GetValues<TipoMidia>().Select(tipo => ApoioDashboard.Item(tipo: tipo)).ToArray()); Assert.Equal("6", t.Campo<TextBlock>("TxtTotal").Text); Assert.All(t.Campo<ItemsControl>("DistribuicaoMidias").Items.Cast<DistribuicaoDashboard>(), d => Assert.Equal(1, d.Quantidade));
    }

    [AvaloniaFact(DisplayName = "BD08 | Status locais | Resumo mantém fila andamento e conclusão")]
    public void Caso08()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item(status: "Na Fila"), ApoioDashboard.Item(status: "Jogando"), ApoioDashboard.Item(status: "Zerado")); Assert.Equal("1", t.Campo<TextBlock>("TxtNaFilaDashboard").Text); Assert.Equal("1", t.Campo<TextBlock>("TxtJogando").Text); Assert.Equal("1", t.Campo<TextBlock>("TxtZerados").Text); Assert.InRange(t.Campo<ProgressBar>("ProgressoBacklog").Value, 33, 34);
    }

    [AvaloniaFact(DisplayName = "BD09 | Edição no acompanhamento | Abre formulário existente sem duplicar obra")]
    public void Caso09()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item(status: "Jogando")); t.Campo<ItemsControl>("ListaContinuar").BringIntoView(); ApoioBeta.Render(t); var b = t.Campo<ItemsControl>("ListaContinuar").GetVisualDescendants().OfType<Button>().First(); b.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Button.ClickEvent)); Assert.True(t.Campo<ScrollViewer>("PaginaCadastro").IsVisible); Assert.Single(t.Salvos);
    }

    [AvaloniaFact(DisplayName = "BD10 | Progresso não informado | Sem percentual pessoal fabricado")]
    public void Caso10()
    {
        var i = ApoioDashboard.Item(status: "Jogando"); i = i with { Pessoal = i.Pessoal with { Progresso = null } }; using var t = ApoioBeta.Tela(i); var e = Assert.IsType<EntradaDashboard>(Assert.Single(t.Campo<ItemsControl>("ListaContinuar").Items)); Assert.Equal("Progresso não informado", e.Progresso);
    }

    [AvaloniaFact(DisplayName = "BD11 | Textos simplificados | Sem data próxima obra e explicações extensas removidos")]
    public void Caso11()
    {
        using var t = ApoioBeta.Tela(); ApoioBeta.Render(t); var textos = t.Campo<ScrollViewer>("PaginaInicio").GetVisualDescendants().OfType<TextBlock>().Select(x => x.Text ?? ""); Assert.DoesNotContain(textos, x => x.Contains("Qual será") || x.Contains("Sem data de adição") || x.Contains("não representa último acesso") || x.Contains("proporção de registros") || x.Contains("Conclusões no período") || x.Contains("Amostra de até"));
    }

    [AvaloniaFact(DisplayName = "BD12 | Estatísticas independem dos filtros | Filtro de mídia não reduz Dashboard")]
    public void Caso12()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item(), ApoioDashboard.Item(tipo: TipoMidia.Livro)); t.Clicar("NavBacklog"); t.Campo<ComboBox>("CmbTipoBiblioteca").SelectedIndex = (int)TipoMidia.Livro; t.Clicar("NavInicio"); Assert.Equal("2", t.Campo<TextBlock>("TxtTotal").Text);
    }

    [AvaloniaFact(DisplayName = "BD13 | Janela mínima e teclado | Período focável e conteúdo final alcançável")]
    public void Caso13()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item()); t.Janela.Width = 900; t.Janela.Height = 660; ApoioBeta.Render(t); var c = t.Campo<ComboBox>("CmbPeriodoDashboard"); c.BringIntoView(); ApoioBeta.Render(t); Assert.True(c.Focus(NavigationMethod.Tab)); var s = t.Campo<ScrollViewer>("PaginaInicio"); s.Offset = new Vector(0, 10000); ApoioBeta.Render(t); Assert.True(s.Offset.Y > 0);
    }

    [AvaloniaFact(DisplayName = "BD14 | Falha de leitura preservada | Arquivo inválido não vira biblioteca vazia")]
    public void Caso14()
    {
        using var t = new TelaTeste(json: "{"); Assert.True(t.Campo<Border>("FalhaDashboard").IsVisible); Assert.False(t.Campo<StackPanel>("ConteudoDashboard").IsVisible); Assert.Equal("{", File.ReadAllText(t.Pasta.Arquivo));
    }

    [AvaloniaFact(DisplayName = "BD15 | Consulta local sem escrita | Trocar período e navegar não grava nem chama IGDB")]
    public void Caso15()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item(), ApoioDashboard.Item() with { DataAdicao = null }); var antes = File.ReadAllBytes(t.Pasta.Arquivo); t.Campo<ComboBox>("CmbPeriodoDashboard").SelectedIndex = 0; t.Clicar("NavConfiguracoes"); t.Clicar("NavInicio"); Assert.Equal(antes, File.ReadAllBytes(t.Pasta.Arquivo)); Assert.Equal(0, t.Http.Chamadas);
    }

    [AvaloniaFact(DisplayName = "BD16 | Legado sem identificador | Título SteamAppId e categoria antigos preservados")]
    public void Caso16()
    {
        var i = PastaTeste.Jogo("Legado isolado"); i.SteamAppId = 361855; using var t = new TelaTeste(new[] { i }); var antes = File.ReadAllBytes(t.Pasta.Arquivo); t.Clicar("NavInicio"); Assert.Equal(361855, Assert.Single(t.Salvos).SteamAppId); Assert.Null(Assert.Single(t.Salvos).IgdbId); Assert.Equal(i.Categoria, Assert.Single(t.Salvos).Categoria); Assert.Equal(antes, File.ReadAllBytes(t.Pasta.Arquivo));
    }

}
