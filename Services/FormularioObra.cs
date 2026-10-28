using System;
using System.IO;
using BacklogManager.Models;

namespace BacklogManager.Services;

public static class FormularioObra
{
    public static ItemBiblioteca Preparar(DadosFormularioJogo dados, ObraCatalogo? selecionada,
        string? textoNaSelecao, ItemBiblioteca? emEdicao, TipoMidia tipo)
    {
        ArgumentNullException.ThrowIfNull(dados);
        if (!Enum.IsDefined(tipo) || emEdicao != null && emEdicao.Catalogo.Tipo != tipo)
            throw new ValidacaoFormularioException("Tipo inválido", "A edição deve manter o tipo de mídia original.");
        if (emEdicao == null && (!SelecaoValida(selecionada, tipo) || dados.TextoTitulo != textoNaSelecao))
            throw new ValidacaoFormularioException(tipo == TipoMidia.Jogo ? "Selecione um jogo" : "Selecione uma obra", "Escolha uma sugestão válida desta categoria antes de adicionar.");
        if (dados.Nota is not decimal nota || nota < 0 || nota > 5)
            throw new ValidacaoFormularioException("Nota inválida", "Informe uma nota entre 0 e 5.");
        if (string.IsNullOrWhiteSpace(dados.Status) || tipo != TipoMidia.Jogo && string.IsNullOrWhiteSpace(dados.Categoria))
            throw new ValidacaoFormularioException("Dados incompletos", tipo == TipoMidia.Jogo
                ? "Selecione um status antes de salvar." : "Selecione categoria e status antes de salvar.");
        var inalterada = emEdicao != null && dados.Review == emEdicao.Pessoal.Review;
        var review = inalterada ? dados.Review : dados.Review?.Trim();
        if (review?.Length > 1000 && !inalterada)
            throw new ValidacaoFormularioException("Review muito longa", "Use até 1000 caracteres na review.");
        try { RegrasProgresso.ValidarPreenchimento(tipo, dados.Progresso, emEdicao?.Pessoal.Progresso); }
        catch (InvalidDataException erro) { throw new ValidacaoFormularioException("Progresso inválido", erro.Message); }
        var pessoal = new DadosPessoaisBiblioteca
        {
            // Jogos novos usam gêneros do catálogo; a coluna antiga permanece só por compatibilidade.
            Categoria = tipo == TipoMidia.Jogo ? emEdicao?.Pessoal.Categoria ?? string.Empty : dados.Categoria!,
            Status = dados.Status,
            Nota = (double)nota,
            Review = inalterada ? review! : string.IsNullOrWhiteSpace(review) ? "Nenhuma Review" : review,
            Progresso = dados.Progresso is { Vazio: false } ? dados.Progresso : null
        };
        return emEdicao == null ? new ItemBiblioteca(selecionada!, pessoal) { DataAdicao = DateTimeOffset.UtcNow }
            : emEdicao with { Pessoal = pessoal };
    }

    public static bool SelecaoValida(ObraCatalogo? obra, TipoMidia tipo) =>
        tipo == TipoMidia.Jogo ? FormularioJogo.SelecaoValida(obra) :
        Enum.IsDefined(tipo) && obra != null && obra.Tipo == tipo && obra.Simulado && obra.Origem == "Simulado" &&
        !string.IsNullOrWhiteSpace(obra.Titulo) && !string.IsNullOrWhiteSpace(obra.IdentificadorExterno);
}
