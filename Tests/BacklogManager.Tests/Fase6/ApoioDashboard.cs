using BacklogManager.Models;
using BacklogManager.Services;
using BacklogManager.Data;
using Avalonia.Controls;
using Avalonia.Threading;
namespace BacklogManager.Tests.Fase6;

internal static class ApoioDashboard
{
    internal static readonly DateTimeOffset Agora = new(2026,10,29,15,30,0,TimeSpan.Zero);
    internal static ServicoDashboard Servico => new(()=>Agora,TimeZoneInfo.Utc);
    internal static ItemBiblioteca Item(int dias=0, string status="Na Fila",TipoMidia tipo=TipoMidia.Jogo)
    {
        var i=Fase2.ApoioMultimidia.Item(tipo);
        return i with {DataAdicao=Agora.AddDays(-dias),Pessoal=i.Pessoal with {Status=status}};
    }
    internal static TelaTeste Tela(params ItemBiblioteca[] itens) => new(itens.Select(ConversorBibliotecaJson.ParaRegistro),dashboard:Servico);
    internal static void Render(TelaTeste t){Dispatcher.UIThread.RunJobs();t.Janela.UpdateLayout();}
}
