using Avalonia.Headless.XUnit;
using BacklogManager.Aparencia;
using Xunit;
namespace BacklogManager.Tests.Fase7;
public sealed class PreferenciasNaSessaoTests
{
    private static void PersonalizarSemSalvar(TelaBackup t,AmbienteBackup a)
    {
        using var f=new FileStream(a.Caminhos.Arquivos["Aparencia"]+".lock",FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.None);
        t.Janela.AparenciaAtual.Personalizar(ElementoCor.FundoPrincipal,"#234567");Assert.True(t.Janela.AparenciaAtual.PendenteSalvar);
    }
    private static void Conferir(TelaBackup t)
    {
        Assert.True(t.Janela.AparenciaAtual.PendenteSalvar);
        Assert.Equal("#234567",t.Janela.AparenciaAtual.Preferencias.Personalizacoes["Acolhedor/Escuro"][ElementoCor.FundoPrincipal]);
    }
    [AvaloniaFact(DisplayName="QI18 | Restauração somente dos dados e cor pendente | Escolha da sessão não é descartada pela recarga dos dados")]
    public async Task Dados()
    {
        using var a=new AmbienteBackup();a.Preferencias();using var t=new TelaBackup(a);PersonalizarSemSalvar(t,a);
        var antes=a.Snapshot();a.Importar();await t.Selecionar(a.Importado);t.Confirmar();Conferir(t);
        Assert.Equal(antes["Aparencia"],File.ReadAllBytes(a.Caminhos.Arquivos["Aparencia"]));
    }
    [AvaloniaFact(DisplayName="QI19 | Backup só com preferência de lista | Optar por preferências não reinicia a aparência ausente do backup")]
    public async Task Visualizacao()
    {
        using var a=new AmbienteBackup();a.Preferencias();using var t=new TelaBackup(a);PersonalizarSemSalvar(t,a);
        var d=AmbienteBackup.Basico();d.Componentes["Visualizacao"]=AmbienteBackup.C(System.Text.Encoding.UTF8.GetBytes("{\"Versao\":1,\"Visualizacao\":0}"));
        a.Importar(d);await t.Selecionar(a.Importado);t.Confirmar(true);Conferir(t);
        Assert.Equal(0,t.Campo<Avalonia.Controls.ComboBox>("CmbVisualizacao").SelectedIndex);
    }
}
