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
public sealed class ValidacaoTests
{
    [Fact(DisplayName="QV01 | JSON truncado | Importação recusada sem escrita")]
    public void Caso01()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); File.WriteAllText(a.Importado,"{"); Assert.ThrowsAny<Exception>(()=>a.Servico.Prever(a.Importado)); Assert.Equal("Anterior",a.LerBiblioteca()[0].Catalogo.Titulo);
    }
    [Fact(DisplayName="QV02 | Envelope nulo | Documento nulo recusado")]
    public void Caso02()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); File.WriteAllText(a.Importado,"null"); Assert.ThrowsAny<Exception>(()=>a.Servico.Prever(a.Importado));
    }
    [Fact(DisplayName="QV03 | Arquivo isolado | backlog isolado não é backup completo")]
    public void Caso03()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); File.WriteAllText(a.Importado,"[]"); Assert.ThrowsAny<Exception>(()=>a.Servico.Prever(a.Importado));
    }
    [Fact(DisplayName="QV04 | Versão futura | Versão desconhecida recusada")]
    public void Caso04()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); a.Invalido(d with {Versao=2});
    }
    [Fact(DisplayName="QV05 | Formato desconhecido | Identificador incompatível recusado")]
    public void Caso05()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); a.Invalido(d with {Formato="outro"});
    }
    [Fact(DisplayName="QV06 | Sem data declarada | Envelope incompleto recusado")]
    public void Caso06()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); a.Invalido(d with {CriadoEm=default});
    }
    [Fact(DisplayName="QV07 | Biblioteca ausente no manifesto | Componente obrigatório não vira vazio")]
    public void Caso07()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); d.Componentes.Remove("Biblioteca"); a.Invalido(d);
    }
    [Fact(DisplayName="QV08 | Coleções ausentes no manifesto | Par incompleto recusado")]
    public void Caso08()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); d.Componentes.Remove("Colecoes"); a.Invalido(d);
    }
    [Fact(DisplayName="QV09 | Hash divergente | Corrupção detectada antes da escrita")]
    public void Caso09()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); d.Componentes["Biblioteca"]=d.Componentes["Biblioteca"] with {Sha256="0"}; a.Invalido(d);
    }
    [Fact(DisplayName="QV10 | Componente desconhecido | Sem destinos arbitrários")]
    public void Caso10()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); d.Componentes["../../segredo"]=AmbienteBackup.C(null); a.Invalido(d);
    }
    [Fact(DisplayName="QV11 | Propriedade duplicada | Ambiguidade no envelope recusada")]
    public void Caso11()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); a.Importar(d); var s=File.ReadAllText(a.Importado).Replace("\"Versao\": 1","\"Versao\": 1, \"Versao\": 1"); File.WriteAllText(a.Importado,s); Assert.ThrowsAny<Exception>(()=>a.Servico.Prever(a.Importado));
    }
    [Fact(DisplayName="QV12 | Duplicação interna | Biblioteca ambígua recusada")]
    public void Caso12()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); d.Componentes["Biblioteca"]=AmbienteBackup.C(Encoding.UTF8.GetBytes("[{\"Titulo\":\"A\",\"Titulo\":\"B\"}]")); a.Invalido(d);
    }
    [Fact(DisplayName="QV13 | Profundidade excessiva | JSON profundo recusado")]
    public void Caso13()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); File.WriteAllText(a.Importado,new string('[',70)+new string(']',70)); Assert.ThrowsAny<Exception>(()=>a.Servico.Prever(a.Importado));
    }
    [Fact(DisplayName="QV14 | Arquivo muito grande | Limite de 64 MiB aplicado antes da análise")]
    public void Caso14()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); using(var f=File.Create(a.Importado)) f.SetLength(ArquivosBackup.Limite+1L); Assert.Throws<InvalidDataException>(()=>a.Servico.Prever(a.Importado));
    }
    [Fact(DisplayName="QV15 | ID interno repetido | Duplicação de identidade recusada")]
    public void Caso15()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); var j=PastaTeste.Jogo(); j.IdInterno=Guid.NewGuid(); d.Componentes["Biblioteca"]=AmbienteBackup.C(JsonSerializer.SerializeToUtf8Bytes(new[]{j,j})); a.Invalido(d);
    }
    [Fact(DisplayName="QV16 | ID vazio | Identidade vazia recusada")]
    public void Caso16()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); var j=PastaTeste.Jogo(); j.IdInterno=Guid.Empty; d.Componentes["Biblioteca"]=AmbienteBackup.C(JsonSerializer.SerializeToUtf8Bytes(new[]{j})); a.Invalido(d);
    }
    [Fact(DisplayName="QV17 | Mídia inválida | Enum multimídia desconhecido recusado")]
    public void Caso17()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); var j=ConversorBibliotecaJson.ParaRegistro(Fase2.ApoioMultimidia.Item(TipoMidia.Livro)); var s=JsonSerializer.Serialize(new[]{j}).Replace("Livro","Inexistente"); d.Componentes["Biblioteca"]=AmbienteBackup.C(Encoding.UTF8.GetBytes(s)); a.Invalido(d);
    }
    [Fact(DisplayName="QV18 | Nota fora da escala | Nota inválida recusada")]
    public void Caso18()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); var j=PastaTeste.Jogo(); j.Nota=6; d.Componentes["Biblioteca"]=AmbienteBackup.C(JsonSerializer.SerializeToUtf8Bytes(new[]{j})); a.Invalido(d);
    }
    [Fact(DisplayName="QV19 | Progresso negativo | Valor negativo recusado")]
    public void Caso19()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); var j=ConversorBibliotecaJson.ParaRegistro(Fase2.ApoioMultimidia.Item(TipoMidia.Livro,new(){Pagina=1})); d.Componentes["Biblioteca"]=AmbienteBackup.C(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new[]{j}).Replace("\"Pagina\":1","\"Pagina\":-1"))); a.Invalido(d);
    }
    [Fact(DisplayName="QV20 | Nomes repetidos de coleções | Duplicação semântica recusada")]
    public void Caso20()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); d.Componentes["Colecoes"]=AmbienteBackup.C(JsonSerializer.SerializeToUtf8Bytes(new{Versao=1,Colecoes=new[]{new ColecaoPersonalizada(Guid.NewGuid(),"A","",Array.Empty<Guid>()),new ColecaoPersonalizada(Guid.NewGuid(),"a","",Array.Empty<Guid>())}})); a.Invalido(d);
    }
    [Fact(DisplayName="QV21 | Vínculo repetido | Relacionamento duplicado recusado")]
    public void Caso21()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); var id=Guid.NewGuid(); d.Componentes["Colecoes"]=AmbienteBackup.C(JsonSerializer.SerializeToUtf8Bytes(new{Versao=1,Colecoes=new[]{new ColecaoPersonalizada(Guid.NewGuid(),"A","",new[]{id,id})}})); a.Invalido(d);
    }
    [Fact(DisplayName="QV22 | Referência ausente | Avisada e preservada sem reassociação")]
    public void Caso22()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); d.Componentes["Colecoes"]=AmbienteBackup.C(JsonSerializer.SerializeToUtf8Bytes(new{Versao=1,Colecoes=new[]{new ColecaoPersonalizada(Guid.NewGuid(),"A","",new[]{Guid.NewGuid()})}})); var p=a.Previa(d); Assert.Equal(1,p.ReferenciasAusentes); Assert.Contains("preservadas",p.Aviso);
    }
    [Fact(DisplayName="QV23 | Preferência futura | Dados continuam elegíveis sem aplicar preferências")]
    public void Caso23()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); d.Componentes["Aparencia"]=AmbienteBackup.C(Encoding.UTF8.GetBytes("{\"Versao\":99}")); var p=a.Previa(d); Assert.False(p.PreferenciasValidas); a.Restaurado(p);
    }
    [Fact(DisplayName="QV24 | Preferência truncada | Dados válidos podem ser restaurados isoladamente")]
    public void Caso24()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico(); d.Componentes["Visualizacao"]=AmbienteBackup.C(Encoding.UTF8.GetBytes("{")); var p=a.Previa(d); Assert.False(p.PreferenciasValidas); Assert.Throws<InvalidDataException>(()=>a.Servico.Restaurar(p,true,true)); a.Restaurado(p);
    }
}
