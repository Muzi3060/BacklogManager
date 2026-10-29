using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using BacklogManager.Models;
using BacklogManager.Services;

namespace BacklogManager;

public partial class MainWindow
{
    private bool _ajustandoFiltros;
    private bool _carregandoVisualizacao;

    private void CarregarVisualizacao()
    {
        _carregandoVisualizacao = true;
        try { CmbVisualizacao.SelectedIndex = _backup?.Pendencia != null ? 0 : (int)_preferencias.Carregar(); }
        catch (Exception erro) when (FalhaDeArquivo(erro))
        {
            CmbVisualizacao.SelectedIndex = 0;
            ExibirAviso("Preferência não carregada", "Usando grade nesta sessão. A biblioteca continua disponível; alternar a visualização substituirá a preferência inválida.", true);
        }
        finally { _carregandoVisualizacao = false; }
        AplicarVisualizacao();
    }

    private void Visualizacao_Changed(object? sender, SelectionChangedEventArgs e)
    {
        if (!_interfacePronta || _carregandoVisualizacao) return;
        AplicarVisualizacao();
        try { _preferencias.Salvar((VisualizacaoBiblioteca)CmbVisualizacao.SelectedIndex); }
        catch (Exception erro) when (FalhaDeArquivo(erro))
        {
            ExibirAviso("Preferência não salva", "A visualização foi alterada nesta sessão. Confira a permissão da pasta para guardá-la ao reiniciar. Sua biblioteca permanece disponível.", true);
        }
    }

    private void AplicarVisualizacao()
    {
        var grade = CmbVisualizacao.SelectedIndex == 0;
        ListaBacklog.ItemTemplate = (IDataTemplate)Resources[grade ? "TemplateGrade" : "TemplateLista"]!;
        ListaBacklog.ItemsPanel = new FuncTemplate<Panel?>(() => grade
            ? new WrapPanel() : new VirtualizingStackPanel());
    }

    private ConsultaBiblioteca CriarConsulta(string busca, string? status) => new()
    {
        Termo = busca,
        Status = status,
        Tipo = TipoBiblioteca,
        Genero = CmbGenero.SelectedIndex > 0 ? CmbGenero.SelectedItem as string : null,
        Plataforma = CmbPlataforma.IsEnabled && CmbPlataforma.SelectedIndex > 0 ? CmbPlataforma.SelectedItem as string : null,
        NotaMinima = NumNotaMinima.Value is { } nota ? (double)nota : null,
        AdicionadaDe = DataAdicaoDe.SelectedDate is { } de ? DateOnly.FromDateTime(de) : null,
        AdicionadaAte = DataAdicaoAte.SelectedDate is { } ate ? DateOnly.FromDateTime(ate) : null,
        Ordenacao = (OrdenacaoBiblioteca)Math.Max(0, CmbOrdenacao.SelectedIndex)
    };

    private void AtualizarOpcoesFiltros()
    {
        _ajustandoFiltros = true;
        try
        {
            var itens = _biblioteca.Filtrar(null, null, TipoBiblioteca);
            PreencherFiltro(CmbGenero, itens.SelectMany(ServicoBiblioteca.GenerosParaFiltro), "Todos os gêneros");
            var plataformas = itens.SelectMany(i => i.Catalogo.Plataformas).ToArray();
            PreencherFiltro(CmbPlataforma, plataformas, "Todas as plataformas");
            CmbPlataforma.IsEnabled = TipoBiblioteca == TipoMidia.Jogo && plataformas.Length > 0;
        }
        finally { _ajustandoFiltros = false; }
    }

    private static void PreencherFiltro(ComboBox combo, System.Collections.Generic.IEnumerable<string> valores, string todos)
    {
        var anterior = combo.SelectedItem as string;
        var opcoes = new[] { todos }.Concat(valores.Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(v => v, StringComparer.CurrentCultureIgnoreCase)).ToArray();
        combo.ItemsSource = opcoes;
        combo.SelectedIndex = Math.Max(0, Array.FindIndex(opcoes, v => v == anterior));
    }

    private void NotaFiltro_Changed(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        if (_interfacePronta && !_ajustandoFiltros) AtualizarLista();
    }

    private void DataFiltro_Changed(object? sender, SelectionChangedEventArgs e)
    {
        if (_interfacePronta && !_ajustandoFiltros) AtualizarLista();
    }

    private void LimparFiltros_Click(object? sender, RoutedEventArgs e)
    {
        _ajustandoFiltros = true;
        try
        {
            TxtBusca.Text = string.Empty;
            CmbFiltroStatus.SelectedIndex = CmbGenero.SelectedIndex = CmbPlataforma.SelectedIndex = CmbOrdenacao.SelectedIndex = 0;
            NumNotaMinima.Value = null;
            DataAdicaoDe.SelectedDate = DataAdicaoAte.SelectedDate = null;
        }
        finally { _ajustandoFiltros = false; }
        AtualizarLista();
    }
}
