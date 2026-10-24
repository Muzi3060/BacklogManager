using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BacklogManager.Data;
using BacklogManager.Models;

namespace BacklogManager;

public partial class MainWindow : Window
{
    private BacklogItem? _itemEmEdicao;
    private readonly ObservableCollection<BacklogItem> _itens;
    private bool _interfacePronta;

    public MainWindow()
    {
        InitializeComponent();
        _itens = new ObservableCollection<BacklogItem>(FileManager.Carregar());
        _interfacePronta = true;
        AtualizarColecao();
    }

    private void Navegar_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: string pagina })
            MostrarPagina(pagina);
    }

    private void MostrarPagina(string pagina)
    {
        PaginaInicio.IsVisible = pagina == "inicio";
        PaginaBacklog.IsVisible = pagina == "backlog";
        PaginaCadastro.IsVisible = pagina == "cadastro";
        NavInicio.Classes.Set("active", pagina == "inicio");
        NavBacklog.Classes.Set("active", pagina == "backlog");
        NavCadastro.Classes.Set("active", pagina == "cadastro");
    }

    private void NovoJogo_Click(object? sender, RoutedEventArgs e)
    {
        LimparFormulario();
        MostrarPagina("cadastro");
        TxtTitulo.Focus();
    }

    private void LimparFormulario_Click(object? sender, RoutedEventArgs e)
    {
        LimparFormulario();
        TxtTitulo.Focus();
    }

    private void LimparFormulario()
    {
        _itemEmEdicao = null;
        ListaBacklog.SelectedItem = null;
        TxtTitulo.Text = string.Empty;
        CmbCategoria.SelectedIndex = 0;
        CmbStatus.SelectedIndex = 0;
        NumNota.Value = 5.0m;
        TxtReview.Text = string.Empty;
        AtualizarModoFormulario();
    }

    private void AtualizarModoFormulario()
    {
        var editando = _itemEmEdicao != null;
        TxtTituloCadastro.Text = editando ? "Editar jogo" : "Adicionar jogo";
        TxtSubtituloCadastro.Text = editando
            ? "Atualize os detalhes e registre seu progresso."
            : "Uma nova aventura para a sua coleção.";
        BtnAdicionar.IsVisible = !editando;
        BtnAtualizar.IsVisible = editando;
    }

    private BacklogItem? LerFormulario()
    {
        var titulo = TxtTitulo.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(titulo))
        {
            ExibirAviso("Confira os dados", "Informe o título do jogo para continuar.", true);
            TxtTitulo.Focus();
            return null;
        }

        var categoria = (CmbCategoria.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Outro";
        var status = (CmbStatus.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Na Fila";
        var review = TxtReview.Text?.Trim();
        return new BacklogItem(titulo, categoria, status, (double)(NumNota.Value ?? 0),
            string.IsNullOrWhiteSpace(review) ? "Nenhuma Review" : review);
    }

    private void BtnAdicionar_Click(object? sender, RoutedEventArgs e)
    {
        var novoItem = LerFormulario();
        if (novoItem == null) return;

        _itens.Add(novoItem);
        FileManager.Salvar(_itens);
        LimparFormulario();
        AtualizarColecao();
        ExibirAviso("Jogo adicionado", $"“{novoItem.Titulo}” foi adicionado à sua coleção.");
    }

    private void BtnAtualizar_Click(object? sender, RoutedEventArgs e)
    {
        if (_itemEmEdicao == null) return;
        var atualizado = LerFormulario();
        if (atualizado == null) return;

        var index = _itens.IndexOf(_itemEmEdicao);
        if (index < 0) return;
        _itens[index] = atualizado;
        FileManager.Salvar(_itens);
        LimparFormulario();
        AtualizarColecao();
        ExibirAviso("Jogo atualizado", $"As alterações de “{atualizado.Titulo}” foram salvas com sucesso.");
    }

    private void ExibirAviso(string titulo, string mensagem, bool erro = false)
    {
        TxtAvisoTitulo.Text = titulo;
        TxtAvisoMensagem.Text = mensagem;
        TxtIconeAviso.Text = erro ? "!" : "✓";
        AvisoOperacao.Classes.Set("erro", erro);
        AvisoOperacao.IsVisible = true;
    }

    private void FecharAviso_Click(object? sender, RoutedEventArgs e)
    {
        AvisoOperacao.IsVisible = false;
    }

    private void BtnRemover_Click(object? sender, RoutedEventArgs e)
    {
        if (ListaBacklog.SelectedItem is not BacklogItem item) return;

        _itens.Remove(item);
        FileManager.Salvar(_itens);
        if (_itemEmEdicao == item)
            LimparFormulario();
        AtualizarColecao();
        ExibirAviso("Jogo removido", $"“{item.Titulo}” foi removido da coleção.");
    }

    private void ListaBacklog_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (!_interfacePronta) return;
        var selecionado = ListaBacklog.SelectedItem is BacklogItem;
        BtnEditar.IsEnabled = selecionado;
        BtnRemover.IsEnabled = selecionado;
    }

    private void EditarSelecionado_Click(object? sender, RoutedEventArgs e)
    {
        if (ListaBacklog.SelectedItem is BacklogItem item)
            EditarJogo(item);
    }

    private void Recente_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: BacklogItem item })
            EditarJogo(item);
    }

    private void EditarJogo(BacklogItem item)
    {
        _itemEmEdicao = item;
        TxtTitulo.Text = item.Titulo;
        SelecionarValor(CmbCategoria, item.Categoria);
        SelecionarValor(CmbStatus, item.Status);
        NumNota.Value = (decimal)item.Nota;
        TxtReview.Text = item.Review;
        AtualizarModoFormulario();
        MostrarPagina("cadastro");
    }

    private static void SelecionarValor(ComboBox comboBox, string valor)
    {
        foreach (var opcao in comboBox.Items.OfType<ComboBoxItem>())
        {
            if (opcao.Content?.ToString() != valor) continue;
            comboBox.SelectedItem = opcao;
            return;
        }

        // Mantém categorias e status já existentes em arquivos de versões anteriores.
        var existente = new ComboBoxItem { Content = valor };
        comboBox.Items.Add(existente);
        comboBox.SelectedItem = existente;
    }

    private void Filtros_Changed(object? sender, RoutedEventArgs e)
    {
        if (_interfacePronta)
            AtualizarLista();
    }

    private void AtualizarColecao()
    {
        var total = _itens.Count;
        var zerados = _itens.Count(item => item.Status == "Zerado");
        var naFila = _itens.Count(item => item.Status == "Na Fila");
        var progresso = total == 0 ? 0 : (double)zerados / total * 100;

        TxtTotal.Text = total.ToString();
        TxtJogando.Text = _itens.Count(item => item.Status == "Jogando").ToString();
        TxtZerados.Text = zerados.ToString();
        TxtContagemNav.Text = total.ToString();
        TxtProgresso.Text = $"{progresso:0}% concluído";
        ProgressoBacklog.Value = progresso;
        TxtResumo.Text = total == 0
            ? "Adicione seu primeiro jogo e comece a acompanhar seu progresso."
            : $"{zerados} de {total} jogos zerados · {naFila} na fila para a próxima aventura.";
        ListaRecentes.ItemsSource = _itens.Reverse().Take(3).ToArray();
        EstadoInicioVazio.IsVisible = total == 0;
        AtualizarLista();
    }

    private void AtualizarLista()
    {
        var busca = TxtBusca.Text?.Trim() ?? string.Empty;
        var status = CmbFiltroStatus.SelectedIndex > 0
            ? (CmbFiltroStatus.SelectedItem as ComboBoxItem)?.Content?.ToString()
            : null;
        var selecionado = ListaBacklog.SelectedItem as BacklogItem;
        var filtrados = _itens.Where(item =>
            (status == null || item.Status == status) &&
            (item.Titulo.Contains(busca, StringComparison.OrdinalIgnoreCase) ||
             item.Categoria.Contains(busca, StringComparison.OrdinalIgnoreCase))).ToArray();

        ListaBacklog.ItemsSource = filtrados;
        if (selecionado != null && filtrados.Contains(selecionado))
            ListaBacklog.SelectedItem = selecionado;
        BtnEditar.IsEnabled = ListaBacklog.SelectedItem is BacklogItem;
        BtnRemover.IsEnabled = BtnEditar.IsEnabled;
        TxtContagemLista.Text = $"{filtrados.Length} de {_itens.Count} jogos · Selecione para editar";
        EstadoListaVazia.IsVisible = filtrados.Length == 0;
        TxtListaVazia.Text = _itens.Count == 0 ? "Sua coleção ainda está vazia" : "Nenhum jogo encontrado";
        TxtDicaListaVazia.Text = _itens.Count == 0
            ? "Use “Adicionar jogo” para começar seu backlog."
            : "Tente outro título, categoria ou status.";
    }
}
