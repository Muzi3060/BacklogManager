using System.Text.Json;
using BacklogManager.Backup;
using Xunit;
namespace BacklogManager.Tests.Fase7;
public sealed class DiarioDefensivoTests
{
    [Fact(DisplayName="QC29 | Diário acima do limite | Recuperação bloqueia antes de alocar conteúdo excessivo")]
    public void Limite()
    {
        using var a=new AmbienteBackup();var antes=a.Snapshot();using(var f=File.Create(a.Caminhos.Diario))f.SetLength(ArquivosBackup.Limite*3L+1);
        Assert.False(a.Servico.Recuperar());a.Igual(antes);Assert.True(File.Exists(a.Caminhos.Diario));
    }
    [Fact(DisplayName="QC30 | Diário com manifesto anterior nulo | Recuperação mantém bloqueio sem exceção não tratada")]
    public void ManifestoNulo()
    {
        using var a=new AmbienteBackup();var antes=a.Snapshot();var d=new DiarioRestauracao{Versao=1,Estado="Aplicando",Anterior=AmbienteBackup.Basico() with {Componentes=null!},Novos=AmbienteBackup.Basico().Componentes};
        File.WriteAllBytes(a.Caminhos.Diario,JsonSerializer.SerializeToUtf8Bytes(d));Assert.False(a.Servico.Recuperar());a.Igual(antes);
    }
    [Fact(DisplayName="QC31 | Destino arbitrário no diário | Caminhos externos são recusados e estado atual preservado")]
    public void DestinoExterno()
    {
        using var a=new AmbienteBackup();var antes=a.Snapshot();var anterior=a.Documento();var novos=AmbienteBackup.Basico().Componentes;var alvo=Path.Combine(a.Raiz,"protegido.json");File.WriteAllText(alvo,"inalterado");novos[alvo]=AmbienteBackup.C(null);
        var d=new DiarioRestauracao{Versao=1,Estado="Aplicando",Anterior=anterior,Novos=novos};File.WriteAllBytes(a.Caminhos.Diario,JsonSerializer.SerializeToUtf8Bytes(d));
        Assert.False(a.Servico.Recuperar());Assert.Equal("inalterado",File.ReadAllText(alvo));a.Igual(antes);
    }
}
