using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Tests.Fase2;
using Xunit;
namespace BacklogManager.Tests.AjustesFase3;

public sealed class OfflineTests
{
    private static BacklogItem[] Itens() => new[] { PastaTeste.Jogo("Zeta"), PastaTeste.Jogo("Alfa") };
    [AvaloniaFact(DisplayName = "OL01 | Abrir biblioteca sem credenciais | Itens locais visíveis sem HTTP IGDB")]
    public void Abrir() { using var t = new TelaTeste(Itens(), semServico: true); Assert.Equal(2, t.Exibidos.Length); Assert.Equal(0, t.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "OL02 | Editar nota de legado offline | Salva sem pedir seleção IGDB ou converter ID")]
    public void Nota() { using var t = new TelaTeste(Itens(), semServico: true); t.Editar(); t.Campo<NumericUpDown>("NumNota").Value = 1; t.Clicar("BtnAtualizar"); Assert.Equal(1, t.Salvos[0].Nota); Assert.Equal(1010, t.Salvos[0].SteamAppId); Assert.Null(t.Salvos[0].IgdbId); Assert.Equal(0, t.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "OL03 | Editar review offline | Texto pessoal persiste sem requisição IGDB")]
    public void Review() { using var t = new TelaTeste(Itens(), semServico: true); t.Editar(); t.Campo<TextBox>("TxtReview").Text = "Minha experiência offline"; t.Clicar("BtnAtualizar"); Assert.Equal("Minha experiência offline", t.Salvos[0].Review); Assert.Equal(0, t.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "OL04 | Filtrar local por categoria e nota | Operação continua offline")]
    public void Filtros() { using var t = new TelaTeste(Itens(), semServico: true); t.Campo<ComboBox>("CmbGenero").SelectedItem = "RPG"; t.Campo<NumericUpDown>("NumNotaMinima").Value = 5; Assert.Empty(t.Exibidos); Assert.Equal(0, t.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "OL05 | Ordenar local offline | Ordem alfabética correta sem regravar JSON")]
    public void Ordenar() { using var t = new TelaTeste(Itens(), semServico: true); var bytes = File.ReadAllBytes(t.Pasta.Arquivo); t.Campo<ComboBox>("CmbOrdenacao").SelectedIndex = 1; Assert.Equal("Alfa", t.Exibidos[0].Titulo); Assert.Equal(bytes, File.ReadAllBytes(t.Pasta.Arquivo)); Assert.Equal(0, t.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "OL06 | Excluir legado sem ID IGDB offline | Demais registros preservados")]
    public void Excluir() { using var t = new TelaTeste(Itens(), semServico: true); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; t.Clicar("BtnRemover"); Assert.Equal("Alfa", Assert.Single(t.Salvos).Titulo); Assert.Equal(0, t.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "OL07 | Editar jogo novo com gêneros offline | Metadados de catálogo permanecem e nota muda")]
    public void NovoOffline() { var i = new ItemBiblioteca(CadastroIgdbTests.Jogo with { CapaUrl = null }, new() { Categoria = "", Status = "Na Fila", Nota = 4, Review = "" }); using var t = new TelaTeste(new[] { ConversorBibliotecaJson.ParaRegistro(i) }, semServico: true); t.Editar(); t.Campo<NumericUpDown>("NumNota").Value = 5; t.Clicar("BtnAtualizar"); Assert.Equal(i.Catalogo.Generos, t.Salvos[0].Multimidia!.Generos); Assert.Equal(5, t.Salvos[0].Nota); Assert.Equal(0, t.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "OL08 | Editar página de livro offline | Progresso positivo é salvo sem IGDB")]
    public void Livro() { using var t = new TelaTeste(new[] { ConversorBibliotecaJson.ParaRegistro(ApoioMultimidia.Item(TipoMidia.Livro)) }, semServico: true); t.Campo<ComboBox>("CmbTipoBiblioteca").SelectedIndex = (int)TipoMidia.Livro; t.Editar(); t.Campo<NumericUpDown>("NumPagina").Value = 10; t.Clicar("BtnAtualizar"); Assert.Equal(10, Assert.Single(t.Salvos).Multimidia!.Progresso!.Pagina); Assert.Equal(0, t.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "OL09 | Registro sem qualquer identificador | Edição offline não exige recadastro")]
    public void SemId() { var r = PastaTeste.Jogo(); r.SteamAppId = null; using var t = new TelaTeste(new[] { r }, semServico: true); t.Editar(); t.Clicar("BtnAtualizar"); var salvo = Assert.Single(t.Salvos); Assert.Null(salvo.IgdbId); Assert.Null(salvo.SteamAppId); Assert.Equal(0, t.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "OL10 | Alternar visualização e navegar offline | Biblioteca local continua acessível")]
    public void Navegar() { using var t = new TelaTeste(Itens(), semServico: true); t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex = 1; t.Clicar("NavBacklog"); t.Clicar("NavInicio"); t.Clicar("NavBacklog"); Assert.Equal(2, t.Exibidos.Length); Assert.Equal(0, t.Http.Chamadas); }
}
