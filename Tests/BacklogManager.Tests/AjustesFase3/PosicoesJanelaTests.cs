using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Tests.Fase2;
using Xunit;
namespace BacklogManager.Tests.AjustesFase3;

public sealed class PosicoesJanelaTests
{
    private static TelaTeste Nova(TipoMidia tipo)
    {
        var t = new TelaTeste(); t.Campo<ComboBox>("CmbTipoCadastro").SelectedIndex = (int)tipo; CadastroIgdbTests.Selecionar(t, ApoioMultimidia.Catalogo(tipo)); return t;
    }
    private static void Recusar(TipoMidia tipo, string campo, decimal valor)
    {
        using var t = Nova(tipo); t.Campo<NumericUpDown>(campo).Value = valor; t.Clicar("BtnAdicionar"); Assert.Empty(t.Salvos); Assert.Equal("Progresso inválido", t.Campo<TextBlock>("TxtAvisoTitulo").Text);
    }
    [AvaloniaFact(DisplayName = "UP01 | Página zero no cadastro | Aviso e nenhuma gravação")]
    public void PaginaZero() => Recusar(TipoMidia.Livro, "NumPagina", 0);
    [AvaloniaFact(DisplayName = "UP02 | Capítulo zero em livro | Aviso e nenhuma gravação")]
    public void CapLivroZero() => Recusar(TipoMidia.Livro, "NumCapituloLivro", 0);
    [AvaloniaFact(DisplayName = "UP03 | Capítulo zero em mangá | Aviso e nenhuma gravação")]
    public void CapMangaZero() => Recusar(TipoMidia.Manga, "NumCapituloManga", 0);
    [AvaloniaFact(DisplayName = "UP04 | Volume zero no cadastro | Aviso e nenhuma gravação")]
    public void VolumeZero() => Recusar(TipoMidia.Manga, "NumVolume", 0);
    [AvaloniaFact(DisplayName = "UP05 | Página negativa no controle | Valor negativo não é persistido nem aceito como zero novo")]
    public void Negativo() => Recusar(TipoMidia.Livro, "NumPagina", -1);
    [AvaloniaFact(DisplayName = "UP06 | Página fracionária | Número não é truncado para inteiro")]
    public void Fracao() => Recusar(TipoMidia.Livro, "NumPagina", 1.5m);
    [AvaloniaFact(DisplayName = "UP07 | Livro sem posição informada | Campos vazios persistem como ausência")]
    public void Vazio() { using var t = Nova(TipoMidia.Livro); t.Clicar("BtnAdicionar"); Assert.Null(Assert.Single(t.Salvos).Multimidia!.Progresso); }
    [AvaloniaFact(DisplayName = "UP08 | Primeiro capítulo e volume | Posições um aceitas e recarregadas")]
    public void Um() { using var t = Nova(TipoMidia.Manga); t.Campo<NumericUpDown>("NumCapituloManga").Value = 1; t.Campo<NumericUpDown>("NumVolume").Value = 1; t.Clicar("BtnAdicionar"); var p = Assert.Single(t.Salvos).Multimidia!.Progresso!; Assert.Equal(1, p.Capitulo); Assert.Equal(1, p.Volume); }
    [AvaloniaFact(DisplayName = "UP09 | Review de livro com página zero legada | Salva review preservando zero antigo")]
    public void LegadoZero() { using var t = new TelaTeste(new[] { ConversorBibliotecaJson.ParaRegistro(ApoioMultimidia.Item(TipoMidia.Livro, new() { Pagina = 0 })) }); t.Campo<ComboBox>("CmbTipoBiblioteca").SelectedIndex = (int)TipoMidia.Livro; t.Editar(); t.Campo<TextBox>("TxtReview").Text = "Atualizada"; t.Clicar("BtnAtualizar"); var r = Assert.Single(t.Salvos); Assert.Equal("Atualizada", r.Review); Assert.Equal(0, r.Multimidia!.Progresso!.Pagina); }
    [AvaloniaFact(DisplayName = "UP10 | Limpar zero legado no formulário | Ausência substitui zero somente por escolha explícita")]
    public void Limpar() { using var t = new TelaTeste(new[] { ConversorBibliotecaJson.ParaRegistro(ApoioMultimidia.Item(TipoMidia.Livro, new() { Pagina = 0 })) }); t.Campo<ComboBox>("CmbTipoBiblioteca").SelectedIndex = (int)TipoMidia.Livro; t.Editar(); t.Campo<NumericUpDown>("NumPagina").Value = null; t.Clicar("BtnAtualizar"); Assert.Null(Assert.Single(t.Salvos).Multimidia!.Progresso); }
    [AvaloniaFact(DisplayName = "UP11 | Alterar posição positiva para zero | Arquivo anterior permanece intacto")]
    public void Regredir() { using var t = new TelaTeste(new[] { ConversorBibliotecaJson.ParaRegistro(ApoioMultimidia.Item(TipoMidia.Livro, new() { Pagina = 4 })) }); var antes = File.ReadAllBytes(t.Pasta.Arquivo); t.Campo<ComboBox>("CmbTipoBiblioteca").SelectedIndex = (int)TipoMidia.Livro; t.Editar(); t.Campo<NumericUpDown>("NumPagina").Value = 0; t.Clicar("BtnAtualizar"); Assert.Equal(antes, File.ReadAllBytes(t.Pasta.Arquivo)); }
    [AvaloniaFact(DisplayName = "UP12 | Corrigir posição após erro | Cadastro funciona com posição positiva sem perder seleção")]
    public void Recuperar() { using var t = Nova(TipoMidia.Livro); t.Campo<NumericUpDown>("NumPagina").Value = 0; t.Clicar("BtnAdicionar"); Assert.Empty(t.Salvos); t.Campo<NumericUpDown>("NumPagina").Value = 2; t.Clicar("BtnAdicionar"); Assert.Equal(2, Assert.Single(t.Salvos).Multimidia!.Progresso!.Pagina); }
}
