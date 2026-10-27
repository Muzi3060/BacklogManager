using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Services;
using System.Threading;

namespace BacklogManager;

public partial class MainWindow : Window
{
    private BacklogItem? _itemEmEdicao;
    private readonly ObservableCollection<BacklogItem> _itens;
    private bool _interfacePronta;
    private IgdbGame? _jogoSelecionado;
    private string? _textoNaSelecao;
    private CancellationTokenSource? _buscaCancellation;
    private readonly IgdbService? _igdbService;
    private readonly FileManager _arquivos;
    private readonly HttpClient? _clienteHttpProprio;
    private bool _persistenciaDisponivel;
    private bool _fechada;
    internal Task BuscaPendente { get; private set; } = Task.CompletedTask;

    public MainWindow() : this(new FileManager(), null, usarConfiguracaoLocal: true) { }

    // Permite testar a janela com arquivos temporários e HTTP simulado.
    internal MainWindow(FileManager arquivos, IgdbService? servico, bool usarConfiguracaoLocal = false)
    {
        _arquivos = arquivos;
        _itens = new ObservableCollection<BacklogItem>();
        InitializeComponent();
        try
        {
            foreach (var item in _arquivos.Carregar()) _itens.Add(item);
            _persistenciaDisponivel = true;
        }
        catch (Exception erro) when (FalhaDeArquivo(erro))
        {
            ExibirAviso("Coleção não carregada", $"O original foi preservado. Gravação bloqueada. Arquivo: {_arquivos.CaminhoArquivo}", true);
        }

        _igdbService = servico;
        if (usarConfiguracaoLocal && ConfiguracaoIgdb.TentarLer(out var clientId, out var accessToken))
        {
            _clienteHttpProprio = new HttpClient(new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(5)
            })
            { Timeout = TimeSpan.FromSeconds(15) };
            _igdbService = new IgdbService(_clienteHttpProprio, clientId, accessToken);
        }
        if (_persistenciaDisponivel && _igdbService == null)
            ExibirAviso("Pesquisa indisponível", "Configure IGDB_CLIENT_ID e IGDB_ACESS_TOKEN (ou IGDB_ACCESS_TOKEN). Sua coleção local continua disponível.", true);

        _interfacePronta = true;
        AtualizarColecao();
        BtnAdicionar.IsEnabled = _persistenciaDisponivel;
        BtnAtualizar.IsEnabled = _persistenciaDisponivel;
        if (!_persistenciaDisponivel)
        {
            TxtListaVazia.Text = "Não foi possível carregar a coleção";
            TxtDicaListaVazia.Text = "O arquivo original foi preservado. Verifique o aviso antes de reabrir o aplicativo.";
        }
    }

    private static bool FalhaDeArquivo(Exception erro) => erro is IOException or InvalidDataException or
        UnauthorizedAccessException or SecurityException or InvalidOperationException or System.Text.Json.JsonException;

    private bool TentarSalvar(System.Collections.Generic.List<BacklogItem> candidatos)
    {
        if (!_persistenciaDisponivel)
        {
            ExibirAviso("Gravação bloqueada", "A coleção não foi carregada. O arquivo original será preservado.", true);
            return false;
        }
        try
        {
            _arquivos.Salvar(candidatos);
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
        _clienteHttpProprio?.Dispose();
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

    private BacklogItem? LerFormulario()
    {
        if (_itemEmEdicao == null && (_jogoSelecionado == null || _jogoSelecionado.Id <= 0 ||
            string.IsNullOrWhiteSpace(_jogoSelecionado.Name) || TxtTitulo.Text != _textoNaSelecao))
        {
            ExibirAviso("Selecione um jogo", "Escolha uma sugestão válida antes de adicionar à coleção.", true);
            return null;
        }
        if (NumNota.Value is not decimal nota || nota < 0 || nota > 5)
        {
            ExibirAviso("Nota inválida", "Informe uma nota entre 0 e 5.", true);
            return null;
        }
        var categoria = (CmbCategoria.SelectedItem as ComboBoxItem)?.Content?.ToString();
        var status = (CmbStatus.SelectedItem as ComboBoxItem)?.Content?.ToString();
        if (string.IsNullOrWhiteSpace(categoria) || string.IsNullOrWhiteSpace(status))
        {
            ExibirAviso("Dados incompletos", "Selecione categoria e status antes de salvar.", true);
            return null;
        }
        var textoReview = TxtReview.Text;
        var reviewInalterada = _itemEmEdicao != null && textoReview == _itemEmEdicao.Review;
        var review = reviewInalterada ? textoReview : textoReview?.Trim();
        if (review?.Length > 1000 && review != _itemEmEdicao?.Review)
        {
            ExibirAviso("Review muito longa", "Use até 1000 caracteres na review.", true);
            return null;
        }
        return new BacklogItem(_itemEmEdicao?.Titulo ?? _jogoSelecionado!.Name, categoria, status,
            (double)nota, reviewInalterada ? review! : string.IsNullOrWhiteSpace(review) ? "Nenhuma Review" : review,
            _itemEmEdicao?.SteamAppId)
        {
            IgdbId = _itemEmEdicao?.IgdbId ?? (_itemEmEdicao == null ? _jogoSelecionado?.Id : null),
            DadosAdicionais = _itemEmEdicao?.DadosAdicionais
        };
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

        var candidatos = _itens.ToList();
        candidatos.Add(novoItem);
        if (!TentarSalvar(candidatos)) return;
        _itens.Add(novoItem);
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
        var candidatos = _itens.ToList();
        candidatos[index] = atualizado;
        if (!TentarSalvar(candidatos)) return;
        _itens[index] = atualizado;
        LimparFormulario();
        AtualizarColecao();
        ExibirAviso("Jogo atualizado", $"As alterações de “{atualizado.Titulo}” foram salvas com sucesso.");
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
        if (ListaBacklog.SelectedItem is not BacklogItem item) return;

        var candidatos = _itens.ToList();
        candidatos.Remove(item);
        if (!TentarSalvar(candidatos)) return;
        _itens.Remove(item);
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
        BtnRemover.IsEnabled = selecionado && _persistenciaDisponivel;
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
        CancelarBusca();
        ListaSugestoes.IsVisible = false;
        _jogoSelecionado = null;
        _itemEmEdicao = item;
        TxtTitulo.Text = item.Titulo;
        SelecionarValor(CmbCategoria, item.Categoria);
        SelecionarValor(CmbStatus, item.Status);
        NumNota.Value = (decimal)item.Nota;
        TxtReview.MaxLength = Math.Max(1000, item.Review.Length);
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
        BtnRemover.IsEnabled = BtnEditar.IsEnabled && _persistenciaDisponivel;
        TxtContagemLista.Text = $"{filtrados.Length} de {_itens.Count} jogos · Selecione para editar";
        EstadoListaVazia.IsVisible = filtrados.Length == 0;
        TxtListaVazia.Text = _itens.Count == 0 ? "Sua coleção ainda está vazia" : "Nenhum jogo encontrado";
        TxtDicaListaVazia.Text = _itens.Count == 0
            ? "Use “Adicionar jogo” para começar seu backlog."
            : "Tente outro título, categoria ou status.";
    }

    private void ListaSugestoes_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ListaSugestoes.SelectedItem is IgdbGame { Id: > 0 } jogo && !string.IsNullOrWhiteSpace(jogo.Name))
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
        if (_igdbService == null)
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
            var resultado = await _igdbService.BuscarJogosAsync(termo, buscaAtual.Token);
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
