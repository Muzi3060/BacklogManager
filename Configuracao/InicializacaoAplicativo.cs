using System;
using System.IO;
using Avalonia.Controls.ApplicationLifetimes;
using BacklogManager.Backup;

namespace BacklogManager.Configuracao;

internal static class InicializacaoAplicativo
{
    internal static void Abrir(IClassicDesktopStyleApplicationLifetime desktop)
    {
        try
        {
            var caminhos=ResolvedorDiretorios.Preparar(ResolvedorDiretorios.Atual(desktop.Args ?? []));
            var backup=new ServicoBackupLocal(caminhos);
            if(!backup.Recuperar()) throw new IOException(backup.Pendencia);
            var args=desktop.Args ?? [];
            var indice=Array.IndexOf(args,"--migrate-from");
            if(indice>=0 && (indice+1>=args.Length || !Path.IsPathFullyQualified(args[indice+1]))) throw new ArgumentException("--migrate-from exige uma pasta absoluta.");
            var legado=indice>=0?args[indice+1]:Services.ServicoMigracaoLocal.IdentificarLegado(caminhos,Environment.CurrentDirectory,AppContext.BaseDirectory);
            if(!ResolvedorDiretorios.TemDados(caminhos) && legado!=null)
            {
                var janela=new JanelaInicializacaoDados(caminhos,legado);
                janela.Continuar=()=>AbrirPrincipal(desktop,caminhos,janela);
                desktop.MainWindow=janela;
            }
            else AbrirPrincipal(desktop,caminhos,null);
        }
        catch(Exception e) when(ServicoBackupLocal.Falha(e))
        { desktop.MainWindow=JanelaInicializacaoDados.Falha("Não foi possível abrir a pasta de dados. Nenhum perfil alternativo foi aberto.\n"+e.Message); }
    }
    private static void AbrirPrincipal(IClassicDesktopStyleApplicationLifetime desktop,CaminhosDadosLocais caminhos,Avalonia.Controls.Window? anterior)
    {
        if(!File.Exists(caminhos.Arquivos["Biblioteca"]))
        { var arquivo=new Data.FileManager(caminhos.Arquivos["Biblioteca"]);var itens=arquivo.Carregar();arquivo.Salvar(itens); }
        var janela=new MainWindow(ComposicaoAplicativo.Criar(caminhos.Arquivos["Biblioteca"]));
        desktop.MainWindow=janela;
        if(anterior!=null){janela.Show();anterior.Close();}
    }
}
