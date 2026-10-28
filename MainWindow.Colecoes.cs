using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BacklogManager.Models;

namespace BacklogManager;

public partial class MainWindow
{
    private bool _atualizandoColecoes;
    private Guid? _exclusaoPendente;
    private bool ColecoesDisponiveis => _colecoes.Carregadas && PersistenciaDisponivel;
    private Guid? ColecaoSelecionada => (CmbColecoes.SelectedItem as ColecaoPersonalizada)?.Id;

    private void AtualizarPaginaColecoes(Guid? selecionar = null)
    {
        if (!_interfacePronta) return;
        var id = selecionar ?? ColecaoSelecionada;
        var obraSelecionada = CmbObraColecao.SelectedItem as ItemBiblioteca;
        _atualizandoColecoes = true;
        try
        {
            CmbColecoes.ItemsSource = _colecoes.Carregadas ? _colecoes.Colecoes : Array.Empty<ColecaoPersonalizada>();
            CmbColecoes.SelectedIndex = -1;
            for (var i = 0; i < CmbColecoes.Items.Count; i++)
                if (CmbColecoes.Items[i] is ColecaoPersonalizada c && c.Id == id) { CmbColecoes.SelectedIndex = i; break; }
            CmbObraColecao.ItemsSource = _biblioteca.Itens;
            CmbObraColecao.SelectedIndex = -1;
            for (var i = 0; i < _biblioteca.Itens.Count; i++)
                if (ReferenceEquals(_biblioteca.Itens[i], obraSelecionada) || obraSelecionada?.IdInterno is { } obraId && _biblioteca.Itens[i].IdInterno == obraId)
                { CmbObraColecao.SelectedIndex = i; break; }
        }
        finally { _atualizandoColecoes = false; }
        AtualizarObrasColecao();
        AtualizarAcoesColecoes();
    }

    private void Colecao_Changed(object? sender, SelectionChangedEventArgs e)
    {
        if (!_interfacePronta || _atualizandoColecoes) return;
        CancelarExclusaoColecao();
        if (CmbColecoes.SelectedItem is ColecaoPersonalizada c)
        { TxtNomeColecao.Text = c.Nome; TxtDescricaoColecao.Text = c.Descricao; }
        AtualizarObrasColecao();
        AtualizarAcoesColecoes();
    }

    private void AtualizarObrasColecao()
    {
        var itens = ColecoesDisponiveis && ColecaoSelecionada is { } id ? _colecoes.ObterObras(id) : Array.Empty<ItemBiblioteca>();
        ListaObrasColecao.ItemsSource = itens;
        ListaObrasColecao.SelectedIndex = -1;
        var ausentes = ColecoesDisponiveis && ColecaoSelecionada is { } selecionada ? _colecoes.ContarReferenciasAusentes(selecionada) : 0;
        TxtResumoColecao.Text = ColecaoSelecionada == null ? "Coleções não duplicam obras na biblioteca." :
            $"{itens.Count} {(itens.Count == 1 ? "obra nesta coleção" : "obras nesta coleção")}." +
            (ausentes == 0 ? string.Empty : $" {ausentes} referências de obras ausentes foram preservadas; restaure a biblioteca correspondente para recuperá-las.");
        TxtColecaoVazia.IsVisible = itens.Count == 0;
        TxtColecaoVazia.Text = ColecaoSelecionada == null ? "Selecione ou crie uma coleção para ver suas obras." : "Esta coleção ainda não tem obras disponíveis.";
    }

    private void ObraColecao_Changed(object? sender, SelectionChangedEventArgs e)
    {
        if (_interfacePronta && !_atualizandoColecoes) AtualizarAcoesColecoes();
    }

    private void AtualizarAcoesColecoes()
    {
        var selecionada = ColecaoSelecionada != null;
        BtnNovaColecao.IsEnabled = BtnCriarColecao.IsEnabled = ColecoesDisponiveis;
        BtnSalvarColecao.IsEnabled = BtnExcluirColecao.IsEnabled = ColecoesDisponiveis && selecionada;
        BtnAdicionarObraColecao.IsEnabled = ColecoesDisponiveis && selecionada && CmbObraColecao.SelectedItem is ItemBiblioteca;
        BtnRemoverObraColecao.IsEnabled = ColecoesDisponiveis && selecionada && ListaObrasColecao.SelectedItem is ItemBiblioteca;
        BtnEditarObraColecao.IsEnabled = PersistenciaDisponivel && ListaObrasColecao.SelectedItem is ItemBiblioteca;
        TxtNomeColecao.IsEnabled = TxtDescricaoColecao.IsEnabled = ColecoesDisponiveis;
        CmbObraColecao.IsEnabled = ColecoesDisponiveis && selecionada;
    }

    private bool TentarColecoes(Action acao)
    {
        if (!ColecoesDisponiveis)
        { ExibirAviso("Coleções indisponíveis", "Carregue a biblioteca e as coleções com sucesso antes de salvar.", true); return false; }
        try { acao(); return true; }
        catch (Exception erro) when (FalhaDeArquivo(erro) || erro is ArgumentException)
        {
            var mensagem = erro is ArgumentException ? erro.Message : erro is BacklogManager.Data.ArquivoAlteradoException
                ? "Um arquivo mudou fora desta janela. Reabra o aplicativo antes de salvar coleções ou associações." :
                "A alteração da coleção não foi aplicada. Confira os arquivos e permissões. Se uma identidade de obra já foi salva, ela permanece segura na biblioteca; tente associar novamente.";
            ExibirAviso("Coleção não salva", mensagem, true);
            return false;
        }
    }

    private void NovaColecao_Click(object? sender, RoutedEventArgs e)
    {
        CmbColecoes.SelectedIndex = -1;
        TxtNomeColecao.Text = TxtDescricaoColecao.Text = string.Empty;
        CancelarExclusaoColecao();
        AtualizarObrasColecao();
        AtualizarAcoesColecoes();
        TxtNomeColecao.Focus();
    }

    private void CriarColecao_Click(object? sender, RoutedEventArgs e)
    {
        Guid id = default;
        if (!TentarColecoes(() => id = _colecoes.Criar(TxtNomeColecao.Text!, TxtDescricaoColecao.Text))) return;
        CancelarExclusaoColecao();
        AtualizarPaginaColecoes(id);
        PreencherMetadadosColecao();
        ExibirAviso("Coleção criada", "Sua nova coleção está pronta para receber obras da biblioteca.");
    }

    private void SalvarColecao_Click(object? sender, RoutedEventArgs e)
    {
        if (ColecaoSelecionada is not { } id || !TentarColecoes(() => _colecoes.Editar(id, TxtNomeColecao.Text!, TxtDescricaoColecao.Text))) return;
        CancelarExclusaoColecao();
        AtualizarPaginaColecoes(id);
        PreencherMetadadosColecao();
        ExibirAviso("Coleção atualizada", "Nome e descrição foram salvos. As obras associadas foram preservadas.");
    }

    private void ExcluirColecao_Click(object? sender, RoutedEventArgs e)
    {
        if (!ColecoesDisponiveis || CmbColecoes.SelectedItem is not ColecaoPersonalizada c) return;
        _exclusaoPendente = c.Id;
        TxtConfirmacaoColecao.Text = $"Excluir a coleção “{c.Nome}”? As obras continuarão na biblioteca e nas outras coleções.";
        ConfirmacaoExclusaoColecao.IsVisible = true;
    }

    private void ConfirmarExcluirColecao_Click(object? sender, RoutedEventArgs e)
    {
        if (_exclusaoPendente is not { } id || id != ColecaoSelecionada) return;
        if (!TentarColecoes(() => _colecoes.Excluir(id))) return;
        NovaColecao_Click(null, e);
        AtualizarPaginaColecoes();
        ExibirAviso("Coleção excluída", "As obras continuam disponíveis na biblioteca.");
    }

    private void CancelarExclusaoColecao() { _exclusaoPendente = null; ConfirmacaoExclusaoColecao.IsVisible = false; }
    private void PreencherMetadadosColecao()
    { if (CmbColecoes.SelectedItem is ColecaoPersonalizada c) { TxtNomeColecao.Text = c.Nome; TxtDescricaoColecao.Text = c.Descricao; } }
    private void CancelarExcluirColecao_Click(object? sender, RoutedEventArgs e) => CancelarExclusaoColecao();

    private void AdicionarObraColecao_Click(object? sender, RoutedEventArgs e)
    {
        if (ColecaoSelecionada is not { } id || CmbObraColecao.SelectedItem is not ItemBiblioteca obra) return;
        var indice = CmbObraColecao.SelectedIndex;
        var adicionada = false;
        var sucesso = TentarColecoes(() => adicionada = _colecoes.AdicionarObra(id, obra));
        // A primeira associação pode ter atribuído a identidade na biblioteca, mesmo se o segundo arquivo falhou.
        AtualizarColecao();
        CmbObraColecao.SelectedIndex = indice < CmbObraColecao.Items.Count ? indice : -1;
        AtualizarAcoesColecoes();
        if (sucesso) ExibirAviso(adicionada ? "Obra associada" : "Obra já associada", adicionada ? "A obra participa desta coleção sem ser duplicada na biblioteca." : "Esta obra já pertence à coleção.");
    }

    private void RemoverObraColecao_Click(object? sender, RoutedEventArgs e)
    {
        if (ColecaoSelecionada is not { } id || ListaObrasColecao.SelectedItem is not ItemBiblioteca { IdInterno: { } obraId }) return;
        if (!TentarColecoes(() => _colecoes.RemoverObra(id, obraId))) return;
        AtualizarPaginaColecoes(id);
        ExibirAviso("Associação removida", "A obra continua na biblioteca e nas outras coleções.");
    }

    private void EditarObraColecao_Click(object? sender, RoutedEventArgs e)
    { if (ListaObrasColecao.SelectedItem is ItemBiblioteca item) EditarJogo(item); }
}
