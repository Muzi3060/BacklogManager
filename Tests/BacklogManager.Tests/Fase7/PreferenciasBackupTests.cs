using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using BacklogManager.Backup;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Services;
using BacklogManager.Aparencia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Xunit;
namespace BacklogManager.Tests.Fase7;
public sealed class PreferenciasBackupTests
{
    [Fact(DisplayName="QP01 | Caminhos compatíveis | Nomes e diretório atuais mantidos")]
    public void Caso01()
    {
        using var a=new AmbienteBackup(); Assert.Equal(Path.Combine(a.Dados,"backlog.json"),a.Caminhos.Arquivos["Biblioteca"]); Assert.Equal(Path.Combine(a.Dados,"backlog.colecoes.json"),a.Caminhos.Arquivos["Colecoes"]); Assert.Equal(Path.Combine(a.Dados,"biblioteca.preferencias.json"),a.Caminhos.Arquivos["Visualizacao"]); Assert.Equal(Path.Combine(a.Dados,"aparencia.preferencias.json"),a.Caminhos.Arquivos["Aparencia"]);
    }
    [Fact(DisplayName="QP02 | Caminho especial de visualização | Sufixo anterior evita colisão com biblioteca")]
    public void Caso02()
    {
        using var a=new AmbienteBackup(); var c=new CaminhosDadosLocais(Path.Combine(a.Dados,"biblioteca.preferencias.json")); Assert.EndsWith(".visualizacao.json",c.Arquivos["Visualizacao"]);
    }
    [Fact(DisplayName="QP03 | Caminho especial de aparência | Sufixo anterior evita colisão com biblioteca")]
    public void Caso03()
    {
        using var a=new AmbienteBackup(); var c=new CaminhosDadosLocais(Path.Combine(a.Dados,"aparencia.preferencias.json")); Assert.EndsWith(".visual.json",c.Arquivos["Aparencia"]);
    }
    [Fact(DisplayName="QP04 | Preferência ausente | Grade sem criar arquivo")]
    public void Caso04()
    {
        using var a=new AmbienteBackup(); var c=new ArmazenamentoPreferenciasBiblioteca(a.Caminhos.Arquivos["Visualizacao"]); Assert.Equal(VisualizacaoBiblioteca.Grade,c.Carregar()); Assert.False(File.Exists(c.CaminhoArquivo));
    }
    [Fact(DisplayName="QP05 | Preferência versão anterior | Lista versão 1 continua legível")]
    public void Caso05()
    {
        using var a=new AmbienteBackup(); File.WriteAllText(a.Caminhos.Arquivos["Visualizacao"],"{\"Versao\":1,\"Visualizacao\":1}"); Assert.Equal(VisualizacaoBiblioteca.Lista,new ArmazenamentoPreferenciasBiblioteca(a.Caminhos.Arquivos["Visualizacao"]).Carregar());
    }
    [Fact(DisplayName="QP06 | Visualização inválida | Não é aplicada por importação")]
    public void Caso06()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); d.Componentes["Visualizacao"]=AmbienteBackup.C(Encoding.UTF8.GetBytes("{\"Versao\":1,\"Visualizacao\":99}")); Assert.False(a.Previa(d).PreferenciasValidas);
    }
    [Fact(DisplayName="QP07 | Preferência futura de lista | Dados restauráveis sem preferência futura")]
    public void Caso07()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); d.Componentes["Visualizacao"]=AmbienteBackup.C(Encoding.UTF8.GetBytes("{\"Versao\":2,\"Visualizacao\":0}")); var prev=a.Previa(d); Assert.False(prev.PreferenciasValidas); a.Restaurado(prev);
    }
    [Fact(DisplayName="QP08 | Extensões de aparência | Dados desconhecidos preservados no backup e restauro")]
    public void Caso08()
    {
        using var a=new AmbienteBackup(); File.WriteAllText(a.Caminhos.Arquivos["Aparencia"],"{\"Versao\":1,\"Tema\":\"Moderno\",\"Modo\":\"Claro\",\"Extra\":123}"); var d=a.Documento(); File.Delete(a.Caminhos.Arquivos["Aparencia"]); a.Restaurado(a.Previa(d),true); Assert.Contains("Extra",File.ReadAllText(a.Caminhos.Arquivos["Aparencia"]));
    }
    [Fact(DisplayName="QP09 | Tema fixo restaurado | Tema e modo persistem na reabertura")]
    public void Caso09()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var d=a.Documento(); File.Delete(a.Caminhos.Arquivos["Aparencia"]); a.Restaurado(a.Previa(d),true); var l=new ArmazenamentoAparenciaJson(a.Caminhos.Arquivos["Aparencia"]).Carregar(); Assert.Equal(TemaVisual.Acolhedor,l.Preferencias.Tema); Assert.Equal(ModoVisual.Escuro,l.Preferencias.Modo);
    }
    [Fact(DisplayName="QP10 | Lock de visualização | Gravação concorrente falha sem mudar a preferência")]
    public void Caso10()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var s=a.Snapshot(); using var f=new FileStream(a.Caminhos.Arquivos["Visualizacao"]+".lock",FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.None); Assert.Throws<IOException>(()=>new ArmazenamentoPreferenciasBiblioteca(a.Caminhos.Arquivos["Visualizacao"]).Salvar(VisualizacaoBiblioteca.Grade)); a.Igual(s);
    }
    [Fact(DisplayName="QP11 | Diário bloqueia biblioteca | Leitor carregado não pode sobrescrever recuperação")]
    public void Caso11()
    {
        using var a=new AmbienteBackup(); var f=new FileManager(a.Caminhos.Arquivos["Biblioteca"]); var b=f.Carregar(); File.WriteAllText(a.Caminhos.Diario,"{}"); Assert.Throws<InvalidOperationException>(()=>f.Salvar(b));
    }
    [Fact(DisplayName="QP12 | Diário bloqueia coleções | Gravação de coleções aguarda recuperação")]
    public void Caso12()
    {
        using var a=new AmbienteBackup(); var c=new ArmazenamentoColecoesJson(a.Caminhos.Arquivos["Colecoes"]); var l=c.Carregar(); File.WriteAllText(a.Caminhos.Diario,"{}"); Assert.Throws<InvalidOperationException>(()=>c.Salvar(l));
    }
    [Fact(DisplayName="QP13 | Diário bloqueia aparência | Aparência salva é preservada")]
    public void Caso13()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var c=new ArmazenamentoAparenciaJson(a.Caminhos.Arquivos["Aparencia"]); var p=c.Carregar().Preferencias; File.WriteAllText(a.Caminhos.Diario,"{}"); Assert.Throws<InvalidOperationException>(()=>c.Salvar(p));
    }
    [Fact(DisplayName="QP14 | Diário bloqueia visualização | Lista salva não é trocada durante recuperação")]
    public void Caso14()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); File.WriteAllText(a.Caminhos.Diario,"{}"); Assert.Throws<InvalidOperationException>(()=>new ArmazenamentoPreferenciasBiblioteca(a.Caminhos.Arquivos["Visualizacao"]).Salvar(VisualizacaoBiblioteca.Grade)); Assert.Equal(VisualizacaoBiblioteca.Lista,new ArmazenamentoPreferenciasBiblioteca(a.Caminhos.Arquivos["Visualizacao"]).Carregar());
    }
    [Fact(DisplayName="QP15 | Retomada de gravação | Remover pendência por recuperação permite salvar")]
    public void Caso15()
    {
        using var a=new AmbienteBackup(); var prev=a.Previa(); a.Servico.Etapa=s=>{if(s=="depois-Biblioteca"||s=="rollback-Biblioteca")throw new IOException();}; Assert.Throws<IOException>(()=>a.Servico.Restaurar(prev,false,true)); a.Servico.Etapa=null; Assert.True(a.Servico.Recuperar()); new ArmazenamentoPreferenciasBiblioteca(a.Caminhos.Arquivos["Visualizacao"]).Salvar(VisualizacaoBiblioteca.Lista); Assert.Equal(VisualizacaoBiblioteca.Lista,new ArmazenamentoPreferenciasBiblioteca(a.Caminhos.Arquivos["Visualizacao"]).Carregar());
    }
    [Fact(DisplayName="QP16 | Preferência duplicada | Dados válidos mas preferências ambíguas não são aplicadas")]
    public void Caso16()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); d.Componentes["Visualizacao"]=AmbienteBackup.C(Encoding.UTF8.GetBytes("{\"Versao\":1,\"Visualizacao\":0,\"Visualizacao\":1}")); Assert.False(a.Previa(d).PreferenciasValidas);
    }
}
