using System.Text;
using System.Text.Json.Nodes;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Styling;
using Avalonia.VisualTree;
using BacklogManager.Aparencia;
using BacklogManager.Backup;
using BacklogManager.Controls;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Tests.Fase5;
using BacklogManager.Tests.Fase6;
using BacklogManager.Tests.Fase7;
using Xunit;
namespace BacklogManager.Tests.BetaInterface;

public sealed class TemasFixosBetaTests
{
    [Fact(DisplayName = "BT01 | Moderno Claro persistido | Reabrir recupera tema e modo em arquivo isolado")]
    public void Caso01()
    {
        using var p = new PastaTeste(); var caminho = Path.Combine(p.Pasta, "aparencia.json"); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(caminho)); s.Carregar(); s.Selecionar(TemaVisual.Moderno, ModoVisual.Claro); Assert.False(s.PendenteSalvar); var l = new ArmazenamentoAparenciaJson(caminho).Carregar(); Assert.Null(l.Aviso); Assert.Equal(TemaVisual.Moderno, l.Preferencias.Tema); Assert.Equal(ModoVisual.Claro, l.Preferencias.Modo);
    }

    [Fact(DisplayName = "BT02 | Moderno Claro contraste | Textos indicadores foco estados e mensagens legíveis")]
    public void Caso02()
    {
        ApoioBeta.Contraste(new() { Tema = TemaVisual.Moderno, Modo = ModoVisual.Claro });
    }

    [AvaloniaFact(DisplayName = "BT03 | Moderno Claro nas telas | Recursos ativos sem editor e capturas no tamanho mínimo")]
    public void Caso03()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item(status: "Jogando")); t.Janela.Width = 900; t.Janela.Height = 660; ApoioAparencia.Tema(t, TemaVisual.Moderno, ModoVisual.Claro); var r = ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias).Recursos; Assert.Equal(r["PageBackground"], ApoioAparencia.Cor(t.Janela.Background)); Assert.Equal(ThemeVariant.Light, t.Janela.ActualThemeVariant); foreach (var nav in new[] { "NavInicio", "NavBacklog", "NavColecoes", "NavCadastro", "NavConfiguracoes" }) { t.Clicar(nav); ApoioBeta.Render(t); ApoioBeta.SemEditor(t.Janela); ApoioBeta.SemPromocoes(t.Janela); ApoioBeta.Capturar(t, "Moderno-Claro-" + nav); }
    }

    [Fact(DisplayName = "BT04 | Moderno Escuro persistido | Reabrir recupera tema e modo em arquivo isolado")]
    public void Caso04()
    {
        using var p = new PastaTeste(); var caminho = Path.Combine(p.Pasta, "aparencia.json"); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(caminho)); s.Carregar(); s.Selecionar(TemaVisual.Moderno, ModoVisual.Escuro); Assert.False(s.PendenteSalvar); var l = new ArmazenamentoAparenciaJson(caminho).Carregar(); Assert.Null(l.Aviso); Assert.Equal(TemaVisual.Moderno, l.Preferencias.Tema); Assert.Equal(ModoVisual.Escuro, l.Preferencias.Modo);
    }

    [Fact(DisplayName = "BT05 | Moderno Escuro contraste | Textos indicadores foco estados e mensagens legíveis")]
    public void Caso05()
    {
        ApoioBeta.Contraste(new() { Tema = TemaVisual.Moderno, Modo = ModoVisual.Escuro });
    }

    [AvaloniaFact(DisplayName = "BT06 | Moderno Escuro nas telas | Recursos ativos sem editor e capturas no tamanho mínimo")]
    public void Caso06()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item(status: "Jogando")); t.Janela.Width = 900; t.Janela.Height = 660; ApoioAparencia.Tema(t, TemaVisual.Moderno, ModoVisual.Escuro); var r = ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias).Recursos; Assert.Equal(r["PageBackground"], ApoioAparencia.Cor(t.Janela.Background)); Assert.Equal(ThemeVariant.Dark, t.Janela.ActualThemeVariant); foreach (var nav in new[] { "NavInicio", "NavBacklog", "NavColecoes", "NavCadastro", "NavConfiguracoes" }) { t.Clicar(nav); ApoioBeta.Render(t); ApoioBeta.SemEditor(t.Janela); ApoioBeta.SemPromocoes(t.Janela); ApoioBeta.Capturar(t, "Moderno-Escuro-" + nav); }
    }

    [Fact(DisplayName = "BT07 | Tecnologico Claro persistido | Reabrir recupera tema e modo em arquivo isolado")]
    public void Caso07()
    {
        using var p = new PastaTeste(); var caminho = Path.Combine(p.Pasta, "aparencia.json"); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(caminho)); s.Carregar(); s.Selecionar(TemaVisual.Tecnologico, ModoVisual.Claro); Assert.False(s.PendenteSalvar); var l = new ArmazenamentoAparenciaJson(caminho).Carregar(); Assert.Null(l.Aviso); Assert.Equal(TemaVisual.Tecnologico, l.Preferencias.Tema); Assert.Equal(ModoVisual.Claro, l.Preferencias.Modo);
    }

    [Fact(DisplayName = "BT08 | Tecnologico Claro contraste | Textos indicadores foco estados e mensagens legíveis")]
    public void Caso08()
    {
        ApoioBeta.Contraste(new() { Tema = TemaVisual.Tecnologico, Modo = ModoVisual.Claro });
    }

    [AvaloniaFact(DisplayName = "BT09 | Tecnologico Claro nas telas | Recursos ativos sem editor e capturas no tamanho mínimo")]
    public void Caso09()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item(status: "Jogando")); t.Janela.Width = 900; t.Janela.Height = 660; ApoioAparencia.Tema(t, TemaVisual.Tecnologico, ModoVisual.Claro); var r = ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias).Recursos; Assert.Equal(r["PageBackground"], ApoioAparencia.Cor(t.Janela.Background)); Assert.Equal(ThemeVariant.Light, t.Janela.ActualThemeVariant); foreach (var nav in new[] { "NavInicio", "NavBacklog", "NavColecoes", "NavCadastro", "NavConfiguracoes" }) { t.Clicar(nav); ApoioBeta.Render(t); ApoioBeta.SemEditor(t.Janela); ApoioBeta.SemPromocoes(t.Janela); ApoioBeta.Capturar(t, "Tecnologico-Claro-" + nav); }
    }

    [Fact(DisplayName = "BT10 | Tecnologico Escuro persistido | Reabrir recupera tema e modo em arquivo isolado")]
    public void Caso10()
    {
        using var p = new PastaTeste(); var caminho = Path.Combine(p.Pasta, "aparencia.json"); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(caminho)); s.Carregar(); s.Selecionar(TemaVisual.Tecnologico, ModoVisual.Escuro); Assert.False(s.PendenteSalvar); var l = new ArmazenamentoAparenciaJson(caminho).Carregar(); Assert.Null(l.Aviso); Assert.Equal(TemaVisual.Tecnologico, l.Preferencias.Tema); Assert.Equal(ModoVisual.Escuro, l.Preferencias.Modo);
    }

    [Fact(DisplayName = "BT11 | Tecnologico Escuro contraste | Textos indicadores foco estados e mensagens legíveis")]
    public void Caso11()
    {
        ApoioBeta.Contraste(new() { Tema = TemaVisual.Tecnologico, Modo = ModoVisual.Escuro });
    }

    [AvaloniaFact(DisplayName = "BT12 | Tecnologico Escuro nas telas | Recursos ativos sem editor e capturas no tamanho mínimo")]
    public void Caso12()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item(status: "Jogando")); t.Janela.Width = 900; t.Janela.Height = 660; ApoioAparencia.Tema(t, TemaVisual.Tecnologico, ModoVisual.Escuro); var r = ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias).Recursos; Assert.Equal(r["PageBackground"], ApoioAparencia.Cor(t.Janela.Background)); Assert.Equal(ThemeVariant.Dark, t.Janela.ActualThemeVariant); foreach (var nav in new[] { "NavInicio", "NavBacklog", "NavColecoes", "NavCadastro", "NavConfiguracoes" }) { t.Clicar(nav); ApoioBeta.Render(t); ApoioBeta.SemEditor(t.Janela); ApoioBeta.SemPromocoes(t.Janela); ApoioBeta.Capturar(t, "Tecnologico-Escuro-" + nav); }
    }

    [Fact(DisplayName = "BT13 | Acolhedor Claro persistido | Reabrir recupera tema e modo em arquivo isolado")]
    public void Caso13()
    {
        using var p = new PastaTeste(); var caminho = Path.Combine(p.Pasta, "aparencia.json"); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(caminho)); s.Carregar(); s.Selecionar(TemaVisual.Acolhedor, ModoVisual.Claro); Assert.False(s.PendenteSalvar); var l = new ArmazenamentoAparenciaJson(caminho).Carregar(); Assert.Null(l.Aviso); Assert.Equal(TemaVisual.Acolhedor, l.Preferencias.Tema); Assert.Equal(ModoVisual.Claro, l.Preferencias.Modo);
    }

    [Fact(DisplayName = "BT14 | Acolhedor Claro contraste | Textos indicadores foco estados e mensagens legíveis")]
    public void Caso14()
    {
        ApoioBeta.Contraste(new() { Tema = TemaVisual.Acolhedor, Modo = ModoVisual.Claro });
    }

    [AvaloniaFact(DisplayName = "BT15 | Acolhedor Claro nas telas | Recursos ativos sem editor e capturas no tamanho mínimo")]
    public void Caso15()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item(status: "Jogando")); t.Janela.Width = 900; t.Janela.Height = 660; ApoioAparencia.Tema(t, TemaVisual.Acolhedor, ModoVisual.Claro); var r = ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias).Recursos; Assert.Equal(r["PageBackground"], ApoioAparencia.Cor(t.Janela.Background)); Assert.Equal(ThemeVariant.Light, t.Janela.ActualThemeVariant); foreach (var nav in new[] { "NavInicio", "NavBacklog", "NavColecoes", "NavCadastro", "NavConfiguracoes" }) { t.Clicar(nav); ApoioBeta.Render(t); ApoioBeta.SemEditor(t.Janela); ApoioBeta.SemPromocoes(t.Janela); ApoioBeta.Capturar(t, "Acolhedor-Claro-" + nav); }
    }

    [Fact(DisplayName = "BT16 | Acolhedor Escuro persistido | Reabrir recupera tema e modo em arquivo isolado")]
    public void Caso16()
    {
        using var p = new PastaTeste(); var caminho = Path.Combine(p.Pasta, "aparencia.json"); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(caminho)); s.Carregar(); s.Selecionar(TemaVisual.Acolhedor, ModoVisual.Escuro); Assert.False(s.PendenteSalvar); var l = new ArmazenamentoAparenciaJson(caminho).Carregar(); Assert.Null(l.Aviso); Assert.Equal(TemaVisual.Acolhedor, l.Preferencias.Tema); Assert.Equal(ModoVisual.Escuro, l.Preferencias.Modo);
    }

    [Fact(DisplayName = "BT17 | Acolhedor Escuro contraste | Textos indicadores foco estados e mensagens legíveis")]
    public void Caso17()
    {
        ApoioBeta.Contraste(new() { Tema = TemaVisual.Acolhedor, Modo = ModoVisual.Escuro });
    }

    [AvaloniaFact(DisplayName = "BT18 | Acolhedor Escuro nas telas | Recursos ativos sem editor e capturas no tamanho mínimo")]
    public void Caso18()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item(status: "Jogando")); t.Janela.Width = 900; t.Janela.Height = 660; ApoioAparencia.Tema(t, TemaVisual.Acolhedor, ModoVisual.Escuro); var r = ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias).Recursos; Assert.Equal(r["PageBackground"], ApoioAparencia.Cor(t.Janela.Background)); Assert.Equal(ThemeVariant.Dark, t.Janela.ActualThemeVariant); foreach (var nav in new[] { "NavInicio", "NavBacklog", "NavColecoes", "NavCadastro", "NavConfiguracoes" }) { t.Clicar(nav); ApoioBeta.Render(t); ApoioBeta.SemEditor(t.Janela); ApoioBeta.SemPromocoes(t.Janela); ApoioBeta.Capturar(t, "Acolhedor-Escuro-" + nav); }
    }

    [Fact(DisplayName = "BT19 | DarkMode Claro persistido | Reabrir recupera tema e modo em arquivo isolado")]
    public void Caso19()
    {
        using var p = new PastaTeste(); var caminho = Path.Combine(p.Pasta, "aparencia.json"); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(caminho)); s.Carregar(); s.Selecionar(TemaVisual.DarkMode, ModoVisual.Claro); Assert.False(s.PendenteSalvar); var l = new ArmazenamentoAparenciaJson(caminho).Carregar(); Assert.Null(l.Aviso); Assert.Equal(TemaVisual.DarkMode, l.Preferencias.Tema); Assert.Equal(ModoVisual.Claro, l.Preferencias.Modo);
    }

    [Fact(DisplayName = "BT20 | DarkMode Claro contraste | Textos indicadores foco estados e mensagens legíveis")]
    public void Caso20()
    {
        ApoioBeta.Contraste(new() { Tema = TemaVisual.DarkMode, Modo = ModoVisual.Claro });
    }

    [AvaloniaFact(DisplayName = "BT21 | DarkMode Claro nas telas | Recursos ativos sem editor e capturas no tamanho mínimo")]
    public void Caso21()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item(status: "Jogando")); t.Janela.Width = 900; t.Janela.Height = 660; ApoioAparencia.Tema(t, TemaVisual.DarkMode, ModoVisual.Claro); var r = ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias).Recursos; Assert.Equal(r["PageBackground"], ApoioAparencia.Cor(t.Janela.Background)); Assert.Equal(ThemeVariant.Light, t.Janela.ActualThemeVariant); foreach (var nav in new[] { "NavInicio", "NavBacklog", "NavColecoes", "NavCadastro", "NavConfiguracoes" }) { t.Clicar(nav); ApoioBeta.Render(t); ApoioBeta.SemEditor(t.Janela); ApoioBeta.SemPromocoes(t.Janela); ApoioBeta.Capturar(t, "DarkMode-Claro-" + nav); }
    }

    [Fact(DisplayName = "BT22 | DarkMode Escuro persistido | Reabrir recupera tema e modo em arquivo isolado")]
    public void Caso22()
    {
        using var p = new PastaTeste(); var caminho = Path.Combine(p.Pasta, "aparencia.json"); var s = new ServicoAparencia(new ArmazenamentoAparenciaJson(caminho)); s.Carregar(); s.Selecionar(TemaVisual.DarkMode, ModoVisual.Escuro); Assert.False(s.PendenteSalvar); var l = new ArmazenamentoAparenciaJson(caminho).Carregar(); Assert.Null(l.Aviso); Assert.Equal(TemaVisual.DarkMode, l.Preferencias.Tema); Assert.Equal(ModoVisual.Escuro, l.Preferencias.Modo);
    }

    [Fact(DisplayName = "BT23 | DarkMode Escuro contraste | Textos indicadores foco estados e mensagens legíveis")]
    public void Caso23()
    {
        ApoioBeta.Contraste(new() { Tema = TemaVisual.DarkMode, Modo = ModoVisual.Escuro });
    }

    [AvaloniaFact(DisplayName = "BT24 | DarkMode Escuro nas telas | Recursos ativos sem editor e capturas no tamanho mínimo")]
    public void Caso24()
    {
        using var t = ApoioBeta.Tela(ApoioDashboard.Item(status: "Jogando")); t.Janela.Width = 900; t.Janela.Height = 660; ApoioAparencia.Tema(t, TemaVisual.DarkMode, ModoVisual.Escuro); var r = ResolvedorAparencia.Resolver(t.Janela.AparenciaAtual.Preferencias).Recursos; Assert.Equal(r["PageBackground"], ApoioAparencia.Cor(t.Janela.Background)); Assert.Equal(ThemeVariant.Dark, t.Janela.ActualThemeVariant); foreach (var nav in new[] { "NavInicio", "NavBacklog", "NavColecoes", "NavCadastro", "NavConfiguracoes" }) { t.Clicar(nav); ApoioBeta.Render(t); ApoioBeta.SemEditor(t.Janela); ApoioBeta.SemPromocoes(t.Janela); ApoioBeta.Capturar(t, "DarkMode-Escuro-" + nav); }
    }

}
