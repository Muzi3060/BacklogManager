using System;
using System.Globalization;
using BacklogManager.Models;

namespace BacklogManager.Services;

public sealed record DadosFormularioJogo(string? TextoTitulo, string? Categoria, string? Status, decimal? Nota, string? Review);

public static class FormularioJogo
{
    public static ItemBiblioteca Preparar(DadosFormularioJogo dados, ObraCatalogo? selecionada,
        string? textoNaSelecao, ItemBiblioteca? emEdicao)
    {
        ArgumentNullException.ThrowIfNull(dados);
        if (emEdicao == null && (!SelecaoValida(selecionada) || dados.TextoTitulo != textoNaSelecao))
            throw new ValidacaoFormularioException("Selecione um jogo", "Escolha uma sugestão válida antes de adicionar à coleção.");
        if (dados.Nota is not decimal nota || nota < 0 || nota > 5)
            throw new ValidacaoFormularioException("Nota inválida", "Informe uma nota entre 0 e 5.");
        if (string.IsNullOrWhiteSpace(dados.Categoria) || string.IsNullOrWhiteSpace(dados.Status))
            throw new ValidacaoFormularioException("Dados incompletos", "Selecione categoria e status antes de salvar.");
        var inalterada = emEdicao != null && dados.Review == emEdicao.Pessoal.Review;
        var review = inalterada ? dados.Review : dados.Review?.Trim();
        if (review?.Length > 1000 && !inalterada)
            throw new ValidacaoFormularioException("Review muito longa", "Use até 1000 caracteres na review.");
        var pessoal = new DadosPessoaisBiblioteca
        {
            Categoria = dados.Categoria,
            Status = dados.Status,
            Nota = (double)nota,
            Review = inalterada ? review! : string.IsNullOrWhiteSpace(review) ? "Nenhuma Review" : review
        };
        return emEdicao == null ? new ItemBiblioteca(selecionada!, pessoal) : emEdicao with { Pessoal = pessoal };
    }

    internal static bool SelecaoValida(ObraCatalogo? obra) =>
        obra is { Tipo: TipoMidia.Jogo, Origem: "IGDB" } && !string.IsNullOrWhiteSpace(obra.Titulo) &&
        int.TryParse(obra.IdentificadorExterno, NumberStyles.None, CultureInfo.InvariantCulture, out var id) && id > 0 &&
        id.ToString(CultureInfo.InvariantCulture) == obra.IdentificadorExterno;
}

public sealed class ValidacaoFormularioException : Exception
{
    public string Titulo { get; }
    public ValidacaoFormularioException(string titulo, string mensagem) : base(mensagem) => Titulo = titulo;
}
