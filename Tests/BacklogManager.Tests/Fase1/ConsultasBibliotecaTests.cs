using BacklogManager.Models;
using Xunit;
using static BacklogManager.Tests.Fase1.ApoioArquitetura;

namespace BacklogManager.Tests.Fase1;

public sealed class ConsultasBibliotecaTests
{
    [Fact(DisplayName = "FI01 | Consulta sem filtros | Todos os itens retornados na ordem original")]
    public void Todos() { var (s, _) = Carregada(Jogo("Um"), Jogo("Dois")); Assert.Equal(new[] { "Um", "Dois" }, s.Filtrar(null, null).Select(i => i.Catalogo.Titulo)); }
    [Fact(DisplayName = "FI02 | Filtrar título com caixa diferente | Comparação ignora maiúsculas")]
    public void Titulo() { var (s, _) = Carregada(Jogo("Persona"), Jogo("Outro")); Assert.Equal("Persona", Assert.Single(s.Filtrar("pErSo", null)).Catalogo.Titulo); }
    [Fact(DisplayName = "FI03 | Filtrar categoria pessoal | Categoria pesquisável sem misturar com gêneros de catálogo")]
    public void Categoria() { var (s, _) = Carregada(Jogo()); Assert.Single(s.Filtrar("rpg", null)); }
    [Fact(DisplayName = "FI04 | Texto e status combinados | Ambos os critérios respeitados")]
    public void Combinados() { var (s, _) = Carregada(Jogo("Um", "Jogando"), Jogo("Dois")); Assert.Empty(s.Filtrar("Dois", "Jogando")); }
    [Fact(DisplayName = "FI05 | Filtro sem resultado | Dados originais continuam na biblioteca")]
    public void SemResultado() { var (s, a) = Carregada(Jogo()); Assert.Empty(s.Filtrar("Ausente", null)); Assert.Single(s.Itens); Assert.Equal(0, a.Gravacoes); }
    [Fact(DisplayName = "FI06 | Espaços no termo | Bordas aparadas antes da pesquisa")]
    public void Espacos() { var (s, _) = Carregada(Jogo("Persona")); Assert.Single(s.Filtrar("  Persona  ", null)); }
    [Fact(DisplayName = "FI07 | Resumo vazio | Contagens e progresso zero sem divisão inválida")]
    public void ResumoVazio() { var (s, _) = Carregada(); var r = s.ObterResumo(); Assert.Equal(0, r.Total); Assert.Equal(0, r.Progresso); }
    [Fact(DisplayName = "FI08 | Resumo com status mistos | Contagens reais e percentual preservados")]
    public void ResumoMisto()
    {
        var (s, _) = Carregada(Jogo("Um", "Zerado"), Jogo("Dois", "Jogando"), Jogo("Três"), Jogo("Quatro", "Antigo"));
        var r = s.ObterResumo(); Assert.Equal(4, r.Total); Assert.Equal(1, r.Zerados); Assert.Equal(1, r.Jogando); Assert.Equal(1, r.NaFila); Assert.Equal(25, r.Progresso);
    }
    [Fact(DisplayName = "FI09 | Recentes com mais de três itens | Últimos três por inserção em ordem inversa")]
    public void Recentes() { var (s, _) = Carregada(Jogo("Um"), Jogo("Dois"), Jogo("Três"), Jogo("Quatro")); Assert.Equal(new[] { "Quatro", "Três", "Dois" }, s.ObterRecentes().Select(i => i.Catalogo.Titulo)); }
    [Fact(DisplayName = "FI10 | Recentes vazios | Lista vazia válida")]
    public void RecentesVazios() { var (s, _) = Carregada(); Assert.Empty(s.ObterRecentes()); }
    [Fact(DisplayName = "FI11 | Filtros ativos | Resumo continua refletindo a coleção inteira")]
    public void ResumoNaoFiltrado() { var (s, _) = Carregada(Jogo("Um"), Jogo("Dois")); Assert.Single(s.Filtrar("Um", null)); Assert.Equal(2, s.ObterResumo().Total); }
    [Fact(DisplayName = "FI12 | Alterar array retornado pela consulta | Coleção interna não pode ser removida pelo resultado")]
    public void ResultadoIsolado() { var (s, _) = Carregada(Jogo()); var resultado = s.Filtrar(null, null); ((ItemBiblioteca[])resultado)[0] = Jogo("Outro"); Assert.Equal("Jogo de teste", Assert.Single(s.Itens).Catalogo.Titulo); }
    [Fact(DisplayName = "FI13 | Editar status e remover | Resumo acompanha somente operações confirmadas")]
    public void ResumoAtualizado() { var item = Jogo(); var (s, _) = Carregada(item); s.Atualizar(item, item.Pessoal with { Status = "Zerado" }); Assert.Equal(100, s.ObterResumo().Progresso); s.Remover(Assert.Single(s.Itens)); Assert.Equal(0, s.ObterResumo().Progresso); }
}
