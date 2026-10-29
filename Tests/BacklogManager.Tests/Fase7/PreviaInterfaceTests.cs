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
public sealed class PreviaInterfaceTests
{
    [AvaloniaFact(DisplayName="QQ08 | Selecionar backup na interface | Prévia visível sem confirmação automática")]
    public async Task Caso08()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); a.Importar(); var antes=a.Snapshot(); await t.Selecionar(a.Importado); a.Igual(antes); Assert.True(t.Campo<Border>("PreviaBackup").IsVisible); Assert.False(t.Campo<Button>("BtnConfirmarBackup").IsEnabled); Assert.NotEqual(true,t.Campo<CheckBox>("ChkRestaurarPreferencias").IsChecked);
    }
    [AvaloniaFact(DisplayName="QQ09 | Cancelar prévia | Seleção e consentimento são descartados")]
    public async Task Caso09()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); a.Importar();await t.Selecionar(a.Importado); t.Campo<CheckBox>("ChkConfirmarSubstituicao").IsChecked=true;t.Clicar("BtnCancelarBackup");Assert.False(t.Campo<Border>("PreviaBackup").IsVisible);Assert.False(t.Campo<Button>("BtnConfirmarBackup").IsEnabled);Assert.Equal("Anterior",a.LerBiblioteca()[0].Catalogo.Titulo);
    }
    [AvaloniaFact(DisplayName="QQ10 | Troca de arquivo selecionado | Nova prévia exige nova confirmação")]
    public async Task Caso10()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); a.Importar();await t.Selecionar(a.Importado);t.Campo<CheckBox>("ChkConfirmarSubstituicao").IsChecked=true;await t.Selecionar(a.Importado);Assert.NotEqual(true,t.Campo<CheckBox>("ChkConfirmarSubstituicao").IsChecked);
    }
    [AvaloniaFact(DisplayName="QQ11 | Seleção inválida após válida | Prévia anterior não pode mais ser aplicada")]
    public async Task Caso11()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); a.Importar();await t.Selecionar(a.Importado);File.WriteAllText(a.Importado,"inválido");await t.Selecionar(a.Importado);Assert.False(t.Campo<Border>("PreviaBackup").IsVisible);t.Clicar("BtnConfirmarBackup");Assert.Equal("Anterior",a.LerBiblioteca()[0].Catalogo.Titulo);
    }
    [AvaloniaFact(DisplayName="QQ12 | Alteração de escopo | Escolher preferências invalida confirmação anterior")]
    public async Task Caso12()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a); a.Preferencias();a.Importar(a.Documento());await t.Selecionar(a.Importado);t.Campo<CheckBox>("ChkConfirmarSubstituicao").IsChecked=true;t.Campo<CheckBox>("ChkRestaurarPreferencias").IsChecked=true;Assert.NotEqual(true,t.Campo<CheckBox>("ChkConfirmarSubstituicao").IsChecked);Assert.False(t.Campo<Button>("BtnConfirmarBackup").IsEnabled);
    }
}
