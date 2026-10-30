using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BacklogManager.Models;

namespace BacklogManager.Services;

public sealed class IgdbService
{
    private readonly HttpClient _httpClient;
    private readonly string _clientId;
    private readonly string _accessToken;
    private readonly bool _credenciaisBeta;

    // Quem fornece o HttpClient continua responsável por seu descarte.
    public IgdbService(HttpClient httpClient, string clientId, string accessToken, bool credenciaisBeta = false)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        if (!ConfiguracaoIgdb.CredencialValida(clientId) || !ConfiguracaoIgdb.CredencialValida(accessToken))
            throw new ArgumentException("As credenciais locais da IGDB são inválidas.");
        _httpClient = httpClient;
        _clientId = clientId;
        _accessToken = accessToken;
        _credenciaisBeta = credenciaisBeta;
    }

    public async Task<List<IgdbGame>> BuscarJogosAsync(string gameName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var termo = gameName?.Trim() ?? string.Empty;
        if (termo.Length == 0) return new List<IgdbGame>();
        if (termo.Length > 200 || termo.Any(char.IsControl))
            throw new PesquisaIgdbException("Use um título de até 200 caracteres, sem quebras de linha ou caracteres de controle.");

        // Escapa o texto dentro da string APICalypse, sem permitir novas instruções.
        var escapado = termo.Replace("\\", "\\\\").Replace("\"", "\\\"");
        using var requisicao = new HttpRequestMessage(HttpMethod.Post, "https://api.igdb.com/v4/games");
        requisicao.Headers.Add("Client-ID", _clientId);
        requisicao.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        requisicao.Content = new StringContent($"search \"{escapado}\"; fields id,name,cover.image_id,cover.url,genres.name,platforms.name; limit 15;", Encoding.UTF8, "text/plain");

        try
        {
            using var resposta = await _httpClient.SendAsync(requisicao, cancellationToken);
            if (!resposta.IsSuccessStatusCode)
                throw new PesquisaIgdbException(resposta.StatusCode switch
                {
                    HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden =>
                        _credenciaisBeta ? "A IGDB recusou a autenticação. O token da beta pode ter expirado. Verifique se há uma nova versão em Configurações. Sua biblioteca local continua disponível." :
                        "A IGDB recusou a autenticação. Confira o Client ID e o token na configuração local.",
                    HttpStatusCode.TooManyRequests =>
                        "O limite de buscas da IGDB foi atingido. Aguarde um pouco antes de tentar novamente.",
                    _ when (int)resposta.StatusCode >= 500 =>
                        "A IGDB está temporariamente indisponível. Tente novamente mais tarde.",
                    _ => $"A IGDB não pôde concluir a pesquisa (HTTP {(int)resposta.StatusCode})."
                });

            var json = await resposta.Content.ReadAsStringAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            var jogos = JsonSerializer.Deserialize<List<IgdbGame>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (jogos == null || jogos.Any(j => j == null || j.Id <= 0 || string.IsNullOrWhiteSpace(j.Name)))
                throw new PesquisaIgdbException("A IGDB retornou resultados inválidos. Tente novamente.");
            return jogos;
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            throw new PesquisaIgdbException("A pesquisa demorou demais. Confira sua conexão e tente novamente.");
        }
        catch (HttpRequestException)
        {
            throw new PesquisaIgdbException("Não foi possível conectar à IGDB. Confira sua conexão e tente novamente.");
        }
        catch (JsonException)
        {
            throw new PesquisaIgdbException("A IGDB retornou uma resposta inválida. Tente novamente.");
        }
    }
}

public sealed class PesquisaIgdbException : Exception
{
    public PesquisaIgdbException(string mensagem) : base(mensagem) { }
}
