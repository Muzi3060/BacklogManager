using System.Text;
using System.Text.Json.Nodes;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Styling;
using Avalonia.VisualTree;
using BacklogManager.Aparencia;
using BacklogManager.Backup;
using BacklogManager.Controls;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Tests.Fase5;
using BacklogManager.Tests.Fase6;
using BacklogManager.Tests.Fase7;
using Xunit;
namespace BacklogManager.Tests.BetaInterface;

public sealed class CompatibilidadeBetaTests
{
    [Fact(DisplayName = "BP01 | Preferências ausentes | Padrão seguro sem criar arquivo")]
    public void Caso01()
    {
        using var p = new PastaTeste(); var path = Path.Combine(p.Pasta, "aparencia.json"); var l = new ArmazenamentoAparenciaJson(path).Carregar(); Assert.Equal(TemaVisual.Moderno, l.Preferencias.Tema); Assert.Equal(ModoVisual.Claro, l.Preferencias.Modo); Assert.False(File.Exists(path));
    }

    [Fact(DisplayName = "BP02 | Tema e modo antigos | Versão um lida sem migrar automaticamente")]
    public void Caso02()
    {
        using var p = new PastaTeste(); var json = """{"Versao":1,"Tema":"Acolhedor","Modo":"Escuro"}"""; File.WriteAllText(p.Arquivo, json); var l = new ArmazenamentoAparenciaJson(p.Arquivo).Carregar(); Assert.Null(l.Aviso); Assert.Equal(TemaVisual.Acolhedor, l.Preferencias.Tema); Assert.Equal(ModoVisual.Escuro, l.Preferencias.Modo); Assert.Equal(json, File.ReadAllText(p.Arquivo));
    }

    [Fact(DisplayName = "BP03 | Cores legadas válidas | Cor antiga não sobrepõe tema fixo e bytes são preservados")]
    public void Caso03()
    {
        using var p = new PastaTeste(); var json = """{"Versao":1,"Tema":"Moderno","Modo":"Claro","Personalizacoes":{"Moderno/Claro":{"FundoPrincipal":"#123456"}}}"""; File.WriteAllText(p.Arquivo, json); var l = new ArmazenamentoAparenciaJson(p.Arquivo).Carregar(); Assert.Null(l.Aviso); Assert.Equal("#F3F4FA", ResolvedorAparencia.Resolver(l.Preferencias).Recursos["PageBackground"]); Assert.Equal(json, File.ReadAllText(p.Arquivo));
    }

    [Fact(DisplayName = "BP04 | Cores legadas opacas | Valores e extensões desconhecidos sobrevivem à seleção explícita")]
    public void Caso04()
    {
        using var p = new PastaTeste(); var json = """{"Versao":1,"Personalizacoes":{"Moderno/Claro":{"FundoPrincipal":"#abc","Futuro":[1,2]}},"Extensao":{"x":true}}"""; File.WriteAllText(p.Arquivo, json); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo)); s.Carregar(); s.Selecionar(TemaVisual.DarkMode, ModoVisual.Escuro); Assert.False(s.PendenteSalvar); var antes = JsonNode.Parse(json)!; var depois = JsonNode.Parse(File.ReadAllText(p.Arquivo))!; Assert.True(JsonNode.DeepEquals(antes["Personalizacoes"], depois["Personalizacoes"])); Assert.True(JsonNode.DeepEquals(antes["Extensao"], depois["Extensao"])); Assert.Equal(json, File.ReadAllText(p.Arquivo + ".bak"));
    }

    [Fact(DisplayName = "BP05 | Preferência incompleta | Tema preservado e modo ausente usa Claro")]
    public void Caso05()
    {
        using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, """{"Tema":"Tecnologico"}"""); var l = new ArmazenamentoAparenciaJson(p.Arquivo).Carregar(); Assert.Equal(TemaVisual.Tecnologico, l.Preferencias.Tema); Assert.Equal(ModoVisual.Claro, l.Preferencias.Modo);
    }

    [Fact(DisplayName = "BP06 | Tema desconhecido | Fallback não regrava arquivo")]
    public void Caso06()
    {
        using var p = new PastaTeste(); var json = """{"Versao":1,"Tema":"Desconhecido","Modo":"Escuro"}"""; File.WriteAllText(p.Arquivo, json); var l = new ArmazenamentoAparenciaJson(p.Arquivo).Carregar(); Assert.Equal(TemaVisual.Moderno, l.Preferencias.Tema); Assert.Equal(ModoVisual.Escuro, l.Preferencias.Modo); Assert.Equal(json, File.ReadAllText(p.Arquivo));
    }

    [Fact(DisplayName = "BP07 | Dark Mode versão dois | Reabertura reconhece seleção e paleta neutra")]
    public void Caso07()
    {
        using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, """{"Versao":2,"Tema":"DarkMode","Modo":"Escuro"}"""); var l = new ArmazenamentoAparenciaJson(p.Arquivo).Carregar(); Assert.Null(l.Aviso); Assert.Equal(TemaVisual.DarkMode, l.Preferencias.Tema); Assert.Equal("#121212", ResolvedorAparencia.Resolver(l.Preferencias).Recursos["PageBackground"]);
    }

    [Fact(DisplayName = "BP08 | Versão futura | Leitura conservadora e gravação bloqueada")]
    public void Caso08()
    {
        using var p = new PastaTeste(); var json = """{"Versao":3,"Tema":"DarkMode","Modo":"Escuro","Futuro":123}"""; File.WriteAllText(p.Arquivo, json); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo)); s.Carregar(); Assert.NotNull(s.Aviso); s.Selecionar(TemaVisual.Moderno, ModoVisual.Claro); Assert.True(s.PendenteSalvar); Assert.Equal(json, File.ReadAllText(p.Arquivo));
    }

    [Fact(DisplayName = "BP09 | Alteração externa | Conflito impede sobrescrever preferência")]
    public void Caso09()
    {
        using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "{}"); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo)); s.Carregar(); File.WriteAllText(p.Arquivo, """{"Tema":"Acolhedor"}"""); var bytes = File.ReadAllBytes(p.Arquivo); s.Selecionar(TemaVisual.DarkMode, ModoVisual.Escuro); Assert.True(s.PendenteSalvar); Assert.Equal(bytes, File.ReadAllBytes(p.Arquivo));
    }

    [Fact(DisplayName = "BP10 | JSON inválido | Aviso sem sobrescrita na abertura")]
    public void Caso10()
    {
        using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "{"); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo)); s.Carregar(); Assert.NotNull(s.Aviso); Assert.Equal("{", File.ReadAllText(p.Arquivo)); Assert.False(File.Exists(p.Arquivo + ".bak"));
    }

    [Fact(DisplayName = "BP11 | Salvar nova preferência | Versão dois e backup do documento anterior")]
    public void Caso11()
    {
        using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, """{"Versao":1,"Tema":"Moderno"}"""); var antes = File.ReadAllBytes(p.Arquivo); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(p.Arquivo)); s.Carregar(); s.Selecionar(TemaVisual.Tecnologico, ModoVisual.Claro); Assert.Equal(2, (int)JsonNode.Parse(File.ReadAllText(p.Arquivo))!["Versao"]!); Assert.Equal(antes, File.ReadAllBytes(p.Arquivo + ".bak"));
    }

    [Fact(DisplayName = "BP12 | Backup de preferências antigas | Exportar e restaurar preserva cores como conteúdo inativo")]
    public void Caso12()
    {
        using var a = new AmbienteBackup(); var json = """{"Versao":1,"Tema":"Acolhedor","Modo":"Escuro","Personalizacoes":{"Acolhedor/Escuro":{"FundoPrincipal":"#123456"}}}"""; File.WriteAllText(a.Caminhos.Arquivos["Aparencia"], json); var d = a.Documento(); File.Delete(a.Caminhos.Arquivos["Aparencia"]); a.Restaurado(a.Previa(d), true); Assert.Equal(json, File.ReadAllText(a.Caminhos.Arquivos["Aparencia"])); var l = new ArmazenamentoAparenciaJson(a.Caminhos.Arquivos["Aparencia"]).Carregar(); Assert.NotEqual("#123456", ResolvedorAparencia.Resolver(l.Preferencias).Recursos["PageBackground"]);
    }

    [Fact(DisplayName = "BP13 | Backup Dark Mode | Documento versão dois passa validação e restauração")]
    public void Caso13()
    {
        using var a = new AmbienteBackup(); a.Preferencias(TemaVisual.DarkMode, ModoVisual.Claro); var d = a.Documento(); var prev = a.Previa(d); Assert.True(prev.PreferenciasValidas); a.Restaurado(prev, true); var l = new ArmazenamentoAparenciaJson(a.Caminhos.Arquivos["Aparencia"]).Carregar(); Assert.Equal(TemaVisual.DarkMode, l.Preferencias.Tema); Assert.Equal(ModoVisual.Claro, l.Preferencias.Modo);
    }

    [Fact(DisplayName = "BP14 | Preferência futura no backup | Dados restauráveis sem aceitar preferência futura")]
    public void Caso14()
    {
        using var a = new AmbienteBackup(); var d = AmbienteBackup.Basico(); d.Componentes["Aparencia"] = AmbienteBackup.C(Encoding.UTF8.GetBytes("""{"Versao":3,"Tema":"DarkMode"}""")); var prev = a.Previa(d); Assert.False(prev.PreferenciasValidas); a.Restaurado(prev); Assert.False(File.Exists(a.Caminhos.Arquivos["Aparencia"]));
    }

    [Fact(DisplayName = "BP15 | Temas não alteram arquivos de dados | Biblioteca coleções e preferência de lista mantêm bytes")]
    public void Caso15()
    {
        using var a = new AmbienteBackup(); a.Preferencias(); var antes = a.Snapshot(); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(a.Caminhos.Arquivos["Aparencia"])); s.Carregar(); foreach (var t in Enum.GetValues<TemaVisual>()) foreach (var m in Enum.GetValues<ModoVisual>()) s.Selecionar(t, m); foreach (var nome in new[] { "Biblioteca", "Colecoes", "Visualizacao" }) Assert.Equal(antes[nome], File.ReadAllBytes(a.Caminhos.Arquivos[nome]));
    }

    [AvaloniaFact(DisplayName = "BP16 | Reabertura da janela | Tema fixo salvo reaparece sem iniciar no padrão")]
    public void Caso16()
    {
        using var p = new PastaTeste(); var path = Path.Combine(p.Pasta, "aparencia.json"); using (var t = new TelaTeste(aparencia: new(new ArmazenamentoAparenciaJson(path)))) { ApoioAparencia.Tema(t, TemaVisual.DarkMode, ModoVisual.Escuro); } using var b = new TelaTeste(aparencia: new(new ArmazenamentoAparenciaJson(path))); b.Clicar("NavConfiguracoes"); Assert.Equal(3, b.Campo<ComboBox>("CmbTema").SelectedIndex); Assert.Equal(1, b.Campo<ComboBox>("CmbModo").SelectedIndex); Assert.Equal("#121212", ApoioAparencia.Cor(b.Janela.Background));
    }

}
