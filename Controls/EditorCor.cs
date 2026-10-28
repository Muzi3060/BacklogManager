using System;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using BacklogManager.Aparencia;

namespace BacklogManager.Controls;

public sealed class EditorCor : UserControl
{
    public ElementoCor Elemento { get; }
    internal TextBox Campo { get; } = new() { MinWidth = 130, MaxLength = 32 };
    internal Border Amostra { get; } = new() { Width = 38, Height = 38, CornerRadius = new CornerRadius(6), BorderThickness = new Thickness(1) };
    internal TextBlock Mensagem { get; } = new() { IsVisible = false, TextWrapping = TextWrapping.Wrap, FontSize = 12 };
    internal TextBlock Ajuste { get; } = new() { IsVisible = false, TextWrapping = TextWrapping.Wrap, FontSize = 12 };
    private bool _preenchendo;
    private string? _textoExibido;
    public event Action<ElementoCor, string>? CorAlterada;
    public EditorCor(ElementoCor elemento)
    {
        Elemento = elemento;
        Campo.Name = "Hex" + elemento; AutomationProperties.SetName(Campo, TemasPredefinidos.Nome(elemento) + " em hexadecimal");
        Amostra.Name = "Amostra" + elemento;
        var campos = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10, Children = { Amostra, Campo } };
        var paleta = new WrapPanel();
        foreach (var cor in CoresAparencia.Paleta)
        {
            var amostra = new Border { Background = Brush.Parse(cor), Width = 17, Height = 17, CornerRadius = new CornerRadius(3), BorderThickness = new Thickness(1), BorderBrush = Brush.Parse(CoresAparencia.BordaSegura(cor)) };
            var botao = new Button { Content = amostra, Padding = new Thickness(5), Margin = new Thickness(0, 0, 3, 3), MinHeight = 0 };
            ToolTip.SetTip(botao, cor); AutomationProperties.SetName(botao, "Usar " + cor + " para " + TemasPredefinidos.Nome(elemento));
            botao.Click += (_, _) => Campo.Text = cor;
            paleta.Children.Add(botao);
        }
        var painel = new StackPanel
        {
            Spacing = 8,
            Children =
        { new TextBlock { Text = TemasPredefinidos.Nome(elemento), FontWeight = FontWeight.SemiBold }, campos, paleta, Mensagem, Ajuste }
        };
        Content = new Border { Classes = { "card" }, Child = painel, Padding = new Thickness(14) };
        Width = 278; Margin = new Thickness(0, 0, 12, 12);
        Mensagem.Bind(TextBlock.ForegroundProperty, new DynamicResourceExtension("ErrorOnPanel"));
        Ajuste.Bind(TextBlock.ForegroundProperty, new DynamicResourceExtension("WarningOnPanel"));
        Campo.PropertyChanged += (_, mudanca) =>
        {
            if (mudanca.Property != TextBox.TextProperty) return;
            if (_preenchendo) return;
            if (Campo.Text == _textoExibido) { Mensagem.IsVisible = false; Mensagem.Text = string.Empty; return; }
            if (!CoresAparencia.TentarNormalizar(Campo.Text, out var cor))
            { Mensagem.IsVisible = true; Mensagem.Text = "Use #RRGGBB, como #3366CC. A última cor válida continua aplicada."; return; }
            Mensagem.IsVisible = false; Mensagem.Text = string.Empty;
            Preencher(cor); CorAlterada?.Invoke(Elemento, cor);
        };
    }
    public void Preencher(string cor)
    {
        _preenchendo = true;
        _textoExibido = cor;
        try { Campo.Text = cor; Amostra.Background = Brush.Parse(cor); Amostra.BorderBrush = Brush.Parse(CoresAparencia.BordaSegura(cor)); Mensagem.IsVisible = false; Mensagem.Text = string.Empty; }
        finally { _preenchendo = false; }
    }
    public void MostrarAjuste(string? aviso) { Ajuste.Text = aviso ?? string.Empty; Ajuste.IsVisible = !string.IsNullOrEmpty(aviso); }
}
