using System.Text.Json;
using BacklogManager.Data;
using BacklogManager.Models;
using Xunit;

namespace BacklogManager.Tests;

public sealed class PersistenciaTests : IDisposable
{
    private readonly PastaTeste _pasta = new();
    private FileManager Arquivos => new(_pasta.Arquivo);
    public void Dispose() => _pasta.Dispose();
    private FileManager Preparar(string? json = null)
    {
        File.WriteAllText(_pasta.Arquivo, json ?? JsonSerializer.Serialize(new[] { PastaTeste.Jogo() }));
        var arquivos = Arquivos;
        arquivos.Carregar();
        return arquivos;
    }

    [Fact(DisplayName = "P01 | Arquivo inexistente | Lista vazia sem criar arquivo")]
    public void Ausente() { Assert.Empty(Arquivos.Carregar()); Assert.False(File.Exists(_pasta.Arquivo)); }

    [Fact(DisplayName = "P02 | Lista vazia válida | Carregamento aceito")]
    public void ListaVazia() { File.WriteAllText(_pasta.Arquivo, "[]"); Assert.Empty(Arquivos.Carregar()); }

    [Fact(DisplayName = "P03 | JSON legado válido | Todos os campos preservados")]
    public void Legado()
    {
        var a = Preparar(); var item = Assert.Single(a.Carregar());
        Assert.Equal("Jogo simulado", item.Titulo); Assert.Equal("RPG", item.Categoria);
        Assert.Equal("Na Fila", item.Status); Assert.Equal(4.5, item.Nota);
        Assert.Equal("Review simulada", item.Review); Assert.Equal(1010, item.SteamAppId); Assert.Null(item.IgdbId);
    }

    [Theory]
    [InlineData("P04", "")]
    [InlineData("P05", "{")]
    [InlineData("P06", "null")]
    [InlineData("P07", "{}")]
    [InlineData("P08", "[null]")]
    [InlineData("P09", "[{\"Titulo\":null}]")]
    [InlineData("P10", "[{\"Titulo\":\"Jogo\",\"Categoria\":null,\"Status\":\"Na Fila\"}]")]
    [InlineData("P11", "[{\"Titulo\":\"Jogo\",\"Categoria\":\"RPG\",\"Status\":null}]")]
    [InlineData("P12", "[{\"Titulo\":\"Jogo\",\"Categoria\":\"RPG\",\"Status\":\"Na Fila\",\"Nota\":6}]")]
    [InlineData("P13", "[{\"Titulo\":\"Jogo\",\"Categoria\":\"RPG\",\"Status\":\"Na Fila\",\"Review\":null}]")]
    public void DadosInvalidosBloqueiamGravacaoSemAlterarOriginal(string id, string json)
    {
        _ = id; File.WriteAllText(_pasta.Arquivo, json); var a = Arquivos;
        Assert.Throws<InvalidDataException>(() => a.Carregar());
        Assert.Throws<InvalidOperationException>(() => a.Salvar(new[] { PastaTeste.Jogo() }));
        Assert.Equal(json, File.ReadAllText(_pasta.Arquivo));
    }

    [Fact(DisplayName = "P14 | Primeira gravação | Arquivo válido e recarregável")]
    public void PrimeiroArquivo()
    {
        var a = Arquivos; a.Carregar(); a.Salvar(new[] { PastaTeste.Jogo() });
        Assert.Single(Arquivos.Carregar()); Assert.Empty(Directory.GetFiles(_pasta.Pasta, "*.tmp"));
    }

    [Fact(DisplayName = "P15 | Atualização de arquivo | Backup byte a byte da versão anterior")]
    public void Backup()
    {
        var a = Preparar(); var original = File.ReadAllBytes(_pasta.Arquivo);
        a.Salvar(new[] { PastaTeste.Jogo("Atualizado") });
        Assert.Equal(original, File.ReadAllBytes(_pasta.Arquivo + ".bak"));
        Assert.Equal("Atualizado", Assert.Single(Arquivos.Carregar()).Titulo);
    }

    [Fact(DisplayName = "P16 | Gravações sucessivas | Backup acompanha última versão válida")]
    public void Sucessivas()
    {
        var a = Preparar(); a.Salvar(new[] { PastaTeste.Jogo("Segunda") });
        var segunda = File.ReadAllBytes(_pasta.Arquivo); a.Salvar(new[] { PastaTeste.Jogo("Terceira") });
        Assert.Equal(segunda, File.ReadAllBytes(_pasta.Arquivo + ".bak"));
        Assert.Equal("Terceira", Assert.Single(Arquivos.Carregar()).Titulo);
    }

    [Fact(DisplayName = "P17 | Arquivo alterado externamente | Gravação recusada e versão externa intacta")]
    public void Conflito()
    {
        var a = Preparar(); File.WriteAllText(_pasta.Arquivo, "[]");
        Assert.Throws<ArquivoAlteradoException>(() => a.Salvar(new[] { PastaTeste.Jogo() }));
        Assert.Equal("[]", File.ReadAllText(_pasta.Arquivo));
    }

    [Fact(DisplayName = "P18 | Arquivo removido externamente | Nenhuma recriação silenciosa")]
    public void Removido()
    {
        var a = Preparar(); File.Delete(_pasta.Arquivo);
        Assert.Throws<ArquivoAlteradoException>(() => a.Salvar(new[] { PastaTeste.Jogo() }));
        Assert.False(File.Exists(_pasta.Arquivo));
    }

    [Fact(DisplayName = "P19 | Arquivo criado após leitura vazia | Arquivo externo preservado")]
    public void CriadoFora()
    {
        var a = Arquivos; a.Carregar(); File.WriteAllText(_pasta.Arquivo, "[]");
        Assert.Throws<ArquivoAlteradoException>(() => a.Salvar(new[] { PastaTeste.Jogo() })); Assert.Equal("[]", File.ReadAllText(_pasta.Arquivo));
    }

    [Fact(DisplayName = "P20 | Gravação sem leitura anterior | Operação recusada sem criar arquivo")]
    public void SemLeitura()
    {
        Assert.Throws<InvalidOperationException>(() => Arquivos.Salvar(new[] { PastaTeste.Jogo() }));
        Assert.False(File.Exists(_pasta.Arquivo));
    }

    [Fact(DisplayName = "P21 | Registro inválido ao salvar | Arquivo e backup não alterados")]
    public void SalvarInvalido()
    {
        var a = Preparar(); var original = File.ReadAllBytes(_pasta.Arquivo);
        var invalido = PastaTeste.Jogo(); invalido.Nota = double.NaN;
        Assert.Throws<InvalidDataException>(() => a.Salvar(new[] { invalido }));
        Assert.Equal(original, File.ReadAllBytes(_pasta.Arquivo)); Assert.False(File.Exists(_pasta.Arquivo + ".bak"));
    }

    [Fact(DisplayName = "P22 | Bloqueio por outra instância | Falha segura sem modificar coleção")]
    public void Bloqueio()
    {
        var a = Preparar(); var original = File.ReadAllBytes(_pasta.Arquivo);
        using var bloqueio = new FileStream(_pasta.Arquivo + ".lock", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        Assert.Throws<IOException>(() => a.Salvar(new[] { PastaTeste.Jogo("Outro") }));
        Assert.Equal(original, File.ReadAllBytes(_pasta.Arquivo));
    }

    [Fact(DisplayName = "P23 | Falha na substituição do backup | Original intacto e temporário removido")]
    public void FalhaBackup()
    {
        var a = Preparar(); var original = File.ReadAllBytes(_pasta.Arquivo); Directory.CreateDirectory(_pasta.Arquivo + ".bak");
        Assert.ThrowsAny<IOException>(() => a.Salvar(new[] { PastaTeste.Jogo("Outro") }));
        Assert.Equal(original, File.ReadAllBytes(_pasta.Arquivo)); Assert.Empty(Directory.GetFiles(_pasta.Pasta, "*.tmp"));
    }

    [Fact(DisplayName = "P24 | Recuperação após falha de gravação | Nova tentativa salva corretamente")]
    public void Recuperacao()
    {
        var a = Preparar(); Directory.CreateDirectory(_pasta.Arquivo + ".bak");
        Assert.ThrowsAny<IOException>(() => a.Salvar(new[] { PastaTeste.Jogo("Outro") }));
        Directory.Delete(_pasta.Arquivo + ".bak"); a.Salvar(new[] { PastaTeste.Jogo("Recuperado") });
        Assert.Equal("Recuperado", Assert.Single(Arquivos.Carregar()).Titulo);
    }

    [Fact(DisplayName = "P25 | Campos desconhecidos | Dados adicionais preservados no roundtrip")]
    public void CamposAdicionais()
    {
        var jogo = PastaTeste.Jogo(); jogo.DadosAdicionais = new() { ["campoFuturo"] = JsonSerializer.SerializeToElement(new { valor = 42 }) };
        var a = Arquivos; a.Carregar(); a.Salvar(new[] { jogo });
        var carregado = Assert.Single(a.Carregar()); a.Salvar(new[] { carregado });
        Assert.Equal(42, Assert.Single(Arquivos.Carregar()).DadosAdicionais!["campoFuturo"].GetProperty("valor").GetInt32());
    }

    [Fact(DisplayName = "P26 | Onze registros simulados e IDs repetidos | Quantidade e valores integralmente preservados")]
    public void OnzeRegistros()
    {
        var jogos = Enumerable.Range(1, 11).Select(n => PastaTeste.Jogo($"Jogo {n}")).ToList();
        var a = Arquivos; a.Carregar(); a.Salvar(jogos); var lidos = a.Carregar();
        Assert.Equal(JsonSerializer.Serialize(jogos), JsonSerializer.Serialize(lidos)); Assert.Equal(11, lidos.Count);
    }

    [Fact(DisplayName = "P27 | Unicode e review com quebras | Texto preservado após gravação")]
    public void Unicode()
    {
        var jogo = PastaTeste.Jogo("Ação 日本語 🎮"); jogo.Review = "Linha 1\nLinha 2 ç";
        var a = Arquivos; a.Carregar(); a.Salvar(new[] { jogo }); var lido = Assert.Single(a.Carregar());
        Assert.Equal(jogo.Titulo, lido.Titulo); Assert.Equal(jogo.Review, lido.Review);
    }

    [Fact(DisplayName = "P28 | Caminho padrão | Diretório de trabalho original mantido")]
    public void CaminhoPadrao() => Assert.Equal(Path.GetFullPath("backlog.json"), new FileManager().CaminhoArquivo);

    [Fact(DisplayName = "P29 | Diretório de destino ausente | Falha sem criar pastas implicitamente")]
    public void PastaAusente()
    {
        var a = new FileManager(Path.Combine(_pasta.Pasta, "ausente", "backlog.json")); a.Carregar();
        Assert.Throws<DirectoryNotFoundException>(() => a.Salvar(new[] { PastaTeste.Jogo() }));
        Assert.False(Directory.Exists(Path.GetDirectoryName(a.CaminhoArquivo)));
    }

    [Fact(DisplayName = "P30 | Caminho ocupado por diretório | Erro de leitura e gravação bloqueada")]
    public void CaminhoDiretorio()
    {
        Directory.CreateDirectory(_pasta.Arquivo); var a = Arquivos;
        Assert.Throws<UnauthorizedAccessException>(() => a.Carregar());
        Assert.Throws<InvalidOperationException>(() => a.Salvar(new[] { PastaTeste.Jogo() }));
    }

    [Fact(DisplayName = "P31 | Duas janelas com versões diferentes | Segunda janela não sobrescreve a primeira")]
    public void DuasInstancias()
    {
        var primeira = Preparar(); var segunda = Arquivos; segunda.Carregar();
        primeira.Salvar(new[] { PastaTeste.Jogo("Primeira") });
        Assert.Throws<ArquivoAlteradoException>(() => segunda.Salvar(new[] { PastaTeste.Jogo("Segunda") }));
        Assert.Equal("Primeira", Assert.Single(Arquivos.Carregar()).Titulo);
    }

    [Fact(DisplayName = "P32 | Leitura falha após leitura válida | Autorização anterior para gravar é revogada")]
    public void RecargaInvalida()
    {
        var a = Preparar(); File.WriteAllText(_pasta.Arquivo, "null");
        Assert.Throws<InvalidDataException>(() => a.Carregar());
        Assert.Throws<InvalidOperationException>(() => a.Salvar(new[] { PastaTeste.Jogo() }));
        Assert.Equal("null", File.ReadAllText(_pasta.Arquivo));
    }
    [Fact(DisplayName = "P33 | JSON UTF-8 com BOM | Carregamento compatível sem alterar bytes originais")]
    public void Utf8Bom()
    {
        File.WriteAllText(_pasta.Arquivo, JsonSerializer.Serialize(new[] { PastaTeste.Jogo() }), new System.Text.UTF8Encoding(true));
        var bytes = File.ReadAllBytes(_pasta.Arquivo); Assert.Single(Arquivos.Carregar()); Assert.Equal(bytes, File.ReadAllBytes(_pasta.Arquivo));
    }
    [Fact(DisplayName = "P34 | JSON UTF-16 com BOM | Codificação reconhecida como na leitura anterior")]
    public void Utf16Bom()
    {
        File.WriteAllText(_pasta.Arquivo, JsonSerializer.Serialize(new[] { PastaTeste.Jogo() }), System.Text.Encoding.Unicode);
        Assert.Single(Arquivos.Carregar());
    }

}
