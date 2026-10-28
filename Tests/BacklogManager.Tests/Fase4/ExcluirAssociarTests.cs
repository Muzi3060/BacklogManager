using BacklogManager.Models;
using BacklogManager.Tests.Fase2;
using Xunit;
namespace BacklogManager.Tests.Fase4;

public sealed class ExcluirAssociarTests
{
    [Fact(DisplayName = "CD01 | Excluir coleção vazia | Somente a coleção desaparece")]
    public void Vazia() { var a = new AmbienteColecoes(); var id = a.Criar(); a.Servico.Excluir(id); Assert.Empty(a.Servico.Colecoes); }
    [Fact(DisplayName = "CD02 | Excluir coleção com obra | Obra permanece na biblioteca")]
    public void ComObra() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); var id = a.Criar(); a.Servico.AdicionarObra(id, i); a.Servico.Excluir(id); Assert.Single(a.Biblioteca.Itens); Assert.Empty(a.Servico.Colecoes); }
    [Fact(DisplayName = "CD03 | Obra em duas coleções | Excluir uma preserva outra e sua associação")]
    public void Duas() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); var x = a.Criar("A"); var y = a.Criar("B"); a.Servico.AdicionarObra(x, i); a.Servico.AdicionarObra(y, i); a.Servico.Excluir(x); Assert.Single(a.Servico.ObterObras(y)); Assert.Single(a.Biblioteca.Itens); }
    [Fact(DisplayName = "CD04 | Excluir coleção inexistente | Nenhum estado é alterado")]
    public void Inexistente() { var a = new AmbienteColecoes(); a.Criar(); var antes = a.Servico.Colecoes; Assert.Throws<ArgumentException>(() => a.Servico.Excluir(Guid.NewGuid())); Assert.Same(antes, a.Servico.Colecoes); }
    [Fact(DisplayName = "CD05 | Falha ao excluir | Coleção e vínculos anteriores permanecem")]
    public void Falha() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); var id = a.Criar(); a.Servico.AdicionarObra(id, i); a.Arquivo.FalhaGravacao = new IOException(); Assert.Throws<IOException>(() => a.Servico.Excluir(id)); Assert.Single(a.Servico.ObterObras(id)); }
    [Fact(DisplayName = "CD06 | Excluir sem biblioteca carregada | Operação bloqueada")]
    public void BibliotecaBloqueada() { var a = new AmbienteColecoes(); var id = a.Criar(); a.ArquivoBiblioteca.FalhaLeitura = new IOException(); Assert.Throws<IOException>(() => a.Biblioteca.Carregar()); Assert.Throws<InvalidOperationException>(() => a.Servico.Excluir(id)); Assert.Single(a.Servico.Colecoes); }
    [Fact(DisplayName = "CD07 | Excluir última coleção | Biblioteca e identificadores permanecem")]
    public void Ultima() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); var id = a.Criar(); a.Servico.AdicionarObra(id, i); var obraId = a.Biblioteca.Itens[0].IdInterno; a.Servico.Excluir(id); Assert.Equal(obraId, a.Biblioteca.Itens[0].IdInterno); }
    [Fact(DisplayName = "CD08 | Excluir não grava biblioteca | Arquivo de obras não recebe nova gravação")]
    public void SemGravarBiblioteca() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); var id = a.Criar(); a.Servico.AdicionarObra(id, i); var n = a.ArquivoBiblioteca.Gravacoes; a.Servico.Excluir(id); Assert.Equal(n, a.ArquivoBiblioteca.Gravacoes); }
    [Fact(DisplayName = "CD09 | Excluir e reiniciar | Exclusão persistida e obras recarregadas")]
    public void Reiniciar() { using var a = new ArquivosColecoes(AmbienteColecoes.Jogo()); var id = a.Servico.Criar("A"); a.Servico.AdicionarObra(id, a.Biblioteca.Itens[0]); a.Servico.Excluir(id); var (b, c) = a.Reabrir(); Assert.Empty(c.Colecoes); Assert.Single(b.Itens); }
    [Fact(DisplayName = "CD10 | Coleção com seis mídias | Excluir coleção não remove nenhuma categoria")]
    public void Multimidia() { var itens = Enum.GetValues<TipoMidia>().Select(t => ApoioMultimidia.Item(t)).ToArray(); var a = new AmbienteColecoes(itens); var id = a.Criar(); foreach (var i in itens) a.Servico.AdicionarObra(id, i); a.Servico.Excluir(id); Assert.Equal(6, a.Biblioteca.Itens.Count); }
    [Fact(DisplayName = "CD11 | Reutilizar nome de coleção excluída | Nova coleção tem identidade diferente")]
    public void ReutilizarNome() { var a = new AmbienteColecoes(); var id = a.Criar(); a.Servico.Excluir(id); Assert.NotEqual(id, a.Criar()); }
    [Fact(DisplayName = "CD12 | Falha seguida de recuperação | Excluir funciona após restaurar o armazenamento")]
    public void Recuperar() { var a = new AmbienteColecoes(); var id = a.Criar(); a.Arquivo.FalhaGravacao = new IOException(); Assert.Throws<IOException>(() => a.Servico.Excluir(id)); a.Arquivo.FalhaGravacao = null; a.Servico.Excluir(id); Assert.Empty(a.Servico.Colecoes); }

    [Fact(DisplayName = "AC01 | Associar obra existente | Vínculo criado sem duplicar biblioteca")]
    public void Associar() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); var id = a.Criar(); Assert.True(a.Servico.AdicionarObra(id, i)); Assert.Single(a.Biblioteca.Itens); Assert.Single(a.Servico.ObterObras(id)); }
    [Fact(DisplayName = "AC02 | Uma obra em duas coleções | Mesma identidade e uma única obra pessoal")]
    public void Varias() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); var x = a.Criar("A"); var y = a.Criar("B"); a.Servico.AdicionarObra(x, i); a.Servico.AdicionarObra(y, i); Assert.Equal(a.Servico.Colecoes[0].Obras[0], a.Servico.Colecoes[1].Obras[0]); Assert.Single(a.Biblioteca.Itens); }
    [Fact(DisplayName = "AC03 | Associar novamente mesma obra | Operação idempotente sem nova gravação")]
    public void Repetir() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); var id = a.Criar(); a.Servico.AdicionarObra(id, i); var n = a.Arquivo.Gravacoes; Assert.False(a.Servico.AdicionarObra(id, i)); Assert.Equal(n, a.Arquivo.Gravacoes); }
    [Fact(DisplayName = "AC04 | Registros iguais na biblioteca | Instâncias recebem identidades e vínculos independentes")]
    public void Duplicatas() { var um = AmbienteColecoes.Jogo(); var dois = um with { }; var a = new AmbienteColecoes(um, dois); var id = a.Criar(); a.Servico.AdicionarObra(id, um); a.Servico.AdicionarObra(id, dois); Assert.Equal(2, a.Servico.ObterObras(id).Count); Assert.NotEqual(a.Biblioteca.Itens[0].IdInterno, a.Biblioteca.Itens[1].IdInterno); }
    [Fact(DisplayName = "AC05 | Associar à coleção inexistente | Nenhuma identidade ou gravação na biblioteca")]
    public void ColecaoAusente() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); Assert.Throws<ArgumentException>(() => a.Servico.AdicionarObra(Guid.NewGuid(), i)); Assert.Equal(0, a.ArquivoBiblioteca.Gravacoes); }
    [Fact(DisplayName = "AC06 | Obra que não pertence à biblioteca | Recusa sem adicionar registro manual")]
    public void ObraAusente() { var a = new AmbienteColecoes(); var id = a.Criar(); Assert.Throws<InvalidOperationException>(() => a.Servico.AdicionarObra(id, AmbienteColecoes.Jogo())); Assert.Empty(a.Biblioteca.Itens); }
    [Fact(DisplayName = "AC07 | Falha ao salvar identidade da obra | Nenhum vínculo é salvo")]
    public void FalhaBiblioteca() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); var id = a.Criar(); a.ArquivoBiblioteca.FalhaGravacao = new IOException(); Assert.Throws<IOException>(() => a.Servico.AdicionarObra(id, i)); Assert.Empty(a.Servico.Colecoes[0].Obras); Assert.Null(a.Biblioteca.Itens[0].IdInterno); }
    [Fact(DisplayName = "AC08 | Falha no segundo arquivo | Identidade salva permanece e associação não é aplicada")]
    public void FalhaColecoes() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); var id = a.Criar(); a.Arquivo.FalhaGravacao = new IOException(); Assert.Throws<IOException>(() => a.Servico.AdicionarObra(id, i)); Assert.NotNull(a.Biblioteca.Itens[0].IdInterno); Assert.Empty(a.Servico.Colecoes[0].Obras); }
    [Fact(DisplayName = "AC09 | Tentar novamente após falha no segundo arquivo | Identidade é reaproveitada e vínculo é criado")]
    public void RecuperarAssociacao() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); var id = a.Criar(); a.Arquivo.FalhaGravacao = new IOException(); Assert.Throws<IOException>(() => a.Servico.AdicionarObra(id, i)); var guid = a.Biblioteca.Itens[0].IdInterno; a.Arquivo.FalhaGravacao = null; a.Servico.AdicionarObra(id, i); Assert.Equal(guid, a.Servico.Colecoes[0].Obras[0]); Assert.Equal(1, a.ArquivoBiblioteca.Gravacoes); }
    [Fact(DisplayName = "AC10 | Seis categorias na mesma coleção | Vínculos independem do tipo oficial de mídia")]
    public void SeisMidias() { var itens = Enum.GetValues<TipoMidia>().Select(t => ApoioMultimidia.Item(t)).ToArray(); var a = new AmbienteColecoes(itens); var id = a.Criar(); foreach (var i in itens) a.Servico.AdicionarObra(id, i); Assert.Equal(6, a.Servico.ObterObras(id).Count); Assert.Equal(6, a.Biblioteca.Itens.Count); }
    [Fact(DisplayName = "AC11 | Registro legado sem ID de catálogo | Associação não exige IGDB ou recadastro")]
    public void Legado() { var i = AmbienteColecoes.Jogo() with { Catalogo = new ObraCatalogo { Titulo = "Legado" }, SteamAppIdLegado = 361855 }; var a = new AmbienteColecoes(i); var id = a.Criar(); a.Servico.AdicionarObra(id, i); Assert.Null(a.Biblioteca.Itens[0].Catalogo.IdentificadorExterno); Assert.Equal(361855, a.Biblioteca.Itens[0].SteamAppIdLegado); }
    [Fact(DisplayName = "AC12 | Associar e reiniciar | Associação aponta para mesma obra com dados pessoais preservados")]
    public void Reabrir() { using var a = new ArquivosColecoes(AmbienteColecoes.Jogo()); var id = a.Servico.Criar("A"); a.Servico.AdicionarObra(id, a.Biblioteca.Itens[0]); var (b, c) = a.Reabrir(); Assert.Same(b.Itens[0], Assert.Single(c.ObterObras(id))); Assert.Equal("Review antiga", c.ObterObras(id)[0].Pessoal.Review); }
    [Fact(DisplayName = "AC13 | Associação com ID já persistido | Biblioteca não é regravada desnecessariamente")]
    public void Identificada() { var i = AmbienteColecoes.Jogo() with { IdInterno = Guid.NewGuid() }; var a = new AmbienteColecoes(i); var id = a.Criar(); a.Servico.AdicionarObra(id, i); Assert.Equal(0, a.ArquivoBiblioteca.Gravacoes); }
    [Fact(DisplayName = "AC14 | Remover obra da biblioteca e cadastrar mesmo catálogo | Nova instância não herda vínculo antigo")]
    public void NovaInstancia() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); var id = a.Criar(); a.Servico.AdicionarObra(id, i); a.Biblioteca.Remover(i); a.Biblioteca.Adicionar(AmbienteColecoes.Jogo()); Assert.Empty(a.Servico.ObterObras(id)); Assert.Equal(1, a.Servico.ContarReferenciasAusentes(id)); }
}
