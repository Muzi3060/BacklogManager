using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Avalonia.VisualTree;
using BacklogManager.Aparencia;
using BacklogManager.Models;
using BacklogManager.Tests.Fase5;
using BacklogManager.Tests.Fase6;
using Xunit;

namespace BacklogManager.Tests.BetaInterface;

internal static class ApoioBeta
{
    internal static TelaTeste Tela(params ItemBiblioteca[] itens) => ApoioDashboard.Tela(itens);
    internal static void Render(TelaTeste t) => ApoioAparencia.Render(t);
    internal static string[] Textos(Control controle) => controle.GetVisualDescendants().OfType<TextBlock>()
        .Where(t => t.IsEffectivelyVisible).Select(t => t.Text ?? "").ToArray();
    internal static void SemPromocoes(Control controle) => Assert.DoesNotContain(Textos(controle), texto =>
        texto.Contains("promoç", StringComparison.OrdinalIgnoreCase) || texto.Contains("oferta", StringComparison.OrdinalIgnoreCase)
        || texto.Contains("R$", StringComparison.Ordinal) || texto.Contains("lista de desejos", StringComparison.OrdinalIgnoreCase));
    internal static void SemEditor(MainWindow janela)
    {
        foreach (var nome in new[] { "NavAparencia", "PaginaAparencia", "EditoresCores", "BtnAparenciaConfiguracoes", "BtnRestaurarCores", "BtnRestaurarAparencia", "CmbVisualizacaoConfiguracoes" })
            Assert.Null(janela.FindControl<Control>(nome));
    }
    internal static void Contraste(PreferenciasAparencia p)
    {
        var r = ResolvedorAparencia.Resolver(p).Recursos;
        foreach (var superficie in new[] { "Page", "Sidebar", "Panel", "Card" })
        {
            ApoioAparencia.Par(r[superficie + "Foreground"], r[superficie + "Background"]);
            ApoioAparencia.Par(r[superficie + "Muted"], r[superficie + "Background"]);
            ApoioAparencia.Par(r[superficie + "Border"], r[superficie + "Background"], 3);
        }
        foreach (var estado in new[] { "ButtonBackground", "ButtonHover", "ButtonPressed" })
            ApoioAparencia.Par(r["ButtonFocus"], r[estado], 3);
        ApoioAparencia.Par(r["ButtonForeground"], r["ButtonBackground"]);
        ApoioAparencia.Par(r["ButtonHoverText"], r["ButtonHover"]);
        ApoioAparencia.Par(r["ButtonPressedText"], r["ButtonPressed"]);
        ApoioAparencia.Par(r["SelectionForeground"], r["SelectionBackground"]);
        foreach (var tipo in new[] { "Success", "Warning", "Error" })
            ApoioAparencia.Par(r[tipo + "Brush"], r[tipo + "Background"]);
    }
    internal static void Capturar(TelaTeste t, string nome)
    {
        Render(t);
        using var imagem = t.Janela.CaptureRenderedFrame();
        Assert.NotNull(imagem);
        var pasta = Path.Combine(Path.GetTempPath(), "minhas-realizacoes-beta1-resultados", "imagens");
        Directory.CreateDirectory(pasta);
        imagem.Save(Path.Combine(pasta, nome + ".png"), PngBitmapEncoderOptions.Default);
    }
}
