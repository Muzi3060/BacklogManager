using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace BacklogManager.Controls;

public sealed class CapaObra : UserControl
{
    internal const double RaioCantos = 8;
    public static readonly StyledProperty<string?> EnderecoProperty = AvaloniaProperty.Register<CapaObra, string?>(nameof(Endereco));
    public string? Endereco { get => GetValue(EnderecoProperty); set => SetValue(EnderecoProperty, value); }
    public static readonly StyledProperty<Stretch> PreenchimentoProperty = AvaloniaProperty.Register<CapaObra, Stretch>(nameof(Preenchimento), Stretch.Uniform);
    public Stretch Preenchimento { get => GetValue(PreenchimentoProperty); set => SetValue(PreenchimentoProperty, value); }
    private readonly Image _imagem = new() { Stretch = Stretch.Uniform, IsVisible = false };
    private readonly TextBlock _padrao = new()
    {
        Text = "Sem capa",
        Foreground = Brush.Parse("#BEBEC5"),
        FontSize = 12,
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center
    };
    private CancellationTokenSource? _cancelamento;
    internal Task CarregamentoPendente { get; private set; } = Task.CompletedTask;

    public CapaObra()
    {
        Content = new Border
        {
            Background = Brush.Parse("#39343F"),
            ClipToBounds = true,
            CornerRadius = new CornerRadius(RaioCantos),
            Child = new Grid { Children = { _padrao, _imagem } }
        };
        // O raio do fundo não recorta a imagem filha: o controle também precisa de clip arredondado.
        SizeChanged += (_, _) => Clip = new RectangleGeometry(new Rect(Bounds.Size), RaioCantos, RaioCantos);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == PreenchimentoProperty) _imagem.Stretch = Preenchimento;
        if (change.Property == EnderecoProperty && TopLevel.GetTopLevel(this) != null) Iniciar();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        Iniciar();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        _cancelamento?.Cancel();
        _imagem.Source = null;
        base.OnDetachedFromVisualTree(e);
    }

    private void Iniciar()
    {
        _cancelamento?.Cancel();
        _imagem.Source = null;
        _imagem.IsVisible = false;
        _padrao.IsVisible = true;
        var janela = this.FindAncestorOfType<MainWindow>();
        if (janela == null) return;
        var fonte = new CancellationTokenSource();
        _cancelamento = fonte;
        CarregamentoPendente = ExibirAsync(janela, Endereco, fonte);
    }

    private async Task ExibirAsync(MainWindow janela, string? endereco, CancellationTokenSource fonte)
    {
        try
        {
            var imagem = await janela.Capas.CarregarAsync(endereco, fonte.Token);
            if (fonte.IsCancellationRequested) return;
            _imagem.Source = imagem;
            _imagem.IsVisible = imagem != null;
            _padrao.IsVisible = imagem == null;
        }
        catch (OperationCanceledException) { }
        finally
        {
            if (ReferenceEquals(_cancelamento, fonte)) _cancelamento = null;
            fonte.Dispose();
        }
    }
}
