using System.Text.Json;
using System.Text.Json.Nodes;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Services;
using Xunit;
using static BacklogManager.Tests.Fase2.ApoioMultimidia;

namespace BacklogManager.Tests.Fase2;

public sealed class PersistenciaMultimidiaTests : IDisposable
{
    private readonly PastaTeste _pasta = new();
    private ArmazenamentoJsonBiblioteca Criar() => new(new FileManager(_pasta.Arquivo));
    public void Dispose() => _pasta.Dispose();
    private JsonNode JsonLivro()
    {
        var a = Criar(); a.Carregar(); a.Salvar(new[] { Item(TipoMidia.Livro, new ProgressoPessoal { Pagina = 12 }) });
        return JsonNode.Parse(File.ReadAllText(_pasta.Arquivo))!;
    }
    private void Recusar(JsonNode json)
    {
        var texto = json.ToJsonString(); File.WriteAllText(_pasta.Arquivo, texto); var a = Criar();
        Assert.Throws<InvalidDataException>(() => a.Carregar());
        Assert.Throws<InvalidOperationException>(() => a.Salvar(new[] { Item(TipoMidia.Jogo) }));
        Assert.Equal(texto, File.ReadAllText(_pasta.Arquivo));
    }

    [Fact(DisplayName = "JM01 | Arquivo legado sem bloco novo | Leitura não grava nem converte SteamAppId")]
    public void Legado()
    {
        var r = PastaTeste.Jogo(); r.SteamAppId = 361855; File.WriteAllText(_pasta.Arquivo, JsonSerializer.Serialize(new[] { r }));
        var bytes = File.ReadAllBytes(_pasta.Arquivo); var item = Assert.Single(Criar().Carregar());
        Assert.Equal(TipoMidia.Jogo, item.Catalogo.Tipo); Assert.Null(item.Catalogo.Origem); Assert.Equal(361855, item.SteamAppIdLegado);
        Assert.Equal(bytes, File.ReadAllBytes(_pasta.Arquivo));
    }
    [Fact(DisplayName = "JM02 | Roundtrip legado sem progresso | JSON semanticamente igual sem bloco opcional")]
    public void LegadoMesmoJson()
    {
        File.WriteAllText(_pasta.Arquivo, JsonSerializer.Serialize(new[] { PastaTeste.Jogo() }));
        var antes = JsonNode.Parse(File.ReadAllText(_pasta.Arquivo)); var a = Criar(); a.Salvar(a.Carregar());
        Assert.True(JsonNode.DeepEquals(antes, JsonNode.Parse(File.ReadAllText(_pasta.Arquivo)))); Assert.DoesNotContain("Multimidia", File.ReadAllText(_pasta.Arquivo));
    }
    [Fact(DisplayName = "JM03 | Coleção mista com seis tipos | Todos os registros e tipos sobrevivem à recarga")]
    public void SeisTipos()
    {
        var a = Criar(); a.Carregar(); a.Salvar(Enum.GetValues<TipoMidia>().Select(t => Item(t)).ToArray());
        Assert.Equal(Enum.GetValues<TipoMidia>(), a.Carregar().Select(i => i.Catalogo.Tipo));
    }
    [Fact(DisplayName = "JM04 | Versão do bloco multimídia | Versão um explícita sem mudar raiz de lista")]
    public void Versao() { var j = JsonLivro(); Assert.IsType<JsonArray>(j); Assert.Equal(1, j[0]!["Multimidia"]!["Versao"]!.GetValue<int>()); }
    [Fact(DisplayName = "JM05 | Versão futura do bloco | Leitura e gravação bloqueadas sem apagar arquivo")]
    public void VersaoFutura() { var j = JsonLivro(); j[0]!["Multimidia"]!["Versao"] = 2; Recusar(j); }
    [Fact(DisplayName = "JM06 | Bloco sem versão | Formato incompleto recusado sem assumir versão")]
    public void SemVersao() { var j = JsonLivro(); j[0]!["Multimidia"]!.AsObject().Remove("Versao"); Recusar(j); }
    [Fact(DisplayName = "JM07 | Bloco sem tipo | Tipo ausente não vira jogo silenciosamente")]
    public void SemTipo() { var j = JsonLivro(); j[0]!["Multimidia"]!.AsObject().Remove("Tipo"); Recusar(j); }
    [Fact(DisplayName = "JM08 | Tipo desconhecido | Arquivo preservado sem reinterpretar mídia")]
    public void TipoDesconhecido() { var j = JsonLivro(); j[0]!["Multimidia"]!["Tipo"] = "Podcast"; Recusar(j); }
    [Fact(DisplayName = "JM09 | Tipo numérico indefinido | Arquivo preservado sem aceitar enum inválido")]
    public void EnumInvalido() { var j = JsonLivro(); j[0]!["Multimidia"]!["Tipo"] = 999; Recusar(j); }
    [Fact(DisplayName = "JM10 | Campo novo desconhecido no bloco | Carga bloqueada em vez de descartar informação futura")]
    public void CampoDesconhecido() { var j = JsonLivro(); j[0]!["Multimidia"]!["CampoFuturo"] = 42; Recusar(j); }
    [Fact(DisplayName = "JM11 | Campo de progresso desconhecido | Nenhum progresso futuro descartado silenciosamente")]
    public void ProgressoDesconhecido() { var j = JsonLivro(); j[0]!["Multimidia"]!["Progresso"]!["PercentualFuturo"] = 25; Recusar(j); }
    [Fact(DisplayName = "JM12 | Progresso negativo no JSON | Carga recusada sem normalizar dados")]
    public void Negativo() { var j = JsonLivro(); j[0]!["Multimidia"]!["Progresso"]!["Pagina"] = -1; Recusar(j); }
    [Fact(DisplayName = "JM13 | Progresso com unidade incompatível | Carga recusada sem perda de dados")]
    public void UnidadeErrada() { var j = JsonLivro(); j[0]!["Multimidia"]!["Progresso"]!["HorasJogadas"] = 3; Recusar(j); }
    [Fact(DisplayName = "JM14 | Gêneros nulos | Arquivo recusado com dados originais preservados")]
    public void GenerosNulos() { var j = JsonLivro(); j[0]!["Multimidia"]!["Generos"] = null; Recusar(j); }
    [Fact(DisplayName = "JM15 | Simulado sem identificação explícita | Carga recusada sem apresentar dados fictícios como reais")]
    public void SimuladoFalso() { var j = JsonLivro(); j[0]!["Multimidia"]!["Simulado"] = false; Recusar(j); }
    [Fact(DisplayName = "JM16 | ID simulado em campo IGDB | Conflito recusado sem conversão de identificadores")]
    public void IgdbEmLivro() { var j = JsonLivro(); j[0]!["IgdbId"] = 42; Recusar(j); }
    [Fact(DisplayName = "JM17 | Referência externa incompleta | Arquivo preservado e gravação bloqueada")]
    public void ReferenciaIncompleta() { var j = JsonLivro(); j[0]!["Multimidia"]!["IdentificadorExterno"] = null; Recusar(j); }
    [Fact(DisplayName = "JM18 | Game com progresso e ID IGDB | ID continua no campo correto e bloco concorda com ele")]
    public void JogoIgdb()
    {
        var a = Criar(); a.Carregar(); a.Salvar(new[] { Item(TipoMidia.Jogo, new ProgressoPessoal { HorasJogadas = 2.5m }) });
        var r = Assert.Single(new FileManager(_pasta.Arquivo).Carregar()); Assert.Equal(42, r.IgdbId); Assert.Equal("42", r.Multimidia!.IdentificadorExterno); Assert.Null(r.SteamAppId);
    }
    [Fact(DisplayName = "JM19 | Jogo com IDs conflitantes | Arquivo recusado sem escolher um identificador")]
    public void ConflitoIds()
    {
        var a = Criar(); a.Carregar(); a.Salvar(new[] { Item(TipoMidia.Jogo, new ProgressoPessoal { HorasJogadas = 2 }) });
        var j = JsonNode.Parse(File.ReadAllText(_pasta.Arquivo))!; j[0]!["IgdbId"] = 99; Recusar(j);
    }
    [Fact(DisplayName = "JM20 | Falha de backup com coleção multimídia | Arquivo anterior preservado e memória não confirmada")]
    public void BackupFalha()
    {
        JsonLivro(); var bytes = File.ReadAllBytes(_pasta.Arquivo); var a = Criar(); var s = new ServicoBiblioteca(a); s.Carregar();
        Directory.CreateDirectory(_pasta.Arquivo + ".bak");
        var item = Assert.Single(s.Itens);
        Assert.ThrowsAny<IOException>(() => s.Atualizar(item, item.Pessoal with { Nota = 1 }));
        Assert.Equal(bytes, File.ReadAllBytes(_pasta.Arquivo)); Assert.Same(item, Assert.Single(s.Itens));
        Directory.Delete(_pasta.Arquivo + ".bak");
        s.Atualizar(item, item.Pessoal with { Nota = 1 });
        Assert.Equal(1, Assert.Single(a.Carregar()).Pessoal.Nota);
    }
    [Fact(DisplayName = "JM21 | Extras legados desconhecidos e bloco novo | Extras raiz preservados junto do progresso")]
    public void Extras()
    {
        var item = Item(TipoMidia.Livro, new ProgressoPessoal { Pagina = 12 }) with { SteamAppIdLegado = 361855, DadosAdicionais = new Dictionary<string, JsonElement> { ["futuroRaiz"] = JsonSerializer.SerializeToElement(new { valor = 7 }) } };
        var lido = Roundtrip(item); Assert.Equal(361855, lido.SteamAppIdLegado); Assert.Equal(7, lido.DadosAdicionais!["futuroRaiz"].GetProperty("valor").GetInt32());
    }
    [Fact(DisplayName = "JM22 | Alteração externa após carga multimídia | Versão externa não sobrescrita")]
    public void ConflitoExterno()
    {
        JsonLivro(); var a = Criar(); var itens = a.Carregar(); File.WriteAllText(_pasta.Arquivo, "[]");
        Assert.Throws<ArquivoAlteradoException>(() => a.Salvar(itens)); Assert.Equal("[]", File.ReadAllText(_pasta.Arquivo));
    }
    [Fact(DisplayName = "JM23 | Dois registros simulados iguais | Nenhuma deduplicação no arquivo")]
    public void Duplicados()
    {
        var a = Criar(); a.Carregar(); a.Salvar(new[] { Item(TipoMidia.Anime), Item(TipoMidia.Anime) }); Assert.Equal(2, a.Carregar().Count);
    }
    [Fact(DisplayName = "JM24 | Editar dados pessoais de simulado | Origem, ID e referência de capa mantidos")]
    public void Editar()
    {
        var a = Criar(); var s = new ServicoBiblioteca(a); s.Carregar(); var item = Item(TipoMidia.Livro); s.Adicionar(item);
        s.Atualizar(item, item.Pessoal with { Progresso = new ProgressoPessoal { Pagina = 25 } });
        var lido = Assert.Single(a.Carregar()); Assert.Equal(item.Catalogo.IdentificadorExterno, lido.Catalogo.IdentificadorExterno); Assert.Equal(item.Catalogo.CapaUrl, lido.Catalogo.CapaUrl); Assert.True(lido.Catalogo.Simulado);
    }
    [Fact(DisplayName = "JM25 | Campo opcional de progresso vazio | Não cria bloco adicional em jogo legado")]
    public void VazioSemBloco()
    {
        var r = PastaTeste.Jogo(); var item = ConversorBibliotecaJson.ParaModelo(r); var registro = ConversorBibliotecaJson.ParaRegistro(item with { Pessoal = item.Pessoal with { Progresso = new ProgressoPessoal() } }); Assert.Null(registro.Multimidia);
    }
    [Fact(DisplayName = "JM26 | Hora fracionária | Precisão decimal preservada após recarga")]
    public void Fracao() => Assert.Equal(1.25m, Roundtrip(Item(TipoMidia.Jogo, new ProgressoPessoal { HorasJogadas = 1.25m })).Pessoal.Progresso!.HorasJogadas);
    [Fact(DisplayName = "JM27 | Campo inteiro fracionário no arquivo | JSON recusado sem arredondar página")]
    public void PaginaFracionaria() { var j = JsonLivro(); j[0]!["Multimidia"]!["Progresso"]!["Pagina"] = 1.5m; Recusar(j); }
    [Fact(DisplayName = "JM28 | Lote com progresso inválido | Nenhum registro é gravado parcialmente")]
    public void LoteInvalido()
    {
        var a = Criar(); a.Carregar(); Assert.Throws<InvalidDataException>(() => a.Salvar(new[] { Item(TipoMidia.Anime), Item(TipoMidia.Livro, new ProgressoPessoal { HorasJogadas = 1 }) })); Assert.False(File.Exists(_pasta.Arquivo));
    }
    [Fact(DisplayName = "JM29 | Recarga de simulado | Gêneros e capa persistem junto da origem fictícia")]
    public void CatalogoCompleto()
    {
        var item = Item(TipoMidia.Manga); var lido = Roundtrip(item); Assert.Equal(item.Catalogo.Generos, lido.Catalogo.Generos); Assert.Equal(item.Catalogo.CapaUrl, lido.Catalogo.CapaUrl); Assert.Equal("Simulado", lido.Catalogo.Origem);
    }
    [Fact(DisplayName = "JM30 | Jogo legado com progresso novo | SteamAppId original mantido sem preencher IgdbId")]
    public void LegadoComProgresso()
    {
        var item = ConversorBibliotecaJson.ParaModelo(PastaTeste.Jogo()); var lido = Roundtrip(item with { Pessoal = item.Pessoal with { Progresso = new ProgressoPessoal { HorasJogadas = 7 } } }); Assert.Equal(1010, lido.SteamAppIdLegado); Assert.Null(lido.Catalogo.Origem); Assert.Equal(7m, lido.Pessoal.Progresso!.HorasJogadas);
    }
}
