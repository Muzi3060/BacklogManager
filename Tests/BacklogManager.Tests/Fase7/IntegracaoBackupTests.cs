using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using BacklogManager.Backup;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Services;
using BacklogManager.Aparencia;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Xunit;
namespace BacklogManager.Tests.Fase7;
public sealed class IntegracaoBackupTests
{
    [AvaloniaFact(DisplayName="QI01 | Dashboard após restauração | Resumo usa dados novos sem referências antigas")]
    public async Task Caso01()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);t.Confirmar();Assert.Contains("concluída e dados recarregados",t.Estado);Assert.Equal("1",t.Campo<TextBlock>("TxtTotal").Text);Assert.Equal("Restaurado",t.Biblioteca.Itens[0].Catalogo.Titulo);
    }
    [AvaloniaFact(DisplayName="QI02 | Formulário em edição | Seleção antiga é limpa depois da substituição")]
    public async Task Caso02()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a);t.Clicar("NavBacklog");t.Campo<ListBox>("ListaBacklog").SelectedIndex=0;t.Clicar("BtnEditar");t.Clicar("NavConfiguracoes");a.Importar();await t.Selecionar(a.Importado);t.Confirmar();Assert.Equal("",t.Campo<TextBox>("TxtTitulo").Text);Assert.True(t.Campo<Button>("BtnAdicionar").IsVisible);Assert.False(t.Campo<Button>("BtnAtualizar").IsVisible);
    }
    [AvaloniaFact(DisplayName="QI03 | Coleções recarregadas | Lista mostra novo conjunto")]
    public async Task Caso03()
    {
        using var a=new AmbienteBackup(); var d=AmbienteBackup.Basico();d.Componentes["Colecoes"]=AmbienteBackup.C(JsonSerializer.SerializeToUtf8Bytes(new{Versao=1,Colecoes=new[]{new ColecaoPersonalizada(Guid.NewGuid(),"Nova","",Array.Empty<Guid>())}}));using var t=new TelaBackup(a);a.Importar(d);await t.Selecionar(a.Importado);t.Confirmar();Assert.Single(t.Colecoes.Colecoes);Assert.Equal("Nova",t.Colecoes.Colecoes[0].Nome);Assert.Single(t.Campo<ComboBox>("CmbColecoes").Items);
    }
    [AvaloniaFact(DisplayName="QI04 | Visualização após recarga | Preferência restaurada aparece nos dois seletores")]
    public async Task Caso04()
    {
        using var a=new AmbienteBackup(); a.Preferencias();var d=a.Documento();File.Delete(a.Caminhos.Arquivos["Visualizacao"]);using var t=new TelaBackup(a);a.Importar(d);await t.Selecionar(a.Importado);t.Confirmar(true);Assert.Equal(1,t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex);Assert.Equal(1,t.Campo<ComboBox>("CmbVisualizacaoConfiguracoes").SelectedIndex);
    }
    [AvaloniaFact(DisplayName="QI05 | Tema após recarga | Acolhedor escuro reaplicado sem reiniciar")]
    public async Task Caso05()
    {
        using var a=new AmbienteBackup(); a.Preferencias();var d=a.Documento();File.Delete(a.Caminhos.Arquivos["Aparencia"]);using var t=new TelaBackup(a);a.Importar(d);await t.Selecionar(a.Importado);t.Confirmar(true);Assert.Equal(TemaVisual.Acolhedor,t.Janela.AparenciaAtual.Preferencias.Tema);Assert.Equal(ModoVisual.Escuro,t.Janela.AparenciaAtual.Preferencias.Modo);
    }
    [AvaloniaFact(DisplayName="QI06 | Falha de gravação na interface | Sem mensagem de sucesso e estado antigo recarregado")]
    public async Task Caso06()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);a.Servico.Etapa=s=>{if(s=="depois-Biblioteca")throw new IOException("simulada");};t.Confirmar();Assert.Contains("não concluída",t.Estado);Assert.DoesNotContain("concluída e dados recarregados",t.Estado);Assert.Equal("Anterior",t.Biblioteca.Itens[0].Catalogo.Titulo);
    }
    [AvaloniaFact(DisplayName="QI07 | Recuperação pendente na inicialização | Gravação e backup desabilitados sem carregar par misto")]
    public async Task Caso07()
    {
        using var a=new AmbienteBackup(); File.WriteAllText(a.Caminhos.Diario,"{}");a.Servico.Recuperar();using var t=new TelaBackup(a,false);Assert.False(t.Biblioteca.Carregada);Assert.False(t.Colecoes.Carregadas);Assert.False(t.Campo<Button>("BtnExportarBackup").IsEnabled);Assert.True(t.Campo<Button>("BtnRecuperarBackup").IsVisible);await Task.CompletedTask;
    }
    [AvaloniaFact(DisplayName="QI08 | Exportação pela interface | Arquivo verificado e mensagem com destino")]
    public async Task Caso08()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a);await t.Exportar(a.Saida);Assert.Equal(1,a.Servico.Prever(a.Saida).Obras);Assert.Contains("exportado e verificado",t.Estado);
    }
    [AvaloniaFact(DisplayName="QI09 | Falha de preferência local | Visualização na sessão preserva dados")]
    public async Task Caso09()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a);using var trava=new FileStream(a.Caminhos.Arquivos["Visualizacao"]+".lock",FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.None);t.Campo<ComboBox>("CmbVisualizacaoConfiguracoes").SelectedIndex=1;Assert.Equal(1,t.Campo<ComboBox>("CmbVisualizacao").SelectedIndex);Assert.False(File.Exists(a.Caminhos.Arquivos["Visualizacao"]));await Task.CompletedTask;
    }
    [AvaloniaFact(DisplayName="QI10 | Biblioteca filtrada | Exportação contém biblioteca completa")]
    public async Task Caso10()
    {
        using var a=new AmbienteBackup(); a.Biblioteca(PastaTeste.Jogo("A"),PastaTeste.Jogo("B"));using var t=new TelaBackup(a);t.Campo<TextBox>("TxtBusca").Text="A";await t.Exportar(a.Saida);Assert.Equal(2,a.Servico.Prever(a.Saida).Obras);
    }
    [AvaloniaFact(DisplayName="QI11 | IGDB simulada após restauração | Pesquisa e seleção continuam disponíveis")]
    public async Task Caso11()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);t.Confirmar();t.Clicar("NavCadastro");t.Campo<TextBox>("TxtTitulo").Text="Teste IGDB";await Dispatcher.UIThread.InvokeAsync(()=>{},DispatcherPriority.Background); await t.Janela.BuscaPendente.WaitAsync(TimeSpan.FromSeconds(5),TestContext.Current.CancellationToken);Assert.True(t.Http.Chamadas>0);
    }
    [AvaloniaFact(DisplayName="QI12 | Cinco catálogos simulados | Pesquisa não jogo continua offline")]
    public async Task Caso12()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);t.Confirmar();t.Clicar("NavCadastro");foreach(var tipo in Enum.GetValues<TipoMidia>().Where(x=>x!=TipoMidia.Jogo)){t.Campo<ComboBox>("CmbTipoCadastro").SelectedIndex=(int)tipo;t.Campo<TextBox>("TxtTitulo").Text="a";await Dispatcher.UIThread.InvokeAsync(()=>{},DispatcherPriority.Background); await t.Janela.BuscaPendente.WaitAsync(TimeSpan.FromSeconds(5),TestContext.Current.CancellationToken);Assert.True(t.Campo<ListBox>("ListaSugestoes").Items.Count>0);}Assert.Equal(0,t.Http.Chamadas);
    }
    [AvaloniaFact(DisplayName="QI13 | CRUD local após recarga | Edição pessoal persiste e obra pode ser excluída")]
    public async Task Caso13()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);t.Confirmar();var item=t.Biblioteca.Itens[0];t.Biblioteca.Atualizar(item,item.Pessoal with {Review="Nova review"});Assert.Equal("Nova review",a.LerBiblioteca()[0].Pessoal.Review);t.Biblioteca.Remover(t.Biblioteca.Itens[0]);Assert.Empty(a.LerBiblioteca());
    }
    [AvaloniaFact(DisplayName="QI14 | Reabertura completa | Preferências biblioteca e coleções persistem juntos")]
    public async Task Caso14()
    {
        using var a=new AmbienteBackup(); a.Preferencias();var d=a.Documento();a.Importar(d);using(var t=new TelaBackup(a)){await t.Selecionar(a.Importado);t.Confirmar(true);}using var nova=new TelaBackup(a);Assert.Single(nova.Biblioteca.Itens);Assert.Equal(1,nova.Campo<ComboBox>("CmbVisualizacao").SelectedIndex);Assert.Equal(TemaVisual.Acolhedor,nova.Janela.AparenciaAtual.Preferencias.Tema);
    }
    [AvaloniaFact(DisplayName="QI15 | Recuperação pela interface | Retry recupera e recarrega serviços antes de liberar ações")]
    public async Task Caso15()
    {
        using var a=new AmbienteBackup(); var p=a.Previa();a.Servico.Etapa=s=>{if(s=="depois-Biblioteca"||s=="rollback-Biblioteca")throw new IOException();};Assert.Throws<IOException>(()=>a.Servico.Restaurar(p,false,true));a.Servico.Etapa=null;using var t=new TelaBackup(a,false);t.Clicar("BtnRecuperarBackup");Assert.True(t.Biblioteca.Carregada);Assert.True(t.Campo<Button>("BtnExportarBackup").IsEnabled);Assert.Equal("Anterior",t.Biblioteca.Itens[0].Catalogo.Titulo);Assert.False(t.Campo<Border>("AvisoColecoesIndisponiveis").IsVisible);Assert.False(t.Campo<Border>("AvisoOperacao").IsVisible);await Task.CompletedTask;
    }
    [AvaloniaFact(DisplayName="QI16 | Offline sem IGDB | Backup e navegação não exigem requisições de catálogo")]
    public async Task Caso16()
    {
        using var a=new AmbienteBackup(); using var t=new TelaBackup(a);a.Importar();await t.Selecionar(a.Importado);t.Confirmar();t.Clicar("NavInicio");t.Clicar("NavColecoes");t.Clicar("NavBacklog");Assert.Equal(0,t.Http.Chamadas);
    }
}
