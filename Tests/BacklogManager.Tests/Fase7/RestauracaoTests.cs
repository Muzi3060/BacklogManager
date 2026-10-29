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
public sealed class RestauracaoTests
{
    [Fact(DisplayName="QR01 | Substituição completa | Novo conjunto e cópia anterior verificáveis")]
    public void Caso01()
    {
        using var a=new AmbienteBackup(); var prev=a.Previa(); a.Restaurado(prev); Assert.Equal("Restaurado",a.LerBiblioteca()[0].Catalogo.Titulo);
    }
    [Fact(DisplayName="QR02 | Obras não mescladas | Itens atuais são substituídos e não somados")]
    public void Caso02()
    {
        using var a=new AmbienteBackup(); a.Biblioteca(PastaTeste.Jogo("A"),PastaTeste.Jogo("B")); a.Restaurado(a.Previa()); Assert.Single(a.LerBiblioteca());
    }
    [Fact(DisplayName="QR03 | Preferências preservadas | Restauração padrão altera somente dados")]
    public void Caso03()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var antes=a.Snapshot(); a.Restaurado(a.Previa()); Assert.Equal(antes["Aparencia"],File.ReadAllBytes(a.Caminhos.Arquivos["Aparencia"])); Assert.Equal(antes["Visualizacao"],File.ReadAllBytes(a.Caminhos.Arquivos["Visualizacao"]));
    }
    [Fact(DisplayName="QR04 | Preferências selecionadas | Tema e lista do backup aplicados")]
    public void Caso04()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var d=a.Documento(); a.Biblioteca(PastaTeste.Jogo("Novo")); File.Delete(a.Caminhos.Arquivos["Visualizacao"]); File.Delete(a.Caminhos.Arquivos["Aparencia"]); a.Restaurado(a.Previa(d),true); Assert.Equal(VisualizacaoBiblioteca.Lista,new ArmazenamentoPreferenciasBiblioteca(a.Caminhos.Arquivos["Visualizacao"]).Carregar()); Assert.Equal(TemaVisual.Acolhedor,new ArmazenamentoAparenciaJson(a.Caminhos.Arquivos["Aparencia"]).Carregar().Preferencias.Tema);
    }
    [Fact(DisplayName="QR05 | Preferência ausente não apaga | Mesmo optando por preferências as atuais sobrevivem")]
    public void Caso05()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var antes=a.Snapshot(); var d=AmbienteBackup.Basico(); d.Componentes["Aparencia"]=AmbienteBackup.C(null); a.Restaurado(a.Previa(d),true); Assert.Equal(antes["Aparencia"],File.ReadAllBytes(a.Caminhos.Arquivos["Aparencia"]));
    }
    [Fact(DisplayName="QR06 | Backup com ausência de biblioteca | Estado vazio confirmado deixa arquivo ausente")]
    public void Caso06()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); d.Componentes["Biblioteca"]=AmbienteBackup.C(null); a.Restaurado(a.Previa(d)); Assert.False(File.Exists(a.Caminhos.Arquivos["Biblioteca"])); Assert.Empty(a.LerBiblioteca());
    }
    [Fact(DisplayName="QR07 | Ausência de coleções | Coleções antigas removidas sem apagar obras restauradas")]
    public void Caso07()
    {
        using var a=new AmbienteBackup(); a.Colecoes(new ColecaoPersonalizada(Guid.NewGuid(),"Antiga","",Array.Empty<Guid>())); var d=AmbienteBackup.Basico(); d.Componentes["Colecoes"]=AmbienteBackup.C(null); a.Restaurado(a.Previa(d)); Assert.False(File.Exists(a.Caminhos.Arquivos["Colecoes"])); Assert.Single(a.LerBiblioteca());
    }
    [Fact(DisplayName="QR08 | Duplicatas no restauro | Duas obras iguais preservadas")]
    public void Caso08()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); var j=PastaTeste.Jogo(); d.Componentes["Biblioteca"]=AmbienteBackup.C(JsonSerializer.SerializeToUtf8Bytes(new[]{j,j})); a.Restaurado(a.Previa(d)); Assert.Equal(2,a.LerBiblioteca().Count);
    }
    [Fact(DisplayName="QR09 | Identidades e vínculos | Várias coleções continuam apontando à mesma obra")]
    public void Caso09()
    {
        using var a=new AmbienteBackup(); var j=PastaTeste.Jogo(); j.IdInterno=Guid.NewGuid(); a.Biblioteca(j); a.Colecoes(new ColecaoPersonalizada(Guid.NewGuid(),"A","",new[]{j.IdInterno.Value}),new ColecaoPersonalizada(Guid.NewGuid(),"B","",new[]{j.IdInterno.Value})); var d=a.Documento(); a.Biblioteca(); a.Colecoes(); a.Restaurado(a.Previa(d)); Assert.Equal(j.IdInterno,a.LerBiblioteca()[0].IdInterno); Assert.Equal(2,a.Servico.Prever(a.Saida).Vinculos);
    }
    [Fact(DisplayName="QR10 | Steam legado | Valor antigo não é reinterpretado como IGDB")]
    public void Caso10()
    {
        using var a=new AmbienteBackup(); var j=PastaTeste.Jogo(); j.SteamAppId=361855; a.Biblioteca(j); var d=a.Documento(); a.Restaurado(a.Previa(d)); var salvo=new FileManager(a.Caminhos.Arquivos["Biblioteca"]).Carregar()[0]; Assert.Equal(361855,salvo.SteamAppId); Assert.Null(salvo.IgdbId);
    }
    [Fact(DisplayName="QR11 | Campos desconhecidos | Metadados extras preservados byte a byte")]
    public void Caso11()
    {
        using var a=new AmbienteBackup(); var j=PastaTeste.Jogo(); j.DadosAdicionais=new(){{"Extra",JsonSerializer.SerializeToElement(new{X="valor"})}}; a.Biblioteca(j); var d=a.Documento(); a.Biblioteca(); a.Restaurado(a.Previa(d)); Assert.Equal(d.Componentes["Biblioteca"].Conteudo,File.ReadAllBytes(a.Caminhos.Arquivos["Biblioteca"]));
    }
    [Fact(DisplayName="QR12 | Zero legado | Progresso zero carregado sem migração")]
    public void Caso12()
    {
        using var a=new AmbienteBackup(); var j=ConversorBibliotecaJson.ParaRegistro(Fase2.ApoioMultimidia.Item(TipoMidia.Livro,new(){Pagina=0})); a.Biblioteca(j); var d=a.Documento(); a.Restaurado(a.Previa(d)); Assert.Equal(0,a.LerBiblioteca()[0].Pessoal.Progresso!.Pagina);
    }
    [Fact(DisplayName="QR13 | Sem datas inventadas | Registro sem data continua sem data")]
    public void Caso13()
    {
        using var a=new AmbienteBackup(); a.Restaurado(a.Previa()); Assert.Null(a.LerBiblioteca()[0].DataAdicao);
    }
    [Fact(DisplayName="QR14 | Seis mídias e progresso | Conteúdo multimídia não muda")]
    public void Caso14()
    {
        using var a=new AmbienteBackup(); a.Biblioteca(Enum.GetValues<TipoMidia>().Select(t=>ConversorBibliotecaJson.ParaRegistro(Fase2.ApoioMultimidia.Item(t))).ToArray()); var d=a.Documento(); a.Biblioteca(); a.Restaurado(a.Previa(d)); Assert.Equal(6,a.LerBiblioteca().Select(i=>i.Catalogo.Tipo).Distinct().Count());
    }
    [Fact(DisplayName="QR15 | Referências ausentes | Vínculo órfão permanece disponível para recuperação posterior")]
    public void Caso15()
    {
        using var a=new AmbienteBackup(); a.Colecoes(new ColecaoPersonalizada(Guid.NewGuid(),"Órfãos","",new[]{Guid.NewGuid()})); var d=a.Documento(); a.Restaurado(a.Previa(d)); Assert.Equal(1,a.Servico.Prever(a.Saida).ReferenciasAusentes);
    }
    [Fact(DisplayName="QR16 | Falha no backup obrigatório | Nenhuma escrita ativa ou diário")]
    public void Caso16()
    {
        using var a=new AmbienteBackup(); var prev=a.Previa(); var antes=a.Snapshot(); a.Servico.Etapa=s=>{if(s=="antes-backup")throw new IOException("disco");}; Assert.Throws<IOException>(()=>a.Servico.Restaurar(prev,false,true)); a.Igual(antes); Assert.False(File.Exists(a.Caminhos.Diario));
    }
    [Fact(DisplayName="QR17 | Pasta externa bloqueada | Sem substituir quando backup não pode ser criado")]
    public void Caso17()
    {
        using var a=new AmbienteBackup(); var prev=a.Previa(); var antes=a.Snapshot(); File.WriteAllText(a.Servico.PastaSeguranca,"bloqueio"); Assert.Throws<IOException>(()=>a.Servico.Restaurar(prev,false,true)); a.Igual(antes);
    }
    [Fact(DisplayName="QR18 | Backup dentro dos dados | Destino de segurança inadequado é recusado")]
    public void Caso18()
    {
        using var a=new AmbienteBackup(); var prev=a.Previa(); var s=new ServicoBackupLocal(a.Caminhos,a.Dados); var antes=a.Snapshot(); Assert.Throws<IOException>(()=>s.Restaurar(prev,false,true)); a.Igual(antes);
    }
    [Fact(DisplayName="QR19 | Falha depois de verificar cópia | Backup permanece mas dados não mudam")]
    public void Caso19()
    {
        using var a=new AmbienteBackup(); var prev=a.Previa(); var antes=a.Snapshot(); a.Servico.Etapa=s=>{if(s=="backup-verificado")throw new IOException("falha");}; Assert.Throws<IOException>(()=>a.Servico.Restaurar(prev,false,true)); a.Igual(antes); Assert.True(File.Exists(a.Servico.UltimoBackupSeguranca));
    }
    [Fact(DisplayName="QR20 | Lock de restauração | Outra instância impede alteração")]
    public void Caso20()
    {
        using var a=new AmbienteBackup(); var prev=a.Previa(); var antes=a.Snapshot(); using var l=new FileStream(a.Caminhos.Trava,FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.None); Assert.Throws<IOException>(()=>a.Servico.Restaurar(prev,false,true)); a.Igual(antes);
    }
    [Fact(DisplayName="QR21 | Backups antigos | Arquivos .bak existentes não são consumidos")]
    public void Caso21()
    {
        using var a=new AmbienteBackup(); File.WriteAllText(a.Caminhos.Arquivos["Biblioteca"]+".bak","antigo"); a.Restaurado(a.Previa()); Assert.Equal("antigo",File.ReadAllText(a.Caminhos.Arquivos["Biblioteca"]+".bak"));
    }
    [Fact(DisplayName="QR22 | Estado atual inválido | Bytes danificados são recuperáveis na cópia obrigatória")]
    public void Caso22()
    {
        using var a=new AmbienteBackup(); File.WriteAllText(a.Caminhos.Arquivos["Biblioteca"],"{"); var prev=a.Previa(); Assert.Equal(-1,prev.ObrasAtuais); a.Restaurado(prev); Assert.Equal("Restaurado",a.LerBiblioteca()[0].Catalogo.Titulo);
    }
    [Fact(DisplayName="QR23 | Preferências não incluídas | Optar por preferências não inventa componentes")]
    public void Caso23()
    {
        using var a=new AmbienteBackup(); a.Preferencias(); var antes=a.Snapshot(); a.Restaurado(a.Previa(),true); Assert.Equal(antes["Visualizacao"],File.ReadAllBytes(a.Caminhos.Arquivos["Visualizacao"]));
    }
    [Fact(DisplayName="QR24 | Reabertura após restauro | Leitores novos recarregam o conjunto e permitem salvar")]
    public void Caso24()
    {
        using var a=new AmbienteBackup(); a.Restaurado(a.Previa()); var f=new FileManager(a.Caminhos.Arquivos["Biblioteca"]); var itens=f.Carregar(); itens[0].Review="Editado"; f.Salvar(itens); Assert.Equal("Editado",a.LerBiblioteca()[0].Pessoal.Review);
    }
}
