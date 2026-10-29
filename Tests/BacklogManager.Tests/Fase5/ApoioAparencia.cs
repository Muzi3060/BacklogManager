using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using BacklogManager.Aparencia;
using BacklogManager.Controls;
namespace BacklogManager.Tests.Fase5;

internal sealed class AparenciaSimulada : IArmazenamentoAparencia
{
    internal PreferenciasAparencia Preferencias = new();
    internal Exception? FalhaLer;
    internal Exception? FalhaSalvar;
    internal int Gravacoes;
    public LeituraAparencia Carregar() => FalhaLer == null ? new(Preferencias) : throw FalhaLer;
    public void Salvar(PreferenciasAparencia p) { if (FalhaSalvar != null) throw FalhaSalvar; Preferencias = p; Gravacoes++; }
    internal ServicoAparencia Criar() { var s = new ServicoAparencia(this); s.Carregar(); return s; }
}
internal static class ApoioAparencia
{
    internal static string Cor(object? brush) { var c = ((ISolidColorBrush)brush!).Color; return $"#{c.R:X2}{c.G:X2}{c.B:X2}"; }
    internal static void Render(TelaTeste t) { Dispatcher.UIThread.RunJobs(); t.Janela.UpdateLayout(); }
    internal static void Tema(TelaTeste t, TemaVisual tema, ModoVisual modo)
    { t.Clicar("NavConfiguracoes"); t.Campo<ComboBox>("CmbTema").SelectedIndex = (int)tema; t.Campo<ComboBox>("CmbModo").SelectedIndex = (int)modo; Render(t); }
    internal static void Capturar(TelaTeste t, string nome)
    { Render(t); using var frame = t.Janela.CaptureRenderedFrame(); Xunit.Assert.NotNull(frame); var p = Path.Combine(Path.GetTempPath(), "backlogmanager-fase5-resultados", "imagens"); Directory.CreateDirectory(p); frame.Save(Path.Combine(p, nome + ".png"), PngBitmapEncoderOptions.Default); }
    internal static void Par(string frente, string fundo, double minimo=4.5) => Xunit.Assert.True(CoresAparencia.Contraste(frente,fundo) >= minimo, $"{frente} sobre {fundo}: {CoresAparencia.Contraste(frente,fundo):F3} < {minimo}");
}
