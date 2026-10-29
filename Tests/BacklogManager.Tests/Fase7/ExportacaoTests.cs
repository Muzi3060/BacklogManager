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
public sealed class ExportacaoTests
{
    [Fact(DisplayName="QE01 | Exportação vazia | Zero obras e zero coleções verificadas")]
    public void Caso01()
    {
        using var a=new AmbienteBackup(); a.Biblioteca(); a.Documento(); Assert.Equal(0,a.Servico.Prever(a.Saida).Obras);
    }
    [Fact(DisplayName="QE02 | Legado sem identidade | Steam preservado sem IgdbId nem identidade")]
    public void Caso02()
    {
        using var a=new AmbienteBackup(); var d=a.Documento(); var p=JsonSerializer.Deserialize<BacklogItem[]>(d.Componentes["Biblioteca"].Conteudo!)![0]; Assert.Equal(1010,p.SteamAppId); Assert.Null(p.IgdbId); Assert.Null(p.IdInterno);
    }
    [Fact(DisplayName="QE03 | Seis mídias | Todos os tipos exportados sem conversão")]
    public void Caso03()
    {
        using var a=new AmbienteBackup(); a.Biblioteca(Enum.GetValues<TipoMidia>().Select(t=>ConversorBibliotecaJson.ParaRegistro(Fase2.ApoioMultimidia.Item(t))).ToArray()); a.Documento(); Assert.Equal(6,a.Servico.Prever(a.Saida).Obras);
    }
    [Fact(DisplayName="QE04 | Duplicatas pessoais | Duas obras iguais continuam presentes")]
    public void Caso04()
    {
        using var a=new AmbienteBackup(); var j=PastaTeste.Jogo(); a.Biblioteca(j,j); a.Documento(); Assert.Equal(2,a.Servico.Prever(a.Saida).Obras);
    }
    [Fact(DisplayName="QE05 | Coleções compartilhadas | Vínculo e identidade exatos")]
    public void Caso05()
    {
        using var a=new AmbienteBackup(); var j=PastaTeste.Jogo(); j.IdInterno=Guid.NewGuid(); a.Biblioteca(j); a.Colecoes(new ColecaoPersonalizada(Guid.NewGuid(),"C","Descrição",new[]{j.IdInterno.Value})); a.Documento(); Assert.Equal(1,a.Servico.Prever(a.Saida).Vinculos);
    }
    [Fact(DisplayName="QE06 | Preferências incluídas | Quatro componentes e preferências válidas")]
    public void Caso06()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); Assert.Equal(4,a.Documento().Componentes.Count); Assert.True(a.Servico.Prever(a.Saida).PreferenciasValidas);
    }
    [Fact(DisplayName="QE07 | Preferências omitidas | Somente o par obrigatório")]
    public void Caso07()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); Assert.Equal(2,a.Documento(false).Componentes.Count);
    }
    [Fact(DisplayName="QE08 | Componentes ausentes | Ausência explicitamente representada")]
    public void Caso08()
    {
        using var a=new AmbienteBackup(); File.Delete(a.Caminhos.Arquivos["Colecoes"]); var d=a.Documento(); Assert.Null(d.Componentes["Colecoes"].Conteudo); Assert.Equal("AUSENTE",d.Componentes["Aparencia"].Sha256);
    }
    [Fact(DisplayName="QE09 | Biblioteca ilegível | Sem criação de falso backup vazio")]
    public void Caso09()
    {
        using var a=new AmbienteBackup(); Directory.CreateDirectory(a.Caminhos.Arquivos["Aparencia"]); Assert.ThrowsAny<Exception>(()=>a.Documento()); Assert.False(File.Exists(a.Saida));
    }
    [Fact(DisplayName="QE10 | Destino anterior | Backup anterior permanece em .bak")]
    public void Caso10()
    {
        using var a=new AmbienteBackup(); File.WriteAllText(a.Saida,"anterior"); a.Documento(); Assert.Equal("anterior",File.ReadAllText(a.Saida+".bak"));
    }
    [Fact(DisplayName="QE11 | Destino biblioteca | Arquivo ativo permanece intacto")]
    public void Caso11()
    {
        using var a=new AmbienteBackup(); var s=a.Snapshot(); Assert.Throws<IOException>(()=>a.Servico.Exportar(a.Caminhos.Arquivos["Biblioteca"],true)); a.Igual(s);
    }
    [Fact(DisplayName="QE12 | Destino coleções | Coleções não são sobrescritas")]
    public void Caso12()
    {
        using var a=new AmbienteBackup(); var s=a.Snapshot(); Assert.Throws<IOException>(()=>a.Servico.Exportar(a.Caminhos.Arquivos["Colecoes"],true)); a.Igual(s);
    }
    [Fact(DisplayName="QE13 | Destino preferência | Preferências não são sobrescritas")]
    public void Caso13()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var s=a.Snapshot(); Assert.Throws<IOException>(()=>a.Servico.Exportar(a.Caminhos.Arquivos["Aparencia"],true)); a.Igual(s);
    }
    [Fact(DisplayName="QE14 | Alias de destino | Link simbólico não é seguido")]
    public void Caso14()
    {
        using var a=new AmbienteBackup(); var link=Path.Combine(a.Raiz,"alias.json"); File.CreateSymbolicLink(link,a.Caminhos.Arquivos["Biblioteca"]); Assert.Throws<IOException>(()=>a.Servico.Exportar(link,true)); Assert.Equal("Anterior",a.LerBiblioteca()[0].Catalogo.Titulo);
    }
    [Fact(DisplayName="QE15 | Destino inexistente | Falha de pasta sem alterar dados")]
    public void Caso15()
    {
        using var a=new AmbienteBackup(); var s=a.Snapshot(); Assert.Throws<DirectoryNotFoundException>(()=>a.Servico.Exportar(Path.Combine(a.Raiz,"ausente","b.json"),true)); a.Igual(s);
    }
    [Fact(DisplayName="QE16 | Componente bloqueado | Conflito de lock aborta exportação")]
    public void Caso16()
    {
        using var a=new AmbienteBackup(); using var l=new FileStream(a.Caminhos.Arquivos["Colecoes"]+".lock",FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.None); Assert.Throws<IOException>(()=>a.Documento()); Assert.False(File.Exists(a.Saida));
    }
    [Fact(DisplayName="QE17 | Mudança durante leitura | Snapshot inconsistente é recusado")]
    public void Caso17()
    {
        using var a=new AmbienteBackup(); a.Servico.Etapa=s=>{if(s=="exportacao-validada") a.Biblioteca(PastaTeste.Jogo("Externo"));}; Assert.Throws<IOException>(()=>a.Documento()); Assert.False(File.Exists(a.Saida));
    }
    [Fact(DisplayName="QE18 | Serviço carregado divergente | Arquivo alterado fora da janela é recusado")]
    public void Caso18()
    {
        using var a=new AmbienteBackup(); var f=new FileManager(a.Caminhos.Arquivos["Biblioteca"]); f.Carregar(); a.Servico.ConferirEstadoCarregado=f.ConferirEstadoAtual; a.Biblioteca(PastaTeste.Jogo("Outro")); Assert.Throws<ArquivoAlteradoException>(()=>a.Documento());
    }
    [Fact(DisplayName="QE19 | Bytes e BOM | UTF16 e campos desconhecidos idênticos")]
    public void Caso19()
    {
        using var a=new AmbienteBackup(); var raw=Encoding.Unicode.GetPreamble().Concat(Encoding.Unicode.GetBytes("[ {\"Titulo\":\"A\",\"Categoria\":\"RPG\",\"Status\":\"Na Fila\",\"Nota\":0,\"Review\":\"\",\"Extra\":{\"x\":1}} ]")).ToArray(); File.WriteAllBytes(a.Caminhos.Arquivos["Biblioteca"],raw); Assert.Equal(raw,a.Documento().Componentes["Biblioteca"].Conteudo);
    }
    [Fact(DisplayName="QE20 | Sem coleta de arquivos pessoais | Somente componentes permitidos são exportados")]
    public void Caso20()
    {
        using var a=new AmbienteBackup(); File.WriteAllText(Path.Combine(a.Dados,".env"),"SEGREDO_TESTE"); File.WriteAllText(Path.Combine(a.Dados,"qodana.yaml"),"PESSOAL_TESTE"); var bytes=JsonSerializer.Serialize(a.Documento()); Assert.DoesNotContain("SEGREDO_TESTE",bytes); Assert.DoesNotContain("PESSOAL_TESTE",bytes); Assert.Equal(4,a.Documento().Componentes.Count);
    }
}
