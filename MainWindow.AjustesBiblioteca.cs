using BacklogManager.Models;
using BacklogManager.Services;

namespace BacklogManager;

public partial class MainWindow
{
    private void AtualizarDisponibilidadeCadastro()
    {
        BtnAdicionar.IsEnabled = PersistenciaDisponivel && (TipoCadastro != TipoMidia.Jogo ||
            _jogoSelecionado != null && FormularioJogo.SelecaoValida(_jogoSelecionado) && TxtTitulo.Text == _textoNaSelecao);
    }

    private void AtualizarGenerosCadastro()
    {
        var obra = _itemEmEdicao?.Catalogo ?? _jogoSelecionado;
        var legado = _itemEmEdicao?.Pessoal.Categoria;
        CampoCategoriaLegada.IsVisible = TipoCadastro != TipoMidia.Jogo;
        CampoGenerosCatalogo.IsVisible = TipoCadastro == TipoMidia.Jogo;
        TxtGenerosCatalogo.Text = obra?.Generos is { Count: > 0 } ? obra.GenerosTexto :
            _itemEmEdicao == null && obra == null ? "Selecione um resultado da IGDB para preencher os gêneros." :
            string.IsNullOrWhiteSpace(legado) ? "Gêneros não informados pelo catálogo." : $"Categoria legada preservada: {legado}";
    }
}
