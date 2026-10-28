using BacklogManager.Models;
using BacklogManager.Services;
using BacklogManager.Tests.Fase1;
using Xunit;
using static BacklogManager.Tests.Fase2.ApoioMultimidia;

namespace BacklogManager.Tests.Fase2;

public sealed class CategoriasBibliotecaTests
{
    private static ServicoBiblioteca Biblioteca()
    {
        var armazenamento = new ArmazenamentoSimulado { Registros = Enum.GetValues<TipoMidia>().Select(t => Item(t)).ToArray() };
        var s = new ServicoBiblioteca(armazenamento); s.Carregar(); return s;
    }
    [Fact(DisplayName = "MT01 | Filtrar jogos em coleção mista | Somente jogos retornados")]
    public void Jogos() => Assert.Equal(TipoMidia.Jogo, Assert.Single(Biblioteca().Filtrar(null, null, TipoMidia.Jogo)).Catalogo.Tipo);
    [Fact(DisplayName = "MT02 | Filtrar animes em coleção mista | Somente animes retornados")]
    public void Animes() => Assert.Equal(TipoMidia.Anime, Assert.Single(Biblioteca().Filtrar(null, null, TipoMidia.Anime)).Catalogo.Tipo);
    [Fact(DisplayName = "MT03 | Filtrar séries em coleção mista | Somente séries retornadas")]
    public void Series() => Assert.Equal(TipoMidia.Serie, Assert.Single(Biblioteca().Filtrar(null, null, TipoMidia.Serie)).Catalogo.Tipo);
    [Fact(DisplayName = "MT04 | Filtrar filmes em coleção mista | Somente filmes retornados")]
    public void Filmes() => Assert.Equal(TipoMidia.Filme, Assert.Single(Biblioteca().Filtrar(null, null, TipoMidia.Filme)).Catalogo.Tipo);
    [Fact(DisplayName = "MT05 | Filtrar mangás em coleção mista | Somente mangás retornados")]
    public void Mangas() => Assert.Equal(TipoMidia.Manga, Assert.Single(Biblioteca().Filtrar(null, null, TipoMidia.Manga)).Catalogo.Tipo);
    [Fact(DisplayName = "MT06 | Filtrar livros em coleção mista | Somente livros retornados")]
    public void Livros() => Assert.Equal(TipoMidia.Livro, Assert.Single(Biblioteca().Filtrar(null, null, TipoMidia.Livro)).Catalogo.Tipo);
    [Fact(DisplayName = "MT07 | Tipo combinado com texto e status | Todos os critérios aplicados sem alterar coleção")]
    public void Combinados() { var s = Biblioteca(); Assert.Empty(s.Filtrar("Obra", "Jogando", TipoMidia.Livro)); Assert.Single(s.Filtrar("Obra", "Na Fila", TipoMidia.Livro)); Assert.Equal(6, s.Itens.Count); }
    [Fact(DisplayName = "MT08 | Consulta sem categoria | Compatibilidade permite consultar todos os registros")]
    public void Todos() => Assert.Equal(6, Biblioteca().Filtrar(null, null).Count);
    [Fact(DisplayName = "MT09 | Status de jogo legado | Textos originais continuam inalterados")]
    public void JogoLegado() { Assert.Equal("Na Fila", TextosMidia.Status(TipoMidia.Jogo, "Na Fila")); Assert.Equal("Jogando", TextosMidia.Status(TipoMidia.Jogo, "Jogando")); Assert.Equal("Zerado", TextosMidia.Status(TipoMidia.Jogo, "Zerado")); }
    [Fact(DisplayName = "MT10 | Status de mídias assistidas | Rótulos adaptados sem converter valores internos")]
    public void Assistir() { foreach (var t in new[] { TipoMidia.Anime, TipoMidia.Serie, TipoMidia.Filme }) { Assert.Equal("Quero assistir", TextosMidia.Status(t, "Na Fila")); Assert.Equal("Assistindo", TextosMidia.Status(t, "Jogando")); Assert.Equal("Assistido", TextosMidia.Status(t, "Zerado")); } }
    [Fact(DisplayName = "MT11 | Status de mídias lidas | Rótulos adequados e status legados desconhecidos preservados")]
    public void Ler() { foreach (var t in new[] { TipoMidia.Manga, TipoMidia.Livro }) { Assert.Equal("Quero ler", TextosMidia.Status(t, "Na Fila")); Assert.Equal("Lendo", TextosMidia.Status(t, "Jogando")); Assert.Equal("Lido", TextosMidia.Status(t, "Zerado")); Assert.Equal("Antigo", TextosMidia.Status(t, "Antigo")); } }
    [Fact(DisplayName = "MT12 | Resumo de coleção mista | Estatísticas usam registros pessoais sem inventar atividade")]
    public void Resumo() { var s = Biblioteca(); var item = s.Itens.Single(i => i.Catalogo.Tipo == TipoMidia.Livro); s.Atualizar(item, item.Pessoal with { Status = "Zerado" }); Assert.Equal(6, s.ObterResumo().Total); Assert.Equal(1, s.ObterResumo().Zerados); Assert.Equal(5, s.ObterResumo().NaFila); }
}
