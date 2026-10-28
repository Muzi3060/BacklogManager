using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.VisualTree;
using BacklogManager.Controls;
using BacklogManager.Models;
using Xunit;
namespace BacklogManager.Tests.Revisao2;

public sealed class PosicaoImediataTests
{
    private static TelaTeste Tela()
    {
        var t = new TelaTeste(); t.Campo<ComboBox>("CmbTipoCadastro").SelectedIndex = (int)TipoMidia.Livro;
        t.Clicar("NavCadastro"); Dispatcher.UIThread.RunJobs(); t.Janela.UpdateLayout(); return t;
    }
    private static PosicaoNumerica Campo(TelaTeste t) => t.Campo<PosicaoNumerica>("NumPagina");
    private static TextBox Editor(TelaTeste t) => Campo(t).GetVisualDescendants().OfType<TextBox>().Single();
    private static void Tecla(TelaTeste t, Key k) { Editor(t).Focus(); t.Janela.KeyPress(k, RawInputModifiers.None, default, null); t.Janela.KeyRelease(k, RawInputModifiers.None, default, null); Dispatcher.UIThread.RunJobs(); }
    [AvaloniaFact(DisplayName = "NI01 | Configuração das posições | Mínimo um, incremento um, limite inteiro e ausência inicial")]
    public void Padrao() { using var t = Tela(); var c = Campo(t); Assert.Equal(1, c.Minimum); Assert.Equal(1, c.Increment); Assert.Equal(int.MaxValue, c.Maximum); Assert.Null(c.Value); }
    [AvaloniaFact(DisplayName = "NI02 | Campo vazio ganha foco e perde foco | Ausência não se transforma em um")]
    public void FocoVazio() { using var t = Tela(); Editor(t).Focus(); t.Campo<TextBox>("TxtReview").Focus(); Dispatcher.UIThread.RunJobs(); Assert.Null(Campo(t).Value); }
    [AvaloniaFact(DisplayName = "NI03 | Atribuição de zero | Valor vira um imediatamente")]
    public void Zero() { using var t = Tela(); Campo(t).Value = 0; Assert.Equal(1, Campo(t).Value); Assert.Equal("1", Editor(t).Text); }
    [AvaloniaFact(DisplayName = "NI04 | Atribuição de negativo | Valor vira um imediatamente")]
    public void Negativo() { using var t = Tela(); Campo(t).Value = -5; Assert.Equal(1, Campo(t).Value); }
    [AvaloniaFact(DisplayName = "NI05 | Valor acima do limite | Limite inteiro é conservado sem overflow")]
    public void Maximo() { using var t = Tela(); Campo(t).Value = (decimal)int.MaxValue + 1; Assert.Equal(int.MaxValue, Campo(t).Value); }
    [AvaloniaFact(DisplayName = "NI06 | Atribuição fracionária | Inteiro anterior é mantido sem truncar")]
    public void Fracao() { using var t = Tela(); Campo(t).Value = 3; Campo(t).Value = 1.5m; Assert.Equal(3, Campo(t).Value); }
    [AvaloniaFact(DisplayName = "NI07 | Texto inteiro válido | Texto e valor sincronizam imediatamente")]
    public void TextoValido() { using var t = Tela(); Editor(t).Text = "23"; Assert.Equal(23, Campo(t).Value); Assert.Equal("23", Editor(t).Text); }
    [AvaloniaFact(DisplayName = "NI08 | Texto zero | Editor e valor exibem um sem esperar salvar")]
    public void TextoZero() { using var t = Tela(); Editor(t).Text = "0"; Assert.Equal(1, Campo(t).Value); Assert.Equal("1", Editor(t).Text); }
    [AvaloniaFact(DisplayName = "NI09 | Texto negativo | Editor normaliza para um imediatamente")]
    public void TextoNegativo() { using var t = Tela(); Editor(t).Text = "-12"; Assert.Equal(1, Campo(t).Value); Assert.Equal("1", Editor(t).Text); }
    [AvaloniaFact(DisplayName = "NI10 | Letras digitadas | Inteiro anterior é restaurado no editor")]
    public void Letras() { using var t = Tela(); Campo(t).Value = 3; Editor(t).Text = "abc"; Assert.Equal(3, Campo(t).Value); Assert.Equal("3", Editor(t).Text); }
    [AvaloniaFact(DisplayName = "NI11 | Texto fracionário | Valor e editor restauram inteiro anterior")]
    public void TextoFracao() { using var t = Tela(); Campo(t).Value = 3; Editor(t).Text = "1.5"; Assert.Equal(3, Campo(t).Value); Assert.Equal("3", Editor(t).Text); }
    [AvaloniaFact(DisplayName = "NI12 | Limpar texto após preencher | Valor fica null sem produzir um")]
    public void Limpar() { using var t = Tela(); Campo(t).Value = 8; Editor(t).Text = ""; Assert.Null(Campo(t).Value); }
    [AvaloniaFact(DisplayName = "NI13 | Texto só de espaços | Normaliza para ausência")]
    public void Espacos() { using var t = Tela(); Campo(t).Value = 8; Editor(t).Text = " "; Assert.Null(Campo(t).Value); Assert.True(string.IsNullOrEmpty(Editor(t).Text)); }
    [AvaloniaFact(DisplayName = "NI14 | Limpar zero legado | Valor vira ausência e mínimo volta a um")]
    public void LimparLegado() { using var t = Tela(); Campo(t).CarregarValorLegado(0); Editor(t).Text = ""; Assert.Null(Campo(t).Value); Assert.Equal(1, Campo(t).Minimum); }
    [AvaloniaFact(DisplayName = "NI15 | Seta para cima no vazio | Primeira posição é um")]
    public void CimaVazio() { using var t = Tela(); Tecla(t, Key.Up); Assert.Equal(1, Campo(t).Value); }
    [AvaloniaFact(DisplayName = "NI16 | Seta para baixo no vazio | Campo permanece não informado em vez de receber máximo")]
    public void BaixoVazio() { using var t = Tela(); Tecla(t, Key.Down); Assert.Null(Campo(t).Value); }
    [AvaloniaFact(DisplayName = "NI17 | Seta para baixo em um | Nunca produz zero")]
    public void BaixoUm() { using var t = Tela(); Campo(t).Value = 1; Tecla(t, Key.Down); Assert.Equal(1, Campo(t).Value); }
    [AvaloniaFact(DisplayName = "NI18 | Seta para cima em dois | Incremento inicial continua um")]
    public void Incremento() { using var t = Tela(); Campo(t).Value = 2; Tecla(t, Key.Up); Assert.Equal(3, Campo(t).Value); }
    [AvaloniaFact(DisplayName = "NI19 | Carregar zero legado | Zero exibido sem normalização e orientação explícita")]
    public void Legado() { using var t = Tela(); Campo(t).CarregarValorLegado(0); Assert.Equal(0, Campo(t).Value); Assert.Equal("0", Editor(t).Text); Assert.Contains("legado", ToolTip.GetTip(Campo(t))!.ToString()!); }
    [AvaloniaFact(DisplayName = "NI20 | Aumentar zero legado | Passa para um e não permite retornar a zero")]
    public void AumentarLegado() { using var t = Tela(); Campo(t).CarregarValorLegado(0); Tecla(t, Key.Up); Assert.Equal(1, Campo(t).Value); Assert.Equal(1, Campo(t).Minimum); Tecla(t, Key.Down); Assert.Equal(1, Campo(t).Value); }
    [AvaloniaFact(DisplayName = "NI21 | Introduzir zero depois de limpar legado | Novo zero normaliza para um")]
    public void NovoZero() { using var t = Tela(); Campo(t).CarregarValorLegado(0); Campo(t).Value = null; Campo(t).Value = 0; Assert.Equal(1, Campo(t).Value); }
    [AvaloniaFact(DisplayName = "NI22 | Reutilizar formulário após legado | Novo campo vazio não herda permissão de zero")]
    public void Reutilizar() { using var t = Tela(); Campo(t).CarregarValorLegado(0); Campo(t).CarregarValorLegado(null); Assert.Null(Campo(t).Value); Campo(t).Value = 0; Assert.Equal(1, Campo(t).Value); }
    [AvaloniaFact(DisplayName = "NI23 | Digitação real de zero | Texto visível é um imediatamente")]
    public void TecladoZero() { using var t = Tela(); Editor(t).Focus(); t.Janela.KeyTextInput("0"); Assert.Equal(1, Campo(t).Value); Assert.Equal("1", Editor(t).Text); }
    [AvaloniaFact(DisplayName = "NI24 | Apagar seleção com teclado | Mantém ausência em vez de impor valor mínimo")]
    public void ApagarTeclado() { using var t = Tela(); Campo(t).Value = 8; Editor(t).Focus(); Editor(t).SelectAll(); t.Janela.KeyPress(Key.Back, RawInputModifiers.None, default, null); t.Janela.KeyRelease(Key.Back, RawInputModifiers.None, default, null); Assert.Null(Campo(t).Value); }
    [AvaloniaFact(DisplayName = "NI25 | Texto inválido em campo vazio | Permanece vazio sem inventar posição")]
    public void LetrasVazio() { using var t = Tela(); Editor(t).Text = "inválido"; Assert.Null(Campo(t).Value); Assert.True(string.IsNullOrEmpty(Editor(t).Text)); }
    [AvaloniaFact(DisplayName = "NI26 | Separador decimal brasileiro | Fração não vira página truncada")]
    public void Virgula() { using var t = Tela(); Campo(t).NumberFormat = System.Globalization.CultureInfo.GetCultureInfo("pt-BR").NumberFormat; Campo(t).Value = 5; Editor(t).Text = "2,5"; Assert.Equal(5, Campo(t).Value); Assert.Equal("5", Editor(t).Text); }
    [AvaloniaFact(DisplayName = "NI27 | Posições das duas mídias | Quatro campos usam o controle específico com incremento um")]
    public void QuatroCampos() { using var t = Tela(); foreach (var nome in new[] { "NumPagina", "NumCapituloLivro", "NumCapituloManga", "NumVolume" }) { var c = t.Campo<PosicaoNumerica>(nome); Assert.Equal(1, c.Minimum); c.Value = 0; Assert.Equal(1, c.Value); Assert.Equal(1, c.Increment); } }
    [AvaloniaFact(DisplayName = "NI28 | Nota pessoal | Escala zero a cinco e frações permanecem inalteradas")]
    public void Nota() { using var t = Tela(); var n = t.Campo<NumericUpDown>("NumNota"); Assert.IsNotType<PosicaoNumerica>(n); Assert.Equal(0, n.Minimum); Assert.Equal(5, n.Maximum); n.Value = 0; Assert.Equal(0, n.Value); n.Value = 4.5m; Assert.Equal(4.5m, n.Value); }
    [AvaloniaFact(DisplayName = "NI29 | Episódios assistidos | Contagem zero continua permitida")]
    public void Episodios() { using var t = Tela(); var n = t.Campo<NumericUpDown>("NumEpisodiosAssistidos"); Assert.IsNotType<PosicaoNumerica>(n); n.Value = 0; Assert.Equal(0, n.Value); }
    [AvaloniaFact(DisplayName = "NI30 | Carregar formulário legado sem salvar | Nenhum JSON é regravado automaticamente")]
    public void SemGravacao() { var i = Fase2.ApoioMultimidia.Item(TipoMidia.Livro, new() { Pagina = 0 }); using var t = new TelaTeste(new[] { BacklogManager.Data.ConversorBibliotecaJson.ParaRegistro(i) }); var bytes = File.ReadAllBytes(t.Pasta.Arquivo); t.Campo<ComboBox>("CmbTipoBiblioteca").SelectedIndex = (int)TipoMidia.Livro; t.Editar(); Assert.Equal(0, t.Campo<PosicaoNumerica>("NumPagina").Value); Assert.Equal(bytes, File.ReadAllBytes(t.Pasta.Arquivo)); }
    private static Button Seta(TelaTeste t, string nome) => Campo(t).GetVisualDescendants().OfType<Button>().Single(b => b.Name == nome);
    [AvaloniaFact(DisplayName = "NI31 | Botão de diminuir em campo vazio | Clique não preenche com o máximo nem com zero")]
    public void BotaoBaixoVazio() { using var t = Tela(); Seta(t, "PART_DecreaseButton").RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Button.ClickEvent)); Assert.Null(Campo(t).Value); }
    [AvaloniaFact(DisplayName = "NI32 | Botão de aumentar e limite inferior | Clique cria um e diminuir não produz zero")]
    public void BotaoCima() { using var t = Tela(); Seta(t, "PART_IncreaseButton").RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Button.ClickEvent)); Assert.Equal(1, Campo(t).Value); Seta(t, "PART_DecreaseButton").RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Button.ClickEvent)); Assert.Equal(1, Campo(t).Value); }
}
