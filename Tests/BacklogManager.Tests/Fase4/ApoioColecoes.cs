using BacklogManager.Contracts;
using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Services;
using BacklogManager.Tests.Fase1;
namespace BacklogManager.Tests.Fase4;

internal sealed class ColecoesSimuladas : IArmazenamentoColecoes
{
    internal IReadOnlyList<ColecaoPersonalizada> Registros { get; set; } = Array.Empty<ColecaoPersonalizada>();
    internal Exception? FalhaLeitura { get; set; }
    internal Exception? FalhaGravacao { get; set; }
    internal int Gravacoes { get; private set; }
    public string DescricaoOrigem => "coleções simuladas";
    public IReadOnlyList<ColecaoPersonalizada> Carregar() { if (FalhaLeitura != null) throw FalhaLeitura; return Registros; }
    public void Salvar(IReadOnlyList<ColecaoPersonalizada> colecoes) { if (FalhaGravacao != null) throw FalhaGravacao; Registros = colecoes; Gravacoes++; }
}
internal sealed class AmbienteColecoes
{
    internal ServicoBiblioteca Biblioteca { get; }
    internal ArmazenamentoSimulado ArquivoBiblioteca { get; }
    internal ColecoesSimuladas Arquivo { get; } = new();
    internal ServicoColecoes Servico { get; }
    internal AmbienteColecoes(params ItemBiblioteca[] itens)
    {
        (Biblioteca, ArquivoBiblioteca) = ApoioArquitetura.Carregada(itens);
        Servico = new ServicoColecoes(Arquivo, Biblioteca); Servico.Carregar();
    }
    internal static ItemBiblioteca Jogo(string titulo = "Jogo fictício") => ApoioArquitetura.Jogo(titulo);
    internal Guid Criar(string nome = "Favoritos", string descricao = "") => Servico.Criar(nome, descricao);
}
internal sealed class ArquivosColecoes : IDisposable
{
    internal PastaTeste Pasta { get; } = new();
    internal string Caminho => Path.Combine(Pasta.Pasta, "backlog.colecoes.json");
    internal ServicoBiblioteca Biblioteca { get; }
    internal ServicoColecoes Servico { get; }
    internal ArmazenamentoColecoesJson Arquivo { get; }
    internal ArquivosColecoes(params ItemBiblioteca[] itens)
    {
        var a = new ArmazenamentoJsonBiblioteca(new FileManager(Pasta.Arquivo)); a.Carregar(); a.Salvar(itens);
        Biblioteca = new ServicoBiblioteca(new ArmazenamentoJsonBiblioteca(new FileManager(Pasta.Arquivo))); Biblioteca.Carregar();
        Arquivo = new ArmazenamentoColecoesJson(Caminho); Servico = new ServicoColecoes(Arquivo, Biblioteca); Servico.Carregar();
    }
    internal (ServicoBiblioteca biblioteca, ServicoColecoes colecoes) Reabrir()
    {
        var b = new ServicoBiblioteca(new ArmazenamentoJsonBiblioteca(new FileManager(Pasta.Arquivo))); b.Carregar();
        var c = new ServicoColecoes(new ArmazenamentoColecoesJson(Caminho), b); c.Carregar(); return (b, c);
    }
    public void Dispose() => Pasta.Dispose();
}
