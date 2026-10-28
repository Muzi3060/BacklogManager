using BacklogManager.Data;
using BacklogManager.Models;
using BacklogManager.Services;

namespace BacklogManager.Tests.Fase2;

internal static class ApoioMultimidia
{
    internal static ObraCatalogo Catalogo(TipoMidia tipo) => tipo == TipoMidia.Jogo
        ? new ObraCatalogo { Tipo = tipo, Titulo = "Jogo de teste", Origem = "IGDB", IdentificadorExterno = "42" }
        : new ObraCatalogo { Tipo = tipo, Titulo = "Obra de teste (simulado)", Origem = "Simulado", IdentificadorExterno = $"simulado:{tipo}:1", Simulado = true, Generos = new[] { "Fantasia" }, CapaUrl = $"avares://BacklogManager/Assets/CapasSimuladas/{tipo}.svg" };

    internal static ItemBiblioteca Item(TipoMidia tipo, ProgressoPessoal? progresso = null) =>
        new(Catalogo(tipo), new DadosPessoaisBiblioteca { Categoria = "Fantasia", Status = "Na Fila", Nota = 4, Review = "Review", Progresso = progresso });

    internal static ItemBiblioteca Roundtrip(ItemBiblioteca item)
    {
        using var pasta = new PastaTeste();
        var armazenamento = new ArmazenamentoJsonBiblioteca(new FileManager(pasta.Arquivo));
        armazenamento.Carregar(); armazenamento.Salvar(new[] { item });
        return armazenamento.Carregar()[0];
    }
}
