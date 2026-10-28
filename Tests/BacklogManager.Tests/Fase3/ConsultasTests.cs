using BacklogManager.Models;
using BacklogManager.Services;
using BacklogManager.Tests.Fase1;
using Xunit;

namespace BacklogManager.Tests.Fase3;

public sealed class ConsultasTests
{
    private static readonly DateTimeOffset Data = new(2026, 10, 20, 12, 0, 0, TimeSpan.Zero);
    internal static ItemBiblioteca Item(string titulo = "Alfa", double nota = 4, string genero = "RPG", string plataforma = "PC", DateTimeOffset? data = null) =>
        new(new ObraCatalogo { Titulo = titulo, Origem = "IGDB", IdentificadorExterno = "42", Generos = new[] { genero }, Plataformas = new[] { plataforma } },
            new DadosPessoaisBiblioteca { Categoria = "Outro", Status = "Na Fila", Nota = nota, Review = "Review" })
        { DataAdicao = data };
    private static ServicoBiblioteca Biblioteca(params ItemBiblioteca[] itens) => ApoioArquitetura.Carregada(itens).servico;
    private static ItemBiblioteca[] Dados() => new[] { Item(data: Data), Item("Beta", 2, "Aventura", "Switch", Data.AddDays(1)), Item("Gama", 5, "RPG", "PC") };
    private static IReadOnlyList<ItemBiblioteca> Filtrar(ConsultaBiblioteca q) => Biblioteca(Dados()).Consultar(q);

    [Fact(DisplayName = "QB01 | Consulta vazia | Todos os itens permanecem na ordem original")]
    public void Vazia() => Assert.Equal(new[] { "Alfa", "Beta", "Gama" }, Filtrar(new()).Select(i => i.Catalogo.Titulo));
    [Fact(DisplayName = "QB02 | Gênero do catálogo | Somente obras com o gênero escolhido")]
    public void Genero() => Assert.Equal(2, Filtrar(new() { Genero = "RPG" }).Count);
    [Fact(DisplayName = "QB03 | Gênero sem diferença de caixa | rpg encontra RPG")]
    public void CaixaGenero() => Assert.Equal(2, Filtrar(new() { Genero = "rpg" }).Count);
    [Fact(DisplayName = "QB04 | Gênero não encontrado | Resultado vazio sem alterar coleção")]
    public void GeneroAusente() => Assert.Empty(Filtrar(new() { Genero = "Terror" }));
    [Fact(DisplayName = "QB05 | Legado sem gêneros | Categoria pessoal serve de alternativa")]
    public void GeneroLegado() => Assert.Single(Biblioteca(ApoioArquitetura.Jogo()).Consultar(new() { Genero = "RPG" }));
    [Fact(DisplayName = "QB06 | Catálogo com gêneros | Categoria pessoal não substitui gênero disponível")]
    public void PrioridadeGenero() => Assert.Empty(Filtrar(new() { Genero = "Outro" }));
    [Fact(DisplayName = "QB07 | Obra com múltiplos gêneros | Segundo gênero também pode ser filtrado")]
    public void MultiploGenero() { var i = Item(); i = i with { Catalogo = i.Catalogo with { Generos = new[] { "Ação", "RPG" } } }; Assert.Single(Biblioteca(i).Consultar(new() { Genero = "RPG" })); }
    [Fact(DisplayName = "QB08 | Plataforma de jogo | PC encontra apenas os jogos correspondentes")]
    public void Plataforma() => Assert.Equal(2, Filtrar(new() { Plataforma = "PC" }).Count);
    [Fact(DisplayName = "QB09 | Plataforma sem diferença de caixa | pc encontra PC")]
    public void CaixaPlataforma() => Assert.Equal(2, Filtrar(new() { Plataforma = "pc" }).Count);
    [Fact(DisplayName = "QB10 | Plataforma desconhecida no legado | Legado não ganha plataforma inventada")]
    public void PlataformaLegado() => Assert.Empty(Biblioteca(ApoioArquitetura.Jogo()).Consultar(new() { Plataforma = "PC" }));
    [Fact(DisplayName = "QB11 | Plataformas múltiplas | Segunda plataforma é pesquisável")]
    public void Multiplataforma() { var i = Item(); i = i with { Catalogo = i.Catalogo with { Plataformas = new[] { "PC", "Linux" } } }; Assert.Single(Biblioteca(i).Consultar(new() { Plataforma = "Linux" })); }
    [Fact(DisplayName = "QB12 | Plataforma e gênero combinados | Interseção correta")]
    public void Combinados() => Assert.Empty(Filtrar(new() { Genero = "RPG", Plataforma = "Switch" }));
    [Fact(DisplayName = "QB13 | Nota mínima inclusiva | Nota exatamente no limite permanece")]
    public void Nota() => Assert.Equal(2, Filtrar(new() { NotaMinima = 4 }).Count);
    [Fact(DisplayName = "QB14 | Nota mínima zero | Nenhum item é excluído")]
    public void Zero() => Assert.Equal(3, Filtrar(new() { NotaMinima = 0 }).Count);
    [Fact(DisplayName = "QB15 | Nota mínima cinco | Somente a nota máxima")]
    public void Cinco() => Assert.Equal("Gama", Assert.Single(Filtrar(new() { NotaMinima = 5 })).Catalogo.Titulo);
    [Fact(DisplayName = "QB16 | Nota fracionária | Comparação preserva casas decimais")]
    public void Fracao() => Assert.Single(Filtrar(new() { NotaMinima = 4.5 }));
    [Fact(DisplayName = "QB17 | Nota negativa | Filtro rejeitado sem alterar dados")]
    public void Negativa() => Assert.Throws<ArgumentException>(() => Filtrar(new() { NotaMinima = -1 }));
    [Fact(DisplayName = "QB18 | Nota acima do máximo | Filtro rejeitado")]
    public void Acima() => Assert.Throws<ArgumentException>(() => Filtrar(new() { NotaMinima = 6 }));
    [Fact(DisplayName = "QB19 | Nota não numérica | NaN rejeitado")]
    public void Nan() => Assert.Throws<ArgumentException>(() => Filtrar(new() { NotaMinima = double.NaN }));
    [Fact(DisplayName = "QB20 | Data inicial inclusiva | Apenas registros com data conhecida no intervalo")]
    public void De() => Assert.Equal(2, Filtrar(new() { AdicionadaDe = DateOnly.FromDateTime(Data.LocalDateTime) }).Count);
    [Fact(DisplayName = "QB21 | Data final inclusiva | Registro do dia final permanece")]
    public void Ate() => Assert.Equal("Alfa", Assert.Single(Filtrar(new() { AdicionadaAte = DateOnly.FromDateTime(Data.LocalDateTime) })).Catalogo.Titulo);
    [Fact(DisplayName = "QB22 | Intervalo de um dia | Horário não exclui item do mesmo dia")]
    public void Dia() { var dia = DateOnly.FromDateTime(Data.LocalDateTime); Assert.Single(Filtrar(new() { AdicionadaDe = dia, AdicionadaAte = dia })); }
    [Fact(DisplayName = "QB23 | Intervalo invertido | Filtro rejeitado sem perda de itens")]
    public void Invertido() => Assert.Throws<ArgumentException>(() => Filtrar(new() { AdicionadaDe = new(2026, 10, 22), AdicionadaAte = new(2026, 10, 20) }));
    [Fact(DisplayName = "QB24 | Sem filtro de data | Registros sem data continuam visíveis")]
    public void SemData() => Assert.Contains(Filtrar(new()), i => i.DataAdicao == null);
    [Fact(DisplayName = "QB25 | Intervalo sem correspondência | Lista vazia sem exceção")]
    public void DataAusente() => Assert.Empty(Filtrar(new() { AdicionadaDe = new(2030, 1, 1) }));
    [Fact(DisplayName = "QB26 | Busca junto de novos filtros | Termo, gênero e nota combinam")]
    public void Busca() => Assert.Equal("Gama", Assert.Single(Filtrar(new() { Termo = "gam", Genero = "RPG", NotaMinima = 5 })).Catalogo.Titulo);
    [Fact(DisplayName = "QB27 | Status junto de novos filtros | Status incompatível retorna vazio")]
    public void Status() => Assert.Empty(Filtrar(new() { Status = "Zerado", Genero = "RPG" }));
    [Fact(DisplayName = "QB28 | Consulta com mídia diferente | Nenhum jogo aparece como livro")]
    public void Tipo() => Assert.Empty(Filtrar(new() { Tipo = TipoMidia.Livro }));
    [Fact(DisplayName = "QB29 | Filtros não gravam | Consulta deixa persistência e instâncias intactas")]
    public void SomenteLeitura() { var dados = Dados(); var (s, a) = ApoioArquitetura.Carregada(dados); s.Consultar(new() { Genero = "RPG", NotaMinima = 4 }); Assert.Equal(0, a.Gravacoes); Assert.Same(dados[0], s.Itens[0]); }
    [Fact(DisplayName = "QB30 | Consulta nula | Argumento rejeitado claramente")]
    public void Nula() => Assert.Throws<ArgumentNullException>(() => Biblioteca().Consultar(null!));
    [Fact(DisplayName = "QB31 | Coleção vazia | Filtros válidos retornam vazio")]
    public void ColecaoVazia() => Assert.Empty(Biblioteca().Consultar(new() { Genero = "RPG", NotaMinima = 5 }));
    [Fact(DisplayName = "QB32 | Limpar filtros | Todos os registros voltam à consulta")]
    public void Limpar() { var s = Biblioteca(Dados()); Assert.Single(s.Consultar(new() { NotaMinima = 5 })); Assert.Equal(3, s.Consultar(new()).Count); }
    [Fact(DisplayName = "QB33 | Data em fuso diferente | Filtro usa o dia local da adição")]
    public void Fuso() { var i = Item(data: Data.ToOffset(TimeSpan.FromHours(9))); var dia = DateOnly.FromDateTime(Data.LocalDateTime); Assert.Single(Biblioteca(i).Consultar(new() { AdicionadaDe = dia, AdicionadaAte = dia })); }
    [Fact(DisplayName = "QB34 | Tipo enum desconhecido | Consulta inválida rejeitada")]
    public void TipoInvalido() => Assert.Throws<ArgumentException>(() => Filtrar(new() { Tipo = (TipoMidia)99 }));

    [Fact(DisplayName = "OB01 | Ordenação A a Z | Títulos em ordem alfabética")]
    public void AZ() => Assert.Equal(new[] { "Alfa", "Beta", "Gama" }, Ordenar(OrdenacaoBiblioteca.TituloAscendente));
    [Fact(DisplayName = "OB02 | Ordenação Z a A | Títulos em ordem inversa")]
    public void ZA() => Assert.Equal(new[] { "Gama", "Beta", "Alfa" }, Ordenar(OrdenacaoBiblioteca.TituloDescendente));
    [Fact(DisplayName = "OB03 | Maior nota primeiro | Ordem 5, 4, 2")]
    public void Maior() => Assert.Equal(new[] { "Gama", "Alfa", "Beta" }, Ordenar(OrdenacaoBiblioteca.MaiorNota));
    [Fact(DisplayName = "OB04 | Menor nota primeiro | Ordem 2, 4, 5")]
    public void Menor() => Assert.Equal(new[] { "Beta", "Alfa", "Gama" }, Ordenar(OrdenacaoBiblioteca.MenorNota));
    [Fact(DisplayName = "OB05 | Adição recente primeiro | Datas conhecidas decrescentes e desconhecidas ao final")]
    public void Recente() => Assert.Equal(new[] { "Beta", "Alfa", "Gama" }, Ordenar(OrdenacaoBiblioteca.AdicaoRecente));
    [Fact(DisplayName = "OB06 | Adição antiga primeiro | Datas conhecidas crescentes e desconhecidas ao final")]
    public void Antiga() => Assert.Equal(new[] { "Alfa", "Beta", "Gama" }, Ordenar(OrdenacaoBiblioteca.AdicaoAntiga));
    [Fact(DisplayName = "OB07 | Voltar à ordem de cadastro | Ordenar não altera coleção original")]
    public void Insercao() { var s = Biblioteca(Dados()); s.Consultar(new() { Ordenacao = OrdenacaoBiblioteca.MaiorNota }); Assert.Equal(new[] { "Alfa", "Beta", "Gama" }, s.Consultar(new()).Select(i => i.Catalogo.Titulo)); }
    [Fact(DisplayName = "OB08 | Empate de nota | Ordem relativa das instâncias permanece")]
    public void Empate() { var a = Item("Z"); var b = Item("A"); var r = Biblioteca(a, b).Consultar(new() { Ordenacao = OrdenacaoBiblioteca.MaiorNota }); Assert.Same(a, r[0]); Assert.Same(b, r[1]); }
    [Fact(DisplayName = "OB09 | Duplicatas iguais | Ordenação não deduplica registros")]
    public void Duplicatas() { var a = Item(); var b = a with { }; var r = Biblioteca(a, b).Consultar(new() { Ordenacao = OrdenacaoBiblioteca.TituloAscendente }); Assert.Equal(2, r.Count); Assert.Same(b, r[1]); }
    [Fact(DisplayName = "OB10 | Ordenação inválida | Enum desconhecido é rejeitado")]
    public void OrdemInvalida() => Assert.Throws<ArgumentException>(() => Filtrar(new() { Ordenacao = (OrdenacaoBiblioteca)99 }));
    [Fact(DisplayName = "OB11 | Ordenar resultado filtrado | Filtro de gênero é respeitado")]
    public void OrdemFiltro() => Assert.Equal(new[] { "Gama", "Alfa" }, Filtrar(new() { Genero = "RPG", Ordenacao = OrdenacaoBiblioteca.MaiorNota }).Select(i => i.Catalogo.Titulo));
    [Fact(DisplayName = "OB12 | Todos sem data | Ordenação por data mantém ordem relativa")]
    public void Legados() { var a = Item("Z"); var b = Item("A"); Assert.Same(a, Biblioteca(a, b).Consultar(new() { Ordenacao = OrdenacaoBiblioteca.AdicaoAntiga })[0]); }
    private static string[] Ordenar(OrdenacaoBiblioteca ordem) => Filtrar(new() { Ordenacao = ordem }).Select(i => i.Catalogo.Titulo).ToArray();
}
