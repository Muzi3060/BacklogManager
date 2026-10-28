using Xunit;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;
using BacklogManager.Aparencia;
using BacklogManager.Controls;
using BacklogManager.Data;
using BacklogManager.Models;
using System.Text.Json.Nodes;
namespace BacklogManager.Tests.Fase5;

public sealed class PersistenciaAparenciaTests
{
    [Fact(DisplayName = "VA01 | Arquivo ausente | Padrão seguro e nenhum arquivo criado")]
    public void Cenario01() { using var p = new PastaTeste(); var a = new ArmazenamentoAparenciaJson(Path.Combine(p.Pasta, "aparencia.json")); var l = a.Carregar(); Assert.Equal(ModoVisual.Claro, l.Preferencias.Modo); Assert.False(File.Exists(a.CaminhoArquivo)); }
    [Fact(DisplayName = "VA02 | Tema modo e cor após reiniciar | Preferências solicitadas e cores efetivas recuperadas")]
    public void Cenario02() { using var p = new PastaTeste(); var caminho = Path.Combine(p.Pasta, "aparencia.json"); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(caminho)); s.Carregar(); s.Selecionar(TemaVisual.Acolhedor, ModoVisual.Escuro); s.Personalizar(ElementoCor.FundoPrincipal, "#123456"); var b = new ServicoAparencia(new ArmazenamentoAparenciaJson(caminho)); b.Carregar(); Assert.Equal(TemaVisual.Acolhedor, b.Preferencias.Tema); Assert.Equal(ModoVisual.Escuro, b.Preferencias.Modo); Assert.Equal("#123456", ResolvedorAparencia.Resolver(b.Preferencias).Recursos["PageBackground"]); }
    [Fact(DisplayName = "VA03 | Preferência incompleta antiga | Campos válidos preservados e ausentes preenchidos")]
    public void Cenario03() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "{\"Tema\":\"Tecnologico\"}"); var l = new ArmazenamentoAparenciaJson(p.Arquivo).Carregar(); Assert.Equal(TemaVisual.Tecnologico, l.Preferencias.Tema); Assert.Equal(ModoVisual.Claro, l.Preferencias.Modo); }
    [Fact(DisplayName = "VA04 | Preferência com enum inválido | Inicialização usa padrão seguro sem sobrescrever arquivo")]
    public void Cenario04() { using var p = new PastaTeste(); var json = "{\"Versao\":1,\"Tema\":\"Outro\",\"Modo\":77}"; File.WriteAllText(p.Arquivo, json); var l = new ArmazenamentoAparenciaJson(p.Arquivo).Carregar(); Assert.Equal(TemaVisual.Moderno, l.Preferencias.Tema); Assert.Equal(ModoVisual.Claro, l.Preferencias.Modo); Assert.Equal(json, File.ReadAllText(p.Arquivo)); }
    [Fact(DisplayName = "VA05 | Cores salvas parcialmente inválidas | Cores válidas permanecem e inválidas não são aplicadas")]
    public void Cenario05() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "{\"Personalizacoes\":{\"Moderno/Claro\":{\"FundoPrincipal\":\"#123456\",\"TextoPrincipal\":\"#abc\"}}}"); var r = ResolvedorAparencia.Resolver(new ArmazenamentoAparenciaJson(p.Arquivo).Carregar().Preferencias); Assert.Equal("#123456", r.Solicitadas[ElementoCor.FundoPrincipal]); Assert.NotEqual("#ABC", r.Solicitadas[ElementoCor.TextoPrincipal]); }
    [Fact(DisplayName = "VA06 | Arquivo JSON truncado | Aviso e padrão sem alterar bytes inválidos")]
    public void Cenario06() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "{"); var a = new ArmazenamentoAparenciaJson(p.Arquivo); var l = a.Carregar(); Assert.NotNull(l.Aviso); Assert.Equal("{", File.ReadAllText(p.Arquivo)); }
    [Fact(DisplayName = "VA07 | Substituir preferência inválida explicitamente | Original fica no backup e nova preferência é válida")]
    public void Cenario07() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "{"); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo)); s.Carregar(); s.Selecionar(TemaVisual.Tecnologico, ModoVisual.Claro); Assert.False(s.PendenteSalvar); Assert.Equal("{", File.ReadAllText(p.Arquivo + ".bak")); Assert.NotNull(JsonNode.Parse(File.ReadAllText(p.Arquivo))); }
    [Fact(DisplayName = "VA08 | Versão futura | Abertura funciona e gravação fica bloqueada")]
    public void Cenario08() { using var p = new PastaTeste(); var json = "{\"Versao\":99}"; File.WriteAllText(p.Arquivo, json); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo)); s.Carregar(); s.Personalizar(ElementoCor.FundoPrincipal, "#123456"); Assert.True(s.PendenteSalvar); Assert.Equal(json, File.ReadAllText(p.Arquivo)); }
    [Fact(DisplayName = "VA09 | Campos desconhecidos | Metadados e conjuntos futuros são preservados na gravação")]
    public void Cenario09() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "{\"Extra\":{\"a\":1},\"Personalizacoes\":{\"Futuro/Claro\":{\"Cor\":\"#123456\"},\"Moderno/Claro\":{\"TokenFuturo\":42}}}"); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo)); s.Carregar(); s.Personalizar(ElementoCor.FundoPrincipal, "#ABCDEF"); var j = JsonNode.Parse(File.ReadAllText(p.Arquivo))!; Assert.Equal(1, (int)j["Extra"]!["a"]!); Assert.Equal(42, (int)j["Personalizacoes"]!["Moderno/Claro"]!["TokenFuturo"]!); Assert.NotNull(j["Personalizacoes"]!["Futuro/Claro"]); }
    [Fact(DisplayName = "VA10 | Arquivo mudou externamente | Não sobrescreve alteração de outra janela")]
    public void Cenario10() { using var p = new PastaTeste(); var a = new ArmazenamentoAparenciaJson(p.Arquivo); a.Carregar(); File.WriteAllText(p.Arquivo, "{}"); Assert.Throws<ArquivoAlteradoException>(() => a.Salvar(new())); Assert.Equal("{}", File.ReadAllText(p.Arquivo)); }
    [Fact(DisplayName = "VA11 | Arquivo removido externamente | Detecta remoção e não recria silenciosamente")]
    public void Cenario11() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "{}"); var a = new ArmazenamentoAparenciaJson(p.Arquivo); a.Carregar(); File.Delete(p.Arquivo); Assert.Throws<ArquivoAlteradoException>(() => a.Salvar(new())); Assert.False(File.Exists(p.Arquivo)); }
    [Fact(DisplayName = "VA12 | Lock em uso | Falha controlada sem escrita concorrente")]
    public void Cenario12() { using var p = new PastaTeste(); var a = new ArmazenamentoAparenciaJson(p.Arquivo); a.Carregar(); using var trava = new FileStream(p.Arquivo + ".lock", FileMode.Create, FileAccess.ReadWrite, FileShare.None); Assert.ThrowsAny<IOException>(() => a.Salvar(new())); Assert.False(File.Exists(p.Arquivo)); }
    [Fact(DisplayName = "VA13 | Falha no arquivo de backup | Anterior permanece e temporário é limpo")]
    public void Cenario13() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "{}"); Directory.CreateDirectory(p.Arquivo + ".bak"); var a = new ArmazenamentoAparenciaJson(p.Arquivo); a.Carregar(); Assert.ThrowsAny<IOException>(() => a.Salvar(new())); Assert.Equal("{}", File.ReadAllText(p.Arquivo)); Assert.Empty(Directory.GetFiles(p.Pasta, "*.tmp")); }
    [Fact(DisplayName = "VA14 | Recuperar após falha de backup | Nova gravação funciona sem recarregar nem perder modelo")]
    public void Cenario14() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "{}"); Directory.CreateDirectory(p.Arquivo + ".bak"); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo)); s.Carregar(); s.Selecionar(TemaVisual.Acolhedor, ModoVisual.Escuro); Assert.True(s.PendenteSalvar); Directory.Delete(p.Arquivo + ".bak"); s.TentarSalvarNovamente(); Assert.False(s.PendenteSalvar); Assert.Equal(TemaVisual.Acolhedor, new ArmazenamentoAparenciaJson(p.Arquivo).Carregar().Preferencias.Tema); }
    [Fact(DisplayName = "VA15 | Salvar sem carregar | Recusa escrita sem base conhecida")]
    public void Cenario15() { using var p = new PastaTeste(); var a = new ArmazenamentoAparenciaJson(p.Arquivo); Assert.Throws<InvalidOperationException>(() => a.Salvar(new())); Assert.False(File.Exists(p.Arquivo)); }
    [Fact(DisplayName = "VA16 | Falha de leitura por serviço simulado | Aplicativo usa Moderno claro e explica falha")]
    public void Cenario16() { var a = new AparenciaSimulada { FalhaLer = new UnauthorizedAccessException() }; var s = a.Criar(); Assert.Equal(TemaVisual.Moderno, s.Preferencias.Tema); Assert.NotNull(s.Aviso); Assert.Equal(0, a.Gravacoes); }
    [Fact(DisplayName = "VA17 | Arquivo nulo | Preferência inválida não impede abertura")]
    public void Cenario17() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "null"); var l = new ArmazenamentoAparenciaJson(p.Arquivo).Carregar(); Assert.NotNull(l.Aviso); Assert.Equal(ModoVisual.Claro, l.Preferencias.Modo); }
    [Fact(DisplayName = "VA18 | Objeto com conjuntos de tipo inesperado | Ignora somente configuração inválida e mantém tema válido")]
    public void Cenario18() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "{\"Tema\":\"Acolhedor\",\"Personalizacoes\":[]}"); var l = new ArmazenamentoAparenciaJson(p.Arquivo).Carregar(); Assert.Equal(TemaVisual.Acolhedor, l.Preferencias.Tema); Assert.Empty(l.Preferencias.Personalizacoes); }
    [Fact(DisplayName = "VA19 | Biblioteca coleções e visualização vizinhas | Nenhum dos três arquivos de negócio é alterado")]
    public void Cenario19() { using var p = new PastaTeste(); var nomes = new[] { p.Arquivo, Path.Combine(p.Pasta, "backlog.colecoes.json"), Path.Combine(p.Pasta, "biblioteca.preferencias.json") }; foreach (var n in nomes) File.WriteAllText(n, "dados isolados"); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(Path.Combine(p.Pasta, "aparencia.json"))); s.Carregar(); s.Personalizar(ElementoCor.FundoPrincipal, "#123456"); s.RestaurarTudo(); Assert.All(nomes, n => Assert.Equal("dados isolados", File.ReadAllText(n))); }
    [Fact(DisplayName = "VA20 | Persistir seis combinações | Todas as personalizações sobrevivem a nova instância")]
    public void Cenario20() { using var p = new PastaTeste(); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo)); s.Carregar(); foreach (var t in Enum.GetValues<TemaVisual>()) foreach (var m in Enum.GetValues<ModoVisual>()) { s.Selecionar(t, m); s.Personalizar(ElementoCor.FundoPrincipal, "#123456"); } Assert.Equal(6, new ArmazenamentoAparenciaJson(p.Arquivo).Carregar().Preferencias.Personalizacoes.Count); }
    [Fact(DisplayName = "VA21 | Texto solicitado de baixo contraste após reiniciar | Solicitado e efetivo diferentes são reproduzidos sem perda")]
    public void Cenario21() { using var p = new PastaTeste(); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo)); s.Carregar(); s.Personalizar(ElementoCor.FundoPrincipal, "#FFFFFF"); s.Personalizar(ElementoCor.TextoPrincipal, "#FFFFFF"); var r = ResolvedorAparencia.Resolver(new ArmazenamentoAparenciaJson(p.Arquivo).Carregar().Preferencias); Assert.Equal("#FFFFFF", r.Solicitadas[ElementoCor.TextoPrincipal]); Assert.Equal("#000000", r.Recursos["PageForeground"]); }
    [Fact(DisplayName = "VA22 | UTF8 com BOM | Preferência válida é reconhecida")]
    public void Cenario22() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "{\"Tema\":\"Acolhedor\"}", new System.Text.UTF8Encoding(true)); Assert.Equal(TemaVisual.Acolhedor, new ArmazenamentoAparenciaJson(p.Arquivo).Carregar().Preferencias.Tema); }
    [Fact(DisplayName = "VA23 | UTF16 com BOM | Preferência válida é reconhecida e regravada com backup")]
    public void Cenario23() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "{\"Tema\":\"Acolhedor\"}", System.Text.Encoding.Unicode); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo)); s.Carregar(); Assert.Equal(TemaVisual.Acolhedor, s.Preferencias.Tema); s.Personalizar(ElementoCor.FundoPrincipal, "#123456"); Assert.False(s.PendenteSalvar); Assert.True(File.Exists(p.Arquivo + ".bak")); }
    [Fact(DisplayName = "VA24 | Pasta ausente | Padrão carrega e falha ao salvar é informada")]
    public void Cenario24() { using var p = new PastaTeste(); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(Path.Combine(p.Pasta, "ausente", "aparencia.json"))); s.Carregar(); s.Selecionar(TemaVisual.Acolhedor, ModoVisual.Claro); Assert.True(s.PendenteSalvar); Assert.NotNull(s.Aviso); }

    [Fact(DisplayName = "VA25 | Conflito de preferências visto pelo serviço | Aviso orienta reabrir e preserva alteração externa")]
    public void ConflitoServico() { using var p = new PastaTeste(); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo)); s.Carregar(); File.WriteAllText(p.Arquivo, "{}"); s.Personalizar(ElementoCor.FundoPrincipal, "#123456"); Assert.True(s.PendenteSalvar); Assert.Contains("reabra", s.Aviso!); Assert.Equal("{}", File.ReadAllText(p.Arquivo)); }

    [Fact(DisplayName = "VA26 | Chaves duplicadas no conjunto de cores | Abertura usa padrão e original fica preservado")]
    public void ChavesDuplicadas() { using var p = new PastaTeste(); var json = """{"Versao":1,"Personalizacoes":{"Moderno/Claro":{"FundoPrincipal":"#123456","FundoPrincipal":"#654321"}}}"""; File.WriteAllText(p.Arquivo, json); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo)); s.Carregar(); Assert.NotNull(s.Aviso); Assert.Empty(s.Preferencias.Personalizacoes); Assert.Equal(json, File.ReadAllText(p.Arquivo)); s.Personalizar(ElementoCor.FundoPrincipal, "#ABCDEF"); Assert.False(s.PendenteSalvar); Assert.Equal(json, File.ReadAllText(p.Arquivo + ".bak")); }
    [Fact(DisplayName = "VA27 | Tema duplicado na raiz do JSON | Arquivo ambíguo não produz seleção arbitrária")]
    public void RaizDuplicada() { using var p = new PastaTeste(); var json = """{"Tema":"Moderno","Tema":"Acolhedor"}"""; File.WriteAllText(p.Arquivo, json); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo)); s.Carregar(); Assert.NotNull(s.Aviso); Assert.Equal(TemaVisual.Moderno, s.Preferencias.Tema); Assert.Equal(json, File.ReadAllText(p.Arquivo)); }
}
