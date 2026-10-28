using System;
using System.Linq;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BacklogManager.Data;
using BacklogManager.Contracts;
using BacklogManager.Configuracao;
using BacklogManager.Models;
using BacklogManager.Services;
using System.Threading;

namespace BacklogManager;

public partial class MainWindow : Window
{
    private ItemBiblioteca? _itemEmEdicao;
    private readonly ServicoBiblioteca _biblioteca;
    private bool _interfacePronta;
    private ObraCatalogo? _jogoSelecionado;
    private string? _textoNaSelecao;
    private CancellationTokenSource? _buscaCancellation;
    private readonly IPesquisaCatalogo? _pesquisa;
    private readonly IDisposable? _recursoPesquisa;
    private bool PersistenciaDisponivel => _biblioteca.Carregada;
    private bool _fechada;
    internal Task BuscaPendente { get; private set; } = Task.CompletedTask;

    public MainWindow() : this(ComposicaoAplicativo.Criar()) { }

    private MainWindow(DependenciasJanela dependencias)
        : this(dependencias.Biblioteca, dependencias.Pesquisa, dependencias.RecursoPesquisa) { }

    internal MainWindow(ServicoBiblioteca biblioteca, IPesquisaCatalogo? pesquisa, IDisposable? recursoPesquisa = null)
    {
        _biblioteca = biblioteca;
        _pesquisa = pesquisa;
        _recursoPesquisa = recursoPesquisa;
        InitializeComponent();
        try
        {
            _biblioteca.Carregar();
        }
        catch (Exception erro) when (FalhaDeArquivo(erro))
        {
            ExibirAviso("Coleção não carregada", $"O original foi preservado. Gravação bloqueada. Arquivo: {_biblioteca.DescricaoOrigem}", true);
        }
        if (PersistenciaDisponivel && _pesquisa == null)
            ExibirAviso("Pesquisa indisponível", "Configure IGDB_CLIENT_ID e IGDB_ACESS_TOKEN (ou IGDB_ACCESS_TOKEN). Sua coleção local continua disponível.", true);

        _interfacePronta = true;
        AtualizarColecao();
        BtnAdicionar.IsEnabled = PersistenciaDisponivel;
        BtnAtualizar.IsEnabled = PersistenciaDisponivel;
        if (!PersistenciaDisponivel)
        {
            TxtListaVazia.Text = "Não foi possível carregar a coleção";
            TxtDicaListaVazia.Text = "O arquivo original foi preservado. Verifique o aviso antes de reabrir o aplicativo.";
        }
    }

    private static bool FalhaDeArquivo(Exception erro) => erro is IOException or InvalidDataException or
        UnauthorizedAccessException or SecurityException or InvalidOperationException or System.Text.Json.JsonException or NotSupportedException;

    private bool TentarExecutar(Action operacao)
    {
        if (!PersistenciaDisponivel)
        {
            ExibirAviso("Gravação bloqueada", "A coleção não foi carregada. O arquivo original será preservado.", true);
            return false;
        }
        try
        {
            operacao();
            return true;
        }
        catch (Exception erro) when (FalhaDeArquivo(erro))
        {
            var orientacao = erro switch
            {
                ArquivoAlteradoException => "O arquivo mudou fora desta janela. Reabra o aplicativo antes de salvar.",
                InvalidDataException => "A coleção contém dados inválidos. Revise os campos antes de tentar novamente.",
                UnauthorizedAccessException or SecurityException => "Confira a permissão de escrita no arquivo da coleção.",
                _ => "Confira o acesso ao arquivo e o espaço disponível em disco antes de tentar novamente."
            };
            ExibirAviso("Não foi possível salvar", $"As alterações não foram aplicadas à coleção. {orientacao}", true);
            return false;
        }
    }

    private void CancelarBusca()
    {
        var anterior = _buscaCancellation;
        _buscaCancellation = null;
        anterior?.Cancel();
        // O método assíncrono descarta sua própria fonte no finally.
    }

    protected override void OnClosed(EventArgs e)
    {
        _fechada = true;
        CancelarBusca();
        _recursoPesquisa?.Dispose();
        base.OnClosed(e);
    }

    private void Navegar_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: string pagina })
            MostrarPagina(pagina);
    }

    private void MostrarPagina(string pagina)
    {
        if (pagina != "cadastro")
        {
            CancelarBusca();
            ListaSugestoes.IsVisible = false;
        }
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
        CancelarBusca();
        _itemEmEdicao = null;
        _jogoSelecionado = null;
        ListaSugestoes.ItemsSource = null;
        ListaBacklog.SelectedItem = null;
        TxtTitulo.Text = string.Empty;
        CmbCategoria.SelectedIndex = 0;
        CmbStatus.SelectedIndex = 0;
        NumNota.Value = 5.0m;
        TxtReview.Text = string.Empty;
        TxtReview.MaxLength = 1000;
        AtualizarModoFormulario();
    }

    private void AtualizarModoFormulario()
    {
        var editando = _itemEmEdicao != null;

        TxtTitulo.IsReadOnly = editando;


        TxtTituloCadastro.Text = editando ? "Editar jogo" : "Adicionar jogo";
        TxtSubtituloCadastro.Text = editando
            ? "Atualize os detalhes e registre seu progresso."
            : "Uma nova aventura para a sua coleção.";
        BtnAdicionar.IsVisible = !editando;
        BtnAtualizar.IsVisible = editando;
    }

    private ItemBiblioteca? LerFormulario()
    {
        var dados = new DadosFormularioJogo(TxtTitulo.Text,
            (CmbCategoria.SelectedItem as ComboBoxItem)?.Content?.ToString(),
            (CmbStatus.SelectedItem as ComboBoxItem)?.Content?.ToString(), NumNota.Value, TxtReview.Text);
        try
        {
            return FormularioJogo.Preparar(dados, _jogoSelecionado, _textoNaSelecao, _itemEmEdicao);
        }
        catch (ValidacaoFormularioException erro)
        {
            ExibirAviso(erro.Titulo, erro.Message, true);
            return null;
        }
    }

    private void BtnAdicionar_Click(object? sender, RoutedEventArgs e)
    {

        if (_jogoSelecionado == null)
        {
            ExibirAviso("Selecione um jogo", "Escolha um jogo antes de adicionar à sua coleção.", true);
            return;
        }

        var novoItem = LerFormulario();
        if (novoItem == null) return;

        if (!TentarExecutar(() => _biblioteca.Adicionar(novoItem))) return;
        LimparFormulario();
        AtualizarColecao();
        ExibirAviso("Jogo adicionado", $"“{novoItem.Catalogo.Titulo}” foi adicionado à sua coleção.");
    }

    private void BtnAtualizar_Click(object? sender, RoutedEventArgs e)
    {

        if (_itemEmEdicao == null) return;
        var atualizado = LerFormulario();
        if (atualizado == null) return;

        if (!TentarExecutar(() => _biblioteca.Atualizar(_itemEmEdicao, atualizado.Pessoal))) return;
        LimparFormulario();
        AtualizarColecao();
        ExibirAviso("Jogo atualizado", $"As alterações de “{atualizado.Catalogo.Titulo}” foram salvas com sucesso.");
    }

    private void ExibirAviso(string titulo, string mensagem, bool erro = false)
    {
        TxtAvisoTitulo.Text = titulo;
        TxtAvisoMensagem.Text = mensagem;
        ToolTip.SetTip(AvisoOperacao, mensagem);
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
        if (ListaBacklog.SelectedItem is not ItemBiblioteca item) return;

        if (!TentarExecutar(() => _biblioteca.Remover(item))) return;
        if (ReferenceEquals(_itemEmEdicao, item))
            LimparFormulario();
        AtualizarColecao();
        ExibirAviso("Jogo removido", $"“{item.Catalogo.Titulo}” foi removido da coleção.");
    }

    private void ListaBacklog_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (!_interfacePronta) return;
        var selecionado = ListaBacklog.SelectedItem is ItemBiblioteca;
        BtnEditar.IsEnabled = selecionado;
        BtnRemover.IsEnabled = selecionado && PersistenciaDisponivel;
    }

    private void EditarSelecionado_Click(object? sender, RoutedEventArgs e)
    {
        if (ListaBacklog.SelectedItem is ItemBiblioteca item)
            EditarJogo(item);
    }

    private void Recente_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: ItemBiblioteca item })
            EditarJogo(item);
    }

    private void EditarJogo(ItemBiblioteca item)
    {
        CancelarBusca();
        ListaSugestoes.IsVisible = false;
        _jogoSelecionado = null;
        _itemEmEdicao = item;
        TxtTitulo.Text = item.Catalogo.Titulo;
        SelecionarValor(CmbCategoria, item.Pessoal.Categoria);
        SelecionarValor(CmbStatus, item.Pessoal.Status);
        NumNota.Value = (decimal)item.Pessoal.Nota;
        TxtReview.MaxLength = Math.Max(1000, item.Pessoal.Review.Length);
        TxtReview.Text = item.Pessoal.Review;
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
        var resumo = _biblioteca.ObterResumo();
        var total = resumo.Total;
        var zerados = resumo.Zerados;
        var naFila = resumo.NaFila;
        var progresso = resumo.Progresso;

        TxtTotal.Text = total.ToString();
        TxtJogando.Text = resumo.Jogando.ToString();
        TxtZerados.Text = zerados.ToString();
        TxtContagemNav.Text = total.ToString();
        TxtProgresso.Text = $"{progresso:0}% concluído";
        ProgressoBacklog.Value = progresso;
        TxtResumo.Text = total == 0
            ? "Adicione seu primeiro jogo e comece a acompanhar seu progresso."
            : $"{zerados} de {total} jogos zerados · {naFila} na fila para a próxima aventura.";
        ListaRecentes.ItemsSource = _biblioteca.ObterRecentes();
        EstadoInicioVazio.IsVisible = total == 0;
        AtualizarLista();
    }

    private void AtualizarLista()
    {
        var busca = TxtBusca.Text?.Trim() ?? string.Empty;
        var status = CmbFiltroStatus.SelectedIndex > 0
            ? (CmbFiltroStatus.SelectedItem as ComboBoxItem)?.Content?.ToString()
            : null;
        var selecionado = ListaBacklog.SelectedItem as ItemBiblioteca;
        var filtrados = _biblioteca.Filtrar(busca, status);

        ListaBacklog.ItemsSource = filtrados;
        // Records iguais podem representar registros pessoais diferentes.
        // Restaurar pelo índice da instância evita que o ListBox escolha a primeira cópia igual.
        var indiceSelecionado = -1;
        for (var i = 0; i < filtrados.Count; i++)
            if (ReferenceEquals(filtrados[i], selecionado)) { indiceSelecionado = i; break; }
        ListaBacklog.SelectedIndex = indiceSelecionado;
        BtnEditar.IsEnabled = ListaBacklog.SelectedItem is ItemBiblioteca;
        BtnRemover.IsEnabled = BtnEditar.IsEnabled && PersistenciaDisponivel;
        TxtContagemLista.Text = $"{filtrados.Count} de {_biblioteca.Itens.Count} jogos · Selecione para editar";
        EstadoListaVazia.IsVisible = filtrados.Count == 0;
        TxtListaVazia.Text = _biblioteca.Itens.Count == 0 ? "Sua coleção ainda está vazia" : "Nenhum jogo encontrado";
        TxtDicaListaVazia.Text = _biblioteca.Itens.Count == 0
            ? "Use “Adicionar jogo” para começar seu backlog."
            : "Tente outro título, categoria ou status.";
    }

    private void ListaSugestoes_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ListaSugestoes.SelectedItem is ObraCatalogo jogo && FormularioJogo.SelecaoValida(jogo))
        {
            CancelarBusca();
            _jogoSelecionado = jogo;
            _textoNaSelecao = TxtTitulo.Text;
            ListaSugestoes.IsVisible = false;
        }
        else
        {
            _jogoSelecionado = null;
        }
    }

    private async void TxtTitulo_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (!_interfacePronta || _fechada) return;
        BuscaPendente = BuscarSugestoesAsync();
        await BuscaPendente;
    }

    private async Task BuscarSugestoesAsync()
    {
        CancelarBusca();
        _jogoSelecionado = null;
        ListaSugestoes.ItemsSource = null;
        ListaSugestoes.IsVisible = false;
        var termo = TxtTitulo.Text?.Trim() ?? string.Empty;
        if (_itemEmEdicao != null || termo.Length == 0) return;
        if (_pesquisa == null)
        {
            ExibirAviso("Pesquisa indisponível", "Configure as credenciais locais da IGDB para pesquisar. Sua coleção continua disponível.", true);
            return;
        }

        var buscaAtual = new CancellationTokenSource();
        _buscaCancellation = buscaAtual;
        bool Atual() => !_fechada && _itemEmEdicao == null && TxtTitulo.Text?.Trim() == termo &&
            ReferenceEquals(_buscaCancellation, buscaAtual) && !buscaAtual.IsCancellationRequested;
        try
        {
            await Task.Delay(400, buscaAtual.Token);
            var resultado = await _pesquisa.BuscarAsync(termo, buscaAtual.Token);
            if (!Atual()) return;
            ListaSugestoes.ItemsSource = resultado;
            ListaSugestoes.IsVisible = resultado.Count > 0;
            if (resultado.Count == 0)
                ExibirAviso("Nenhum resultado", "A pesquisa foi concluída, mas nenhum jogo foi encontrado.");
            else if (TxtAvisoTitulo.Text is "Falha na pesquisa" or "Nenhum resultado" or "Pesquisa indisponível")
                AvisoOperacao.IsVisible = false;
        }
        catch (OperationCanceledException) when (buscaAtual.IsCancellationRequested)
        {
            // Cancelar uma busca substituída é esperado, sem aviso de erro.
        }
        catch (PesquisaIgdbException erro)
        {
            if (Atual()) ExibirAviso("Falha na pesquisa", erro.Message, true);
        }
        catch (Exception)
        {
            // Última barreira do evento async void: falhas inesperadas não encerram a janela.
            // Não exibir exceções brutas, que podem conter detalhes de requisições.
            if (Atual()) ExibirAviso("Falha na pesquisa", "Não foi possível concluir a pesquisa. Tente novamente.", true);
        }
        finally
        {
            if (ReferenceEquals(_buscaCancellation, buscaAtual)) _buscaCancellation = null;
            buscaAtual.Dispose();
        }
    }
}
