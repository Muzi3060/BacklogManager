using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace BacklogManager;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        BacklogManager.Aparencia.AplicadorAparencia.Aplicar(new());
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            Configuracao.InicializacaoAplicativo.Abrir(desktop);
        }

        base.OnFrameworkInitializationCompleted();
    }
}