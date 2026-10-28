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
    private static void Normalizar(TipoMidia tipo, string campo, decimal valor)
    {
        using var t = Nova(tipo); t.Campo<NumericUpDown>(campo).Value = valor; Assert.Equal(1, t.Campo<NumericUpDown>(campo).Value); t.Clicar("BtnAdicionar"); Assert.Single(t.Salvos);
    }
    [AvaloniaFact(DisplayName = "UP01 | Página zero no cadastro | Valor normalizado imediatamente para um e cadastro válido")]
    public void PaginaZero() => Normalizar(TipoMidia.Livro, "NumPagina", 0);
    [AvaloniaFact(DisplayName = "UP02 | Capítulo zero em livro | Valor normalizado imediatamente para um e cadastro válido")]
    public void CapLivroZero() => Normalizar(TipoMidia.Livro, "NumCapituloLivro", 0);
    [AvaloniaFact(DisplayName = "UP03 | Capítulo zero em mangá | Valor normalizado imediatamente para um e cadastro válido")]
    public void CapMangaZero() => Normalizar(TipoMidia.Manga, "NumCapituloManga", 0);
    [AvaloniaFact(DisplayName = "UP04 | Volume zero no cadastro | Valor normalizado imediatamente para um e cadastro válido")]
    public void VolumeZero() => Normalizar(TipoMidia.Manga, "NumVolume", 0);
    [AvaloniaFact(DisplayName = "UP05 | Página negativa no controle | Valor normalizado para um antes de salvar")]
    public void Negativo() => Normalizar(TipoMidia.Livro, "NumPagina", -1);
    [AvaloniaFact(DisplayName = "UP06 | Página fracionária | Último inteiro válido é mantido imediatamente")]
    public void Fracao() { using var t = Nova(TipoMidia.Livro); t.Campo<NumericUpDown>("NumPagina").Value = 3; t.Campo<NumericUpDown>("NumPagina").Value = 1.5m; Assert.Equal(3, t.Campo<NumericUpDown>("NumPagina").Value); t.Clicar("BtnAdicionar"); Assert.Equal(3, Assert.Single(t.Salvos).Multimidia!.Progresso!.Pagina); }
    [AvaloniaFact(DisplayName = "UP07 | Livro sem posição informada | Campos vazios persistem como ausência")]
    public void Vazio() { using var t = Nova(TipoMidia.Livro); t.Clicar("BtnAdicionar"); Assert.Null(Assert.Single(t.Salvos).Multimidia!.Progresso); }
    [AvaloniaFact(DisplayName = "UP08 | Primeiro capítulo e volume | Posições um aceitas e recarregadas")]
    public void Um() { using var t = Nova(TipoMidia.Manga); t.Campo<NumericUpDown>("NumCapituloManga").Value = 1; t.Campo<NumericUpDown>("NumVolume").Value = 1; t.Clicar("BtnAdicionar"); var p = Assert.Single(t.Salvos).Multimidia!.Progresso!; Assert.Equal(1, p.Capitulo); Assert.Equal(1, p.Volume); }
    [AvaloniaFact(DisplayName = "UP09 | Review de livro com página zero legada | Salva review preservando zero antigo")]
    public void LegadoZero() { using var t = new TelaTeste(new[] { ConversorBibliotecaJson.ParaRegistro(ApoioMultimidia.Item(TipoMidia.Livro, new() { Pagina = 0 })) }); t.Campo<ComboBox>("CmbTipoBiblioteca").SelectedIndex = (int)TipoMidia.Livro; t.Editar(); t.Campo<TextBox>("TxtReview").Text = "Atualizada"; t.Clicar("BtnAtualizar"); var r = Assert.Single(t.Salvos); Assert.Equal("Atualizada", r.Review); Assert.Equal(0, r.Multimidia!.Progresso!.Pagina); }
    [AvaloniaFact(DisplayName = "UP10 | Limpar zero legado no formulário | Ausência substitui zero somente por escolha explícita")]
    public void Limpar() { using var t = new TelaTeste(new[] { ConversorBibliotecaJson.ParaRegistro(ApoioMultimidia.Item(TipoMidia.Livro, new() { Pagina = 0 })) }); t.Campo<ComboBox>("CmbTipoBiblioteca").SelectedIndex = (int)TipoMidia.Livro; t.Editar(); t.Campo<NumericUpDown>("NumPagina").Value = null; t.Clicar("BtnAtualizar"); Assert.Null(Assert.Single(t.Salvos).Multimidia!.Progresso); }
    [AvaloniaFact(DisplayName = "UP11 | Alterar posição positiva para zero | Posição vira um imediatamente e salva somente por confirmação")]
    public void Regredir() { using var t = new TelaTeste(new[] { ConversorBibliotecaJson.ParaRegistro(ApoioMultimidia.Item(TipoMidia.Livro, new() { Pagina = 4 })) }); var antes = File.ReadAllBytes(t.Pasta.Arquivo); t.Campo<ComboBox>("CmbTipoBiblioteca").SelectedIndex = (int)TipoMidia.Livro; t.Editar(); t.Campo<NumericUpDown>("NumPagina").Value = 0; Assert.Equal(1, t.Campo<NumericUpDown>("NumPagina").Value); Assert.Equal(antes, File.ReadAllBytes(t.Pasta.Arquivo)); t.Clicar("BtnAtualizar"); Assert.Equal(1, t.Salvos[0].Multimidia!.Progresso!.Pagina); }
    [AvaloniaFact(DisplayName = "UP12 | Ajustar posição normalizada | Cadastro funciona sem perder seleção")]
    public void Recuperar() { using var t = Nova(TipoMidia.Livro); t.Campo<NumericUpDown>("NumPagina").Value = 0; Assert.Equal(1, t.Campo<NumericUpDown>("NumPagina").Value); t.Campo<NumericUpDown>("NumPagina").Value = 2; t.Clicar("BtnAdicionar"); Assert.Equal(2, Assert.Single(t.Salvos).Multimidia!.Progresso!.Pagina); }
}
