using BacklogManager.Models;
using BacklogManager.Services;
using Xunit;
namespace BacklogManager.Tests.Fase4;

public sealed class CriarEditarTests
{
    [Fact(DisplayName = "CC01 | Criar coleção normal | ID exclusivo, nome e descrição disponíveis")]
    public void Criar() { var a = new AmbienteColecoes(); var id = a.Criar("Favoritos", "Descrição"); var c = Assert.Single(a.Servico.Colecoes); Assert.Equal(id, c.Id); Assert.NotEqual(Guid.Empty, id); Assert.Equal("Favoritos", c.Nome); Assert.Equal("Descrição", c.Descricao); }
    [Fact(DisplayName = "CC02 | Nome e descrição com espaços externos | Valores são aparados antes de salvar")]
    public void Aparar() { var a = new AmbienteColecoes(); a.Criar(" Favoritos ", " Descrição "); Assert.Equal("Favoritos", a.Servico.Colecoes[0].Nome); Assert.Equal("Descrição", a.Servico.Colecoes[0].Descricao); }
    [Fact(DisplayName = "CC03 | Nome vazio | Recusa sem criar coleção")]
    public void Vazio() { var a = new AmbienteColecoes(); Assert.Throws<ArgumentException>(() => a.Criar("")); Assert.Empty(a.Servico.Colecoes); }
    [Fact(DisplayName = "CC04 | Nome só com espaços | Recusa sem gravação")]
    public void Espacos() { var a = new AmbienteColecoes(); Assert.Throws<ArgumentException>(() => a.Criar("  ")); Assert.Equal(0, a.Arquivo.Gravacoes); }
    [Fact(DisplayName = "CC05 | Nome de 120 caracteres | Limite aceito sem truncamento")]
    public void LimiteNome() { var a = new AmbienteColecoes(); a.Criar(new string('a', 120)); Assert.Equal(120, a.Servico.Colecoes[0].Nome.Length); }
    [Fact(DisplayName = "CC06 | Nome de 121 caracteres | Recusa sem truncar ou salvar")]
    public void NomeLongo() { var a = new AmbienteColecoes(); Assert.Throws<ArgumentException>(() => a.Criar(new string('a', 121))); Assert.Equal(0, a.Arquivo.Gravacoes); }
    [Fact(DisplayName = "CC07 | Nome duplicado com caixa e espaços diferentes | Segunda coleção recusada")]
    public void Duplicado() { var a = new AmbienteColecoes(); a.Criar(); Assert.Throws<ArgumentException>(() => a.Criar(" FAVORITOS ")); Assert.Single(a.Servico.Colecoes); }
    [Fact(DisplayName = "CC08 | Descrição opcional | Ausência vira string vazia sem valor inventado")]
    public void SemDescricao() { var a = new AmbienteColecoes(); a.Servico.Criar("Nome", null); Assert.Equal("", a.Servico.Colecoes[0].Descricao); }
    [Fact(DisplayName = "CC09 | Descrição no limite e acima dele | Mil aceitos e mil e um recusados")]
    public void DescricaoLimite() { var a = new AmbienteColecoes(); a.Criar("A", new string('x', 1000)); Assert.Throws<ArgumentException>(() => a.Criar("B", new string('x', 1001))); Assert.Single(a.Servico.Colecoes); }
    [Fact(DisplayName = "CC10 | Nome Unicode | Acentos e símbolos preservados")]
    public void Unicode() { var a = new AmbienteColecoes(); a.Criar("Ação 🎮 日本語"); Assert.Equal("Ação 🎮 日本語", a.Servico.Colecoes[0].Nome); }
    [Fact(DisplayName = "CC11 | Falha externa ao criar | Coleção não aparece na memória sem persistência")]
    public void Falha() { var a = new AmbienteColecoes(); a.Arquivo.FalhaGravacao = new IOException(); Assert.Throws<IOException>(() => a.Criar()); Assert.Empty(a.Servico.Colecoes); }
    [Fact(DisplayName = "CC12 | Criar sem carregamento válido | Operação bloqueada antes de gravar")]
    public void Bloqueado() { var a = new AmbienteColecoes(); var s = new ServicoColecoes(a.Arquivo, a.Biblioteca); Assert.Throws<InvalidOperationException>(() => s.Criar("Nome")); Assert.Equal(0, a.Arquivo.Gravacoes); }
    [Fact(DisplayName = "CC13 | Nome com quebra de linha | Recusa mensagem ilegível na seleção")]
    public void QuebraNome() { var a = new AmbienteColecoes(); Assert.Throws<ArgumentException>(() => a.Criar("Nome\nOutra linha")); }
    [Fact(DisplayName = "CC14 | Criar não grava biblioteca | Nenhuma obra recebe ID apenas por criar coleção")]
    public void SemObras() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); a.Criar(); Assert.Equal(0, a.ArquivoBiblioteca.Gravacoes); Assert.Null(a.Biblioteca.Itens[0].IdInterno); }

    [Fact(DisplayName = "CE01 | Renomear coleção | ID permanece o mesmo")]
    public void Renomear() { var a = new AmbienteColecoes(); var id = a.Criar(); a.Servico.Editar(id, "Novo"); Assert.Equal(id, a.Servico.Colecoes[0].Id); Assert.Equal("Novo", a.Servico.Colecoes[0].Nome); }
    [Fact(DisplayName = "CE02 | Editar descrição | Texto é persistido no snapshot novo")]
    public void EditarDescricao() { var a = new AmbienteColecoes(); var id = a.Criar(); a.Servico.Editar(id, "Favoritos", "Nova descrição"); Assert.Equal("Nova descrição", a.Arquivo.Registros[0].Descricao); }
    [Fact(DisplayName = "CE03 | Renomear coleção com obras | Associações permanecem")]
    public void ManterObras() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); var id = a.Criar(); a.Servico.AdicionarObra(id, i); var obraId = a.Servico.Colecoes[0].Obras[0]; a.Servico.Editar(id, "Novo"); Assert.Equal(obraId, Assert.Single(a.Servico.Colecoes[0].Obras)); }
    [Fact(DisplayName = "CE04 | Editar para mesmo nome | Atualização válida sem criar coleção extra")]
    public void MesmoNome() { var a = new AmbienteColecoes(); var id = a.Criar(); a.Servico.Editar(id, "Favoritos"); Assert.Single(a.Servico.Colecoes); }
    [Fact(DisplayName = "CE05 | Renomear para nome de outra coleção | Recusa e mantém nomes anteriores")]
    public void NomeOutra() { var a = new AmbienteColecoes(); var id = a.Criar("A"); a.Criar("B"); Assert.Throws<ArgumentException>(() => a.Servico.Editar(id, "b")); Assert.Equal(new[] { "A", "B" }, a.Servico.Colecoes.Select(c => c.Nome)); }
    [Fact(DisplayName = "CE06 | Renomear para vazio | Nome original preservado")]
    public void NomeVazio() { var a = new AmbienteColecoes(); var id = a.Criar(); Assert.Throws<ArgumentException>(() => a.Servico.Editar(id, "")); Assert.Equal("Favoritos", a.Servico.Colecoes[0].Nome); }
    [Fact(DisplayName = "CE07 | Renomear acima do limite | Nome original não é truncado")]
    public void NomeExcessivo() { var a = new AmbienteColecoes(); var id = a.Criar(); Assert.Throws<ArgumentException>(() => a.Servico.Editar(id, new string('x', 121))); Assert.Equal("Favoritos", a.Servico.Colecoes[0].Nome); }
    [Fact(DisplayName = "CE08 | Editar descrição acima do limite | Descrição anterior preservada")]
    public void DescricaoExcessiva() { var a = new AmbienteColecoes(); var id = a.Criar("A", "Antiga"); Assert.Throws<ArgumentException>(() => a.Servico.Editar(id, "A", new string('x', 1001))); Assert.Equal("Antiga", a.Servico.Colecoes[0].Descricao); }
    [Fact(DisplayName = "CE09 | Editar coleção inexistente | Nenhuma coleção ou arquivo novo")]
    public void Inexistente() { var a = new AmbienteColecoes(); Assert.Throws<ArgumentException>(() => a.Servico.Editar(Guid.NewGuid(), "A")); Assert.Equal(0, a.Arquivo.Gravacoes); }
    [Fact(DisplayName = "CE10 | Falha ao editar | Snapshot anterior permanece inteiro")]
    public void FalhaEditar() { var a = new AmbienteColecoes(); var id = a.Criar(); var antes = a.Servico.Colecoes; a.Arquivo.FalhaGravacao = new IOException(); Assert.Throws<IOException>(() => a.Servico.Editar(id, "Novo")); Assert.Same(antes, a.Servico.Colecoes); }
    [Fact(DisplayName = "CE11 | Aparar e limpar descrição | Valores normalizados sem mudar identidade")]
    public void LimparDescricao() { var a = new AmbienteColecoes(); var id = a.Criar("A", "Antiga"); a.Servico.Editar(id, " A nova ", " "); Assert.Equal("A nova", a.Servico.Colecoes[0].Nome); Assert.Equal("", a.Servico.Colecoes[0].Descricao); }
    [Fact(DisplayName = "CE12 | Editar e reiniciar | Nome, descrição, ID e associações recarregam")]
    public void Reiniciar() { using var a = new ArquivosColecoes(AmbienteColecoes.Jogo()); var id = a.Servico.Criar("A"); a.Servico.AdicionarObra(id, a.Biblioteca.Itens[0]); a.Servico.Editar(id, "B", "Descrição"); var (b, c) = a.Reabrir(); Assert.Equal("B", c.Colecoes[0].Nome); Assert.Equal("Descrição", c.Colecoes[0].Descricao); Assert.Equal(id, c.Colecoes[0].Id); Assert.Same(b.Itens[0], Assert.Single(c.ObterObras(id))); }
}
