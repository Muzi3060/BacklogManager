using System.Text;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using BacklogManager.Backup;
using Xunit;
namespace BacklogManager.Tests.Fase7;

public sealed class ProtecoesAdicionaisTests
{
    [Fact(DisplayName="QR25 | Backup falha na verificação | Dados ativos e diário não são modificados")]
    public void VerificacaoObrigatoria()
    {
        using var a=new AmbienteBackup(); var p=a.Previa(); var antes=a.Snapshot();
        a.Servico.Etapa=s=>{if(s=="backup-gravado") File.WriteAllText(Directory.GetFiles(a.Servico.PastaSeguranca).Single(),"corrompido");};
        Assert.ThrowsAny<Exception>(()=>a.Servico.Restaurar(p,false,true)); a.Igual(antes);
        Assert.False(File.Exists(a.Caminhos.Diario)); Assert.Null(a.Servico.UltimoBackupSeguranca);
    }
    [Fact(DisplayName="QR26 | Pasta de segurança sem permissão | Restauração aborta antes de alterar arquivos")]
    public void PermissaoSeguranca()
    {
        using var a=new AmbienteBackup(); var p=a.Previa(); var antes=a.Snapshot(); Directory.CreateDirectory(a.Servico.PastaSeguranca);
        if(OperatingSystem.IsWindows()) throw new PlatformNotSupportedException("Cenário de permissões Unix.");
        var modo=File.GetUnixFileMode(a.Servico.PastaSeguranca);
        try {File.SetUnixFileMode(a.Servico.PastaSeguranca,UnixFileMode.UserRead|UnixFileMode.UserExecute);
            Assert.Throws<UnauthorizedAccessException>(()=>a.Servico.Restaurar(p,false,true)); a.Igual(antes);Assert.False(File.Exists(a.Caminhos.Diario));}
        finally {File.SetUnixFileMode(a.Servico.PastaSeguranca,modo);}
    }
    [Fact(DisplayName="QE21 | Destino sem permissão | Falha real de escrita preserva os dados ativos")]
    public void PermissaoExportacao()
    {
        using var a=new AmbienteBackup(); var pasta=Path.Combine(a.Raiz,"destino");Directory.CreateDirectory(pasta);var antes=a.Snapshot();
        if(OperatingSystem.IsWindows()) throw new PlatformNotSupportedException("Cenário de permissões Unix.");
        var modo=File.GetUnixFileMode(pasta);
        try {File.SetUnixFileMode(pasta,UnixFileMode.UserRead|UnixFileMode.UserExecute);
            Assert.Throws<UnauthorizedAccessException>(()=>a.Servico.Exportar(Path.Combine(pasta,"backup.json"),false));a.Igual(antes);}
        finally {File.SetUnixFileMode(pasta,modo);}
    }
    [Fact(DisplayName="QC25 | Falha na preparação | Nenhum arquivo ativo tocado e temporários limpos")]
    public void Preparacao()
    {
        using var a=new AmbienteBackup();var p=a.Previa();var antes=a.Snapshot();
        var data=File.GetLastWriteTimeUtc(a.Caminhos.Arquivos["Biblioteca"]);
        a.Servico.Etapa=s=>{if(s=="arquivos-preparados")throw new IOException("sem espaço simulado");};
        Assert.Throws<IOException>(()=>a.Servico.Restaurar(p,false,true)); a.Igual(antes);
        Assert.Equal(data,File.GetLastWriteTimeUtc(a.Caminhos.Arquivos["Biblioteca"])); Assert.Empty(Directory.GetFiles(a.Dados,"*.tmp"));
    }
    [Fact(DisplayName="QC26 | Processo encerra durante preparação | Conjunto ativo fica intacto sem diário")]
    public void InterrupcaoPreparacao()
    {
        using var a=new AmbienteBackup();a.Preferencias();var antes=a.Snapshot();ProcessoBackup.Interromper(a,"arquivos-preparados");
        Assert.False(File.Exists(a.Caminhos.Diario));Assert.True(a.Servico.Recuperar());a.Igual(antes);
    }
    [Fact(DisplayName="QC27 | Falha após marcador de confirmação | Rollback usa intenção durável e verifica estado antigo")]
    public void FalhaConfirmacao()
    {
        using var a=new AmbienteBackup();var p=a.Previa();var antes=a.Snapshot();
        a.Servico.Etapa=s=>{if(s=="confirmada")throw new IOException("falha");};
        Assert.Throws<IOException>(()=>a.Servico.Restaurar(p,false,true));a.Igual(antes);Assert.False(File.Exists(a.Caminhos.Diario));
    }
    [Fact(DisplayName="QV25 | Manifesto nulo | Validação cancela sem acesso a destinos do documento")]
    public void ManifestoNulo()
    {using var a=new AmbienteBackup();a.Invalido(AmbienteBackup.Basico() with {Componentes=null!});}
    [Fact(DisplayName="QV26 | Componente nulo | Validação recusa componente incompleto")]
    public void ComponenteNulo()
    {using var a=new AmbienteBackup();var d=AmbienteBackup.Basico();d.Componentes["Colecoes"]=null!;a.Invalido(d);}
    [Fact(DisplayName="QV27 | Vínculo com Guid vazio | Associação inválida não é aplicada")]
    public void VinculoVazio()
    {
        using var a=new AmbienteBackup();var d=AmbienteBackup.Basico();d.Componentes["Colecoes"]=AmbienteBackup.C(JsonSerializer.SerializeToUtf8Bytes(
            new{Versao=1,Colecoes=new[]{new BacklogManager.Models.ColecaoPersonalizada(Guid.NewGuid(),"A","",new[]{Guid.Empty})}}));a.Invalido(d);
    }
    [Fact(DisplayName="QP17 | Caminho reservado | Biblioteca não coincide com o diário de recuperação")]
    public void CaminhoReservado()
    {using var a=new AmbienteBackup();Assert.Throws<InvalidDataException>(()=>new CaminhosDadosLocais(a.Caminhos.Diario));}
    [Fact(DisplayName="QE22 | Preferências inválidas na exportação | Exportar somente os dados permanece possível")]
    public void ExportacaoSemPreferenciasInvalidas()
    {
        using var a=new AmbienteBackup();File.WriteAllText(a.Caminhos.Arquivos["Aparencia"],"{");
        Assert.Throws<InvalidDataException>(()=>a.Documento());Assert.Equal(2,a.Documento(false).Componentes.Count);
    }
    [AvaloniaFact(DisplayName="QN17 | Seletor pendente e navegação | Não abre dois seletores nem reabilita ações antes da conclusão")]
    public async Task SeletorConcorrente()
    {
        using var a=new AmbienteBackup();using var t=new TelaBackup(a);var escolha=new TaskCompletionSource<string?>();var chamadas=0;
        t.Janela.SelecionarArquivoBackup=_=>{chamadas++;return escolha.Task;};t.Clicar("BtnExportarBackup");var pendente=t.Janela.OperacaoBackupPendente;
        t.Clicar("NavInicio");t.Clicar("NavConfiguracoes");Assert.False(t.Campo<Button>("BtnExportarBackup").IsEnabled);
        t.Clicar("BtnSelecionarBackup");Assert.Equal(1,chamadas);escolha.SetResult(null);await pendente;
        Assert.True(t.Campo<Button>("BtnExportarBackup").IsEnabled);
    }
    [AvaloniaFact(DisplayName="QN18 | Prévia no tamanho mínimo | Confirmação alcançável por rolagem e teclado")]
    public async Task PreviaMinima()
    {
        using var a=new AmbienteBackup();using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);
        t.Janela.Width=900;t.Janela.Height=660;var p=t.Campo<ScrollViewer>("PaginaConfiguracoes");p.Offset=new Avalonia.Vector(0,10000);t.Render();
        Assert.True(t.Campo<CheckBox>("ChkConfirmarSubstituicao").Focus(Avalonia.Input.NavigationMethod.Tab));
        Assert.True(t.Campo<CheckBox>("ChkConfirmarSubstituicao").IsFocused);
        t.Campo<CheckBox>("ChkConfirmarSubstituicao").IsChecked=true;t.Render();Assert.True(t.Campo<Button>("BtnConfirmarBackup").IsEnabled);
        using var imagem=Avalonia.Headless.HeadlessWindowExtensions.CaptureRenderedFrame(t.Janela);Assert.NotNull(imagem);
        imagem.Save(Path.Combine(Path.GetTempPath(),"backlogmanager-fase7-resultados","imagens","previa-minima.png"),Avalonia.Media.Imaging.PngBitmapEncoderOptions.Default);
    }
}
