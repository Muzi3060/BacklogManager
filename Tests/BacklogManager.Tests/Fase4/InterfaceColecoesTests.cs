using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Media.Imaging;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Tests.Fase2;
using Xunit;
namespace BacklogManager.Tests.Fase4;

public sealed class InterfaceColecoesTests
{
    [AvaloniaFact(DisplayName = "UC01 | Abrir página de coleções vazia | Estado vazio e ações dependentes desabilitadas")]
    public void Vazia() { using var a = new TelaColecoes(); Assert.True(a.T.Campo<ScrollViewer>("PaginaColecoes").IsVisible); Assert.Empty(a.Obras); Assert.True(a.T.Campo<Button>("BtnCriarColecao").IsEnabled); Assert.False(a.T.Campo<Button>("BtnAdicionarObraColecao").IsEnabled); Assert.False(a.T.Campo<Button>("BtnExcluirColecao").IsEnabled); }
    [AvaloniaFact(DisplayName = "UC02 | Criar coleção pela janela | Seleção, nome normalizado e descrição disponíveis")]
    public void Criar() { using var a = new TelaColecoes(); a.Criar(" Favoritos ", " Descrição "); Assert.Equal("Favoritos", Assert.Single(a.Servico.Colecoes).Nome); Assert.Equal("Favoritos", a.T.Campo<TextBox>("TxtNomeColecao").Text); Assert.Equal("Descrição", a.T.Campo<TextBox>("TxtDescricaoColecao").Text); Assert.Equal(0, a.T.Campo<ComboBox>("CmbColecoes").SelectedIndex); }
    [AvaloniaFact(DisplayName = "UC03 | Nome vazio na janela | Aviso sem criar arquivo de coleções")]
    public void NomeVazio() { using var a = new TelaColecoes(); a.Criar(""); Assert.Empty(a.Servico.Colecoes); Assert.False(File.Exists(a.Caminho)); Assert.Equal("Coleção não salva", a.T.Campo<TextBlock>("TxtAvisoTitulo").Text); }
    [AvaloniaFact(DisplayName = "UC04 | Nome duplicado na janela | Segunda criação não altera coleção existente")]
    public void Duplicado() { using var a = new TelaColecoes(); a.Criar(); a.Criar("FAVORITOS"); Assert.Single(a.Servico.Colecoes); Assert.Contains("Já existe", a.T.Aviso); }
    [AvaloniaFact(DisplayName = "UC05 | Salvar nome e descrição pela janela | Alteração preserva associação")]
    public void Editar() { using var a = new TelaColecoes(); a.Criar(); a.Associar(); var id = a.Servico.Colecoes[0].Id; a.T.Campo<TextBox>("TxtNomeColecao").Text = "Novo nome"; a.T.Campo<TextBox>("TxtDescricaoColecao").Text = "Nova descrição"; a.T.Clicar("BtnSalvarColecao"); Assert.Equal(id, a.Servico.Colecoes[0].Id); Assert.Equal("Novo nome", a.Servico.Colecoes[0].Nome); Assert.Single(a.Obras); }
    [AvaloniaFact(DisplayName = "UC06 | Associar obra pelo seletor | Biblioteca não duplica e lista da coleção mostra obra")]
    public void Associar() { using var a = new TelaColecoes(); a.Criar(); a.Associar(); Assert.Single(a.Obras); Assert.Single(a.T.Salvos); Assert.NotNull(a.T.Salvos[0].IdInterno); }
    [AvaloniaFact(DisplayName = "UC07 | Repetir associação pela janela | Mensagem específica e apenas um vínculo")]
    public void Repetir() { using var a = new TelaColecoes(); a.Criar(); a.Associar(); a.Associar(); Assert.Single(a.Obras); Assert.Equal("Obra já associada", a.T.Campo<TextBlock>("TxtAvisoTitulo").Text); }
    [AvaloniaFact(DisplayName = "UC08 | Associar mesma obra em duas coleções | Uma obra pessoal e dois vínculos independentes")]
    public void Varias() { using var a = new TelaColecoes(); a.Criar("A"); a.Associar(); a.Criar("B"); a.Associar(); Assert.Single(a.T.Salvos); Assert.All(a.Servico.Colecoes, c => Assert.Single(c.Obras)); a.Selecionar(0); Assert.Single(a.Obras); }
    [AvaloniaFact(DisplayName = "UC09 | Remover associação selecionada | Obra permanece na biblioteca e outra coleção")]
    public void Remover() { using var a = new TelaColecoes(); a.Criar("A"); a.Associar(); a.Criar("B"); a.Associar(); a.T.Campo<ListBox>("ListaObrasColecao").SelectedIndex = 0; a.T.Clicar("BtnRemoverObraColecao"); Assert.Empty(a.Obras); Assert.Single(a.T.Salvos); a.Selecionar(0); Assert.Single(a.Obras); }
    [AvaloniaFact(DisplayName = "UC10 | Solicitar exclusão de coleção | Confirmação aparece sem excluir antecipadamente")]
    public void Confirmacao() { using var a = new TelaColecoes(); a.Criar(); a.Associar(); a.T.Clicar("BtnExcluirColecao"); Assert.True(a.T.Campo<Border>("ConfirmacaoExclusaoColecao").IsVisible); Assert.Single(a.Servico.Colecoes); Assert.Single(a.T.Salvos); }
    [AvaloniaFact(DisplayName = "UC11 | Cancelar exclusão | Coleção e associações permanecem")]
    public void Cancelar() { using var a = new TelaColecoes(); a.Criar(); a.Associar(); a.T.Clicar("BtnExcluirColecao"); a.T.Clicar("BtnCancelarExcluirColecao"); Assert.False(a.T.Campo<Border>("ConfirmacaoExclusaoColecao").IsVisible); Assert.Single(a.Obras); }
    [AvaloniaFact(DisplayName = "UC12 | Confirmar exclusão | Coleção removida sem apagar registros pessoais")]
    public void Excluir() { using var a = new TelaColecoes(); a.Criar(); a.Associar(); a.T.Clicar("BtnExcluirColecao"); a.T.Clicar("BtnConfirmarExcluirColecao"); Assert.Empty(a.Servico.Colecoes); Assert.Single(a.T.Salvos); Assert.Empty(a.Obras); }
    [AvaloniaFact(DisplayName = "UC13 | Trocar coleção enquanto confirma exclusão | Confirmação antiga é cancelada sem excluir outra coleção")]
    public void TrocarDuranteExclusao() { using var a = new TelaColecoes(); a.Criar("A"); a.Criar("B"); a.Selecionar(0); a.T.Clicar("BtnExcluirColecao"); a.Selecionar(1); a.T.Clicar("BtnConfirmarExcluirColecao"); Assert.Equal(2, a.Servico.Colecoes.Count); Assert.False(a.T.Campo<Border>("ConfirmacaoExclusaoColecao").IsVisible); }
    [AvaloniaFact(DisplayName = "UC14 | Coleção com todas as mídias | Lista mostra seis tipos sem mudar categorias oficiais")]
    public void Mista() { using var a = new TelaColecoes(Enum.GetValues<TipoMidia>().Select(t => ConversorBibliotecaJson.ParaRegistro(ApoioMultimidia.Item(t)))); a.Criar(); for (var n = 0; n < 6; n++) a.Associar(n); Assert.Equal(6, a.Obras.Length); a.T.Clicar("NavBacklog"); Assert.Single(a.T.Exibidos); Assert.Equal(0, a.T.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "UC15 | Editar dados pessoais pela coleção | Formulário preserva tipo e atualiza obra em todas as visualizações")]
    public void EditarObra() { using var a = new TelaColecoes(new[] { ConversorBibliotecaJson.ParaRegistro(ApoioMultimidia.Item(TipoMidia.Livro)) }); a.Criar(); a.Associar(); a.T.Campo<ListBox>("ListaObrasColecao").SelectedIndex = 0; a.T.Clicar("BtnEditarObraColecao"); Assert.Equal((int)TipoMidia.Livro, a.T.Campo<ComboBox>("CmbTipoCadastro").SelectedIndex); a.T.Campo<NumericUpDown>("NumPagina").Value = 10; a.T.Clicar("BtnAtualizar"); a.T.Clicar("NavColecoes"); Assert.Equal(10, Assert.Single(a.Obras).Pessoal.Progresso!.Pagina); }
    [AvaloniaFact(DisplayName = "UC16 | Arquivo de coleções inválido | Coleções bloqueadas e edição pessoal local continua")]
    public void JsonInvalido() { using var a = new TelaColecoes(jsonColecoes: "{"); Assert.True(a.T.Campo<Border>("AvisoColecoesIndisponiveis").IsVisible); Assert.False(a.T.Campo<Button>("BtnCriarColecao").IsEnabled); a.T.Editar(); a.T.Campo<TextBox>("TxtReview").Text = "Local continua"; a.T.Clicar("BtnAtualizar"); Assert.Equal("Local continua", a.T.Salvos[0].Review); Assert.Equal("{", File.ReadAllText(a.Caminho)); }
    [AvaloniaFact(DisplayName = "UC17 | Biblioteca inválida | Coleções não podem receber alterações ou obras inexistentes")]
    public void BibliotecaInvalida() { using var a = new TelaColecoes(bibliotecaJson: "{"); Assert.False(a.T.Campo<Button>("BtnCriarColecao").IsEnabled); a.Criar(); Assert.Empty(a.Servico.Colecoes); Assert.Equal("{", File.ReadAllText(a.T.Pasta.Arquivo)); }
    [AvaloniaFact(DisplayName = "UC18 | Falha ao salvar coleção | Aviso e estado anterior preservados na janela")]
    public void FalhaCriar() { var s = new ColecoesSimuladas { FalhaGravacao = new IOException() }; using var a = new TelaColecoes(simulado: s); a.Criar(); Assert.Empty(a.Servico.Colecoes); Assert.Equal("Coleção não salva", a.T.Campo<TextBlock>("TxtAvisoTitulo").Text); }
    [AvaloniaFact(DisplayName = "UC19 | Falha na associação após salvar identidade | Interface permanece coerente e repetição funciona")]
    public void FalhaAssociar() { var s = new ColecoesSimuladas(); using var a = new TelaColecoes(simulado: s); a.Criar(); s.FalhaGravacao = new IOException(); a.Associar(); Assert.NotNull(a.T.Salvos[0].IdInterno); Assert.Empty(a.Obras); s.FalhaGravacao = null; a.Associar(); Assert.Single(a.Obras); Assert.Single(a.T.Exibidos); }
    [AvaloniaFact(DisplayName = "UC20 | Duplicatas na janela | Selecionar segunda obra não associa primeira implicitamente")]
    public void Duplicatas() { var r = PastaTeste.Jogo(); using var a = new TelaColecoes(new[] { r, r }); a.Criar(); a.Associar(1); var salvos = a.T.Salvos; Assert.Null(salvos[0].IdInterno); Assert.NotNull(salvos[1].IdInterno); Assert.Single(a.Obras); }
    [AvaloniaFact(DisplayName = "UC21 | Reabrir janela com os dois arquivos | Nome, descrição e associação recarregam")]
    public void Reiniciar() { using var a = new TelaColecoes(); a.Criar("Persistida", "Descrição"); a.Associar(); using var b = new TelaColecoes(a.T.Salvos, File.ReadAllText(a.Caminho)); b.Selecionar(0); Assert.Equal("Persistida", b.T.Campo<TextBox>("TxtNomeColecao").Text); Assert.Equal("Descrição", b.T.Campo<TextBox>("TxtDescricaoColecao").Text); Assert.Single(b.Obras); Assert.Equal(a.T.Salvos[0].IdInterno, b.Obras[0].IdInterno); }
    [AvaloniaFact(DisplayName = "UC22 | Coleção no tamanho mínimo | Controles e obras ficam acessíveis pela rolagem")]
    public void Minimo() { using var a = new TelaColecoes(); a.Criar(); a.Associar(); a.T.Janela.Width = 900; a.T.Janela.Height = 660; a.Renderizar(); var pasta = Path.Combine(Path.GetTempPath(), "backlogmanager-fase4-resultados", "imagens"); Directory.CreateDirectory(pasta); using (var inicio = a.T.Janela.CaptureRenderedFrame()) { Assert.NotNull(inicio); inicio.Save(Path.Combine(pasta, "colecoes-topo.png"), PngBitmapEncoderOptions.Default); } a.T.Campo<ScrollViewer>("PaginaColecoes").Offset = new Avalonia.Vector(0, 10000); a.Renderizar(); using var fim = a.T.Janela.CaptureRenderedFrame(); Assert.NotNull(fim); fim.Save(Path.Combine(pasta, "colecoes-obras.png"), PngBitmapEncoderOptions.Default); Assert.True(a.T.Campo<Button>("NavInicio").Bounds.Height > 0); Assert.Single(a.Obras); }
    [AvaloniaFact(DisplayName = "UC23 | Obra excluída fora da coleção | Contagem e aviso não exibem registro fantasma")]
    public void ExcluirNaBiblioteca() { using var a = new TelaColecoes(); a.Criar(); a.Associar(); a.T.Clicar("NavBacklog"); a.T.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; a.T.Clicar("BtnRemover"); a.T.Clicar("NavColecoes"); Assert.Empty(a.Obras); Assert.Contains("referências", a.T.Campo<TextBlock>("TxtResumoColecao").Text!); }
    [AvaloniaFact(DisplayName = "UC24 | Formulário de edição aberto antes de atribuir ID | Edição posterior salva sem perder associação")]
    public void EdicaoAnterior() { using var a = new TelaColecoes(); a.T.Editar(); a.T.Campo<TextBox>("TxtReview").Text = "Edição preservada"; a.T.Clicar("NavColecoes"); a.Criar(); a.Associar(); a.T.Clicar("NavCadastro"); a.T.Clicar("BtnAtualizar"); Assert.Equal("Edição preservada", a.T.Salvos[0].Review); Assert.NotNull(a.T.Salvos[0].IdInterno); Assert.Single(a.Obras); }
}
