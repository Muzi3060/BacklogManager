using System.Diagnostics;
using BacklogManager.Backup;
using BacklogManager.Configuracao;
using Xunit;
namespace BacklogManager.Tests.Fase7;
public sealed class ComposicaoBackupTests
{
    [Fact(DisplayName="QC28 | Composição após processo interrompido | Recuperação ocorre antes de carregar biblioteca e coleções")]
    public void InicioReal()
    {
        using var a=new AmbienteBackup();a.Preferencias();var antes=a.Snapshot();ProcessoBackup.Interromper(a,"depois-Biblioteca");
        var deps=ComposicaoAplicativo.Criar(a.Caminhos.Arquivos["Biblioteca"],_=>null);
        Assert.Null(deps.Backup!.Pendencia);Assert.False(File.Exists(a.Caminhos.Diario));a.Igual(antes);
        Assert.False(deps.Biblioteca.Carregada);deps.Biblioteca.Carregar();deps.Colecoes!.Carregar();
        Assert.Equal("Anterior",deps.Biblioteca.Itens[0].Catalogo.Titulo);Assert.True(deps.Colecoes.Carregadas);
    }
    [Fact(DisplayName="QI17 | Preparador de validação manual | Dados fictícios e backups válidos sem ler biblioteca pessoal")]
    public void Preparador()
    {
        using var a=new AmbienteBackup();
        var script=Path.GetFullPath("../../../../../scripts/preparar_validacao_fase7.py",AppContext.BaseDirectory);
        var inicio=new ProcessStartInfo("python3"){UseShellExecute=false,RedirectStandardOutput=true,RedirectStandardError=true};inicio.ArgumentList.Add(script);
        using var processo=Process.Start(inicio)!;Assert.True(processo.WaitForExit(30000));Assert.Equal(0,processo.ExitCode);
        var linhas=processo.StandardOutput.ReadToEnd().Split('\n');
        var raiz=linhas.Single(l=>l.StartsWith("Arquivos de backup fictícios: ")).Substring("Arquivos de backup fictícios: ".Length);
        Assert.StartsWith(Path.Combine(Path.GetTempPath(),"minhas-realizacoes-fase7-rider-"),raiz);
        try
        {
            var previa=a.Servico.Prever(Path.Combine(raiz,"backup-simulado.json"));
            Assert.Equal(7,previa.Obras);Assert.Equal(2,previa.Colecoes);Assert.Equal(3,previa.Vinculos);Assert.True(previa.PreferenciasValidas);
            Assert.Equal(0,a.Servico.Prever(Path.Combine(raiz,"backup-vazio.json")).Obras);
            Assert.ThrowsAny<Exception>(()=>a.Servico.Prever(Path.Combine(raiz,"backup-invalido.json")));
        }
        finally {Directory.Delete(raiz,true);}
    }
}
