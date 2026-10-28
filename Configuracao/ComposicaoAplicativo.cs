using System;
using System.Net.Http;
using BacklogManager.Contracts;
using BacklogManager.Data;
using BacklogManager.Services;

namespace BacklogManager.Configuracao;

internal sealed record DependenciasJanela(ServicoBiblioteca Biblioteca, IPesquisaCatalogo? Pesquisa, IDisposable? RecursoPesquisa,
    ArmazenamentoPreferenciasBiblioteca? Preferencias = null, ServicoColecoes? Colecoes = null, BacklogManager.Aparencia.ServicoAparencia? Aparencia = null);

internal static class ComposicaoAplicativo
{
    internal static DependenciasJanela Criar(string caminhoArquivo = "backlog.json",
        Func<string, string?>? lerVariavel = null, Func<HttpClient>? criarCliente = null)
    {
        var biblioteca = new ServicoBiblioteca(new ArmazenamentoJsonBiblioteca(new FileManager(caminhoArquivo)));
        var caminhoBiblioteca = System.IO.Path.GetFullPath(caminhoArquivo);
        var caminhoPreferencias = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(caminhoBiblioteca)!, "biblioteca.preferencias.json");
        if (caminhoPreferencias == caminhoBiblioteca) caminhoPreferencias += ".visualizacao.json";
        var preferencias = new ArmazenamentoPreferenciasBiblioteca(caminhoPreferencias);
        var caminhoColecoes = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(caminhoBiblioteca)!,
            System.IO.Path.GetFileNameWithoutExtension(caminhoBiblioteca) + ".colecoes.json");
        var caminhoAparencia = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(caminhoBiblioteca)!, "aparencia.preferencias.json");
        if (caminhoAparencia == caminhoBiblioteca) caminhoAparencia += ".visual.json";
        var aparencia = new BacklogManager.Aparencia.ServicoAparencia(new BacklogManager.Aparencia.ArmazenamentoAparenciaJson(caminhoAparencia));
        var colecoes = new ServicoColecoes(new ArmazenamentoColecoesJson(caminhoColecoes), biblioteca);
        if (!ConfiguracaoIgdb.TentarLer(out var clientId, out var token, lerVariavel))
            return new DependenciasJanela(biblioteca, null, null, preferencias, colecoes, aparencia);

        // O cliente produzido pela fábrica pertence a esta composição e será fechado pela janela.
        var cliente = criarCliente?.Invoke() ?? new HttpClient(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(5)
        });
        cliente.Timeout = TimeSpan.FromSeconds(15);
        var pesquisa = new PesquisaJogosIgdb(new IgdbService(cliente, clientId, token));
        return new DependenciasJanela(biblioteca, pesquisa, cliente, preferencias, colecoes, aparencia);
    }
}
