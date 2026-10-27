using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Xunit;

namespace BacklogManager.Tests;

public sealed class FiltrosTests
{
    private static TelaTeste Criar()
    {
        var um = PastaTeste.Jogo("Persona"); um.Status = "Jogando";
        var dois = PastaTeste.Jogo("Outro"); dois.Categoria = "Estratégia";
        return new TelaTeste(new[] { um, dois });
    }
    [AvaloniaFact(DisplayName = "F01 | Filtrar título | Somente título correspondente exibido")]
    public void Titulo() { using var t = Criar(); t.Campo<TextBox>("TxtBusca").Text = "Persona"; Avalonia.Threading.Dispatcher.UIThread.RunJobs(); Assert.Equal("Persona", Assert.Single(t.Exibidos).Titulo); }
    [AvaloniaFact(DisplayName = "F02 | Título com caixa diferente | Busca ignora maiúsculas e minúsculas")]
    public void Caixa() { using var t = Criar(); t.Campo<TextBox>("TxtBusca").Text = "pErSoNa"; Avalonia.Threading.Dispatcher.UIThread.RunJobs(); Assert.Single(t.Exibidos); }
    [AvaloniaFact(DisplayName = "F03 | Filtrar categoria | Somente categoria correspondente exibida")]
    public void Categoria() { using var t = Criar(); t.Campo<TextBox>("TxtBusca").Text = "estratégia"; Avalonia.Threading.Dispatcher.UIThread.RunJobs(); Assert.Equal("Outro", Assert.Single(t.Exibidos).Titulo); }
    [AvaloniaFact(DisplayName = "F04 | Busca por trecho | Correspondência parcial preservada")]
    public void Trecho() { using var t = Criar(); t.Campo<TextBox>("TxtBusca").Text = "erso"; Avalonia.Threading.Dispatcher.UIThread.RunJobs(); Assert.Single(t.Exibidos); }
    [AvaloniaFact(DisplayName = "F05 | Filtro sem resultado | Estado vazio exibido sem excluir dados")]
    public void SemResultado() { using var t = Criar(); t.Campo<TextBox>("TxtBusca").Text = "Ausente"; Avalonia.Threading.Dispatcher.UIThread.RunJobs(); Assert.Empty(t.Exibidos); Assert.True(t.Campo<StackPanel>("EstadoListaVazia").IsVisible); Assert.Equal(2, t.Salvos.Count); }
    [AvaloniaFact(DisplayName = "F06 | Filtrar status | Apenas jogos no status escolhido")]
    public void Status() { using var t = Criar(); t.Campo<ComboBox>("CmbFiltroStatus").SelectedIndex = 2; Assert.Equal("Persona", Assert.Single(t.Exibidos).Titulo); }
    [AvaloniaFact(DisplayName = "F07 | Combinar texto e status | Ambos os critérios aplicados")]
    public void Combinados() { using var t = Criar(); t.Campo<TextBox>("TxtBusca").Text = "Outro"; t.Campo<ComboBox>("CmbFiltroStatus").SelectedIndex = 2; Avalonia.Threading.Dispatcher.UIThread.RunJobs(); Assert.Empty(t.Exibidos); }
    [AvaloniaFact(DisplayName = "F08 | Filtro ativo | Total da coleção não é alterado")]
    public void Total() { using var t = Criar(); t.Campo<ComboBox>("CmbFiltroStatus").SelectedIndex = 2; Assert.Equal("2", t.Campo<TextBlock>("TxtTotal").Text); Assert.Equal(2, t.Salvos.Count); }
    [AvaloniaFact(DisplayName = "F09 | Limpar filtros | Todos os registros reaparecem")]
    public void Limpar() { using var t = Criar(); t.Campo<ComboBox>("CmbFiltroStatus").SelectedIndex = 2; t.Campo<ComboBox>("CmbFiltroStatus").SelectedIndex = 0; Assert.Equal(2, t.Exibidos.Length); }
    [AvaloniaFact(DisplayName = "F10 | Seleção sai do filtro | Botões de editar e remover desabilitados")]
    public void Botoes() { using var t = Criar(); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 1; t.Campo<ComboBox>("CmbFiltroStatus").SelectedIndex = 2; Assert.False(t.Campo<Button>("BtnEditar").IsEnabled); Assert.False(t.Campo<Button>("BtnRemover").IsEnabled); }
}
