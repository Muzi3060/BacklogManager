using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using BacklogManager.Configuracao;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Services;
using Xunit;
namespace BacklogManager.Tests.Fase4;

public sealed class IntegracaoColecoesTests
{
    [Fact(DisplayName = "IX01 | Composição local padrão | Coleções usam arquivo irmão e abertura não grava JSON")]
    public void Composicao() { using var p = new PastaTeste(); var d = ComposicaoAplicativo.Criar(p.Arquivo, _ => null); Assert.Equal(Path.Combine(p.Pasta, "backlog.colecoes.json"), d.Colecoes!.DescricaoOrigem); d.Biblioteca.Carregar(); d.Colecoes.Carregar(); Assert.False(File.Exists(p.Arquivo)); Assert.False(File.Exists(d.Colecoes.DescricaoOrigem)); }
    [Fact(DisplayName = "IX02 | Caminho de biblioteca personalizado | Sidecar específico não colide com biblioteca nem preferência")]
    public void CaminhoPersonalizado() { using var p = new PastaTeste(); var caminho = Path.Combine(p.Pasta, "minha.biblioteca.json"); var d = ComposicaoAplicativo.Criar(caminho, _ => null); Assert.Equal(Path.Combine(p.Pasta, "minha.biblioteca.colecoes.json"), d.Colecoes!.DescricaoOrigem); Assert.NotEqual(caminho, d.Colecoes.DescricaoOrigem); Assert.NotEqual(d.Preferencias!.CaminhoArquivo, d.Colecoes.DescricaoOrigem); }
    [Fact(DisplayName = "IX03 | Duplicatas com mesmo ID externo após reiniciar | Duas identidades pessoais e dois vínculos persistem")]
    public void DuplicatasPersistidas() { var i = AmbienteColecoes.Jogo(); using var a = new ArquivosColecoes(i, i with { }); var id = a.Servico.Criar("A"); foreach (var item in a.Biblioteca.Itens.ToArray()) a.Servico.AdicionarObra(id, item); var (b, c) = a.Reabrir(); Assert.Equal(2, b.Itens.Count); Assert.Equal(2, c.ObterObras(id).Count); Assert.NotEqual(b.Itens[0].IdInterno, b.Itens[1].IdInterno); Assert.Equal(b.Itens[0].Catalogo.IdentificadorExterno, b.Itens[1].Catalogo.IdentificadorExterno); }
    [Fact(DisplayName = "IX04 | Identificar legado com extensão desconhecida | Somente novo ID é acrescentado semanticamente")]
    public void PreservarLegado() { using var p = new PastaTeste(); var json = "[{\"Titulo\":\"Legado simulado\",\"Categoria\":\"Antiga\",\"Status\":\"Na Fila\",\"Nota\":4.5,\"Review\":\"Texto original\",\"SteamAppId\":361855,\"Extra\":{\"a\":1}}]"; File.WriteAllText(p.Arquivo, json); var b = new ServicoBiblioteca(new ArmazenamentoJsonBiblioteca(new FileManager(p.Arquivo))); b.Carregar(); b.GarantirIdentidade(b.Itens[0]); var antes = JsonNode.Parse(json)!.AsArray()[0]; var depois = JsonNode.Parse(File.ReadAllText(p.Arquivo))!.AsArray()[0]!.AsObject(); Assert.NotNull(depois["IdInterno"]); depois.Remove("IdInterno"); Assert.True(JsonNode.DeepEquals(antes, depois)); Assert.Equal(json, File.ReadAllText(p.Arquivo + ".bak")); }
    [Fact(DisplayName = "IX05 | Metadados de coleção sem associações | Criar, editar e excluir não mudam bytes da biblioteca")]
    public void SomenteMetadados() { using var a = new ArquivosColecoes(AmbienteColecoes.Jogo()); var bytes = File.ReadAllBytes(a.Pasta.Arquivo); var id = a.Servico.Criar("A"); a.Servico.Editar(id, "B", "Nova"); a.Servico.Excluir(id); Assert.Equal(bytes, File.ReadAllBytes(a.Pasta.Arquivo)); }
    [Fact(DisplayName = "IX06 | Serializer do formato anterior com extensão JSON | ID desconhecido é preservado para recarga das associações")]
    public void PreservarIdComSerializerAnterior() { using var a = new ArquivosColecoes(AmbienteColecoes.Jogo()); var id = a.Servico.Criar("A"); a.Servico.AdicionarObra(id, a.Biblioteca.Itens[0]); var antigos = JsonSerializer.Deserialize<FormatoAnterior[]>(File.ReadAllText(a.Pasta.Arquivo))!; Assert.Contains("IdInterno", antigos[0].Extras!.Keys); File.WriteAllText(a.Pasta.Arquivo, JsonSerializer.Serialize(antigos)); var (b, c) = a.Reabrir(); Assert.Same(b.Itens[0], Assert.Single(c.ObterObras(id))); }
    [Fact(DisplayName = "IX07 | Conflito no arquivo de coleções na primeira associação | Identidade segura e arquivo externo não sobrescrito")]
    public void ConflitoEntreArquivos() { using var a = new ArquivosColecoes(AmbienteColecoes.Jogo()); var id = a.Servico.Criar("A"); File.AppendAllText(a.Caminho, " "); var externo = File.ReadAllBytes(a.Caminho); Assert.Throws<ArquivoAlteradoException>(() => a.Servico.AdicionarObra(id, a.Biblioteca.Itens[0])); Assert.NotNull(a.Biblioteca.Itens[0].IdInterno); Assert.Empty(a.Servico.Colecoes[0].Obras); Assert.Equal(externo, File.ReadAllBytes(a.Caminho)); }
    [Fact(DisplayName = "IX08 | Arquivo de coleções corrompido | Dados pessoais continuam editáveis pelo serviço de biblioteca")]
    public void Independencia() { using var a = new ArquivosColecoes(AmbienteColecoes.Jogo()); File.WriteAllText(a.Caminho, "{"); var c = new ServicoColecoes(new ArmazenamentoColecoesJson(a.Caminho), a.Biblioteca); Assert.Throws<InvalidDataException>(() => c.Carregar()); var item = a.Biblioteca.Itens[0]; a.Biblioteca.Atualizar(item, item.Pessoal with { Nota = 1 }); Assert.Equal(1, a.ReabrirBiblioteca().Itens[0].Pessoal.Nota); Assert.Equal("{", File.ReadAllText(a.Caminho)); }
    [Fact(DisplayName = "IX09 | Biblioteca não carregada com coleções válidas | Operações de vínculo ficam bloqueadas")]
    public void BibliotecaNaoCarregada() { using var p = new PastaTeste(); var b = new ServicoBiblioteca(new ArmazenamentoJsonBiblioteca(new FileManager(p.Arquivo))); var c = new ServicoColecoes(new ArmazenamentoColecoesJson(Path.Combine(p.Pasta, "colecoes.json")), b); c.Carregar(); Assert.Throws<InvalidOperationException>(() => c.Criar("A")); }
    [Fact(DisplayName = "IX10 | Restaurar par consistente de arquivos simulados | Biblioteca e associações recuperam o mesmo estado")]
    public void RestaurarPar() { using var a = new ArquivosColecoes(AmbienteColecoes.Jogo()); var id = a.Servico.Criar("A"); a.Servico.AdicionarObra(id, a.Biblioteca.Itens[0]); var bib = File.ReadAllBytes(a.Pasta.Arquivo); var col = File.ReadAllBytes(a.Caminho); a.Biblioteca.Remover(a.Biblioteca.Itens[0]); a.Servico.Excluir(id); File.WriteAllBytes(a.Pasta.Arquivo, bib); File.WriteAllBytes(a.Caminho, col); var (b, c) = a.Reabrir(); Assert.Single(b.Itens); Assert.Single(c.ObterObras(id)); }
    [Fact(DisplayName = "IX11 | Criar coleção com biblioteca vazia | Coleção persiste sem inventar obras")]
    public void Vazia() { using var a = new ArquivosColecoes(); var id = a.Servico.Criar("Vazia"); var (b, c) = a.Reabrir(); Assert.Empty(b.Itens); Assert.Empty(c.ObterObras(id)); Assert.Single(c.Colecoes); }
    [Fact(DisplayName = "IX12 | Diretório de trabalho muda após composição | Caminhos já resolvidos permanecem na pasta inicial")]
    public void Diretorio() { using var p = new PastaTeste(); var d = ComposicaoAplicativo.Criar(p.Arquivo, _ => null); var anterior = Environment.CurrentDirectory; try { Environment.CurrentDirectory = p.Pasta; d.Biblioteca.Carregar(); d.Colecoes!.Carregar(); d.Colecoes.Criar("A"); Assert.True(File.Exists(Path.Combine(p.Pasta, "backlog.colecoes.json"))); } finally { Environment.CurrentDirectory = anterior; } }
    private sealed class FormatoAnterior
    {
        public string Titulo { get; set; } = "";
        [JsonExtensionData] public Dictionary<string, JsonElement>? Extras { get; set; }
    }
}
internal static class ReabrirBibliotecaExtensao
{
    internal static ServicoBiblioteca ReabrirBiblioteca(this ArquivosColecoes a)
    { var b = new ServicoBiblioteca(new ArmazenamentoJsonBiblioteca(new FileManager(a.Pasta.Arquivo))); b.Carregar(); return b; }
}
