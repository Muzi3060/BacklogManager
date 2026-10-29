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

    [Fact(DisplayName = "VA03 | Preferência incompleta antiga | Campos válidos preservados e ausentes preenchidos")]
    public void Cenario03() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "{\"Tema\":\"Tecnologico\"}"); var l = new ArmazenamentoAparenciaJson(p.Arquivo).Carregar(); Assert.Equal(TemaVisual.Tecnologico, l.Preferencias.Tema); Assert.Equal(ModoVisual.Claro, l.Preferencias.Modo); }
    [Fact(DisplayName = "VA04 | Preferência com enum inválido | Inicialização usa padrão seguro sem sobrescrever arquivo")]
    public void Cenario04() { using var p = new PastaTeste(); var json = "{\"Versao\":1,\"Tema\":\"Outro\",\"Modo\":77}"; File.WriteAllText(p.Arquivo, json); var l = new ArmazenamentoAparenciaJson(p.Arquivo).Carregar(); Assert.Equal(TemaVisual.Moderno, l.Preferencias.Tema); Assert.Equal(ModoVisual.Claro, l.Preferencias.Modo); Assert.Equal(json, File.ReadAllText(p.Arquivo)); }
    [Fact(DisplayName = "VA05 | Cores salvas parcialmente inválidas | Cores legadas válidas e inválidas ficam inativas")]
    public void Cenario05() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "{\"Personalizacoes\":{\"Moderno/Claro\":{\"FundoPrincipal\":\"#123456\",\"TextoPrincipal\":\"#abc\"}}}"); var r = ResolvedorAparencia.Resolver(new ArmazenamentoAparenciaJson(p.Arquivo).Carregar().Preferencias); Assert.Equal(TemasPredefinidos.Obter(TemaVisual.Moderno,ModoVisual.Claro)[ElementoCor.FundoPrincipal], r.Solicitadas[ElementoCor.FundoPrincipal]); Assert.NotEqual("#ABC", r.Solicitadas[ElementoCor.TextoPrincipal]); }
    [Fact(DisplayName = "VA06 | Arquivo JSON truncado | Aviso e padrão sem alterar bytes inválidos")]
    public void Cenario06() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "{"); var a = new ArmazenamentoAparenciaJson(p.Arquivo); var l = a.Carregar(); Assert.NotNull(l.Aviso); Assert.Equal("{", File.ReadAllText(p.Arquivo)); }
    [Fact(DisplayName = "VA07 | Substituir preferência inválida explicitamente | Original fica no backup e nova preferência é válida")]
    public void Cenario07() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "{"); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo)); s.Carregar(); s.Selecionar(TemaVisual.Tecnologico, ModoVisual.Claro); Assert.False(s.PendenteSalvar); Assert.Equal("{", File.ReadAllText(p.Arquivo + ".bak")); Assert.NotNull(JsonNode.Parse(File.ReadAllText(p.Arquivo))); }


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




    [Fact(DisplayName = "VA22 | UTF8 com BOM | Preferência válida é reconhecida")]
    public void Cenario22() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "{\"Tema\":\"Acolhedor\"}", new System.Text.UTF8Encoding(true)); Assert.Equal(TemaVisual.Acolhedor, new ArmazenamentoAparenciaJson(p.Arquivo).Carregar().Preferencias.Tema); }

    [Fact(DisplayName = "VA24 | Pasta ausente | Padrão carrega e falha ao salvar é informada")]
    public void Cenario24() { using var p = new PastaTeste(); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(Path.Combine(p.Pasta, "ausente", "aparencia.json"))); s.Carregar(); s.Selecionar(TemaVisual.Acolhedor, ModoVisual.Claro); Assert.True(s.PendenteSalvar); Assert.NotNull(s.Aviso); }




    [Fact(DisplayName = "VA27 | Tema duplicado na raiz do JSON | Arquivo ambíguo não produz seleção arbitrária")]
    public void RaizDuplicada() { using var p = new PastaTeste(); var json = """{"Tema":"Moderno","Tema":"Acolhedor"}"""; File.WriteAllText(p.Arquivo, json); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo)); s.Carregar(); Assert.NotNull(s.Aviso); Assert.Equal(TemaVisual.Moderno, s.Preferencias.Tema); Assert.Equal(json, File.ReadAllText(p.Arquivo)); }
}
