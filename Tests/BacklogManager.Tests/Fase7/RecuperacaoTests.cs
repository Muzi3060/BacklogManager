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
public sealed class RecuperacaoTests
{
    [Fact(DisplayName="QC01 | Falha antes-Biblioteca | Estado anterior recuperado e verificado sem sucesso parcial")]
    public void Caso01()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var d=a.Documento(); d.Componentes["Biblioteca"]=AmbienteBackup.Basico().Componentes["Biblioteca"]; var p=a.Previa(d); var antes=a.Snapshot(); a.Servico.Etapa=s=>{if(s=="antes-Biblioteca")throw new IOException("falha simulada");}; Assert.Throws<IOException>(()=>a.Servico.Restaurar(p,true,true)); a.Igual(antes); Assert.False(File.Exists(a.Caminhos.Diario)); Assert.True(File.Exists(a.Servico.UltimoBackupSeguranca));
    }
    [Fact(DisplayName="QC02 | Falha depois-Biblioteca | Estado anterior recuperado e verificado sem sucesso parcial")]
    public void Caso02()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var d=a.Documento(); d.Componentes["Biblioteca"]=AmbienteBackup.Basico().Componentes["Biblioteca"]; var p=a.Previa(d); var antes=a.Snapshot(); a.Servico.Etapa=s=>{if(s=="depois-Biblioteca")throw new IOException("falha simulada");}; Assert.Throws<IOException>(()=>a.Servico.Restaurar(p,true,true)); a.Igual(antes); Assert.False(File.Exists(a.Caminhos.Diario)); Assert.True(File.Exists(a.Servico.UltimoBackupSeguranca));
    }
    [Fact(DisplayName="QC03 | Falha antes-Colecoes | Estado anterior recuperado e verificado sem sucesso parcial")]
    public void Caso03()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var d=a.Documento(); d.Componentes["Biblioteca"]=AmbienteBackup.Basico().Componentes["Biblioteca"]; var p=a.Previa(d); var antes=a.Snapshot(); a.Servico.Etapa=s=>{if(s=="antes-Colecoes")throw new IOException("falha simulada");}; Assert.Throws<IOException>(()=>a.Servico.Restaurar(p,true,true)); a.Igual(antes); Assert.False(File.Exists(a.Caminhos.Diario)); Assert.True(File.Exists(a.Servico.UltimoBackupSeguranca));
    }
    [Fact(DisplayName="QC04 | Falha depois-Colecoes | Estado anterior recuperado e verificado sem sucesso parcial")]
    public void Caso04()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var d=a.Documento(); d.Componentes["Biblioteca"]=AmbienteBackup.Basico().Componentes["Biblioteca"]; var p=a.Previa(d); var antes=a.Snapshot(); a.Servico.Etapa=s=>{if(s=="depois-Colecoes")throw new IOException("falha simulada");}; Assert.Throws<IOException>(()=>a.Servico.Restaurar(p,true,true)); a.Igual(antes); Assert.False(File.Exists(a.Caminhos.Diario)); Assert.True(File.Exists(a.Servico.UltimoBackupSeguranca));
    }
    [Fact(DisplayName="QC05 | Falha antes-Visualizacao | Estado anterior recuperado e verificado sem sucesso parcial")]
    public void Caso05()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var d=a.Documento(); d.Componentes["Biblioteca"]=AmbienteBackup.Basico().Componentes["Biblioteca"]; var p=a.Previa(d); var antes=a.Snapshot(); a.Servico.Etapa=s=>{if(s=="antes-Visualizacao")throw new IOException("falha simulada");}; Assert.Throws<IOException>(()=>a.Servico.Restaurar(p,true,true)); a.Igual(antes); Assert.False(File.Exists(a.Caminhos.Diario)); Assert.True(File.Exists(a.Servico.UltimoBackupSeguranca));
    }
    [Fact(DisplayName="QC06 | Falha depois-Visualizacao | Estado anterior recuperado e verificado sem sucesso parcial")]
    public void Caso06()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var d=a.Documento(); d.Componentes["Biblioteca"]=AmbienteBackup.Basico().Componentes["Biblioteca"]; var p=a.Previa(d); var antes=a.Snapshot(); a.Servico.Etapa=s=>{if(s=="depois-Visualizacao")throw new IOException("falha simulada");}; Assert.Throws<IOException>(()=>a.Servico.Restaurar(p,true,true)); a.Igual(antes); Assert.False(File.Exists(a.Caminhos.Diario)); Assert.True(File.Exists(a.Servico.UltimoBackupSeguranca));
    }
    [Fact(DisplayName="QC07 | Falha antes-Aparencia | Estado anterior recuperado e verificado sem sucesso parcial")]
    public void Caso07()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var d=a.Documento(); d.Componentes["Biblioteca"]=AmbienteBackup.Basico().Componentes["Biblioteca"]; var p=a.Previa(d); var antes=a.Snapshot(); a.Servico.Etapa=s=>{if(s=="antes-Aparencia")throw new IOException("falha simulada");}; Assert.Throws<IOException>(()=>a.Servico.Restaurar(p,true,true)); a.Igual(antes); Assert.False(File.Exists(a.Caminhos.Diario)); Assert.True(File.Exists(a.Servico.UltimoBackupSeguranca));
    }
    [Fact(DisplayName="QC08 | Falha depois-Aparencia | Estado anterior recuperado e verificado sem sucesso parcial")]
    public void Caso08()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var d=a.Documento(); d.Componentes["Biblioteca"]=AmbienteBackup.Basico().Componentes["Biblioteca"]; var p=a.Previa(d); var antes=a.Snapshot(); a.Servico.Etapa=s=>{if(s=="depois-Aparencia")throw new IOException("falha simulada");}; Assert.Throws<IOException>(()=>a.Servico.Restaurar(p,true,true)); a.Igual(antes); Assert.False(File.Exists(a.Caminhos.Diario)); Assert.True(File.Exists(a.Servico.UltimoBackupSeguranca));
    }
    [Fact(DisplayName="QC09 | Falha no diário inicial | Escrita não começa e cópia externa permanece")]
    public void Caso09()
    {
        using var a=new AmbienteBackup(); var p=a.Previa(); var antes=a.Snapshot(); Directory.CreateDirectory(a.Caminhos.Diario); Assert.Throws<IOException>(()=>a.Servico.Restaurar(p,false,true)); a.Igual(antes);
    }
    [Fact(DisplayName="QC10 | Falha após gravar diário | Reversão verificada antes de limpar diário")]
    public void Caso10()
    {
        using var a=new AmbienteBackup(); var p=a.Previa(); var antes=a.Snapshot(); a.Servico.Etapa=s=>{if(s=="diario-gravado")throw new IOException();}; Assert.Throws<IOException>(()=>a.Servico.Restaurar(p,false,true)); a.Igual(antes); Assert.False(File.Exists(a.Caminhos.Diario));
    }
    [Fact(DisplayName="QC11 | Rollback também falha | Diário e cópia preservados com bloqueio explícito")]
    public void Caso11()
    {
        using var a=new AmbienteBackup(); var p=a.Previa(); a.Servico.Etapa=s=>{if(s=="depois-Biblioteca"||s=="rollback-Biblioteca")throw new IOException();}; Assert.Throws<IOException>(()=>a.Servico.Restaurar(p,false,true)); Assert.NotNull(a.Servico.Pendencia); Assert.True(File.Exists(a.Caminhos.Diario)); Assert.Throws<InvalidOperationException>(()=>a.Servico.Exportar(a.Saida,true)); a.Servico.Etapa=null; Assert.True(a.Servico.Recuperar()); Assert.Equal("Anterior",a.LerBiblioteca()[0].Catalogo.Titulo);
    }
    [Fact(DisplayName="QC12 | Mudança externa desconhecida | Recuperação não sobrescreve conteúdo inesperado")]
    public void Caso12()
    {
        using var a=new AmbienteBackup(); var p=a.Previa(); a.Servico.Etapa=s=>{if(s=="depois-Biblioteca"){a.Biblioteca(PastaTeste.Jogo("Externo"));throw new IOException();}}; Assert.Throws<IOException>(()=>a.Servico.Restaurar(p,false,true)); Assert.False(a.Servico.Recuperar()); Assert.Equal("Externo",a.LerBiblioteca()[0].Catalogo.Titulo); Assert.True(File.Exists(a.Caminhos.Diario));
    }
    [Fact(DisplayName="QC13 | Diário truncado | Mantém pendência sem mudar dados")]
    public void Caso13()
    {
        using var a=new AmbienteBackup(); var antes=a.Snapshot(); File.WriteAllText(a.Caminhos.Diario,"{"); Assert.False(a.Servico.Recuperar()); a.Igual(antes); Assert.True(File.Exists(a.Caminhos.Diario));
    }
    [Fact(DisplayName="QC14 | Diário futuro | Versão incompatível não é apagada")]
    public void Caso14()
    {
        using var a=new AmbienteBackup(); File.WriteAllText(a.Caminhos.Diario,"{\"Versao\":99,\"Estado\":\"Aplicando\",\"Anterior\":null,\"Novos\":null}"); Assert.False(a.Servico.Recuperar()); Assert.True(File.Exists(a.Caminhos.Diario));
    }
    [Fact(DisplayName="QC15 | Ausência original | Rollback remove somente arquivo criado pela restauração")]
    public void Caso15()
    {
        using var a=new AmbienteBackup(); File.Delete(a.Caminhos.Arquivos["Biblioteca"]); var antes=a.Snapshot(); var p=a.Previa(); a.Servico.Etapa=s=>{if(s=="depois-Biblioteca")throw new IOException();}; Assert.Throws<IOException>(()=>a.Servico.Restaurar(p,false,true)); a.Igual(antes);
    }
    [Fact(DisplayName="QC16 | Falha antes de confirmar conjunto | Todos os alvos voltam ao estado inicial")]
    public void Caso16()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var p=a.Previa(a.Documento()); var antes=a.Snapshot(); a.Servico.Etapa=s=>{if(s=="antes-confirmacao")throw new IOException();}; Assert.Throws<IOException>(()=>a.Servico.Restaurar(p,true,true)); a.Igual(antes);
    }
    [Fact(DisplayName="QC17 | Processo encerrado em depois-Biblioteca | Nova instância reverte o conjunto pelo diário")]
    public void Caso17()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var antes=a.Snapshot(); ProcessoBackup.Interromper(a,"depois-Biblioteca"); Assert.True(File.Exists(a.Caminhos.Diario)); var novo=new ServicoBackupLocal(a.Caminhos,a.Servico.PastaSeguranca); Assert.True(novo.Recuperar()); a.Igual(antes); Assert.False(File.Exists(a.Caminhos.Diario));
    }
    [Fact(DisplayName="QC18 | Processo encerrado em depois-Colecoes | Nova instância reverte o conjunto pelo diário")]
    public void Caso18()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var antes=a.Snapshot(); ProcessoBackup.Interromper(a,"depois-Colecoes"); Assert.True(File.Exists(a.Caminhos.Diario)); var novo=new ServicoBackupLocal(a.Caminhos,a.Servico.PastaSeguranca); Assert.True(novo.Recuperar()); a.Igual(antes); Assert.False(File.Exists(a.Caminhos.Diario));
    }
    [Fact(DisplayName="QC19 | Processo encerrado em depois-Visualizacao | Nova instância reverte o conjunto pelo diário")]
    public void Caso19()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var antes=a.Snapshot(); ProcessoBackup.Interromper(a,"depois-Visualizacao"); Assert.True(File.Exists(a.Caminhos.Diario)); var novo=new ServicoBackupLocal(a.Caminhos,a.Servico.PastaSeguranca); Assert.True(novo.Recuperar()); a.Igual(antes); Assert.False(File.Exists(a.Caminhos.Diario));
    }
    [Fact(DisplayName="QC20 | Processo encerrado em depois-Aparencia | Nova instância reverte o conjunto pelo diário")]
    public void Caso20()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var antes=a.Snapshot(); ProcessoBackup.Interromper(a,"depois-Aparencia"); Assert.True(File.Exists(a.Caminhos.Diario)); var novo=new ServicoBackupLocal(a.Caminhos,a.Servico.PastaSeguranca); Assert.True(novo.Recuperar()); a.Igual(antes); Assert.False(File.Exists(a.Caminhos.Diario));
    }
    [Fact(DisplayName="QC21 | Processo encerrado em diario-gravado | Nova instância reverte o conjunto pelo diário")]
    public void Caso21()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var antes=a.Snapshot(); ProcessoBackup.Interromper(a,"diario-gravado"); Assert.True(File.Exists(a.Caminhos.Diario)); var novo=new ServicoBackupLocal(a.Caminhos,a.Servico.PastaSeguranca); Assert.True(novo.Recuperar()); a.Igual(antes); Assert.False(File.Exists(a.Caminhos.Diario));
    }
    [Fact(DisplayName="QC22 | Processo encerra após confirmar | Nova instância preserva conjunto já confirmado")]
    public void Caso22()
    {
        using var a=new AmbienteBackup(); ProcessoBackup.Interromper(a,"confirmada"); var novo=new ServicoBackupLocal(a.Caminhos,a.Servico.PastaSeguranca); Assert.True(novo.Recuperar()); Assert.Equal("Restaurado",a.LerBiblioteca()[0].Catalogo.Titulo); Assert.Equal(VisualizacaoBiblioteca.Grade,new ArmazenamentoPreferenciasBiblioteca(a.Caminhos.Arquivos["Visualizacao"]).Carregar()); Assert.False(File.Exists(a.Caminhos.Diario));
    }
    [Fact(DisplayName="QC23 | Recuperação sem diário | Operação idempotente não muda dados")]
    public void Caso23()
    {
        using var a=new AmbienteBackup(); var antes=a.Snapshot(); Assert.True(a.Servico.Recuperar()); Assert.True(a.Servico.Recuperar()); a.Igual(antes);
    }
    [Fact(DisplayName="QC24 | Composição encontra diário incompatível | Pendência detectada antes dos leitores da janela")]
    public void Caso24()
    {
        using var a=new AmbienteBackup(); File.WriteAllText(a.Caminhos.Diario,"{}"); var deps=BacklogManager.Configuracao.ComposicaoAplicativo.Criar(a.Caminhos.Arquivos["Biblioteca"],_=>null); Assert.NotNull(deps.Backup!.Pendencia); Assert.False(deps.Biblioteca.Carregada); Assert.False(deps.Colecoes!.Carregadas);
    }
}
