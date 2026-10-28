using BacklogManager.Models;
using BacklogManager.Services;
using Xunit;
using static BacklogManager.Tests.Fase1.ApoioArquitetura;

namespace BacklogManager.Tests.Fase1;

public sealed class ServicoBibliotecaTests
{
    [Fact(DisplayName = "CA01 | Carregar armazenamento vazio | Coleção carregada sem gravar")]
    public void CarregarVazio()
    {
        var (s, a) = Carregada();
        Assert.True(s.Carregada);
        Assert.Empty(s.Itens);
        Assert.Equal(0, a.Gravacoes);
    }
    [Fact(DisplayName = "CA02 | Carregar registros na ordem original | Todos os registros mantidos")]
    public void CarregarOrdem()
    {
        var (s, _) = Carregada(Jogo("Um"), Jogo("Dois"));
        Assert.Equal(new[] { "Um", "Dois" }, s.Itens.Select(i => i.Catalogo.Titulo));
    }
    [Fact(DisplayName = "CA03 | Erro de leitura | Estado permanece não carregado")]
    public void FalhaLeitura()
    {
        var a = new ArmazenamentoSimulado { FalhaLeitura = new IOException() };
        var s = new ServicoBiblioteca(a);
        Assert.Throws<IOException>(s.Carregar);
        Assert.False(s.Carregada);
        Assert.Empty(s.Itens);
    }
    [Fact(DisplayName = "CA04 | Falha ao recarregar | Gravações bloqueadas sem apagar a memória anterior")]
    public void FalhaRecarga()
    {
        var (s, a) = Carregada(Jogo());
        var anterior = s.Itens;
        a.FalhaLeitura = new IOException();
        Assert.Throws<IOException>(s.Carregar);
        Assert.Same(anterior, s.Itens);
        Assert.False(s.Carregada);
        Assert.Throws<InvalidOperationException>(() => s.Adicionar(Jogo("Novo")));
    }
    [Fact(DisplayName = "CA05 | Registro inválido na leitura | Coleção inteira recusada sem carga parcial")]
    public void RegistroInvalido()
    {
        var a = new ArmazenamentoSimulado { Registros = new[] { Jogo(), Jogo() with { Catalogo = new ObraCatalogo() } } };
        var s = new ServicoBiblioteca(a);
        Assert.Throws<InvalidDataException>(s.Carregar);
        Assert.False(s.Carregada);
        Assert.Empty(s.Itens);
    }
    [Fact(DisplayName = "CA06 | Recuperar leitura após erro | Nova carga válida libera operações")]
    public void RecuperarLeitura()
    {
        var a = new ArmazenamentoSimulado { FalhaLeitura = new IOException() };
        var s = new ServicoBiblioteca(a);
        Assert.Throws<IOException>(s.Carregar);
        a.FalhaLeitura = null;
        a.Registros = new[] { Jogo() };
        s.Carregar();
        Assert.True(s.Carregada);
        Assert.Single(s.Itens);
    }
    [Fact(DisplayName = "CA07 | Categorias e status antigos | Textos legados aceitos sem normalização")]
    public void TextosLegados()
    {
        var item = Jogo() with { Pessoal = new DadosPessoaisBiblioteca { Categoria = "Antiga", Status = "Antigo", Review = "", Nota = 0 } };
        var (s, _) = Carregada(item);
        Assert.Same(item, Assert.Single(s.Itens));
    }
    [Fact(DisplayName = "CA08 | Nota não finita na leitura | Coleção recusada")]
    public void NotaNaoFinita()
    {
        var item = Jogo();
        var a = new ArmazenamentoSimulado { Registros = new[] { item with { Pessoal = item.Pessoal with { Nota = double.NaN } } } };
        Assert.Throws<InvalidDataException>(() => new ServicoBiblioteca(a).Carregar());
    }
    [Fact(DisplayName = "CA09 | Tipo de mídia desconhecido | Coleção recusada sem interpretação inventada")]
    public void TipoDesconhecido()
    {
        var item = Jogo();
        var a = new ArmazenamentoSimulado { Registros = new[] { item with { Catalogo = item.Catalogo with { Tipo = (TipoMidia)999 } } } };
        Assert.Throws<InvalidDataException>(() => new ServicoBiblioteca(a).Carregar());
    }
    [Fact(DisplayName = "CA10 | Lista do armazenamento muda após carregar | Quantidade em memória isolada da lista externa")]
    public void ListaExterna()
    {
        var lista = new List<ItemBiblioteca> { Jogo() };
        var a = new ArmazenamentoSimulado { Registros = lista };
        var s = new ServicoBiblioteca(a);
        s.Carregar();
        lista.Clear();
        Assert.Single(s.Itens);
        Assert.Equal("memória de teste", s.DescricaoOrigem);
    }

    [Fact(DisplayName = "AD01 | Adicionar item válido | Persistência e memória recebem o item")]
    public void AdicionarValido()
    {
        var (s, a) = Carregada();
        var item = Jogo();
        s.Adicionar(item);
        Assert.Same(item, Assert.Single(s.Itens));
        Assert.Same(item, Assert.Single(a.Registros));
    }
    [Fact(DisplayName = "AD02 | Adicionar mesmo catálogo duas vezes | Dois registros sem deduplicação")]
    public void AdicionarDuplicado()
    {
        var (s, _) = Carregada(Jogo());
        s.Adicionar(Jogo());
        Assert.Equal(2, s.Itens.Count);
    }
    [Fact(DisplayName = "AD03 | Adicionar antes de carregar | Operação recusada sem gravar")]
    public void AdicionarSemCarga()
    {
        var a = new ArmazenamentoSimulado();
        Assert.Throws<InvalidOperationException>(() => new ServicoBiblioteca(a).Adicionar(Jogo()));
        Assert.Equal(0, a.Gravacoes);
    }
    [Fact(DisplayName = "AD04 | Adicionar título vazio | Item recusado antes da persistência")]
    public void AdicionarTituloVazio()
    {
        var (s, a) = Carregada();
        Assert.Throws<InvalidDataException>(() => s.Adicionar(Jogo() with { Catalogo = new ObraCatalogo { Titulo = " " } }));
        Assert.Equal(0, a.Gravacoes);
    }
    [Fact(DisplayName = "AD05 | Adicionar nota fora do intervalo | Memória e armazenamento preservados")]
    public void AdicionarNotaInvalida()
    {
        var (s, a) = Carregada();
        var item = Jogo();
        Assert.Throws<InvalidDataException>(() => s.Adicionar(item with { Pessoal = item.Pessoal with { Nota = 6 } }));
        Assert.Empty(s.Itens);
        Assert.Empty(a.Registros);
    }
    [Fact(DisplayName = "AD06 | Erro de IO ao adicionar | Coleção anterior mantida")]
    public void AdicionarFalhaIo()
    {
        var (s, a) = Carregada(Jogo());
        var anterior = s.Itens;
        a.FalhaGravacao = new IOException();
        Assert.Throws<IOException>(() => s.Adicionar(Jogo("Novo")));
        Assert.Same(anterior, s.Itens);
        Assert.Single(a.Registros);
    }
    [Fact(DisplayName = "AD07 | Sem permissão ao adicionar | Nenhum item aparece como salvo")]
    public void AdicionarSemPermissao()
    {
        var (s, a) = Carregada();
        a.FalhaGravacao = new UnauthorizedAccessException();
        Assert.Throws<UnauthorizedAccessException>(() => s.Adicionar(Jogo()));
        Assert.Empty(s.Itens);
        Assert.Equal(0, a.Gravacoes);
    }
    [Fact(DisplayName = "AD08 | Recuperação após falha ao adicionar | Nova tentativa adiciona uma única vez")]
    public void AdicionarRecuperacao()
    {
        var (s, a) = Carregada();
        a.FalhaGravacao = new IOException();
        var item = Jogo();
        Assert.Throws<IOException>(() => s.Adicionar(item));
        a.FalhaGravacao = null;
        s.Adicionar(item);
        Assert.Single(s.Itens);
        Assert.Equal(1, a.Gravacoes);
    }
    [Fact(DisplayName = "AD09 | Adições sucessivas | Ordem de inserção preservada")]
    public void AdicionarOrdem()
    {
        var (s, _) = Carregada(Jogo("Primeiro"));
        s.Adicionar(Jogo("Segundo"));
        s.Adicionar(Jogo("Terceiro"));
        Assert.Equal(new[] { "Primeiro", "Segundo", "Terceiro" }, s.Itens.Select(i => i.Catalogo.Titulo));
    }
    [Fact(DisplayName = "AD10 | Coleção exposta à interface | Lista somente leitura impede alteração sem salvar")]
    public void ListaSomenteLeitura()
    {
        var (s, a) = Carregada(Jogo());
        Assert.Throws<NotSupportedException>(() => ((IList<ItemBiblioteca>)s.Itens).Clear());
        Assert.Single(s.Itens);
        Assert.Equal(0, a.Gravacoes);
    }

    [Fact(DisplayName = "ED01 | Editar dados pessoais | Nova nota persistida sem alterar catálogo")]
    public void EditarPessoal()
    {
        var item = Jogo();
        var (s, a) = Carregada(item);
        s.Atualizar(item, item.Pessoal with { Nota = 2 });
        var salvo = Assert.Single(a.Registros);
        Assert.Equal(2, salvo.Pessoal.Nota);
        Assert.Same(item.Catalogo, salvo.Catalogo);
        Assert.Equal(4.5, item.Pessoal.Nota);
    }
    [Fact(DisplayName = "ED02 | Editar registro com metadados legados | ID legado preservado")]
    public void EditarIdLegado()
    {
        var item = Jogo() with { SteamAppIdLegado = 361855 };
        var (s, _) = Carregada(item);
        s.Atualizar(item, item.Pessoal with { Review = "Nova" });
        Assert.Equal(361855, Assert.Single(s.Itens).SteamAppIdLegado);
        Assert.Equal("42", Assert.Single(s.Itens).Catalogo.IdentificadorExterno);
    }
    [Fact(DisplayName = "ED03 | Editar registro externo | Operação recusada sem gravar")]
    public void EditarExterno()
    {
        var (s, a) = Carregada(Jogo());
        Assert.Throws<InvalidOperationException>(() => s.Atualizar(Jogo("Externo"), new DadosPessoaisBiblioteca()));
        Assert.Equal(0, a.Gravacoes);
    }
    [Fact(DisplayName = "ED04 | Dois registros com valores iguais | Somente instância selecionada atualizada")]
    public void EditarDuplicados()
    {
        var um = Jogo(); var dois = Jogo();
        var (s, _) = Carregada(um, dois);
        s.Atualizar(dois, dois.Pessoal with { Nota = 1 });
        Assert.Same(um, s.Itens[0]);
        Assert.Equal(1, s.Itens[1].Pessoal.Nota);
    }
    [Fact(DisplayName = "ED05 | Nota negativa na edição | Persistência não chamada")]
    public void EditarInvalido()
    {
        var item = Jogo(); var (s, a) = Carregada(item);
        Assert.Throws<InvalidDataException>(() => s.Atualizar(item, item.Pessoal with { Nota = -1 }));
        Assert.Equal(0, a.Gravacoes);
    }
    [Fact(DisplayName = "ED06 | IO ao editar | Registro original permanece na memória e no armazenamento")]
    public void EditarFalhaIo()
    {
        var item = Jogo(); var (s, a) = Carregada(item);
        a.FalhaGravacao = new IOException();
        Assert.Throws<IOException>(() => s.Atualizar(item, item.Pessoal with { Nota = 1 }));
        Assert.Same(item, Assert.Single(s.Itens));
        Assert.Same(item, Assert.Single(a.Registros));
    }
    [Fact(DisplayName = "ED07 | Sem permissão ao editar | Nota original não alterada")]
    public void EditarSemPermissao()
    {
        var item = Jogo(); var (s, a) = Carregada(item);
        a.FalhaGravacao = new UnauthorizedAccessException();
        Assert.Throws<UnauthorizedAccessException>(() => s.Atualizar(item, item.Pessoal with { Nota = 1 }));
        Assert.Equal(4.5, Assert.Single(s.Itens).Pessoal.Nota);
    }
    [Fact(DisplayName = "ED08 | Repetir edição após falha | Alteração aplicada uma única vez")]
    public void EditarRecuperacao()
    {
        var item = Jogo(); var (s, a) = Carregada(item);
        a.FalhaGravacao = new IOException();
        Assert.Throws<IOException>(() => s.Atualizar(item, item.Pessoal with { Nota = 1 }));
        a.FalhaGravacao = null;
        s.Atualizar(item, item.Pessoal with { Nota = 1 });
        Assert.Equal(1, Assert.Single(s.Itens).Pessoal.Nota);
        Assert.Equal(1, a.Gravacoes);
    }
    [Fact(DisplayName = "ED09 | Editar item removido | Nenhum outro registro é substituído")]
    public void EditarRemovido()
    {
        var item = Jogo(); var (s, a) = Carregada(item, Jogo("Outro"));
        s.Remover(item);
        Assert.Throws<InvalidOperationException>(() => s.Atualizar(item, item.Pessoal));
        Assert.Equal("Outro", Assert.Single(s.Itens).Catalogo.Titulo);
        Assert.Equal(1, a.Gravacoes);
    }
    [Fact(DisplayName = "ED10 | Editar review legada vazia | Valor vazio preservado sem padrão imposto pelo serviço")]
    public void EditarReviewVazia()
    {
        var item = Jogo(); var (s, _) = Carregada(item);
        s.Atualizar(item, item.Pessoal with { Review = "" });
        Assert.Equal("", Assert.Single(s.Itens).Pessoal.Review);
    }

    [Fact(DisplayName = "RM01 | Remover registro válido | Memória e armazenamento ficam sem o item")]
    public void RemoverValido()
    {
        var item = Jogo(); var (s, a) = Carregada(item);
        s.Remover(item);
        Assert.Empty(s.Itens); Assert.Empty(a.Registros);
    }
    [Fact(DisplayName = "RM02 | Remover registro externo | Coleção preservada sem gravar")]
    public void RemoverExterno()
    {
        var (s, a) = Carregada(Jogo());
        Assert.Throws<InvalidOperationException>(() => s.Remover(Jogo("Externo")));
        Assert.Single(s.Itens); Assert.Equal(0, a.Gravacoes);
    }
    [Fact(DisplayName = "RM03 | Remover segunda instância de registros iguais | Primeira instância preservada")]
    public void RemoverDuplicado()
    {
        var um = Jogo(); var dois = Jogo(); var (s, _) = Carregada(um, dois);
        s.Remover(dois);
        Assert.Same(um, Assert.Single(s.Itens));
    }
    [Fact(DisplayName = "RM04 | Remover com mesmo ID e títulos diferentes | Apenas registro escolhido removido")]
    public void RemoverMesmoId()
    {
        var um = Jogo("Um"); var dois = Jogo("Dois"); var (s, _) = Carregada(um, dois);
        s.Remover(um);
        Assert.Same(dois, Assert.Single(s.Itens));
    }
    [Fact(DisplayName = "RM05 | IO ao remover | Registro não desaparece da memória")]
    public void RemoverFalhaIo()
    {
        var item = Jogo(); var (s, a) = Carregada(item); a.FalhaGravacao = new IOException();
        Assert.Throws<IOException>(() => s.Remover(item));
        Assert.Same(item, Assert.Single(s.Itens)); Assert.Same(item, Assert.Single(a.Registros));
    }
    [Fact(DisplayName = "RM06 | Sem permissão ao remover | Armazenamento não alterado")]
    public void RemoverSemPermissao()
    {
        var item = Jogo(); var (s, a) = Carregada(item); a.FalhaGravacao = new UnauthorizedAccessException();
        Assert.Throws<UnauthorizedAccessException>(() => s.Remover(item));
        Assert.Single(s.Itens); Assert.Equal(0, a.Gravacoes);
    }
    [Fact(DisplayName = "RM07 | Repetir remoção após falha | Remoção confirmada uma única vez")]
    public void RemoverRecuperacao()
    {
        var item = Jogo(); var (s, a) = Carregada(item); a.FalhaGravacao = new IOException();
        Assert.Throws<IOException>(() => s.Remover(item)); a.FalhaGravacao = null; s.Remover(item);
        Assert.Empty(s.Itens); Assert.Equal(1, a.Gravacoes);
    }
    [Fact(DisplayName = "RM08 | Remover o mesmo registro novamente | Segunda operação recusada sem novo salvamento")]
    public void RemoverDuasVezes()
    {
        var item = Jogo(); var (s, a) = Carregada(item); s.Remover(item);
        Assert.Throws<InvalidOperationException>(() => s.Remover(item)); Assert.Equal(1, a.Gravacoes);
    }
    [Fact(DisplayName = "RM09 | Remover registro central | Ordem dos sobreviventes preservada")]
    public void RemoverMeio()
    {
        var um = Jogo("Um"); var dois = Jogo("Dois"); var tres = Jogo("Três"); var (s, _) = Carregada(um, dois, tres);
        s.Remover(dois); Assert.Equal(new[] { "Um", "Três" }, s.Itens.Select(i => i.Catalogo.Titulo));
    }
    [Fact(DisplayName = "RM10 | Remover após recarga falha | Gravação bloqueada e estado anterior mantido")]
    public void RemoverCargaFalha()
    {
        var item = Jogo(); var (s, a) = Carregada(item); a.FalhaLeitura = new IOException();
        Assert.Throws<IOException>(s.Carregar);
        Assert.Throws<InvalidOperationException>(() => s.Remover(item));
        Assert.Single(s.Itens); Assert.Equal(0, a.Gravacoes);
    }
}
