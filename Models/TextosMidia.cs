namespace BacklogManager.Models;

public static class TextosMidia
{
    public static string Plural(TipoMidia tipo) => tipo switch
    {
        TipoMidia.Jogo => "Jogos", TipoMidia.Anime => "Animes", TipoMidia.Serie => "Séries",
        TipoMidia.Filme => "Filmes", TipoMidia.Manga => "Mangás", TipoMidia.Livro => "Livros", _ => "Obras"
    };

    public static string Status(TipoMidia tipo, string status)
    {
        if (tipo == TipoMidia.Jogo) return status;
        var leitura = tipo is TipoMidia.Manga or TipoMidia.Livro;
        return status switch
        {
            "Na Fila" => leitura ? "Quero ler" : "Quero assistir",
            "Jogando" => leitura ? "Lendo" : "Assistindo",
            "Zerado" => leitura ? "Lido" : "Assistido",
            _ => status
        };
    }
    public static string Progresso(TipoMidia tipo, ProgressoPessoal? p)
    {
        if (p == null || p.Vazio) return string.Empty;
        string Numero(decimal? valor) => valor?.ToString("0.##", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")) ?? "—";
        return tipo switch
        {
            TipoMidia.Jogo => Numero(p.HorasJogadas) + " h jogadas",
            TipoMidia.Anime => Numero(p.EpisodiosAssistidos) + " episódios assistidos",
            TipoMidia.Serie => "Temporada " + Numero(p.Temporada) + " · episódio " + Numero(p.Episodio),
            TipoMidia.Filme => Numero(p.MinutosAssistidos) + " min assistidos",
            TipoMidia.Manga => "Capítulo " + Numero(p.Capitulo) + " · volume " + Numero(p.Volume),
            _ => "Página " + Numero(p.Pagina) + " · capítulo " + Numero(p.Capitulo)
        };
    }

}
