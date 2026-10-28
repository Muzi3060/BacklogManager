using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using BacklogManager.Models;
using Xunit;
namespace BacklogManager.Tests.AjustesFase3;

public sealed class TitulosTests
{
    private static void Conferir(TipoMidia tipo, string titulo)
    {
        using var t = new TelaTeste(); t.Campo<ComboBox>("CmbTipoCadastro").SelectedIndex = (int)tipo;
        var c = tipo == TipoMidia.Jogo ? CadastroIgdbTests.Jogo with { Titulo = titulo, CapaUrl = null } : Fase2.ApoioMultimidia.Catalogo(tipo) with { Titulo = titulo };
        CadastroIgdbTests.Selecionar(t, c, "consulta parcial"); Dispatcher.UIThread.RunJobs(); Assert.Equal(titulo, t.Campo<TextBox>("TxtTitulo").Text); t.Clicar("BtnAdicionar"); Assert.Equal(titulo, Assert.Single(t.Salvos).Titulo);
    }
    [AvaloniaFact(DisplayName = "TC01 | Título de Jogo selecionado por consulta parcial | Nome oficial é exibido e persistido sem nova restrição de catálogo")]
    public void Jogo() => Conferir(TipoMidia.Jogo, "Obra oficial Jogo");
    [AvaloniaFact(DisplayName = "TC02 | Título de Anime selecionado por consulta parcial | Nome oficial é exibido e persistido sem nova restrição de catálogo")]
    public void Anime() => Conferir(TipoMidia.Anime, "Obra oficial Anime");
    [AvaloniaFact(DisplayName = "TC03 | Título de Serie selecionado por consulta parcial | Nome oficial é exibido e persistido sem nova restrição de catálogo")]
    public void Serie() => Conferir(TipoMidia.Serie, "Obra oficial Serie");
    [AvaloniaFact(DisplayName = "TC04 | Título de Filme selecionado por consulta parcial | Nome oficial é exibido e persistido sem nova restrição de catálogo")]
    public void Filme() => Conferir(TipoMidia.Filme, "Obra oficial Filme");
    [AvaloniaFact(DisplayName = "TC05 | Título de Manga selecionado por consulta parcial | Nome oficial é exibido e persistido sem nova restrição de catálogo")]
    public void Manga() => Conferir(TipoMidia.Manga, "Obra oficial Manga");
    [AvaloniaFact(DisplayName = "TC06 | Título de Livro selecionado por consulta parcial | Nome oficial é exibido e persistido sem nova restrição de catálogo")]
    public void Livro() => Conferir(TipoMidia.Livro, "Obra oficial Livro");
    [AvaloniaFact(DisplayName = "TC07 | Título Unicode | Título exato do catálogo não é truncado nem substituído pela pesquisa")]
    public void Unicode() => Conferir(TipoMidia.Jogo, "Título 日本語 🎮");
    [AvaloniaFact(DisplayName = "TC08 | Título com aspas | Título exato do catálogo não é truncado nem substituído pela pesquisa")]
    public void Aspas() => Conferir(TipoMidia.Jogo, "O Jogo \"Especial\"");
    [AvaloniaFact(DisplayName = "TC09 | Nome maior que a consulta | Título exato do catálogo não é truncado nem substituído pela pesquisa")]
    public void Longo() => Conferir(TipoMidia.Jogo, "Título muito mais longo que a consulta parcial");
    [AvaloniaFact(DisplayName = "TC10 | Nome menor que a consulta | Título exato do catálogo não é truncado nem substituído pela pesquisa")]
    public void Curto() => Conferir(TipoMidia.Jogo, "AB");
}
