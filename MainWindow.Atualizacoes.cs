using System;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using BacklogManager.Configuracao;
using BacklogManager.Services;
namespace BacklogManager;

public partial class MainWindow
{
    private ServicoAtualizacoesGitHub? _atualizacoes;
    private HttpClient? _httpAtualizacoes;
    private readonly CancellationTokenSource _vidaAtualizacoes=new();
    private Uri? _paginaAtualizacao;
    internal Task ConsultaAtualizacaoPendente {get;private set;}=Task.CompletedTask;
    internal Task AberturaAtualizacaoPendente {get;private set;}=Task.CompletedTask;
    internal Func<Uri,Task>? AbrirPaginaAtualizacao {get;set;}
    private void InicializarAtualizacoes()
    {
        TxtVersaoAplicativo.Text="Versão "+VersaoAplicativo.Atual;
        if(ConfiguracaoDistribuicao.RepositorioReleases is {} repositorio)
        {
            _httpAtualizacoes=new(new SocketsHttpHandler{AllowAutoRedirect=false,PooledConnectionLifetime=TimeSpan.FromMinutes(5)}){Timeout=TimeSpan.FromSeconds(15)};
            try{ConfigurarAtualizacoes(new(_httpAtualizacoes,repositorio));}
            catch(ArgumentException){ConfigurarAtualizacoes(null);}
        }
        else ConfigurarAtualizacoes(null);
    }
    internal void ConfigurarAtualizacoes(ServicoAtualizacoesGitHub? servico)
    {
        _atualizacoes=servico;BtnVerificarAtualizacoes.IsEnabled=servico!=null;
        TxtEstadoAtualizacao.Text=servico==null?"O endereço oficial das releases ainda não está configurado nesta versão.":"A instalação das atualizações é manual. Consulte quando desejar.";
        _paginaAtualizacao=null;BtnAbrirAtualizacao.IsVisible=false;
    }
    private void VerificarAtualizacoes_Click(object? sender,RoutedEventArgs e)
    {if(BtnVerificarAtualizacoes.IsEnabled)ConsultaAtualizacaoPendente=ConsultarAtualizacoes();}
    private async Task ConsultarAtualizacoes()
    {
        if(_atualizacoes==null || !BtnVerificarAtualizacoes.IsEnabled)return;
        BtnVerificarAtualizacoes.IsEnabled=false;BtnAbrirAtualizacao.IsVisible=false;_paginaAtualizacao=null;
        TxtEstadoAtualizacao.Text="Consultando versões…";
        try
        {
            var rid=RuntimeInformation.OSArchitecture==Architecture.X64?(OperatingSystem.IsWindows()?"win-x64":OperatingSystem.IsLinux()?"linux-x64":"outro"):"outro";
            var resultado=await _atualizacoes.Consultar(VersaoAplicativo.Atual,rid,_vidaAtualizacoes.Token);
            if(_fechada)return;
            TxtEstadoAtualizacao.Text=resultado.Mensagem+(resultado.Versao==null?"":" Versão: "+resultado.Versao);
            _paginaAtualizacao=resultado.Pagina;BtnAbrirAtualizacao.IsVisible=_paginaAtualizacao!=null;
        }
        catch(OperationCanceledException){if(!_fechada)TxtEstadoAtualizacao.Text="Consulta cancelada.";}
        finally{if(!_fechada)BtnVerificarAtualizacoes.IsEnabled=true;}
    }
    private void AbrirAtualizacao_Click(object? sender,RoutedEventArgs e)=>AberturaAtualizacaoPendente=AbrirAtualizacao();
    private async Task AbrirAtualizacao()
    {
        if(_atualizacoes==null || !_atualizacoes.PaginaOficial(_paginaAtualizacao))return;
        try
        {
            if(AbrirPaginaAtualizacao!=null)await AbrirPaginaAtualizacao(_paginaAtualizacao!);
            else if(!await Launcher.LaunchUriAsync(_paginaAtualizacao!))throw new InvalidOperationException();
        }
        catch(Exception erro) when(erro is InvalidOperationException or System.ComponentModel.Win32Exception or NotSupportedException or System.IO.IOException)
        {if(!_fechada)TxtEstadoAtualizacao.Text="Não foi possível abrir o navegador. Nenhuma atualização foi instalada.";}
    }
}
