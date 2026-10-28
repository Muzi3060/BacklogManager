using System;
using Avalonia.Controls;
using BacklogManager.Models;

namespace BacklogManager;

public partial class MainWindow
{
    private TipoMidia TipoCadastro => TipoDoCombo(CmbTipoCadastro);
    private TipoMidia TipoBiblioteca => TipoDoCombo(CmbTipoBiblioteca);
    private static TipoMidia TipoDoCombo(ComboBox combo) =>
        Enum.TryParse<TipoMidia>((combo.SelectedItem as ComboBoxItem)?.Tag?.ToString(), out var tipo) && Enum.IsDefined(tipo)
            ? tipo : TipoMidia.Jogo;

    private static void DefinirTipo(ComboBox combo, TipoMidia tipo) => combo.SelectedIndex = (int)tipo;

    private void TipoCadastro_Changed(object? sender, SelectionChangedEventArgs e)
    {
        if (!_interfacePronta || _itemEmEdicao != null) return;
        LimparFormulario();
    }

    private void TipoBiblioteca_Changed(object? sender, SelectionChangedEventArgs e)
    {
        if (!_interfacePronta) return;
        ListaBacklog.SelectedItem = null;
        AtualizarStatus(CmbFiltroStatus, TipoBiblioteca, filtro: true);
        BtnEditar.Content = TipoBiblioteca == TipoMidia.Jogo ? "Editar jogo" : "Editar obra";
        AtualizarOpcoesFiltros();
        AtualizarLista();
    }

    private void AtualizarFormularioMidia()
    {
        TxtTitulo.PlaceholderText = TipoCadastro == TipoMidia.Jogo ? "Ex.: Cyberpunk 2077" : "Digite simulado para ver os exemplos";
        AvisoCatalogoSimulado.IsVisible = TipoCadastro != TipoMidia.Jogo;
        if (TipoCadastro != TipoMidia.Jogo && TxtAvisoTitulo.Text == "Pesquisa indisponível")
            AvisoOperacao.IsVisible = false;
        AtualizarStatus(CmbStatus, TipoCadastro, filtro: false);
        ProgressoJogo.IsVisible = TipoCadastro == TipoMidia.Jogo;
        ProgressoAnime.IsVisible = TipoCadastro == TipoMidia.Anime;
        ProgressoSerie.IsVisible = TipoCadastro == TipoMidia.Serie;
        ProgressoFilme.IsVisible = TipoCadastro == TipoMidia.Filme;
        ProgressoManga.IsVisible = TipoCadastro == TipoMidia.Manga;
        ProgressoLivro.IsVisible = TipoCadastro == TipoMidia.Livro;
        BtnAdicionar.Content = TipoCadastro == TipoMidia.Jogo ? "Adicionar jogo" : "Adicionar obra";
        BtnEditar.Content = TipoBiblioteca == TipoMidia.Jogo ? "Editar jogo" : "Editar obra";
        AtualizarModoFormulario();
    }

    private static void AtualizarStatus(ComboBox combo, TipoMidia tipo, bool filtro)
    {
        var estados = new[] { "Na Fila", "Jogando", "Zerado", "Pausado", "Abandonado" };
        var deslocamento = filtro ? 1 : 0;
        // O ComboBox mantém uma cópia visual do conteúdo selecionado.
        // Reaplicar a seleção atualiza também o texto mostrado quando os rótulos mudam.
        var indiceSelecionado = combo.SelectedIndex;
        combo.SelectedIndex = -1;
        for (var i = 0; i < estados.Length; i++)
        {
            if (combo.Items[i + deslocamento] is not ComboBoxItem opcao) continue;
            opcao.Tag = estados[i];
            opcao.Content = TextosMidia.Status(tipo, estados[i]);
        }
        combo.SelectedIndex = indiceSelecionado;
    }

    private ProgressoPessoal? LerProgresso()
    {
        int? Inteiro(NumericUpDown controle)
        {
            if (controle.Value is not decimal valor) return null;
            if (valor != decimal.Truncate(valor) || valor > int.MaxValue || valor < 0)
                throw new BacklogManager.Services.ValidacaoFormularioException("Progresso inválido", "Use números inteiros, zero ou positivos, para este campo de progresso.");
            return (int)valor;
        }
        var progresso = TipoCadastro switch
        {
            TipoMidia.Jogo => new ProgressoPessoal { HorasJogadas = NumHorasJogadas.Value },
            TipoMidia.Anime => new ProgressoPessoal { EpisodiosAssistidos = Inteiro(NumEpisodiosAssistidos) },
            TipoMidia.Serie => new ProgressoPessoal { Temporada = Inteiro(NumTemporada), Episodio = Inteiro(NumEpisodio) },
            TipoMidia.Filme => new ProgressoPessoal { MinutosAssistidos = NumMinutosAssistidos.Value },
            TipoMidia.Manga => new ProgressoPessoal { Capitulo = Inteiro(NumCapituloManga), Volume = Inteiro(NumVolume) },
            _ => new ProgressoPessoal { Pagina = Inteiro(NumPagina), Capitulo = Inteiro(NumCapituloLivro) }
        };
        return progresso.Vazio ? null : progresso;
    }

    private void PreencherProgresso(ProgressoPessoal? progresso)
    {
        NumHorasJogadas.Value = progresso?.HorasJogadas;
        NumEpisodiosAssistidos.Value = progresso?.EpisodiosAssistidos;
        NumTemporada.Value = progresso?.Temporada;
        NumEpisodio.Value = progresso?.Episodio;
        NumMinutosAssistidos.Value = progresso?.MinutosAssistidos;
        NumCapituloManga.CarregarValorLegado(TipoCadastro == TipoMidia.Manga ? progresso?.Capitulo : null);
        NumVolume.CarregarValorLegado(progresso?.Volume);
        NumPagina.CarregarValorLegado(progresso?.Pagina);
        NumCapituloLivro.CarregarValorLegado(TipoCadastro == TipoMidia.Livro ? progresso?.Capitulo : null);
    }
}
