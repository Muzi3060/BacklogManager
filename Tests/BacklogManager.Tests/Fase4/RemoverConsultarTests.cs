using BacklogManager.Models;
using BacklogManager.Tests.Fase2;
using Xunit;
namespace BacklogManager.Tests.Fase4;

public sealed class RemoverConsultarTests
{
    private static (AmbienteColecoes a, Guid id, ItemBiblioteca i) Preparar()
    { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); var id = a.Criar(); a.Servico.AdicionarObra(id, i); return (a, id, i); }
    [Fact(DisplayName = "RA01 | Remover vínculo | Obra permanece na biblioteca")]
    public void Remover() { var (a, id, _) = Preparar(); Assert.True(a.Servico.RemoverObra(id, a.Biblioteca.Itens[0].IdInterno!.Value)); Assert.Empty(a.Servico.ObterObras(id)); Assert.Single(a.Biblioteca.Itens); }
    [Fact(DisplayName = "RA02 | Remover em uma das duas coleções | Outra associação permanece")]
    public void OutraColecao() { var (a, id, i) = Preparar(); var outro = a.Criar("Outra"); a.Servico.AdicionarObra(outro, i); a.Servico.RemoverObra(id, a.Biblioteca.Itens[0].IdInterno!.Value); Assert.Single(a.Servico.ObterObras(outro)); }
    [Fact(DisplayName = "RA03 | Remover vínculo inexistente | Retorna falso sem gravação")]
    public void Inexistente() { var (a, id, _) = Preparar(); var n = a.Arquivo.Gravacoes; Assert.False(a.Servico.RemoverObra(id, Guid.NewGuid())); Assert.Equal(n, a.Arquivo.Gravacoes); }
    [Fact(DisplayName = "RA04 | Remover de coleção inexistente | Nenhuma associação muda")]
    public void ColecaoInexistente() { var (a, id, _) = Preparar(); Assert.Throws<ArgumentException>(() => a.Servico.RemoverObra(Guid.NewGuid(), a.Biblioteca.Itens[0].IdInterno!.Value)); Assert.Single(a.Servico.ObterObras(id)); }
    [Fact(DisplayName = "RA05 | Falha ao remover vínculo | Associação anterior permanece")]
    public void Falha() { var (a, id, _) = Preparar(); a.Arquivo.FalhaGravacao = new IOException(); Assert.Throws<IOException>(() => a.Servico.RemoverObra(id, a.Biblioteca.Itens[0].IdInterno!.Value)); Assert.Single(a.Servico.ObterObras(id)); }
    [Fact(DisplayName = "RA06 | Remover não grava biblioteca | Conteúdo e identidade da obra não mudam")]
    public void SemGravarBiblioteca() { var (a, id, _) = Preparar(); var n = a.ArquivoBiblioteca.Gravacoes; var item = a.Biblioteca.Itens[0]; a.Servico.RemoverObra(id, item.IdInterno!.Value); Assert.Equal(n, a.ArquivoBiblioteca.Gravacoes); Assert.Same(item, a.Biblioteca.Itens[0]); }
    [Fact(DisplayName = "RA07 | Duplicatas pessoais associadas | Remover segundo vínculo preserva primeira instância")]
    public void Duplicatas() { var um = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(um, um with { }); var id = a.Criar(); foreach (var i in a.Biblioteca.Itens.ToArray()) a.Servico.AdicionarObra(id, i); var primeiro = a.Biblioteca.Itens[0]; a.Servico.RemoverObra(id, a.Biblioteca.Itens[1].IdInterno!.Value); Assert.Same(primeiro, Assert.Single(a.Servico.ObterObras(id))); Assert.Equal(2, a.Biblioteca.Itens.Count); }
    [Fact(DisplayName = "RA08 | Remover e reiniciar | Vínculo continua ausente e obra continua disponível")]
    public void Reiniciar() { using var a = new ArquivosColecoes(AmbienteColecoes.Jogo()); var id = a.Servico.Criar("A"); a.Servico.AdicionarObra(id, a.Biblioteca.Itens[0]); a.Servico.RemoverObra(id, a.Biblioteca.Itens[0].IdInterno!.Value); var (b, c) = a.Reabrir(); Assert.Empty(c.ObterObras(id)); Assert.Single(b.Itens); }
    [Fact(DisplayName = "RA09 | Reassociar após remoção | Usa mesma identidade sem duplicar obra")]
    public void Reassociar() { var (a, id, i) = Preparar(); var guid = a.Biblioteca.Itens[0].IdInterno!.Value; a.Servico.RemoverObra(id, guid); a.Servico.AdicionarObra(id, i); Assert.Equal(guid, a.Servico.Colecoes[0].Obras[0]); Assert.Single(a.Biblioteca.Itens); }
    [Fact(DisplayName = "RA10 | Remover último vínculo | Coleção passa a vazia sem ser excluída")]
    public void Ultimo() { var (a, id, _) = Preparar(); a.Servico.RemoverObra(id, a.Biblioteca.Itens[0].IdInterno!.Value); Assert.Single(a.Servico.Colecoes); Assert.Empty(a.Servico.Colecoes[0].Obras); }
    [Fact(DisplayName = "RA11 | Referência de obra excluída da biblioteca | Pode ser removida explicitamente da coleção")]
    public void Ausente() { var (a, id, i) = Preparar(); var guid = a.Biblioteca.Itens[0].IdInterno!.Value; a.Biblioteca.Remover(i); Assert.Equal(1, a.Servico.ContarReferenciasAusentes(id)); a.Servico.RemoverObra(id, guid); Assert.Equal(0, a.Servico.ContarReferenciasAusentes(id)); }
    [Fact(DisplayName = "RA12 | Recuperar falha de remoção | Repetição funciona quando armazenamento volta")]
    public void Recuperar() { var (a, id, _) = Preparar(); var guid = a.Biblioteca.Itens[0].IdInterno!.Value; a.Arquivo.FalhaGravacao = new IOException(); Assert.Throws<IOException>(() => a.Servico.RemoverObra(id, guid)); a.Arquivo.FalhaGravacao = null; Assert.True(a.Servico.RemoverObra(id, guid)); }

    [Fact(DisplayName = "LC01 | Coleção vazia | Projeção retorna lista vazia")]
    public void Vazia() { var a = new AmbienteColecoes(); var id = a.Criar(); Assert.Empty(a.Servico.ObterObras(id)); }
    [Fact(DisplayName = "LC02 | Projeção de associação | Retorna instância da biblioteca em vez de duplicá-la")]
    public void MesmaInstancia() { var (a, id, _) = Preparar(); Assert.Same(a.Biblioteca.Itens[0], Assert.Single(a.Servico.ObterObras(id))); }
    [Fact(DisplayName = "LC03 | Editar dados pessoais após associar | Todas as coleções exibem a atualização")]
    public void Atualizar() { var (a, id, i) = Preparar(); var outro = a.Criar("Outra"); a.Servico.AdicionarObra(outro, i); a.Biblioteca.Atualizar(i, i.Pessoal with { Nota = 1, Review = "Nova" }); Assert.Equal(1, a.Servico.ObterObras(id)[0].Pessoal.Nota); Assert.Equal("Nova", a.Servico.ObterObras(outro)[0].Pessoal.Review); }
    [Fact(DisplayName = "LC04 | Coleção mista | Todas as seis mídias podem ser exibidas juntas")]
    public void Mista() { var itens = Enum.GetValues<TipoMidia>().Select(t => ApoioMultimidia.Item(t)).ToArray(); var a = new AmbienteColecoes(itens); var id = a.Criar(); foreach (var i in itens) a.Servico.AdicionarObra(id, i); Assert.Equal(Enum.GetValues<TipoMidia>(), a.Servico.ObterObras(id).Select(i => i.Catalogo.Tipo)); }
    [Fact(DisplayName = "LC05 | Ordem de associação | Projeção mantém ordem de inclusão na coleção")]
    public void Ordem() { var um = AmbienteColecoes.Jogo("A"); var dois = AmbienteColecoes.Jogo("B"); var a = new AmbienteColecoes(um, dois); var id = a.Criar(); a.Servico.AdicionarObra(id, dois); a.Servico.AdicionarObra(id, um); Assert.Equal(new[] { "B", "A" }, a.Servico.ObterObras(id).Select(i => i.Catalogo.Titulo)); }
    [Fact(DisplayName = "LC06 | Coleção desconhecida | Projeção recusa sem retornar outra coleção")]
    public void Desconhecida() { var a = new AmbienteColecoes(); Assert.Throws<ArgumentException>(() => a.Servico.ObterObras(Guid.NewGuid())); }
    [Fact(DisplayName = "LC07 | Obra removida da biblioteca | Projeção não mostra registro fantasma e conserva referência")]
    public void Excluida() { var (a, id, i) = Preparar(); a.Biblioteca.Remover(i); Assert.Empty(a.Servico.ObterObras(id)); Assert.Single(a.Servico.Colecoes[0].Obras); Assert.Equal(1, a.Servico.ContarReferenciasAusentes(id)); }
    [Fact(DisplayName = "LC08 | Restaurar obra com ID anterior | Referência preservada volta a resolver sem heurística por título")]
    public void Restaurar() { var (a, id, i) = Preparar(); var salvo = a.Biblioteca.Itens[0]; a.Biblioteca.Remover(i); a.Biblioteca.Adicionar(salvo); Assert.Same(salvo, Assert.Single(a.Servico.ObterObras(id))); }
    [Fact(DisplayName = "LC09 | Novo registro com mesmo título | Não ocupa automaticamente vínculo de obra excluída")]
    public void MesmoTitulo() { var (a, id, i) = Preparar(); a.Biblioteca.Remover(i); a.Biblioteca.Adicionar(AmbienteColecoes.Jogo()); Assert.Empty(a.Servico.ObterObras(id)); }
    [Fact(DisplayName = "LC10 | Consultar não grava | Arquivos e memória não são alterados pela visualização")]
    public void SomenteLeitura() { var (a, id, _) = Preparar(); var b = a.ArquivoBiblioteca.Gravacoes; var c = a.Arquivo.Gravacoes; a.Servico.ObterObras(id); Assert.Equal(b, a.ArquivoBiblioteca.Gravacoes); Assert.Equal(c, a.Arquivo.Gravacoes); }
    [Fact(DisplayName = "LC11 | Snapshot de associações | Lista exposta não permite mutação externa")]
    public void Imutavel() { var (a, _, _) = Preparar(); Assert.Throws<NotSupportedException>(() => ((IList<Guid>)a.Servico.Colecoes[0].Obras).Add(Guid.NewGuid())); }
    [Fact(DisplayName = "LC12 | Coleções e biblioteca recarregadas | Projeção conserva metadados, progresso e IDs")]
    public void RecargaMista() { using var a = new ArquivosColecoes(ApoioMultimidia.Item(TipoMidia.Livro, new() { Pagina = 10 })); var id = a.Servico.Criar("Leitura"); a.Servico.AdicionarObra(id, a.Biblioteca.Itens[0]); var (_, c) = a.Reabrir(); var i = Assert.Single(c.ObterObras(id)); Assert.Equal(10, i.Pessoal.Progresso!.Pagina); Assert.True(i.Catalogo.Simulado); Assert.Equal("Simulado", i.Catalogo.Origem); Assert.NotNull(i.IdInterno); }
}
