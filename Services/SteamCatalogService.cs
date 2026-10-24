using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BacklogManager.Models;

namespace BacklogManager.Services;

public class SteamCatalogService
{
    public async Task<List<SteamGame>> BuscarJogosAsync(string termo)
    {
        var listaJogos = new List<SteamGame>
        {
            new SteamGame(1010, "cyberpunk 2077"),
            new SteamGame(1020, "the witcher 3"),
            new SteamGame(1030, "Persona 5 Royal"),
            new SteamGame(1023710, "Final fantasy Resonance"),
            new SteamGame(1040, "Persona 4 Golden"),
            new SteamGame(1050, "Persona 3 Portable"),
        };
        
        var jogosEncontrado = listaJogos.Where(jogo => jogo.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase)).ToList();
        await Task.Delay(1000);

        return jogosEncontrado;
    }

    public async Task Teste()
    {
        var servico = new SteamCatalogService();
        var jogos = await servico.BuscarJogosAsync("Persona");
        Console.WriteLine($"Jogos encontrados: {jogos.Count}");
    }
}