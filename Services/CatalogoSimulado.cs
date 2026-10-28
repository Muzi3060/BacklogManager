using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BacklogManager.Contracts;
using BacklogManager.Models;

namespace BacklogManager.Services;

// Somente demonstração local; não cria HttpClient nem consulta um catálogo real.
public sealed class CatalogoSimulado : IPesquisaCatalogo
{
    private readonly IReadOnlyList<ObraCatalogo> _obras;
    public CatalogoSimulado(TipoMidia tipo)
    {
        if (!Enum.IsDefined(tipo) || tipo == TipoMidia.Jogo)
            throw new ArgumentOutOfRangeException(nameof(tipo), "Jogos continuam usando a IGDB.");
        var nomes = tipo switch
        {
            TipoMidia.Anime => new[] { "Jardim das Estrelas", "Correio das Nuvens", "Cidade do Amanhã" },
            TipoMidia.Serie => new[] { "Arquivo do Horizonte", "Pontes da Memória", "Estação Aurora" },
            TipoMidia.Filme => new[] { "Uma Janela para o Mar", "A Última Lanterna", "Retratos do Vento" },
            TipoMidia.Manga => new[] { "O Ateliê da Lua", "Mapas de Papel", "Crônicas do Farol" },
            _ => new[] { "A Biblioteca das Marés", "Cartas para o Futuro", "O Caminho das Sementes" }
        };
        _obras = nomes.Select((nome, indice) => new ObraCatalogo
        {
            Tipo = tipo,
            Titulo = nome + " (simulado)",
            Origem = "Simulado",
            IdentificadorExterno = $"simulado:{tipo}:{indice + 1}",
            Simulado = true,
            Generos = new[] { indice == 0 ? "Fantasia" : indice == 1 ? "Aventura" : "Drama" },
            CapaUrl = $"avares://BacklogManager/Assets/CapasSimuladas/{tipo}.svg"
        }).ToArray();
    }

    public Task<IReadOnlyList<ObraCatalogo>> BuscarAsync(string termo, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var texto = termo?.Trim() ?? string.Empty;
        IReadOnlyList<ObraCatalogo> resultados = texto.Length == 0 ? Array.Empty<ObraCatalogo>() :
            _obras.Where(obra => obra.Titulo.Contains(texto, StringComparison.OrdinalIgnoreCase)).ToArray();
        return Task.FromResult(resultados);
    }
}
