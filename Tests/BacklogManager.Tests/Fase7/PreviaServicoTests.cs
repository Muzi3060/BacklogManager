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
public sealed class PreviaServicoTests
{
    [Fact(DisplayName="QQ01 | Prévia não grava | Todos os arquivos ativos permanecem idênticos")]
    public void Caso01()
    {
        using var a=new AmbienteBackup(); var antes=a.Snapshot(); a.Previa(); a.Igual(antes); Assert.Null(a.Servico.UltimoBackupSeguranca);
    }
    [Fact(DisplayName="QQ02 | Contagens e origem | Resumo compara estado atual com backup")]
    public void Caso02()
    {
        using var a=new AmbienteBackup(); var prev=a.Previa(); Assert.Equal(1,prev.ObrasAtuais); Assert.Equal(1,prev.Obras); Assert.Equal(a.Importado,prev.Arquivo);
    }
    [Fact(DisplayName="QQ03 | Backup vazio | Redução para zero explicitamente apresentada")]
    public void Caso03()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); d.Componentes["Biblioteca"]=AmbienteBackup.C(Encoding.UTF8.GetBytes("[]")); var prev=a.Previa(d); Assert.Equal(1,prev.ObrasAtuais); Assert.Equal(0,prev.Obras);
    }
    [Fact(DisplayName="QQ04 | Avisos de relacionamentos | Ausentes contabilizados na prévia")]
    public void Caso04()
    {
        using var a=new AmbienteBackup(); a.Colecoes(new ColecaoPersonalizada(Guid.NewGuid(),"Coleção","",new[]{Guid.NewGuid()})); var prev=a.Servico.Prever(a.Documento() is not null?a.Saida:throw new Exception()); Assert.Equal(1,prev.ReferenciasAusentes);
    }
    [Fact(DisplayName="QQ05 | Consentimento ausente | Sem backup ou escrita sem confirmação")]
    public void Caso05()
    {
        using var a=new AmbienteBackup(); var prev=a.Previa(); var antes=a.Snapshot(); Assert.Throws<InvalidOperationException>(()=>a.Servico.Restaurar(prev,false,false)); a.Igual(antes); Assert.Null(a.Servico.UltimoBackupSeguranca);
    }
    [Fact(DisplayName="QQ06 | Backup alterado depois da prévia | Arquivo é revalidado antes da aplicação")]
    public void Caso06()
    {
        using var a=new AmbienteBackup(); var prev=a.Previa(); File.AppendAllText(a.Importado," "); Assert.Throws<IOException>(()=>a.Servico.Restaurar(prev,false,true)); Assert.Equal("Anterior",a.LerBiblioteca()[0].Catalogo.Titulo);
    }
    [Fact(DisplayName="QQ07 | Dados alterados depois da prévia | Confirmação antiga não apaga edição recente")]
    public void Caso07()
    {
        using var a=new AmbienteBackup(); var prev=a.Previa(); a.Biblioteca(PastaTeste.Jogo("Recente")); Assert.Throws<IOException>(()=>a.Servico.Restaurar(prev,false,true)); Assert.Equal("Recente",a.LerBiblioteca()[0].Catalogo.Titulo);
    }
}
