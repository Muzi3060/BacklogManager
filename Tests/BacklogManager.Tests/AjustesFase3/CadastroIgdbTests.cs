using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Services;
using Xunit;

namespace BacklogManager.Tests.AjustesFase3;

public sealed class CadastroIgdbTests
{
    private static DadosFormularioJogo Dados => new("Título oficial", null, "Na Fila", 4m, "Review");
    internal static ObraCatalogo Jogo => new() { Titulo = "Título oficial", Origem = "IGDB", IdentificadorExterno = "42", Generos = new[] { "RPG", "Adventure" }, Plataformas = new[] { "PC" }, CapaUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/abc.jpg" };
    private static ItemBiblioteca Preparar(ObraCatalogo? obra) => FormularioJogo.Preparar(Dados, obra, "Título oficial", null);
    internal static void Selecionar(TelaTeste t, ObraCatalogo? obra = null, string consulta = "tít")
    {
        obra ??= Jogo; t.Campo<TextBox>("TxtTitulo").Text = consulta;
        var lista = t.Campo<ListBox>("ListaSugestoes"); lista.ItemsSource = new[] { obra }; lista.SelectedIndex = 0;
    }
    [Fact(DisplayName = "GC01 | Digitação sem seleção | Formulário recusa título arbitrário")]
    public void Manual() => Assert.Throws<ValidacaoFormularioException>(() => Preparar(null));
    [Fact(DisplayName = "GC02 | Seleção sem origem IGDB | Cadastro de jogo recusado")]
    public void Origem() => Assert.Throws<ValidacaoFormularioException>(() => Preparar(Jogo with { Origem = null }));
    [Fact(DisplayName = "GC03 | Seleção de catálogo simulado como jogo | Cadastro recusado")]
    public void Simulado() => Assert.Throws<ValidacaoFormularioException>(() => Preparar(Jogo with { Origem = "Simulado", Simulado = true }));
    [Fact(DisplayName = "GC04 | ID inválido na seleção | Cadastro recusado sem converter identificador")]
    public void Id() => Assert.Throws<ValidacaoFormularioException>(() => Preparar(Jogo with { IdentificadorExterno = "00042" }));
    [Fact(DisplayName = "GC05 | Seleção IGDB sem categoria pessoal | Título, gêneros, capa, plataforma e ID vêm do catálogo")]
    public void Metadados() { var c = Jogo; var i = Preparar(c); Assert.Same(c, i.Catalogo); Assert.Equal("", i.Pessoal.Categoria); Assert.Equal(new[] { "RPG", "Adventure" }, i.Catalogo.Generos); }
    [Fact(DisplayName = "GC06 | Texto alterado após selecionar | Formulário bloqueia seleção desatualizada")]
    public void TextoAlterado() => Assert.Throws<ValidacaoFormularioException>(() => FormularioJogo.Preparar(Dados with { TextoTitulo = "Arbitrário" }, Jogo, "Título oficial", null));
    [Fact(DisplayName = "GC07 | API sem gêneros | Cadastro não cria gênero ou categoria fictícia")]
    public void SemGenero() { var i = Preparar(Jogo with { Generos = Array.Empty<string>() }); Assert.Empty(i.Catalogo.Generos); Assert.Empty(i.Pessoal.Categoria); Assert.Empty(ServicoBiblioteca.GenerosParaFiltro(i)); Assert.Equal("Gêneros não informados", i.GenerosExibidos); }
    [Fact(DisplayName = "GC08 | Categoria manual arbitrária em novo jogo | Valor não substitui gêneros externos")]
    public void IgnorarCategoria() { var i = FormularioJogo.Preparar(Dados with { Categoria = "Inventada" }, Jogo, "Título oficial", null); Assert.Empty(i.Pessoal.Categoria); Assert.DoesNotContain("Inventada", i.GenerosExibidos); }
    [Fact(DisplayName = "GC09 | Jogo novo salvo e recarregado | Categoria vazia e metadados reais continuam válidos")]
    public void Roundtrip() { var i = Preparar(Jogo); var r = Fase2.ApoioMultimidia.Roundtrip(i); Assert.Empty(r.Pessoal.Categoria); Assert.Equal(i.Catalogo.Generos, r.Catalogo.Generos); Assert.Equal(i.Catalogo.Plataformas, r.Catalogo.Plataformas); Assert.Equal(i.Catalogo.CapaUrl, r.Catalogo.CapaUrl); }
    [Fact(DisplayName = "GC10 | Jogo sem gêneros salvo e recarregado | Continua sem gênero inventado")]
    public void RoundtripSemGenero() { var r = Fase2.ApoioMultimidia.Roundtrip(Preparar(Jogo with { Generos = Array.Empty<string>() })); Assert.Empty(r.Catalogo.Generos); Assert.Empty(r.Pessoal.Categoria); }
    [AvaloniaFact(DisplayName = "GC11 | Abrir cadastro de jogo | Botão desabilitado antes de seleção válida")]
    public void Botao() { using var t = new TelaTeste(); Assert.False(t.Campo<Button>("BtnAdicionar").IsEnabled); }
    [AvaloniaFact(DisplayName = "GC12 | Digitar título arbitrário na janela | Botão continua desabilitado e nenhuma gravação ocorre")]
    public async Task Digitacao() { using var t = new TelaTeste(semServico: true); t.Campo<TextBox>("TxtTitulo").Text = "Qualquer título"; await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Background); t.Clicar("BtnAdicionar"); Assert.False(t.Campo<Button>("BtnAdicionar").IsEnabled); Assert.Empty(t.Exibidos); Assert.Contains("IGDB", t.Aviso); }
    [AvaloniaFact(DisplayName = "GC13 | Selecionar sugestão por consulta parcial | Campo exibe título oficial e seleção continua válida após eventos")]
    public void Titulo() { using var t = new TelaTeste(); Selecionar(t); Dispatcher.UIThread.RunJobs(); Assert.Equal("Título oficial", t.Campo<TextBox>("TxtTitulo").Text); Assert.True(t.Campo<Button>("BtnAdicionar").IsEnabled); t.Clicar("BtnAdicionar"); Assert.Equal("Título oficial", Assert.Single(t.Salvos).Titulo); }
    [AvaloniaFact(DisplayName = "GC14 | Modificar campo após seleção | Seleção e gêneros antigos são invalidados")]
    public void Modificar() { using var t = new TelaTeste(); Selecionar(t); t.Campo<TextBox>("TxtTitulo").Text = "Outro"; Dispatcher.UIThread.RunJobs(); Assert.False(t.Campo<Button>("BtnAdicionar").IsEnabled); Assert.DoesNotContain("RPG", t.Campo<TextBlock>("TxtGenerosCatalogo").Text!); }
    [AvaloniaFact(DisplayName = "GC15 | Limpar consulta após selecionar | Botão fica desabilitado sem sugestões remanescentes")]
    public void Limpar() { using var t = new TelaTeste(); Selecionar(t); t.Campo<TextBox>("TxtTitulo").Text = ""; Dispatcher.UIThread.RunJobs(); Assert.False(t.Campo<Button>("BtnAdicionar").IsEnabled); Assert.False(t.Campo<ListBox>("ListaSugestoes").IsVisible); }
    [AvaloniaFact(DisplayName = "GC16 | Pesquisa IGDB bem-sucedida e seleção | Catálogo preenche título e todos os gêneros sem categoria manual")]
    public async Task Pesquisa() { using var t = new TelaTeste(); t.Http.Responder = (_, _) => Task.FromResult(HttpSimulado.Json("[{\"id\":42,\"name\":\"Título oficial\",\"genres\":[{\"name\":\"RPG\"},{\"name\":\"Adventure\"}]}]")); t.Campo<TextBox>("TxtTitulo").Text = "tít"; await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Background); await t.Janela.BuscaPendente; t.Campo<ListBox>("ListaSugestoes").SelectedIndex = 0; Dispatcher.UIThread.RunJobs(); Assert.Equal("Título oficial", t.Campo<TextBox>("TxtTitulo").Text); Assert.Equal("RPG, Adventure", t.Campo<TextBlock>("TxtGenerosCatalogo").Text); Assert.False(t.Campo<StackPanel>("CampoCategoriaLegada").IsVisible); t.Clicar("BtnAdicionar"); Assert.Equal(42, Assert.Single(t.Salvos).IgdbId); Assert.Equal(1, t.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "GC17 | IGDB indisponível | Mensagem clara e nenhum cadastro sem seleção")]
    public async Task Falha() { using var t = new TelaTeste(); t.Http.Responder = (_, _) => throw new HttpRequestException("offline"); t.Campo<TextBox>("TxtTitulo").Text = "tít"; await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Background); await t.Janela.BuscaPendente; Assert.False(t.Campo<Button>("BtnAdicionar").IsEnabled); Assert.Equal("Falha na pesquisa", t.Campo<TextBlock>("TxtAvisoTitulo").Text); Assert.Empty(t.Exibidos); }
    [AvaloniaFact(DisplayName = "GC18 | Resultado sem gêneros na janela | Mensagem de ausência sem RPG padrão")]
    public void SemGeneroJanela() { using var t = new TelaTeste(); Selecionar(t, Jogo with { Generos = Array.Empty<string>() }); Assert.Contains("não informados", t.Campo<TextBlock>("TxtGenerosCatalogo").Text!); t.Clicar("BtnAdicionar"); Assert.Empty(Assert.Single(t.Salvos).Categoria); }
    [AvaloniaFact(DisplayName = "GC19 | Deselecionar sugestão | Botão volta a ficar desabilitado")]
    public void Deselecionar() { using var t = new TelaTeste(); Selecionar(t); t.Campo<ListBox>("ListaSugestoes").SelectedIndex = -1; Assert.False(t.Campo<Button>("BtnAdicionar").IsEnabled); }
    [AvaloniaFact(DisplayName = "GC20 | Cadastro concluído | Formulário limpo exige nova seleção para próximo jogo")]
    public void NovoDepois() { using var t = new TelaTeste(); Selecionar(t); t.Clicar("BtnAdicionar"); Assert.False(t.Campo<Button>("BtnAdicionar").IsEnabled); Assert.Empty(t.Campo<TextBox>("TxtTitulo").Text!); Assert.Single(t.Salvos); }
}
