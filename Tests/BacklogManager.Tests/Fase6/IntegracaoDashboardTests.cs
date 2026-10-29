using Xunit;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;
using BacklogManager.Models;
using BacklogManager.Services;
using BacklogManager.Data;
using BacklogManager.Controls;
using BacklogManager.Aparencia;
namespace BacklogManager.Tests.Fase6;

public sealed class IntegracaoDashboardTests
{
    [AvaloniaFact(DisplayName = "DX01 | Associação em duas coleções | Conta uma obra pessoal e conserva vínculos")]
    public void Caso01() { using var a=new Fase4.TelaColecoes();a.Criar("A");a.Associar();a.Criar("B");a.Associar();a.T.Clicar("NavInicio");Assert.Equal("1",a.T.Campo<TextBlock>("TxtTotal").Text);Assert.All(a.Servico.Colecoes,c=>Assert.Single(c.Obras)); }
    [AvaloniaFact(DisplayName = "DX02 | Excluir coleção | Dashboard mantém obra associada")]
    public void Caso02() { using var a=new Fase4.TelaColecoes();a.Criar();a.Associar();a.T.Clicar("BtnExcluirColecao");a.T.Clicar("BtnConfirmarExcluirColecao");a.T.Clicar("NavInicio");Assert.Equal("1",a.T.Campo<TextBlock>("TxtTotal").Text);Assert.Single(a.T.Salvos); }
    [AvaloniaFact(DisplayName = "DX03 | Manter dados de coleções | Trocar período não altera arquivo")]
    public void Caso03() { using var a=new Fase4.TelaColecoes();a.Criar();a.Associar();var antes=File.ReadAllBytes(a.Caminho);a.T.Clicar("NavInicio");a.T.Campo<ComboBox>("CmbPeriodoDashboard").SelectedIndex=0;Assert.Equal(antes,File.ReadAllBytes(a.Caminho)); }
    [AvaloniaFact(DisplayName = "DX04 | Identificador Steam legado | Leitura preserva número sem reinterpretar")]
    public void Caso04() { using var t=new TelaTeste(new[]{PastaTeste.Jogo()});var antes=t.Salvos[0].SteamAppId;t.Clicar("NavInicio");Assert.Equal(antes,t.Salvos[0].SteamAppId);Assert.Null(t.Salvos[0].IdInterno); }
    [AvaloniaFact(DisplayName = "DX05 | Reinício de biblioteca | Resumo é recalculado dos mesmos registros")]
    public void Caso05() { var i=ConversorBibliotecaJson.ParaRegistro(ApoioDashboard.Item(status:"Zerado"));using var a=new TelaTeste(new[]{i});using var b=new TelaTeste(a.Salvos);Assert.Equal(a.Campo<TextBlock>("TxtZerados").Text,b.Campo<TextBlock>("TxtZerados").Text); }
    [AvaloniaFact(DisplayName = "DX06 | Preferência de período da sessão | Não cria novo arquivo de configuração")]
    public void Caso06() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item());var antes=Directory.GetFiles(t.Pasta.Pasta).Order().ToArray();t.Campo<ComboBox>("CmbPeriodoDashboard").SelectedIndex=0;Assert.Equal(antes,Directory.GetFiles(t.Pasta.Pasta).Order().ToArray()); }
    [AvaloniaFact(DisplayName = "DX07 | Retorno ao início após meia noite | Recalcula com relógio atual")]
    public void Caso07() { var agora=ApoioDashboard.Agora;var s=new ServicoDashboard(()=>agora,TimeZoneInfo.Utc);using var t=new TelaTeste(new[]{ConversorBibliotecaJson.ParaRegistro(ApoioDashboard.Item(6))},dashboard:s);t.Campo<ComboBox>("CmbPeriodoDashboard").SelectedIndex=0;Assert.Equal("1",t.Campo<TextBlock>("TxtAdicionadas").Text);t.Clicar("NavBacklog");agora=agora.AddDays(1);t.Clicar("NavInicio");Assert.Equal("0",t.Campo<TextBlock>("TxtAdicionadas").Text); }
    [AvaloniaFact(DisplayName = "DX08 | Falha ao excluir | Sem contagem otimista")]
    public void Caso08() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item());File.AppendAllText(t.Pasta.Arquivo," ");t.Campo<ListBox>("ListaBacklog").SelectedIndex=0;t.Clicar("BtnRemover");Assert.Equal("1",t.Campo<TextBlock>("TxtTotal").Text);Assert.Single(t.Salvos); }
    [AvaloniaFact(DisplayName = "DX09 | Falha ao editar | Contagem anterior permanece")]
    public void Caso09() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item(status:"Jogando"));t.Editar();t.Campo<ComboBox>("CmbStatus").SelectedIndex=2;File.AppendAllText(t.Pasta.Arquivo," ");t.Clicar("BtnAtualizar");Assert.Equal("0",t.Campo<TextBlock>("TxtZerados").Text);Assert.Equal("1",t.Campo<TextBlock>("TxtJogando").Text); }
    [AvaloniaFact(DisplayName = "DX10 | Cadastro simulado de outra mídia | Novo registro atualiza contagens")]
    public void Caso10() { using var t=ApoioDashboard.Tela();t.Campo<ComboBox>("CmbTipoCadastro").SelectedIndex=(int)TipoMidia.Livro;var obra=Fase2.ApoioMultimidia.Item(TipoMidia.Livro).Catalogo;t.Campo<TextBox>("TxtTitulo").Text=obra.Titulo;var lista=t.Campo<ListBox>("ListaSugestoes");lista.ItemsSource=new[]{obra};lista.SelectedItem=obra;t.Clicar("BtnAdicionar");Assert.Equal("1",t.Campo<TextBlock>("TxtTotal").Text); }
    [AvaloniaFact(DisplayName = "DX11 | Jogo sem seleção | Dashboard não contorna exigência IGDB")]
    public void Caso11() { using var t=ApoioDashboard.Tela();t.Campo<TextBox>("TxtTitulo").Text="Arbitrário";t.Clicar("BtnAdicionar");Assert.Equal("0",t.Campo<TextBlock>("TxtTotal").Text);Assert.Empty(t.Salvos); }
    [AvaloniaFact(DisplayName = "DX12 | Seis mídias sem HTTP | Estatísticas não consultam catálogos")]
    public void Caso12() { using var t=ApoioDashboard.Tela(Enum.GetValues<TipoMidia>().Select(tipo=>ApoioDashboard.Item(tipo:tipo)).ToArray());Assert.Equal("6",t.Campo<TextBlock>("TxtTotal").Text);Assert.Equal(0,t.Http.Chamadas); }
    [AvaloniaFact(DisplayName = "DX13 | Categoria legada | Não muda categoria ao apresentar resumo")]
    public void Caso13() { var r=PastaTeste.Jogo();r.Categoria="Categoria antiga";using var t=new TelaTeste(new[]{r});t.Clicar("NavInicio");Assert.Equal("Categoria antiga",t.Salvos[0].Categoria); }
    [AvaloniaFact(DisplayName = "DX14 | Duplicatas pessoais | Não usa título para deduplicar")]
    public void Caso14() { var i=ApoioDashboard.Item();using var t=ApoioDashboard.Tela(i,i with{});Assert.Equal("2",t.Campo<TextBlock>("TxtTotal").Text);Assert.Equal(2,t.Campo<ItemsControl>("ListaAtividade").Items.Count); }
    [AvaloniaFact(DisplayName = "DX15 | Preferência de visualização | Consulta não muda lista para grade")]
    public void Caso15() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item());t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex=1;t.Clicar("NavInicio");Assert.Equal(1,t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex); }
    [AvaloniaFact(DisplayName = "DX16 | Período conserva rascunho | Navegação não apaga review em edição")]
    public void Caso16() { using var t=ApoioDashboard.Tela(ApoioDashboard.Item());t.Editar();t.Campo<TextBox>("TxtReview").Text="Rascunho";t.Clicar("NavInicio");t.Campo<ComboBox>("CmbPeriodoDashboard").SelectedIndex=0;t.Clicar("NavCadastro");Assert.Equal("Rascunho",t.Campo<TextBox>("TxtReview").Text); }
}
