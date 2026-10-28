using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Services;

namespace BacklogManager.Tests;

internal sealed class TelaTeste : IDisposable
{
    public PastaTeste Pasta { get; } = new();
    public HttpSimulado Http { get; } = new();
    private readonly HttpClient _cliente;
    private readonly HttpClient _clienteCapas = new(new HttpSimulado
    {
        Responder = (_, _) => Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.NotFound))
    });
    public MainWindow Janela { get; }
    public TelaTeste(IEnumerable<BacklogItem>? iniciais = null, bool semServico = false, string? json = null, ArmazenamentoPreferenciasBiblioteca? preferencias = null, CarregadorCapas? capas = null,
        Func<ServicoBiblioteca, ServicoColecoes>? criarColecoes = null)
    {
        if (json != null) File.WriteAllText(Pasta.Arquivo, json);
        else if (iniciais != null) File.WriteAllText(Pasta.Arquivo, JsonSerializer.Serialize(iniciais));
        _cliente = new HttpClient(Http);
        var biblioteca = new ServicoBiblioteca(new ArmazenamentoJsonBiblioteca(new FileManager(Pasta.Arquivo)));
        Janela = new MainWindow(biblioteca, semServico ? null : new PesquisaJogosIgdb(new IgdbService(_cliente, "cliente-simulado", "token-simulado")), preferencias: preferencias, capas: capas ?? new CarregadorCapas(_clienteCapas), colecoes: criarColecoes?.Invoke(biblioteca));
        Janela.Show();
    }
    public T Campo<T>(string nome) where T : Control => Janela.FindControl<T>(nome)!;
    public void Clicar(string nome) => Campo<Button>(nome).RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
    public void Selecionar(int id = 42, string nome = "Jogo IGDB simulado")
    {
        Campo<TextBox>("TxtTitulo").Text = nome;
        var lista = Campo<ListBox>("ListaSugestoes");
        var jogo = new ObraCatalogo { Titulo = nome, Origem = "IGDB", IdentificadorExterno = id.ToString(System.Globalization.CultureInfo.InvariantCulture) }; lista.ItemsSource = new[] { jogo }; lista.SelectedItem = jogo;
    }
    public BacklogItem[] Exibidos => Campo<ListBox>("ListaBacklog").Items.Cast<ItemBiblioteca>().Select(ConversorBibliotecaJson.ParaRegistro).ToArray();
    public List<BacklogItem> Salvos => new FileManager(Pasta.Arquivo).Carregar();
    public string Aviso => Campo<TextBlock>("TxtAvisoMensagem").Text ?? "";
    public void Editar(int index = 0)
    {
        Campo<ListBox>("ListaBacklog").SelectedIndex = index; Clicar("BtnEditar");
    }
    public void Dispose() { Janela.Close(); _cliente.Dispose(); _clienteCapas.Dispose(); Pasta.Dispose(); }
}
