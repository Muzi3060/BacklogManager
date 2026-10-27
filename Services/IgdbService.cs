using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BacklogManager.Models;
using System.Text.Json;

namespace BacklogManager.Services;

public class IgdbService
{
    private readonly HttpClient _httpClient;

    public IgdbService(HttpClient httpClient, string clientId, string accessToken)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("Client-ID", clientId);
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
    }

    public async Task<List<IgdbGame>> BuscarJogosAsync(string gameName)
    {
        
        /*
         * 1. Monta a consulta APICalypse.
         * 2. Prepara o corpo da requisição.
         * 3. Envia o POST.
         * 4. Recebe a resposta HTTP.
         * 5. Lê o JSON.
         * 6. Converte o JSON em uma lista de IgdbGame.
         * 7. Retorna essa lista.
         */
        
        
        var query = $"search \"{gameName}\"; fields id,name; limit 15;";
        var conteudo = new StringContent(query, System.Text.Encoding.UTF8, "text/plain"); // corpo que enviamos para a igdb, passando a query que montamos, codificando em UTF8 e dizendo que é do tipo text/plain
        
        var resposta = await _httpClient.PostAsync("https://api.igdb.com/v4/games", conteudo);
        
        resposta.EnsureSuccessStatusCode();

        var json = await resposta.Content.ReadAsStringAsync(); // corpo que recebemos da igdb

        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        
        var jogos = JsonSerializer.Deserialize<List<IgdbGame>>(json, options);

        return jogos ?? new List<IgdbGame>(); // se jogos não for nulo, retorne a lista de jogos, se não retorne uma lista vazia de igdbGame
    }
}