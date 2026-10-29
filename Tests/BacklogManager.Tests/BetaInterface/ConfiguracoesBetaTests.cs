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

public sealed class ConfiguracoesBetaTests
{
    [AvaloniaFact(DisplayName = "BC01 | Temas diretos | Quatro opções aprovadas nas configurações")]
    public void Caso01()
    {
        using var t = new TelaTeste(); t.Clicar("NavConfiguracoes"); Assert.Equal(new[] { "Moderno", "Tecnológico", "Acolhedor", "Dark Mode" }, t.Campo<ComboBox>("CmbTema").Items.Cast<ComboBoxItem>().Select(x => x.Content));
    }

    [AvaloniaFact(DisplayName = "BC02 | Modos diretos | Claro e Escuro preservados")]
    public void Caso02()
    {
        using var t = new TelaTeste(); t.Clicar("NavConfiguracoes"); Assert.Equal(new[] { "Claro", "Escuro" }, t.Campo<ComboBox>("CmbModo").Items.Cast<ComboBoxItem>().Select(x => x.Content));
    }

    [AvaloniaFact(DisplayName = "BC03 | Padrão sem preferência | Moderno claro sem gravação automática")]
    public void Caso03()
    {
        var a = new AparenciaSimulada(); using var t = new TelaTeste(aparencia: a.Criar()); t.Clicar("NavConfiguracoes"); Assert.Equal(0, t.Campo<ComboBox>("CmbTema").SelectedIndex); Assert.Equal(0, t.Campo<ComboBox>("CmbModo").SelectedIndex); Assert.Equal(0, a.Gravacoes);
    }

    [AvaloniaFact(DisplayName = "BC04 | Aplicação imediata | Mudar tema atualiza a mesma janela")]
    public void Caso04()
    {
        using var t = new TelaTeste(); var janela = t.Janela; ApoioAparencia.Tema(t, TemaVisual.DarkMode, ModoVisual.Escuro); Assert.Same(janela, t.Janela); Assert.Equal("#121212", ApoioAparencia.Cor(t.Janela.Background));
    }

    [AvaloniaFact(DisplayName = "BC05 | Editor removido | Nenhum campo de cores ou reset acessível")]
    public void Caso05()
    {
        using var t = new TelaTeste(); t.Clicar("NavConfiguracoes"); ApoioBeta.SemEditor(t.Janela); Assert.DoesNotContain(ApoioBeta.Textos(t.Janela), s => s.Contains("#RRGGBB") || s.Contains("Restaurar cores") || s.Contains("Personalizar aparência"));
    }

    [AvaloniaFact(DisplayName = "BC06 | Grade e lista na biblioteca | Preferência continua disponível no local próprio")]
    public void Caso06()
    {
        using var a = new AmbienteBackup(); using var t = new TelaBackup(a); Assert.Null(t.Janela.FindControl<ComboBox>("CmbVisualizacaoConfiguracoes")); t.Clicar("NavBacklog"); t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex = 1; Assert.Equal(VisualizacaoBiblioteca.Lista, new ArmazenamentoPreferenciasBiblioteca(a.Caminhos.Arquivos["Visualizacao"]).Carregar());
    }

    [AvaloniaFact(DisplayName = "BC07 | Configurações na janela mínima | Rolagem alcança backup e rodapé sem corte lateral")]
    public void Caso07()
    {
        using var a = new AmbienteBackup(); using var t = new TelaBackup(a); t.Janela.Width = 900; t.Janela.Height = 660; t.Render(); var s = t.Campo<ScrollViewer>("PaginaConfiguracoes"); Assert.True(s.Extent.Height > s.Viewport.Height); Assert.InRange(s.Extent.Width, 0, s.Viewport.Width + 1); s.Offset = new Vector(0, 10000); t.Render(); Assert.True(s.Offset.Y > 0);
    }

    [AvaloniaFact(DisplayName = "BC08 | Acessibilidade dos seletores | Tema e modo possuem nomes e foco de teclado")]
    public void Caso08()
    {
        using var t = new TelaTeste(); t.Clicar("NavConfiguracoes"); ApoioBeta.Render(t); foreach (var n in new[] { "CmbTema", "CmbModo" }) { var c = t.Campo<ComboBox>(n); Assert.False(string.IsNullOrWhiteSpace(Avalonia.Automation.AutomationProperties.GetName(c))); Assert.True(c.Focus(NavigationMethod.Tab)); }
    }

    [AvaloniaFact(DisplayName = "BC09 | Exportação em Dark Mode | Backup inclui dados e preferência versão dois")]
    public async Task Caso09()
    {
        using var a = new AmbienteBackup(); using var t = new TelaBackup(a); t.Campo<ComboBox>("CmbTema").SelectedIndex = 3; t.Campo<ComboBox>("CmbModo").SelectedIndex = 1; await t.Exportar(a.Saida); var d = ValidadorBackup.Ler(File.ReadAllBytes(a.Saida)); Assert.Contains("Biblioteca", d.Componentes.Keys); Assert.Contains("Colecoes", d.Componentes.Keys); Assert.Contains("DarkMode", Encoding.UTF8.GetString(d.Componentes["Aparencia"].Conteudo!));
    }

    [AvaloniaFact(DisplayName = "BC10 | Restauração só dos dados | Tema da sessão preservado e dados substituídos")]
    public async Task Caso10()
    {
        using var a = new AmbienteBackup(); using var t = new TelaBackup(a); t.Campo<ComboBox>("CmbTema").SelectedIndex = 3; a.Importar(); await t.Selecionar(a.Importado); t.Confirmar(); Assert.Equal(TemaVisual.DarkMode, t.Janela.AparenciaAtual.Preferencias.Tema); Assert.Contains(t.Biblioteca.Itens, i => i.Catalogo.Titulo != "Anterior"); Assert.Null(a.Servico.Pendencia);
    }

    [AvaloniaFact(DisplayName = "BC11 | Restauração com preferências | Preferência escolhida no backup aparece nos seletores")]
    public async Task Caso11()
    {
        using var a = new AmbienteBackup(); a.Preferencias(TemaVisual.DarkMode, ModoVisual.Escuro); var d = a.Documento(); using var t = new TelaBackup(a); t.Campo<ComboBox>("CmbTema").SelectedIndex = 0; a.Importar(d); await t.Selecionar(a.Importado); t.Confirmar(true); Assert.Equal(3, t.Campo<ComboBox>("CmbTema").SelectedIndex); Assert.Equal(1, t.Campo<ComboBox>("CmbModo").SelectedIndex);
    }

    [AvaloniaFact(DisplayName = "BC12 | Restauração exige consentimento | Prévia não altera dados nem habilita escrita sem confirmação")]
    public async Task Caso12()
    {
        using var a = new AmbienteBackup(); using var t = new TelaBackup(a); var antes = a.Snapshot(); a.Importar(); await t.Selecionar(a.Importado); Assert.False(t.Campo<Button>("BtnConfirmarBackup").IsEnabled); a.Igual(antes);
    }

    [AvaloniaFact(DisplayName = "BC13 | Recuperação pendente | Seletores bloqueados e original de aparência preservado")]
    public void Caso13()
    {
        using var a = new AmbienteBackup(); a.Preferencias(); var bytes = File.ReadAllBytes(a.Caminhos.Arquivos["Aparencia"]); File.WriteAllText(a.Caminhos.Diario, "{}"); a.Servico.Recuperar(); using var t = new TelaBackup(a, recuperar: false); Assert.False(t.Campo<Border>("PreferenciasVisuais").IsEnabled); t.Campo<ComboBox>("CmbTema").SelectedIndex = 3; Assert.Equal(bytes, File.ReadAllBytes(a.Caminhos.Arquivos["Aparencia"]));
    }

    [AvaloniaFact(DisplayName = "BC14 | Falha e nova tentativa de salvar | Aviso honesto e escolha atual persistida ao tentar novamente")]
    public void Caso14()
    {
        var a = new AparenciaSimulada { FalhaSalvar = new IOException() }; using var t = new TelaTeste(aparencia: a.Criar()); ApoioAparencia.Tema(t, TemaVisual.DarkMode, ModoVisual.Escuro); Assert.True(t.Campo<Button>("BtnRepetirSalvarAparencia").IsVisible); Assert.True(t.Janela.AparenciaAtual.PendenteSalvar); a.FalhaSalvar = null; t.Clicar("BtnRepetirSalvarAparencia"); Assert.False(t.Janela.AparenciaAtual.PendenteSalvar); Assert.Equal(TemaVisual.DarkMode, a.Preferencias.Tema);
    }

    [AvaloniaFact(DisplayName = "BC15 | Restaurar preferência com seleção pendente | Recarga elimina aviso de gravação antiga e usa tema restaurado")]
    public async Task Caso15()
    {
        using var a = new AmbienteBackup();
        a.Preferencias(TemaVisual.Acolhedor, ModoVisual.Escuro);
        var documento = a.Documento();
        using var t = new TelaBackup(a);
        using (var trava = new FileStream(a.Caminhos.Arquivos["Aparencia"] + ".lock", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
        {
            t.Campo<ComboBox>("CmbTema").SelectedIndex = 3;
            Assert.True(t.Janela.AparenciaAtual.PendenteSalvar);
        }
        a.Importar(documento);
        await t.Selecionar(a.Importado);
        t.Confirmar(true);
        Assert.Equal(TemaVisual.Acolhedor, t.Janela.AparenciaAtual.Preferencias.Tema);
        Assert.False(t.Janela.AparenciaAtual.PendenteSalvar);
        Assert.False(t.Campo<Button>("BtnRepetirSalvarAparencia").IsVisible);
    }
}
