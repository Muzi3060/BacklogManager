using System.Text.Json;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Services;
using BacklogManager.Tests.Fase1;
using BacklogManager.Tests.Fase2;
using Xunit;

namespace BacklogManager.Tests.Fase3;

public sealed class MetadadosBibliotecaTests
{
    private static ItemBiblioteca Novo() => FormularioObra.Preparar(new("Alfa", "RPG", "Na Fila", 4m, "Texto"), ConsultasTests.Item().Catalogo, "Alfa", null, TipoMidia.Jogo);
    [Fact(DisplayName = "DB01 | Cadastro novo | Data de adição real em UTC é registrada")]
    public void Agora() { var antes = DateTimeOffset.UtcNow; var i = Novo(); Assert.InRange(i.DataAdicao!.Value, antes, DateTimeOffset.UtcNow); Assert.Equal(TimeSpan.Zero, i.DataAdicao.Value.Offset); }
    [Fact(DisplayName = "DB02 | Data salva e recarregada | Instante e offset preservados")]
    public void DataRoundtrip() { var i = ConsultasTests.Item(data: new DateTimeOffset(2026, 10, 20, 23, 42, 1, TimeSpan.FromHours(-3))); Assert.Equal(i.DataAdicao, ApoioMultimidia.Roundtrip(i).DataAdicao); }
    [Fact(DisplayName = "DB03 | JSON legado | Data permanece desconhecida")]
    public void Legado() { var i = ConversorBibliotecaJson.ParaModelo(PastaTeste.Jogo()); Assert.Null(i.DataAdicao); Assert.Null(ConversorBibliotecaJson.ParaRegistro(i).Multimidia); }
    [Fact(DisplayName = "DB04 | Formato da Fase 2 sem data | Nenhuma data inventada no ciclo JSON")]
    public void MultimidiaAntiga() { var i = ApoioMultimidia.Item(TipoMidia.Livro); var r = ApoioMultimidia.Roundtrip(i); Assert.Null(r.DataAdicao); Assert.DoesNotContain("DataAdicao", JsonSerializer.Serialize(ConversorBibliotecaJson.ParaRegistro(r))); }
    [Fact(DisplayName = "DB05 | Edição de legado | Editar nota não cria data de adição")]
    public void EditarLegado() { var i = ApoioArquitetura.Jogo(); var editado = FormularioObra.Preparar(new(i.Catalogo.Titulo, "RPG", "Zerado", 5m, "Nova"), null, null, i, TipoMidia.Jogo); Assert.Null(editado.DataAdicao); }
    [Fact(DisplayName = "DB06 | Edição de registro datado | Data original permanece")]
    public void EditarData() { var i = Novo(); var editado = FormularioObra.Preparar(new("Alfa", "RPG", "Zerado", 5m, "Nova"), null, null, i, TipoMidia.Jogo); Assert.Equal(i.DataAdicao, editado.DataAdicao); }
    [Fact(DisplayName = "DB07 | Atualização pelo serviço | Data e catálogo preservados")]
    public void ServicoData() { var i = Novo(); var (s, _) = ApoioArquitetura.Carregada(i); s.Atualizar(i, i.Pessoal with { Nota = 5 }); Assert.Equal(i.DataAdicao, s.Itens[0].DataAdicao); Assert.Same(i.Catalogo, s.Itens[0].Catalogo); }
    [Fact(DisplayName = "DB08 | Cadastro das cinco mídias simuladas | Todas recebem data sem mudar origem")]
    public void DatasMidias() { foreach (var t in Enum.GetValues<TipoMidia>().Skip(1)) { var c = ApoioMultimidia.Catalogo(t); var i = FormularioObra.Preparar(new(c.Titulo, "Fantasia", "Na Fila", 4m, ""), c, c.Titulo, null, t); Assert.NotNull(i.DataAdicao); Assert.True(i.Catalogo.Simulado); } }
    [Fact(DisplayName = "DB09 | Data inválida no JSON | Carregamento bloqueia gravação e preserva bytes")]
    public void DataInvalida() { using var p = new PastaTeste(); var dto = ConversorBibliotecaJson.ParaRegistro(Novo()); var json = JsonSerializer.Serialize(new[] { dto }); using var d = JsonDocument.Parse(json); var data = d.RootElement[0].GetProperty("Multimidia").GetProperty("DataAdicao").GetRawText(); json = json.Replace(data, "\"inválida\""); File.WriteAllText(p.Arquivo, json); var f = new FileManager(p.Arquivo); Assert.Throws<InvalidDataException>(() => f.Carregar()); Assert.Throws<InvalidOperationException>(() => f.Salvar(Array.Empty<BacklogItem>())); Assert.Equal(json, File.ReadAllText(p.Arquivo)); }
    [Fact(DisplayName = "DB10 | Steam e extras com nova data | Identificador opaco e extensão permanecem")]
    public void SteamExtras() { using var d = JsonDocument.Parse("{\"x\":1}"); var i = Novo() with { SteamAppIdLegado = 361855, DadosAdicionais = new Dictionary<string, JsonElement> { ["Extra"] = d.RootElement.Clone() } }; var r = ApoioMultimidia.Roundtrip(i); Assert.Equal(361855, r.SteamAppIdLegado); Assert.Equal(1, r.DadosAdicionais!["Extra"].GetProperty("x").GetInt32()); }
    [Fact(DisplayName = "DB11 | Plataformas em JSON | Todas as plataformas do catálogo persistem")]
    public void Plataformas() { var i = ConsultasTests.Item(); i = i with { Catalogo = i.Catalogo with { Plataformas = new[] { "PC", "Switch" } } }; Assert.Equal(i.Catalogo.Plataformas, ApoioMultimidia.Roundtrip(i).Catalogo.Plataformas); }
    [Fact(DisplayName = "DB12 | Plataforma ausente | Lista vazia sem propriedade nova no legado")]
    public void PlataformaAusente() { var i = ApoioArquitetura.Jogo(); Assert.Empty(ApoioMultimidia.Roundtrip(i).Catalogo.Plataformas); Assert.DoesNotContain("Plataformas", JsonSerializer.Serialize(ConversorBibliotecaJson.ParaRegistro(i))); }
    [Fact(DisplayName = "DB13 | Plataforma vazia | Gravação recusa metadado inválido")]
    public void PlataformaVazia() { var i = ConsultasTests.Item(plataforma: " "); Assert.Throws<NotSupportedException>(() => ConversorBibliotecaJson.ParaRegistro(i)); }
    [Fact(DisplayName = "DB14 | Plataforma nula no modelo | Gravação recusa contrato inválido")]
    public void PlataformaNula() { var i = ConsultasTests.Item(); i = i with { Catalogo = i.Catalogo with { Plataformas = null! } }; Assert.Throws<NotSupportedException>(() => ConversorBibliotecaJson.ParaRegistro(i)); }
    [Fact(DisplayName = "DB15 | Plataforma em livro | Não aceita plataforma de jogo em outra mídia")]
    public void PlataformaLivro() { var i = ApoioMultimidia.Item(TipoMidia.Livro); i = i with { Catalogo = i.Catalogo with { Plataformas = new[] { "PC" } } }; Assert.Throws<NotSupportedException>(() => ConversorBibliotecaJson.ParaRegistro(i)); }
    [Fact(DisplayName = "DB16 | Edição de jogo com plataforma | Plataforma e capa não são perdidas")]
    public void EditarPlataforma() { var i = ConsultasTests.Item(); i = i with { Catalogo = i.Catalogo with { CapaUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/teste.jpg" } }; var (s, _) = ApoioArquitetura.Carregada(i); s.Atualizar(i, i.Pessoal with { Review = "Nova" }); var r = ApoioMultimidia.Roundtrip(s.Itens[0]); Assert.Equal(i.Catalogo.Plataformas, r.Catalogo.Plataformas); Assert.Equal(i.Catalogo.CapaUrl, r.Catalogo.CapaUrl); }
    [Fact(DisplayName = "DB17 | Mistura de formatos | Legado, multimídia e novos metadados coexistem")]
    public void Misto() { using var p = new PastaTeste(); var a = new ArmazenamentoJsonBiblioteca(new FileManager(p.Arquivo)); a.Carregar(); a.Salvar(new[] { ApoioArquitetura.Jogo(), ApoioMultimidia.Item(TipoMidia.Anime), Novo() }); var r = a.Carregar(); Assert.Equal(3, r.Count); Assert.Null(r[0].DataAdicao); Assert.Null(r[1].DataAdicao); Assert.NotNull(r[2].DataAdicao); }
    [Fact(DisplayName = "DB18 | Falha de persistência com data | Memória anterior permanece intacta")]
    public void Falha() { var i = Novo(); var (s, a) = ApoioArquitetura.Carregada(i); a.FalhaGravacao = new IOException(); Assert.Throws<IOException>(() => s.Atualizar(i, i.Pessoal with { Nota = 0 })); Assert.Same(i, s.Itens[0]); }
    [Fact(DisplayName = "DB19 | Array de plataforma inválido no disco | Leitura recusa e original permanece")]
    public void PlataformaDisco() { var dto = ConversorBibliotecaJson.ParaRegistro(ConsultasTests.Item()); var json = JsonSerializer.Serialize(new[] { dto }).Replace("[\"PC\"]", "[null]"); using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, json); Assert.Throws<InvalidDataException>(() => new FileManager(p.Arquivo).Carregar()); Assert.Equal(json, File.ReadAllText(p.Arquivo)); }
    [Fact(DisplayName = "DB20 | Metadados só de plataforma | Bloco opcional é criado sem alterar IgdbId")]
    public void SomentePlataforma() { var i = ApoioArquitetura.Jogo(); i = i with { Catalogo = i.Catalogo with { Plataformas = new[] { "PC" } } }; var dto = ConversorBibliotecaJson.ParaRegistro(i); Assert.NotNull(dto.Multimidia); Assert.Equal(42, dto.IgdbId); Assert.Null(dto.Multimidia.DataAdicao); }
}
