using System.Text;
using System.Text.Json;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Services;
using Xunit;
namespace BacklogManager.Tests.Fase4;

public sealed class PersistenciaColecoesTests
{
    private static ColecaoPersonalizada Modelo() => new(Guid.NewGuid(), "Favoritos", "Descrição", new[] { Guid.NewGuid() });
    private static string Caminho(PastaTeste p) => Path.Combine(p.Pasta, "backlog.colecoes.json");
    private static void Invalido(string json)
    {
        using var p = new PastaTeste(); File.WriteAllText(Caminho(p), json); var a = new ArmazenamentoColecoesJson(Caminho(p));
        Assert.Throws<InvalidDataException>(() => a.Carregar()); Assert.Throws<InvalidOperationException>(() => a.Salvar(Array.Empty<ColecaoPersonalizada>())); Assert.Equal(json, File.ReadAllText(Caminho(p)));
    }
    [Fact(DisplayName = "JS01 | Arquivo de coleções inexistente | Leitura vazia sem criação automática")]
    public void Inexistente() { using var p = new PastaTeste(); Assert.Empty(new ArmazenamentoColecoesJson(Caminho(p)).Carregar()); Assert.False(File.Exists(Caminho(p))); }
    [Fact(DisplayName = "JS02 | Salvar primeira coleção | Documento versionado e vínculos recarregáveis")]
    public void Primeiro() { using var p = new PastaTeste(); var a = new ArmazenamentoColecoesJson(Caminho(p)); a.Carregar(); var c = Modelo(); a.Salvar(new[] { c }); var r = Assert.Single(new ArmazenamentoColecoesJson(Caminho(p)).Carregar()); Assert.Equal(c.Id, r.Id); Assert.Equal(c.Obras, r.Obras); Assert.Contains("\"Versao\": 1", File.ReadAllText(Caminho(p))); }
    [Fact(DisplayName = "JS03 | Atualizar arquivo | Backup contém bytes da versão anterior")]
    public void Backup() { using var p = new PastaTeste(); var a = new ArmazenamentoColecoesJson(Caminho(p)); a.Carregar(); a.Salvar(new[] { Modelo() }); var antes = File.ReadAllBytes(Caminho(p)); a.Salvar(Array.Empty<ColecaoPersonalizada>()); Assert.Equal(antes, File.ReadAllBytes(Caminho(p) + ".bak")); Assert.Empty(a.Carregar()); }
    [Fact(DisplayName = "JS04 | Salvar sem ler antes | Operação bloqueada sem arquivo novo")]
    public void SemCarregar() { using var p = new PastaTeste(); Assert.Throws<InvalidOperationException>(() => new ArmazenamentoColecoesJson(Caminho(p)).Salvar(new[] { Modelo() })); Assert.False(File.Exists(Caminho(p))); }
    [Fact(DisplayName = "JS05 | JSON truncado | Leitura controlada e gravação bloqueada")]
    public void Truncado() => Invalido("{");
    [Fact(DisplayName = "JS06 | Documento null | Não substitui arquivo por coleção vazia")]
    public void Nulo() => Invalido("null");
    [Fact(DisplayName = "JS07 | Versão desconhecida | Nenhum campo futuro é descartado")]
    public void Versao() => Invalido("{\"Versao\":2,\"Colecoes\":[]}");
    [Fact(DisplayName = "JS08 | Campo futuro desconhecido | Documento preservado sem sobrescrita")]
    public void Futuro() => Invalido("{\"Versao\":1,\"Colecoes\":[],\"Futuro\":true}");
    [Fact(DisplayName = "JS09 | Campo obrigatório ausente | Recusa sem assumir valor padrão")]
    public void Ausente() => Invalido("{\"Versao\":1}");
    [Fact(DisplayName = "JS10 | Coleção nula na lista | Arquivo inválido não é filtrado ou truncado")]
    public void ColecaoNula() => Invalido("{\"Versao\":1,\"Colecoes\":[null]}");
    [Fact(DisplayName = "JS11 | GUID malformado | Erro controlado preserva original")]
    public void GuidInvalido() => Invalido("{\"Versao\":1,\"Colecoes\":[{\"Id\":\"invalido\",\"Nome\":\"A\",\"Descricao\":\"\",\"Obras\":[]}]}");
    [Fact(DisplayName = "JS12 | IDs de coleção duplicados | Recusa sem deduplicar")]
    public void IdDuplicado() { var c = Modelo(); Invalido(JsonSerializer.Serialize(new { Versao = 1, Colecoes = new[] { c, c with { Nome = "Outro" } } })); }
    [Fact(DisplayName = "JS13 | Nomes duplicados no disco | Recusa sem escolher uma coleção arbitrária")]
    public void NomeDuplicado() { var c = Modelo(); Invalido(JsonSerializer.Serialize(new { Versao = 1, Colecoes = new[] { c, c with { Id = Guid.NewGuid(), Nome = " FAVORITOS " } } })); }
    [Fact(DisplayName = "JS14 | Associação repetida no documento | Recusa sem remover vínculo automaticamente")]
    public void VinculoDuplicado() { var c = Modelo(); c = c with { Obras = new[] { c.Obras[0], c.Obras[0] } }; Invalido(JsonSerializer.Serialize(new { Versao = 1, Colecoes = new[] { c } })); }
    [Fact(DisplayName = "JS15 | Associação de GUID vazio | Recusa metadado inválido")]
    public void GuidVazio() { var c = Modelo() with { Obras = new[] { Guid.Empty } }; Invalido(JsonSerializer.Serialize(new { Versao = 1, Colecoes = new[] { c } })); }
    [Fact(DisplayName = "JS16 | Duas instâncias alteram coleções | Segunda gravação detecta conflito e preserva primeira")]
    public void Conflito() { using var p = new PastaTeste(); var a = new ArmazenamentoColecoesJson(Caminho(p)); var b = new ArmazenamentoColecoesJson(Caminho(p)); a.Carregar(); b.Carregar(); a.Salvar(new[] { Modelo() }); var antes = File.ReadAllBytes(Caminho(p)); Assert.Throws<ArquivoAlteradoException>(() => b.Salvar(Array.Empty<ColecaoPersonalizada>())); Assert.Equal(antes, File.ReadAllBytes(Caminho(p))); }
    [Fact(DisplayName = "JS17 | Arquivo removido externamente | Gravação não recria e sobrescreve silenciosamente")]
    public void Removido() { using var p = new PastaTeste(); var a = new ArmazenamentoColecoesJson(Caminho(p)); a.Carregar(); a.Salvar(new[] { Modelo() }); File.Delete(Caminho(p)); Assert.Throws<ArquivoAlteradoException>(() => a.Salvar(Array.Empty<ColecaoPersonalizada>())); Assert.False(File.Exists(Caminho(p))); }
    [Fact(DisplayName = "JS18 | Arquivo de lock em uso | Gravação falha sem mudar documento")]
    public void Bloqueio() { using var p = new PastaTeste(); var a = new ArmazenamentoColecoesJson(Caminho(p)); a.Carregar(); using var bloqueio = new FileStream(Caminho(p) + ".lock", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None); Assert.ThrowsAny<IOException>(() => a.Salvar(new[] { Modelo() })); Assert.False(File.Exists(Caminho(p))); }
    [Fact(DisplayName = "JS19 | Destino do backup inválido | Documento anterior preservado e temporário removido")]
    public void FalhaBackup() { using var p = new PastaTeste(); var a = new ArmazenamentoColecoesJson(Caminho(p)); a.Carregar(); a.Salvar(new[] { Modelo() }); var antes = File.ReadAllBytes(Caminho(p)); Directory.CreateDirectory(Caminho(p) + ".bak"); Assert.ThrowsAny<IOException>(() => a.Salvar(Array.Empty<ColecaoPersonalizada>())); Assert.Equal(antes, File.ReadAllBytes(Caminho(p))); Assert.Empty(Directory.GetFiles(p.Pasta, "*.tmp")); }
    [Fact(DisplayName = "JS20 | Corrigir falha no backup e tentar novamente | Gravação volta a funcionar sem descartar dados")]
    public void RecuperarBackup() { using var p = new PastaTeste(); var a = new ArmazenamentoColecoesJson(Caminho(p)); a.Carregar(); a.Salvar(new[] { Modelo() }); Directory.CreateDirectory(Caminho(p) + ".bak"); Assert.ThrowsAny<IOException>(() => a.Salvar(Array.Empty<ColecaoPersonalizada>())); Directory.Delete(Caminho(p) + ".bak"); a.Salvar(Array.Empty<ColecaoPersonalizada>()); Assert.Empty(a.Carregar()); }
    [Fact(DisplayName = "JS21 | BOM UTF-16 e nomes Unicode | Leitura preserva dados e backup preserva bytes originais")]
    public void Bom() { using var p = new PastaTeste(); var c = Modelo() with { Nome = "Ação 日本語" }; File.WriteAllText(Caminho(p), JsonSerializer.Serialize(new { Versao = 1, Colecoes = new[] { c } }), Encoding.Unicode); var antes = File.ReadAllBytes(Caminho(p)); var a = new ArmazenamentoColecoesJson(Caminho(p)); Assert.Equal(c.Nome, a.Carregar()[0].Nome); a.Salvar(Array.Empty<ColecaoPersonalizada>()); Assert.Equal(antes, File.ReadAllBytes(Caminho(p) + ".bak")); }
    [Fact(DisplayName = "JS22 | Coleções salvas ao lado da biblioteca | Bytes da biblioteca nunca são regravados por metadados de coleção")]
    public void Separado() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "conteúdo de biblioteca simulado"); var a = new ArmazenamentoColecoesJson(Caminho(p)); a.Carregar(); a.Salvar(new[] { Modelo() }); Assert.Equal("conteúdo de biblioteca simulado", File.ReadAllText(p.Arquivo)); }
    [Fact(DisplayName = "JS23 | Caminho relativo resolvido | Origem é absoluta e estável")]
    public void CaminhoAbsoluto() { using var p = new PastaTeste(); var a = new ArmazenamentoColecoesJson(Path.GetRelativePath(Environment.CurrentDirectory, Caminho(p))); Assert.Equal(Caminho(p), a.DescricaoOrigem); }
    [Fact(DisplayName = "JS24 | Recuperação após JSON inválido | Reabrir conteúdo corrigido permite novas alterações")]
    public void RecuperarJson() { using var p = new PastaTeste(); File.WriteAllText(Caminho(p), "{"); var a = new ArmazenamentoColecoesJson(Caminho(p)); Assert.Throws<InvalidDataException>(() => a.Carregar()); File.WriteAllText(Caminho(p), "{\"Versao\":1,\"Colecoes\":[]}"); a.Carregar(); a.Salvar(new[] { Modelo() }); Assert.Single(a.Carregar()); }
}
