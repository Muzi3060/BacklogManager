using System.Text.Json;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Tests.Fase1;
using BacklogManager.Tests.Fase2;
using Xunit;
namespace BacklogManager.Tests.Fase4;

public sealed class IdentidadeTests
{
    [Fact(DisplayName = "GI01 | Carregar legado | Nenhum ID interno ou gravação automática")]
    public void Legado() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); Assert.Null(a.Biblioteca.Itens[0].IdInterno); Assert.Equal(0, a.ArquivoBiblioteca.Gravacoes); }
    [Fact(DisplayName = "GI02 | Identificar explicitamente | GUID não vazio salvo e disponível")]
    public void Identificar() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); var id = a.Biblioteca.GarantirIdentidade(i); Assert.NotEqual(Guid.Empty, id); Assert.Equal(id, a.ArquivoBiblioteca.Registros[0].IdInterno); }
    [Fact(DisplayName = "GI03 | Identificar repetidamente | Mesmo ID sem outra gravação")]
    public void Repetir() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); var id = a.Biblioteca.GarantirIdentidade(i); Assert.Equal(id, a.Biblioteca.GarantirIdentidade(i)); Assert.Equal(1, a.ArquivoBiblioteca.Gravacoes); }
    [Fact(DisplayName = "GI04 | Identidade já persistida | GUID existente nunca é substituído")]
    public void Existente() { var id = Guid.NewGuid(); var i = AmbienteColecoes.Jogo() with { IdInterno = id }; var a = new AmbienteColecoes(i); Assert.Equal(id, a.Biblioteca.GarantirIdentidade(i)); Assert.Equal(0, a.ArquivoBiblioteca.Gravacoes); }
    [Fact(DisplayName = "GI05 | Duplicatas sem ID | Cada instância recebe GUID diferente")]
    public void Duplicatas() { var um = AmbienteColecoes.Jogo(); var dois = um with { }; var a = new AmbienteColecoes(um, dois); Assert.NotEqual(a.Biblioteca.GarantirIdentidade(um), a.Biblioteca.GarantirIdentidade(dois)); }
    [Fact(DisplayName = "GI06 | Falha na gravação de ID | Instância anterior continua sem ID e sem alias confirmado")]
    public void Falha() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); a.ArquivoBiblioteca.FalhaGravacao = new IOException(); Assert.Throws<IOException>(() => a.Biblioteca.GarantirIdentidade(i)); Assert.Same(i, a.Biblioteca.Itens[0]); Assert.Null(i.IdInterno); }
    [Fact(DisplayName = "GI07 | Recuperar após falha de ID | Identificação funciona sem duplicar registro")]
    public void Recuperar() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); a.ArquivoBiblioteca.FalhaGravacao = new IOException(); Assert.Throws<IOException>(() => a.Biblioteca.GarantirIdentidade(i)); a.ArquivoBiblioteca.FalhaGravacao = null; Assert.NotEqual(Guid.Empty, a.Biblioteca.GarantirIdentidade(i)); Assert.Single(a.Biblioteca.Itens); }
    [Fact(DisplayName = "GI08 | Instância anterior à identificação | Edição encontra obra atual e preserva GUID")]
    public void AliasEditar() { var i = AmbienteColecoes.Jogo(); var a = new AmbienteColecoes(i); var id = a.Biblioteca.GarantirIdentidade(i); a.Biblioteca.Atualizar(i, i.Pessoal with { Nota = 1 }); Assert.Equal(id, a.Biblioteca.Itens[0].IdInterno); Assert.Equal(1, a.Biblioteca.Itens[0].Pessoal.Nota); }
    [Fact(DisplayName = "GI09 | Instância anterior à identificação | Remoção local encontra somente a obra correspondente")]
    public void AliasRemover() { var i = AmbienteColecoes.Jogo(); var outro = i with { }; var a = new AmbienteColecoes(i, outro); a.Biblioteca.GarantirIdentidade(i); a.Biblioteca.Remover(i); Assert.Same(outro, Assert.Single(a.Biblioteca.Itens)); }
    [Fact(DisplayName = "GI10 | Identificador de obra externa | Não é atribuído a objeto sem relação com a biblioteca")]
    public void Externa() { var a = new AmbienteColecoes(AmbienteColecoes.Jogo()); Assert.Throws<InvalidOperationException>(() => a.Biblioteca.GarantirIdentidade(AmbienteColecoes.Jogo())); }
    [Fact(DisplayName = "GI11 | Roundtrip do identificador | GUID persiste com todos os campos pessoais")]
    public void Roundtrip() { var i = AmbienteColecoes.Jogo() with { IdInterno = Guid.NewGuid(), SteamAppIdLegado = 361855 }; var r = ApoioMultimidia.Roundtrip(i); Assert.Equal(i.IdInterno, r.IdInterno); Assert.Equal(i.Pessoal, r.Pessoal); Assert.Equal(361855, r.SteamAppIdLegado); }
    [Fact(DisplayName = "GI12 | Legado sem ID no JSON | Propriedade nova é omitida sem migração")]
    public void Omitir() => Assert.DoesNotContain("IdInterno", JsonSerializer.Serialize(ConversorBibliotecaJson.ParaRegistro(AmbienteColecoes.Jogo())));
    [Fact(DisplayName = "GI13 | GUID vazio no arquivo | Leitura bloqueada e bytes preservados")]
    public void GuidVazio() { using var p = new PastaTeste(); var dto = PastaTeste.Jogo(); dto.IdInterno = Guid.Empty; var json = JsonSerializer.Serialize(new[] { dto }); File.WriteAllText(p.Arquivo, json); var f = new FileManager(p.Arquivo); Assert.Throws<InvalidDataException>(() => f.Carregar()); Assert.Equal(json, File.ReadAllText(p.Arquivo)); }
    [Fact(DisplayName = "GI14 | Dois registros com mesmo GUID | Leitura recusa sem deduplicar ou excluir")]
    public void GuidDuplicado() { using var p = new PastaTeste(); var dto = PastaTeste.Jogo(); dto.IdInterno = Guid.NewGuid(); var json = JsonSerializer.Serialize(new[] { dto, dto }); File.WriteAllText(p.Arquivo, json); Assert.Throws<InvalidDataException>(() => new FileManager(p.Arquivo).Carregar()); Assert.Equal(json, File.ReadAllText(p.Arquivo)); }
    [Fact(DisplayName = "GI15 | GUID inválido no texto JSON | Leitura controlada sem gravar")]
    public void GuidInvalido() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "[{\"Titulo\":\"A\",\"Categoria\":\"RPG\",\"Status\":\"Na Fila\",\"Review\":\"\",\"IdInterno\":\"invalido\"}]"); Assert.Throws<InvalidDataException>(() => new FileManager(p.Arquivo).Carregar()); }
    [Fact(DisplayName = "GI16 | Adição com GUID duplicado | Serviço não grava candidato inválido")]
    public void AdicionarDuplicado() { var i = AmbienteColecoes.Jogo() with { IdInterno = Guid.NewGuid() }; var a = new AmbienteColecoes(i); Assert.Throws<InvalidDataException>(() => a.Biblioteca.Adicionar(i with { })); Assert.Single(a.Biblioteca.Itens); }
    [Fact(DisplayName = "GI17 | GUID reservado em extensão JSON | Persistência recusa duplicação de propriedade")]
    public void ExtensaoReservada() { using var p = new PastaTeste(); var f = new FileManager(p.Arquivo); f.Carregar(); using var j = JsonDocument.Parse("\"outro\""); var dto = PastaTeste.Jogo(); dto.DadosAdicionais = new Dictionary<string, JsonElement> { ["IdInterno"] = j.RootElement.Clone() }; Assert.Throws<InvalidDataException>(() => f.Salvar(new[] { dto })); Assert.False(File.Exists(p.Arquivo)); }
    [Fact(DisplayName = "GI18 | ID interno não interpreta IDs externos | Steam e IGDB permanecem distintos")]
    public void Externos() { var i = AmbienteColecoes.Jogo() with { SteamAppIdLegado = 361855 }; var a = new AmbienteColecoes(i); a.Biblioteca.GarantirIdentidade(i); var r = ConversorBibliotecaJson.ParaRegistro(a.Biblioteca.Itens[0]); Assert.Equal(361855, r.SteamAppId); Assert.Equal(42, r.IgdbId); }
    [Fact(DisplayName = "GI19 | Recarga da biblioteca | Identidades persistidas continuam estáveis")]
    public void Reabrir() { using var a = new ArquivosColecoes(AmbienteColecoes.Jogo()); var id = a.Biblioteca.GarantirIdentidade(a.Biblioteca.Itens[0]); var (b, _) = a.Reabrir(); Assert.Equal(id, b.Itens[0].IdInterno); }
    [Fact(DisplayName = "GI20 | Arquivo alterado externamente antes de identificar | Nenhuma associação sobrescreve a versão externa")]
    public void Conflito() { using var a = new ArquivosColecoes(AmbienteColecoes.Jogo()); var c = a.Servico.Criar("A"); File.AppendAllText(a.Pasta.Arquivo, " "); var externo = File.ReadAllBytes(a.Pasta.Arquivo); Assert.Throws<ArquivoAlteradoException>(() => a.Servico.AdicionarObra(c, a.Biblioteca.Itens[0])); Assert.Equal(externo, File.ReadAllBytes(a.Pasta.Arquivo)); Assert.Empty(a.Servico.Colecoes[0].Obras); }
}
