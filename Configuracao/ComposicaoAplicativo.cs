using System;
using System.Net.Http;
using BacklogManager.Contracts;
using BacklogManager.Backup;
using BacklogManager.Data;
using BacklogManager.Services;

namespace BacklogManager.Configuracao;

internal sealed record DependenciasJanela(ServicoBiblioteca Biblioteca, IPesquisaCatalogo? Pesquisa, IDisposable? RecursoPesquisa,
    ArmazenamentoPreferenciasBiblioteca? Preferencias = null, ServicoColecoes? Colecoes = null, BacklogManager.Aparencia.ServicoAparencia? Aparencia = null, ServicoBackupLocal? Backup = null);

internal static class ComposicaoAplicativo
{
    internal static DependenciasJanela Criar(string caminhoArquivo = "backlog.json",
        Func<string, string?>? lerVariavel = null, Func<HttpClient>? criarCliente = null)
    {
        var caminhos = new CaminhosDadosLocais(caminhoArquivo);
        var backup = new ServicoBackupLocal(caminhos);
        backup.Recuperar(); // Sempre antes dos leitores e dos salvamentos automáticos de preferências.
        var arquivoBiblioteca = new FileManager(caminhos.Arquivos["Biblioteca"]);
        var arquivoColecoes = new ArmazenamentoColecoesJson(caminhos.Arquivos["Colecoes"]);
        var biblioteca = new ServicoBiblioteca(new ArmazenamentoJsonBiblioteca(arquivoBiblioteca));
        var preferencias = new ArmazenamentoPreferenciasBiblioteca(caminhos.Arquivos["Visualizacao"]);
        var aparencia = new BacklogManager.Aparencia.ServicoAparencia(new BacklogManager.Aparencia.ArmazenamentoAparenciaJson(caminhos.Arquivos["Aparencia"]));
        var colecoes = new ServicoColecoes(arquivoColecoes, biblioteca);
        backup.ConferirEstadoCarregado = () => { arquivoBiblioteca.ConferirEstadoAtual(); arquivoColecoes.ConferirEstadoAtual(); };
        var credenciais = ProvedorCredenciaisIgdb.Obter(lerVariavel);
        if (credenciais == null)
            return new DependenciasJanela(biblioteca, null, null, preferencias, colecoes, aparencia, backup);

        // O cliente produzido pela fábrica pertence a esta composição e será fechado pela janela.
        var cliente = criarCliente?.Invoke() ?? new HttpClient(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(5)
        });
        cliente.Timeout = TimeSpan.FromSeconds(15);
        var pesquisa = new PesquisaJogosIgdb(new IgdbService(cliente, credenciais.ClientId, credenciais.AccessToken, credenciais.DaBeta));
        return new DependenciasJanela(biblioteca, pesquisa, cliente, preferencias, colecoes, aparencia, backup);
    }
}
