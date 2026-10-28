using System.Text.Json;
using BacklogManager.Data;
using BacklogManager.Models;
using Xunit;
using static BacklogManager.Tests.Fase1.ApoioArquitetura;

namespace BacklogManager.Tests.Fase1;

public sealed class ArmazenamentoJsonTests : IDisposable
{
    private readonly PastaTeste _pasta = new();
    private ArmazenamentoJsonBiblioteca Criar() => new(new FileManager(_pasta.Arquivo));
    public void Dispose() => _pasta.Dispose();

    [Fact(DisplayName = "AR01 | Adaptador sem arquivo | Lista vazia sem criar arquivo")]
    public void SemArquivo() { Assert.Empty(Criar().Carregar()); Assert.False(File.Exists(_pasta.Arquivo)); }
    [Fact(DisplayName = "AR02 | Adaptador carrega JSON legado | Dados separados sem regravar o arquivo")]
    public void CarregarLegado()
    {
        File.WriteAllText(_pasta.Arquivo, JsonSerializer.Serialize(new[] { PastaTeste.Jogo() }));
        var antes = File.ReadAllBytes(_pasta.Arquivo); var item = Assert.Single(Criar().Carregar());
        Assert.Equal(1010, item.SteamAppIdLegado); Assert.Null(item.Catalogo.Origem); Assert.Equal(antes, File.ReadAllBytes(_pasta.Arquivo));
    }
    [Fact(DisplayName = "AR03 | Gravar pelo contrato | JSON continua no formato legado e recarregável")]
    public void SalvarModelo()
    {
        var a = Criar(); a.Carregar(); a.Salvar(new[] { Jogo() });
        using var json = JsonDocument.Parse(File.ReadAllBytes(_pasta.Arquivo));
        Assert.Equal(JsonValueKind.Array, json.RootElement.ValueKind); Assert.Equal(42, json.RootElement[0].GetProperty("IgdbId").GetInt32());
        Assert.False(json.RootElement[0].TryGetProperty("Catalogo", out _)); Assert.Single(a.Carregar());
    }
    [Fact(DisplayName = "AR04 | JSON corrompido no adaptador | Gravação bloqueada e original preservado")]
    public void Corrompido()
    {
        File.WriteAllText(_pasta.Arquivo, "{"); var a = Criar();
        Assert.Throws<InvalidDataException>(() => a.Carregar()); Assert.Throws<InvalidOperationException>(() => a.Salvar(new[] { Jogo() }));
        Assert.Equal("{", File.ReadAllText(_pasta.Arquivo));
    }
    [Fact(DisplayName = "AR05 | Conflito externo pelo adaptador | Versão externa não sobrescrita")]
    public void Conflito()
    {
        var a = Criar(); a.Carregar(); a.Salvar(new[] { Jogo() }); File.WriteAllText(_pasta.Arquivo, "[]");
        Assert.Throws<ArquivoAlteradoException>(() => a.Salvar(new[] { Jogo("Outro") })); Assert.Equal("[]", File.ReadAllText(_pasta.Arquivo));
    }
    [Fact(DisplayName = "AR06 | Backup da gravação pelo adaptador | Bytes anteriores preservados no bak")]
    public void Backup()
    {
        var a = Criar(); a.Carregar(); a.Salvar(new[] { Jogo() }); var antes = File.ReadAllBytes(_pasta.Arquivo);
        a.Salvar(new[] { Jogo("Outro") }); Assert.Equal(antes, File.ReadAllBytes(_pasta.Arquivo + ".bak"));
    }
    [Fact(DisplayName = "AR07 | Origem do adaptador | Caminho explícito mantido sem migração")]
    public void Origem() => Assert.Equal(Path.GetFullPath(_pasta.Arquivo), Criar().DescricaoOrigem);
    [Fact(DisplayName = "AR08 | Propriedades JSON desconhecidas | Roundtrip pelo contrato mantém os extras")]
    public void Extras()
    {
        var r = PastaTeste.Jogo(); r.DadosAdicionais = new() { ["futuro"] = JsonSerializer.SerializeToElement(new[] { 1, 2 }) };
        File.WriteAllText(_pasta.Arquivo, JsonSerializer.Serialize(new[] { r })); var a = Criar(); a.Salvar(a.Carregar());
        Assert.Equal(2, Assert.Single(a.Carregar()).DadosAdicionais!["futuro"][1].GetInt32());
    }
    [Fact(DisplayName = "AR09 | Salvar anime no formato estendido | Tipo preservado no bloco versionado")]
    public void Anime() => Aceitar(Jogo() with { Catalogo = new ObraCatalogo { Tipo = TipoMidia.Anime, Titulo = "Anime" } });
    [Fact(DisplayName = "AR10 | Salvar série no formato estendido | Tipo preservado no bloco versionado")]
    public void Serie() => Aceitar(Jogo() with { Catalogo = new ObraCatalogo { Tipo = TipoMidia.Serie, Titulo = "Série" } });
    [Fact(DisplayName = "AR11 | Salvar filme no formato estendido | Tipo preservado no bloco versionado")]
    public void Filme() => Aceitar(Jogo() with { Catalogo = new ObraCatalogo { Tipo = TipoMidia.Filme, Titulo = "Filme" } });
    [Fact(DisplayName = "AR12 | Salvar mangá no formato estendido | Tipo preservado no bloco versionado")]
    public void Manga() => Aceitar(Jogo() with { Catalogo = new ObraCatalogo { Tipo = TipoMidia.Manga, Titulo = "Mangá" } });
    [Fact(DisplayName = "AR13 | Salvar livro no formato estendido | Tipo preservado no bloco versionado")]
    public void Livro() => Aceitar(Jogo() with { Catalogo = new ObraCatalogo { Tipo = TipoMidia.Livro, Titulo = "Livro" } });
    [Fact(DisplayName = "AR14 | Catálogo com capa | Referência preservada sem download")]
    public void Capa() { var j = Jogo(); Aceitar(j with { Catalogo = j.Catalogo with { CapaUrl = "https://example.invalid/capa" } }); }
    [Fact(DisplayName = "AR15 | Catálogo com gêneros | Gêneros preservados no bloco versionado")]
    public void Generos() { var j = Jogo(); Aceitar(j with { Catalogo = j.Catalogo with { Generos = new[] { "Ação" } } }); }
    [Fact(DisplayName = "AR16 | Referência externa de outro catálogo | Nenhuma conversão para ID IGDB")]
    public void OutraOrigem() { var j = Jogo(); Recusar(j with { Catalogo = j.Catalogo with { Origem = "Outro" } }); }
    [Fact(DisplayName = "AR17 | ID externo não numérico | Recusa sem conversão presumida")]
    public void IdTextual() { var j = Jogo(); Recusar(j with { Catalogo = j.Catalogo with { IdentificadorExterno = "abc" } }); }
    [Fact(DisplayName = "AR18 | Referência incompleta | Origem sem ID não é descartada silenciosamente")]
    public void ReferenciaIncompleta() { var j = Jogo(); Recusar(j with { Catalogo = j.Catalogo with { IdentificadorExterno = null } }); }
    [Fact(DisplayName = "AR19 | Coleção mista com item não suportado | Lote inteiro recusado antes de escrever")]
    public void LoteInvalido()
    {
        var a = Criar(); a.Carregar(); a.Salvar(new[] { Jogo() }); var antes = File.ReadAllBytes(_pasta.Arquivo);
        var livro = Jogo() with { Catalogo = new ObraCatalogo { Tipo = (TipoMidia)999, Titulo = "Inválido" } };
        Assert.Throws<NotSupportedException>(() => a.Salvar(new[] { Jogo("Alterado"), livro }));
        Assert.Equal(antes, File.ReadAllBytes(_pasta.Arquivo)); Assert.False(File.Exists(_pasta.Arquivo + ".bak"));
    }
    [Fact(DisplayName = "AR20 | Onze registros simulados com IDs repetidos | Quantidade e conteúdo preservados pelo adaptador")]
    public void OnzeRegistros()
    {
        var registros = Enumerable.Range(1, 11).Select(n => PastaTeste.Jogo($"Jogo {n}")).ToArray();
        File.WriteAllText(_pasta.Arquivo, JsonSerializer.Serialize(registros)); var a = Criar(); a.Salvar(a.Carregar());
        Assert.Equal(11, a.Carregar().Count);
        Assert.Equal(JsonSerializer.Serialize(registros), JsonSerializer.Serialize(new FileManager(_pasta.Arquivo).Carregar()));
    }
    private void Aceitar(ItemBiblioteca item)
    {
        var a = Criar(); a.Carregar(); a.Salvar(new[] { item });
        var lido = Assert.Single(a.Carregar());
        Assert.Equal(item.Catalogo.Tipo, lido.Catalogo.Tipo);
        Assert.Equal(item.Catalogo.CapaUrl, lido.Catalogo.CapaUrl);
        Assert.Equal(item.Catalogo.Generos, lido.Catalogo.Generos);
        Assert.Equal(1, new FileManager(_pasta.Arquivo).Carregar()[0].Multimidia!.Versao);
    }
    private void Recusar(ItemBiblioteca item)
    {
        var a = Criar(); a.Carregar();
        Assert.Throws<NotSupportedException>(() => a.Salvar(new[] { item })); Assert.False(File.Exists(_pasta.Arquivo));
    }
}
