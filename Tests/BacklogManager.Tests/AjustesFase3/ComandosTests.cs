using BacklogManager.Models;
using BacklogManager.Tests.Fase1;
using BacklogManager.Tests.Fase2;
using Xunit;
namespace BacklogManager.Tests.AjustesFase3;

public sealed class ComandosTests
{
    [Fact(DisplayName = "CS01 | Adição direta de jogo sem IGDB | Serviço bloqueia sem alterar memória ou arquivo")]
    public void JogoSemId() { var (s, a) = ApoioArquitetura.Carregada(); var i = ApoioArquitetura.Jogo() with { Catalogo = new ObraCatalogo { Titulo = "Manual" } }; Assert.Throws<InvalidDataException>(() => s.Adicionar(i)); Assert.Empty(s.Itens); Assert.Equal(0, a.Gravacoes); }
    [Fact(DisplayName = "CS02 | Adição direta de página zero | Serviço aplica a regra positiva")]
    public void Pagina() { var (s, _) = ApoioArquitetura.Carregada(); Assert.Throws<InvalidDataException>(() => s.Adicionar(ApoioMultimidia.Item(TipoMidia.Livro, new() { Pagina = 0 }))); }
    [Fact(DisplayName = "CS03 | Adição direta de capítulo zero | Serviço aplica a regra positiva")]
    public void Capitulo() { var (s, _) = ApoioArquitetura.Carregada(); Assert.Throws<InvalidDataException>(() => s.Adicionar(ApoioMultimidia.Item(TipoMidia.Manga, new() { Capitulo = 0 }))); }
    [Fact(DisplayName = "CS04 | Adição direta de volume zero | Serviço aplica a regra positiva")]
    public void Volume() { var (s, _) = ApoioArquitetura.Carregada(); Assert.Throws<InvalidDataException>(() => s.Adicionar(ApoioMultimidia.Item(TipoMidia.Manga, new() { Volume = 0 }))); }
    [Fact(DisplayName = "CS05 | Atualização para página zero | Serviço rejeita e conserva instância anterior")]
    public void EditarPagina() { var i = ApoioMultimidia.Item(TipoMidia.Livro, new() { Pagina = 1 }); var (s, _) = ApoioArquitetura.Carregada(i); Assert.Throws<InvalidDataException>(() => s.Atualizar(i, i.Pessoal with { Progresso = new() { Pagina = 0 } })); Assert.Same(i, s.Itens[0]); }
    [Fact(DisplayName = "CS06 | Atualização para capítulo zero | Serviço rejeita sem gravação")]
    public void EditarCapitulo() { var i = ApoioMultimidia.Item(TipoMidia.Manga); var (s, a) = ApoioArquitetura.Carregada(i); Assert.Throws<InvalidDataException>(() => s.Atualizar(i, i.Pessoal with { Progresso = new() { Capitulo = 0 } })); Assert.Equal(0, a.Gravacoes); }
    [Fact(DisplayName = "CS07 | Atualização para volume negativo | Serviço rejeita sem gravação")]
    public void EditarVolume() { var i = ApoioMultimidia.Item(TipoMidia.Manga); var (s, a) = ApoioArquitetura.Carregada(i); Assert.Throws<InvalidDataException>(() => s.Atualizar(i, i.Pessoal with { Progresso = new() { Volume = -1 } })); Assert.Equal(0, a.Gravacoes); }
    [Fact(DisplayName = "CS08 | Nota em legado com página zero | Atualização local preserva posição antiga")]
    public void Legado() { var i = ApoioMultimidia.Item(TipoMidia.Livro, new() { Pagina = 0 }); var (s, _) = ApoioArquitetura.Carregada(i); s.Atualizar(i, i.Pessoal with { Nota = 5 }); Assert.Equal(0, s.Itens[0].Pessoal.Progresso!.Pagina); }
    [Fact(DisplayName = "CS09 | Nova mídia sem progresso | Cadastro local simulado continua aceito")]
    public void SemProgresso() { var (s, _) = ApoioArquitetura.Carregada(); s.Adicionar(ApoioMultimidia.Item(TipoMidia.Livro)); Assert.Null(Assert.Single(s.Itens).Pessoal.Progresso); }
    [Fact(DisplayName = "CS10 | Quantidade zero de episódios assistidos | Contagem continua aceitando zero")]
    public void Episodios() { var (s, _) = ApoioArquitetura.Carregada(); s.Adicionar(ApoioMultimidia.Item(TipoMidia.Anime, new() { EpisodiosAssistidos = 0 })); Assert.Equal(0, Assert.Single(s.Itens).Pessoal.Progresso!.EpisodiosAssistidos); }
}
