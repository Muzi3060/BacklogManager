using System;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

namespace BacklogManager.Models;

[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed record ProgressoPessoal
{
    public decimal? HorasJogadas { get; init; }
    public int? EpisodiosAssistidos { get; init; }
    public int? Temporada { get; init; }
    public int? Episodio { get; init; }
    public decimal? MinutosAssistidos { get; init; }
    public int? Capitulo { get; init; }
    public int? Volume { get; init; }
    public int? Pagina { get; init; }

    [JsonIgnore]
    public bool Vazio => HorasJogadas == null && EpisodiosAssistidos == null && Temporada == null &&
        Episodio == null && MinutosAssistidos == null && Capitulo == null && Volume == null && Pagina == null;
}

public static class RegrasProgresso
{
    public static void ValidarPreenchimento(TipoMidia tipo, ProgressoPessoal? progresso, ProgressoPessoal? anterior = null)
    {
        // A leitura continua compatível com zeros antigos. Só novas posições precisam começar em 1.
        if (progresso != null && (PosicaoInvalida(progresso.Pagina, anterior?.Pagina) ||
            PosicaoInvalida(progresso.Capitulo, anterior?.Capitulo) ||
            PosicaoInvalida(progresso.Volume, anterior?.Volume)))
            throw new InvalidDataException("Página, capítulo e volume devem começar em 1. Deixe em branco quando o progresso não estiver informado.");
        Validar(tipo, progresso);
    }

    private static bool PosicaoInvalida(int? valor, int? anterior) => valor is <= 0 && valor != anterior;

    public static void Validar(TipoMidia tipo, ProgressoPessoal? progresso)
    {
        if (!Enum.IsDefined(tipo)) throw new InvalidDataException("Tipo de mídia inválido.");
        if (progresso == null) return;
        if (progresso.HorasJogadas < 0 || progresso.MinutosAssistidos < 0 ||
            new[] { progresso.EpisodiosAssistidos, progresso.Temporada, progresso.Episodio,
                progresso.Capitulo, progresso.Volume, progresso.Pagina }.Any(valor => valor < 0))
            throw new InvalidDataException("O progresso deve ser zero ou positivo.");
        if ((tipo != TipoMidia.Jogo && progresso.HorasJogadas != null) ||
            (tipo != TipoMidia.Anime && progresso.EpisodiosAssistidos != null) ||
            (tipo != TipoMidia.Serie && (progresso.Temporada != null || progresso.Episodio != null)) ||
            (tipo != TipoMidia.Filme && progresso.MinutosAssistidos != null) ||
            (tipo is not (TipoMidia.Manga or TipoMidia.Livro) && progresso.Capitulo != null) ||
            (tipo != TipoMidia.Manga && progresso.Volume != null) ||
            (tipo != TipoMidia.Livro && progresso.Pagina != null))
            throw new InvalidDataException("O progresso informado não pertence a este tipo de mídia.");
    }
}
