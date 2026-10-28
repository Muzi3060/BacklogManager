using BacklogManager.Contracts;
using BacklogManager.Models;
using BacklogManager.Services;

namespace BacklogManager.Tests.Fase1;

internal sealed class ArmazenamentoSimulado : IArmazenamentoBiblioteca
{
    public string DescricaoOrigem => "memória de teste";
    public IReadOnlyList<ItemBiblioteca> Registros { get; set; } = Array.Empty<ItemBiblioteca>();
    public Exception? FalhaLeitura { get; set; }
    public Exception? FalhaGravacao { get; set; }
    public int Gravacoes { get; private set; }
    public IReadOnlyList<ItemBiblioteca> Carregar()
    {
        if (FalhaLeitura != null) throw FalhaLeitura;
        return Registros;
    }
    public void Salvar(IReadOnlyList<ItemBiblioteca> itens)
    {
        if (FalhaGravacao != null) throw FalhaGravacao;
        Registros = itens.ToArray();
        Gravacoes++;
    }
}

internal static class ApoioArquitetura
{
    internal static ItemBiblioteca Jogo(string titulo = "Jogo de teste", string status = "Na Fila") =>
        new(new ObraCatalogo { Titulo = titulo, Origem = "IGDB", IdentificadorExterno = "42" },
            new DadosPessoaisBiblioteca { Categoria = "RPG", Status = status, Nota = 4.5, Review = "Review antiga" });

    internal static (ServicoBiblioteca servico, ArmazenamentoSimulado armazenamento) Carregada(params ItemBiblioteca[] itens)
    {
        var armazenamento = new ArmazenamentoSimulado { Registros = itens };
        var servico = new ServicoBiblioteca(armazenamento);
        servico.Carregar();
        return (servico, armazenamento);
    }
}
