using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using BacklogManager.Backup;
using BacklogManager.Services;

namespace BacklogManager;

public partial class JanelaInicializacaoDados:Window
{
    private readonly ServicoMigracaoLocal _servico;
    private PreviaMigracao? _previa;
    private bool _ocupada;
    internal Action? Continuar {get;set;}
    internal Func<Task<string?>>? SelecionarPasta {get;set;}
    internal Task OperacaoPendente {get;private set;}=Task.CompletedTask;
    public JanelaInicializacaoDados():this(new CaminhosDadosLocais(System.IO.Path.Combine(System.IO.Path.GetTempPath(),"MR.Designer","backlog.json")),"") { }
    internal JanelaInicializacaoDados(CaminhosDadosLocais destino,string origem,string? seguranca=null)
    {
        InitializeComponent();_servico=new(destino,seguranca);
        TxtDestino.Text="Destino: "+destino.Pasta;TxtOrigem.Text=origem;
        TxtOrigem.TextChanged+=(_,_)=>{_previa=null;BtnMigrar.IsEnabled=false;ChkConfirmar.IsChecked=false;};
    }
    internal static Window Falha(string mensagem)=>new()
    {Title="Minhas Realizações — Acesso aos dados",Width=900,Height=660,MinWidth=900,MinHeight=660,
        Content=new TextBlock {Text=mensagem,TextWrapping=Avalonia.Media.TextWrapping.Wrap,Margin=new(24)}};
    private async void Escolher_Click(object? sender,RoutedEventArgs e)
    {
        if(_ocupada)return;
        try
        {
            var pasta=SelecionarPasta!=null? await SelecionarPasta(): (await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions {Title="Selecionar dados anteriores",AllowMultiple=false})) is {Count:>0} pastas?pastas[0].TryGetLocalPath():null;
            if(pasta!=null)TxtOrigem.Text=pasta;
        }
        catch(Exception erro) when(ServicoBackupLocal.Falha(erro)){TxtEstado.Text="Não foi possível selecionar a pasta: "+erro.Message;}
    }
    private void Prever_Click(object? sender,RoutedEventArgs e)
    {
        if(_ocupada)return;
        _previa=null;BtnMigrar.IsEnabled=false;ChkConfirmar.IsChecked=false;
        try
        {
            _previa=_servico.Prever(TxtOrigem.Text??"");
            TxtEstado.Text=$"Origem: {_previa.Origem}\nDestino: {_previa.Destino}\nObras: {_previa.Obras}; coleções: {_previa.Colecoes}; vínculos: {_previa.Vinculos}.\n{_previa.Aviso}";
            ChkPreferencias.IsChecked=_previa.PreferenciasValidas;BtnMigrar.IsEnabled=true;
        }
        catch(Exception erro) when(ServicoBackupLocal.Falha(erro)){TxtEstado.Text="Importação indisponível: "+erro.Message;}
    }
    private void Migrar_Click(object? sender,RoutedEventArgs e){if(!_ocupada)OperacaoPendente=Migrar();}
    private async Task Migrar()
    {
        if(_ocupada || _previa==null)return;
        if(ChkConfirmar.IsChecked!=true){TxtEstado.Text="Confirme explicitamente a cópia dos dados.";return;}
        _ocupada=true;BtnMigrar.IsEnabled=BtnVazia.IsEnabled=BtnEscolher.IsEnabled=BtnPrever.IsEnabled=TxtOrigem.IsEnabled=false;
        var previa=_previa;var preferencias=ChkPreferencias.IsChecked==true;
        try
        {
            var copia=await Task.Run(()=>_servico.Migrar(previa,preferencias,true));
            TxtEstado.Text="Importação verificada. Backup: "+copia;_ocupada=false;Continuar?.Invoke();
        }
        catch(Exception erro) when(ServicoBackupLocal.Falha(erro)){TxtEstado.Text="Importação não concluída. Preserve os backups. "+erro.Message;}
        finally{_ocupada=false;BtnVazia.IsEnabled=BtnEscolher.IsEnabled=BtnPrever.IsEnabled=TxtOrigem.IsEnabled=true;}
    }
    private void Vazia_Click(object? sender,RoutedEventArgs e){if(!_ocupada)Continuar?.Invoke();}
    protected override void OnClosing(WindowClosingEventArgs e){if(_ocupada)e.Cancel=true;base.OnClosing(e);}
}
