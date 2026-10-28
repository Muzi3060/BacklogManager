using System;
using System.Globalization;
using BacklogManager.Models;

namespace BacklogManager.Services;

public sealed record DadosFormularioJogo(string? TextoTitulo, string? Categoria, string? Status, decimal? Nota, string? Review, ProgressoPessoal? Progresso = null);

public static class FormularioJogo
{
    public static ItemBiblioteca Preparar(DadosFormularioJogo dados, ObraCatalogo? selecionada,
        string? textoNaSelecao, ItemBiblioteca? emEdicao)
    {
        return FormularioObra.Preparar(dados, selecionada, textoNaSelecao, emEdicao, TipoMidia.Jogo);
    }

    internal static bool SelecaoValida(ObraCatalogo? obra) =>
        obra is { Tipo: TipoMidia.Jogo, Origem: "IGDB", Simulado: false } && !string.IsNullOrWhiteSpace(obra.Titulo) &&
        int.TryParse(obra.IdentificadorExterno, NumberStyles.None, CultureInfo.InvariantCulture, out var id) && id > 0 &&
        id.ToString(CultureInfo.InvariantCulture) == obra.IdentificadorExterno;
}

public sealed class ValidacaoFormularioException : Exception
{
    public string Titulo { get; }
    public ValidacaoFormularioException(string titulo, string mensagem) : base(mensagem) => Titulo = titulo;
}
