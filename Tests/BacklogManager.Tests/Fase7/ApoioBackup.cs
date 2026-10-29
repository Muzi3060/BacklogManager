using System.Text;
using System.Text.Json;
using BacklogManager.Backup;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Services;
using BacklogManager.Aparencia;
using Xunit;
using Avalonia.Controls;

namespace BacklogManager.Tests.Fase7;

internal sealed class AmbienteBackup : IDisposable
{
    internal string Raiz { get; } = Path.Combine(Path.GetTempPath(), "MinhasRealizacoes.Fase7", Guid.NewGuid().ToString("N"));
    internal string Dados => Path.Combine(Raiz, "dados");
    internal CaminhosDadosLocais Caminhos { get; }
    internal ServicoBackupLocal Servico { get; }
    internal string Saida => Path.Combine(Raiz, "exportado.json");
    internal string Importado => Path.Combine(Raiz, "importado.json");
    internal AmbienteBackup()
    {
        Directory.CreateDirectory(Dados); Caminhos = new(Path.Combine(Dados,"backlog.json"));
        Servico = new(Caminhos, Path.Combine(Raiz, "seguranca"));
        Biblioteca(PastaTeste.Jogo("Anterior")); Colecoes();
    }
    internal void Biblioteca(params BacklogItem[] itens) => File.WriteAllBytes(Caminhos.Arquivos["Biblioteca"], JsonSerializer.SerializeToUtf8Bytes(itens));
    internal void Colecoes(params ColecaoPersonalizada[] colecoes) => File.WriteAllText(Caminhos.Arquivos["Colecoes"], JsonSerializer.Serialize(new { Versao=1, Colecoes=colecoes }));
    internal void Preferencias(TemaVisual tema=TemaVisual.Acolhedor, ModoVisual modo=ModoVisual.Escuro)
    {
        new ArmazenamentoPreferenciasBiblioteca(Caminhos.Arquivos["Visualizacao"]).Salvar(VisualizacaoBiblioteca.Lista);
        var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(Caminhos.Arquivos["Aparencia"])); s.Carregar();
        s.Selecionar(tema, modo);
    }
    internal Dictionary<string, byte[]?> Snapshot() => Caminhos.Arquivos.ToDictionary(p=>p.Key,p=>ArquivosBackup.Ler(p.Value));
    internal void Igual(Dictionary<string, byte[]?> antes)
    { foreach (var (nome,bytes) in antes) Assert.Equal(bytes, ArquivosBackup.Ler(Caminhos.Arquivos[nome])); }
    internal DocumentoBackup Documento(bool preferencias=true)
    {
        Servico.Exportar(Saida,preferencias); return ValidadorBackup.Ler(File.ReadAllBytes(Saida));
    }
    internal static ComponenteBackup C(byte[]? bytes) => new() {Conteudo=bytes,Sha256=ArquivosBackup.Hash(bytes)};
    internal static DocumentoBackup Basico() => new()
    {
        Formato="MinhasRealizacoes.BackupLocal", Versao=1, CriadoEm=new(2026,10,29,12,0,0,TimeSpan.Zero),
        Componentes=new() { ["Biblioteca"]=C(JsonSerializer.SerializeToUtf8Bytes(new[] {PastaTeste.Jogo("Restaurado")})),
          ["Colecoes"]=C(Encoding.UTF8.GetBytes("{\"Versao\":1,\"Colecoes\":[]}")) }
    };
    internal void Importar(DocumentoBackup? d=null) => File.WriteAllBytes(Importado,JsonSerializer.SerializeToUtf8Bytes(d??Basico(),ArquivosBackup.Opcoes));
    internal PreviaRestauracao Previa(DocumentoBackup? d=null) { Importar(d); return Servico.Prever(Importado); }
    internal void Invalido(DocumentoBackup d)
    { Importar(d); var antes=Snapshot(); Assert.ThrowsAny<Exception>(()=>Servico.Prever(Importado)); Igual(antes); Assert.False(File.Exists(Caminhos.Diario)); }
    internal IReadOnlyList<ItemBiblioteca> LerBiblioteca()
    { var b = new ServicoBiblioteca(new ArmazenamentoJsonBiblioteca(new FileManager(Caminhos.Arquivos["Biblioteca"]))); b.Carregar(); return b.Itens; }
    internal void Restaurado(PreviaRestauracao p, bool prefs=false)
    { var antes=Snapshot(); var seguranca=Servico.Restaurar(p,prefs,true); var d=ValidadorBackup.Ler(File.ReadAllBytes(seguranca));
      foreach (var (k,v) in antes) Assert.Equal(v,d.Componentes[k].Conteudo); Assert.False(File.Exists(Caminhos.Diario)); Assert.Null(Servico.Pendencia); }
    public void Dispose() { if(Directory.Exists(Raiz)) Directory.Delete(Raiz,true); }
}

internal sealed class TelaBackup : IDisposable
{
    internal MainWindow Janela { get; }
    internal ServicoBiblioteca Biblioteca { get; }
    internal ServicoColecoes Colecoes { get; }
    internal HttpSimulado Http { get; } = new();
    private readonly HttpClient _cliente;
    internal TelaBackup(AmbienteBackup a, bool recuperar=true)
    {
        if (recuperar) a.Servico.Recuperar();
        var f = new FileManager(a.Caminhos.Arquivos["Biblioteca"]);
        var c = new ArmazenamentoColecoesJson(a.Caminhos.Arquivos["Colecoes"]);
        Biblioteca = new(new ArmazenamentoJsonBiblioteca(f)); Colecoes = new(c,Biblioteca);
        a.Servico.ConferirEstadoCarregado = () => {f.ConferirEstadoAtual();c.ConferirEstadoAtual();};
        _cliente = new(Http);
        Janela = new(Biblioteca,new PesquisaJogosIgdb(new IgdbService(_cliente,"simulado","simulado")),
            preferencias:new(a.Caminhos.Arquivos["Visualizacao"]),colecoes:Colecoes,
            aparencia:new(new ArmazenamentoAparenciaJson(a.Caminhos.Arquivos["Aparencia"])),backup:a.Servico,
            capas:new CarregadorCapas(new HttpClient(new HttpSimulado {Responder=(_,_)=>Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.NotFound))})));
        Janela.Show(); Clicar("NavConfiguracoes");
    }
    internal T Campo<T>(string n) where T:Avalonia.Controls.Control => Janela.FindControl<T>(n)!;
    internal void Clicar(string n) => Campo<Avalonia.Controls.Button>(n).RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
    internal async Task Selecionar(string? arquivo)
    { Janela.SelecionarArquivoBackup=_=>Task.FromResult(arquivo); Clicar("BtnSelecionarBackup"); await Janela.OperacaoBackupPendente; }
    internal async Task Exportar(string? arquivo)
    { Janela.SelecionarArquivoBackup=_=>Task.FromResult(arquivo); Clicar("BtnExportarBackup"); await Janela.OperacaoBackupPendente; }
    internal void Confirmar(bool preferencias=false)
    {Campo<Avalonia.Controls.CheckBox>("ChkRestaurarPreferencias").IsChecked=preferencias;Campo<Avalonia.Controls.CheckBox>("ChkConfirmarSubstituicao").IsChecked=true;Clicar("BtnConfirmarBackup");}
    internal string Estado => Campo<Avalonia.Controls.TextBlock>("TxtEstadoBackup").Text??"";
    internal void Render() {Avalonia.Threading.Dispatcher.UIThread.RunJobs();Janela.UpdateLayout();}
    public void Dispose() {Janela.Close();_cliente.Dispose();}
}

internal static class ProcessoBackup
{
    internal static void Interromper(AmbienteBackup a, string etapa)
    {
        a.Preferencias(); var d=AmbienteBackup.Basico();
        d.Componentes["Visualizacao"]=AmbienteBackup.C(Encoding.UTF8.GetBytes("{\"Versao\":1,\"Visualizacao\":0}"));
        d.Componentes["Aparencia"]=AmbienteBackup.C(Encoding.UTF8.GetBytes("{\"Versao\":1,\"Tema\":\"Moderno\",\"Modo\":\"Claro\"}"));
        a.Importar(d);
        var inicio=new System.Diagnostics.ProcessStartInfo("dotnet") {UseShellExecute=false,RedirectStandardOutput=true,RedirectStandardError=true};
        inicio.ArgumentList.Add(Path.Combine(AppContext.BaseDirectory,"BackupWorker","BacklogManager.BackupWorker.dll"));
        foreach(var arg in new[]{a.Caminhos.Arquivos["Biblioteca"],a.Servico.PastaSeguranca,a.Importado,etapa})inicio.ArgumentList.Add(arg);
        using var processo=System.Diagnostics.Process.Start(inicio)!;
        if(!processo.WaitForExit(30000)){processo.Kill(true);throw new TimeoutException("Worker de recuperação excedeu o tempo.");}
        Assert.True(processo.ExitCode==73,processo.StandardError.ReadToEnd());
    }
}
