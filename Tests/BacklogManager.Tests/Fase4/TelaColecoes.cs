using Avalonia.Controls;
using Avalonia.Threading;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Services;
namespace BacklogManager.Tests.Fase4;

internal sealed class TelaColecoes : IDisposable
{
    internal TelaTeste T { get; }
    internal ServicoColecoes Servico { get; private set; } = null!;
    internal string Caminho { get; private set; } = "";
    internal TelaColecoes(IEnumerable<BacklogItem>? itens = null, string? jsonColecoes = null, ColecoesSimuladas? simulado = null, string? bibliotecaJson = null)
    {
        T = new TelaTeste(itens ?? new[] { PastaTeste.Jogo() }, semServico: true, json: bibliotecaJson, criarColecoes: biblioteca =>
        {
            Caminho = Path.Combine(Path.GetDirectoryName(biblioteca.DescricaoOrigem)!, "backlog.colecoes.json");
            if (jsonColecoes != null) File.WriteAllText(Caminho, jsonColecoes);
            return Servico = new ServicoColecoes(simulado ?? (BacklogManager.Contracts.IArmazenamentoColecoes)new ArmazenamentoColecoesJson(Caminho), biblioteca);
        });
        T.Clicar("NavColecoes"); Renderizar();
    }
    internal void Criar(string nome = "Favoritos", string descricao = "")
    { T.Clicar("BtnNovaColecao"); T.Campo<TextBox>("TxtNomeColecao").Text = nome; T.Campo<TextBox>("TxtDescricaoColecao").Text = descricao; T.Clicar("BtnCriarColecao"); Renderizar(); }
    internal void Associar(int indice = 0)
    { T.Campo<ComboBox>("CmbObraColecao").SelectedIndex = indice; T.Clicar("BtnAdicionarObraColecao"); Renderizar(); }
    internal void Selecionar(int indice) { T.Campo<ComboBox>("CmbColecoes").SelectedIndex = indice; Renderizar(); }
    internal ItemBiblioteca[] Obras => T.Campo<ListBox>("ListaObrasColecao").Items.Cast<ItemBiblioteca>().ToArray();
    internal void Renderizar() { Dispatcher.UIThread.RunJobs(); T.Janela.UpdateLayout(); }
    public void Dispose() => T.Dispose();
}
