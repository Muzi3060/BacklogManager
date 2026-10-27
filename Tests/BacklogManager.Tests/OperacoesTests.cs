using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Xunit;

namespace BacklogManager.Tests;

public sealed class OperacoesTests
{
    [AvaloniaFact(DisplayName = "A01 | Adicionar seleção IGDB | Novo ID correto e SteamAppId nulo")]
    public void Adicionar()
    {
        using var t = new TelaTeste(); t.Selecionar(); t.Clicar("BtnAdicionar"); var salvo = Assert.Single(t.Salvos);
        Assert.Equal(42, salvo.IgdbId); Assert.Null(salvo.SteamAppId); Assert.Equal("Jogo IGDB simulado", salvo.Titulo);
        Assert.Single(t.Exibidos); Assert.Contains("adicionado", t.Aviso);
    }
    [AvaloniaFact(DisplayName = "A02 | Adicionar sem seleção | Nenhum arquivo criado")]
    public void SemSelecao() { using var t = new TelaTeste(); t.Clicar("BtnAdicionar"); Assert.False(File.Exists(t.Pasta.Arquivo)); Assert.Empty(t.Exibidos); }
    [AvaloniaFact(DisplayName = "A03 | Seleção desfeita | Cadastro recusado")]
    public void SelecaoDesfeita() { using var t = new TelaTeste(); t.Selecionar(); t.Campo<ListBox>("ListaSugestoes").SelectedItem = null; t.Clicar("BtnAdicionar"); Assert.Empty(t.Exibidos); Assert.False(File.Exists(t.Pasta.Arquivo)); }
    [AvaloniaFact(DisplayName = "A04 | ID zero na sugestão | Cadastro recusado")]
    public void IdInvalido() { using var t = new TelaTeste(); t.Selecionar(0); t.Clicar("BtnAdicionar"); Assert.Empty(t.Exibidos); Assert.False(File.Exists(t.Pasta.Arquivo)); }
    [AvaloniaFact(DisplayName = "A05 | Nome vazio na sugestão | Cadastro recusado")]
    public void NomeInvalido() { using var t = new TelaTeste(); t.Selecionar(42, " "); t.Clicar("BtnAdicionar"); Assert.Empty(t.Exibidos); Assert.False(File.Exists(t.Pasta.Arquivo)); }
    [AvaloniaFact(DisplayName = "A06 | Nota vazia no cadastro | Aviso e nenhuma gravação")]
    public void NotaVazia() { using var t = new TelaTeste(); t.Selecionar(); t.Campo<NumericUpDown>("NumNota").Value = null; t.Clicar("BtnAdicionar"); Assert.Empty(t.Exibidos); Assert.Contains("nota", t.Aviso); }
    [AvaloniaFact(DisplayName = "A07 | Categoria não selecionada | Cadastro recusado")]
    public void CategoriaVazia() { using var t = new TelaTeste(); t.Selecionar(); t.Campo<ComboBox>("CmbCategoria").SelectedIndex = -1; t.Clicar("BtnAdicionar"); Assert.Empty(t.Exibidos); Assert.Contains("categoria", t.Aviso); }
    [AvaloniaFact(DisplayName = "A08 | Status não selecionado | Cadastro recusado")]
    public void StatusVazio() { using var t = new TelaTeste(); t.Selecionar(); t.Campo<ComboBox>("CmbStatus").SelectedIndex = -1; t.Clicar("BtnAdicionar"); Assert.Empty(t.Exibidos); Assert.Contains("status", t.Aviso); }
    [AvaloniaFact(DisplayName = "A09 | Conflito ao adicionar | Memória e formulário preservados sem falso sucesso")]
    public void ConflitoAdicionar()
    {
        using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Selecionar(); File.WriteAllText(t.Pasta.Arquivo, "[]"); t.Clicar("BtnAdicionar");
        Assert.Single(t.Exibidos); Assert.Equal("[]", File.ReadAllText(t.Pasta.Arquivo)); Assert.Contains("não foram aplicadas", t.Aviso);
        Assert.Equal("Jogo IGDB simulado", t.Campo<TextBox>("TxtTitulo").Text);
    }
    [AvaloniaFact(DisplayName = "A10 | Falha de backup ao adicionar | Lista e arquivo originais preservados")]
    public void FalhaAdicionar()
    {
        using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); var original = File.ReadAllBytes(t.Pasta.Arquivo);
        Directory.CreateDirectory(t.Pasta.Arquivo + ".bak"); t.Selecionar(); t.Clicar("BtnAdicionar");
        Assert.Single(t.Exibidos); Assert.Equal(original, File.ReadAllBytes(t.Pasta.Arquivo)); Assert.Contains("não foram aplicadas", t.Aviso);
    }
    [AvaloniaFact(DisplayName = "A11 | Mesmo ID adicionado duas vezes | Dois registros sem deduplicação")]
    public void Duplicados() { using var t = new TelaTeste(); t.Selecionar(); t.Clicar("BtnAdicionar"); t.Selecionar(); t.Clicar("BtnAdicionar"); Assert.Equal(2, t.Salvos.Count); Assert.All(t.Salvos, j => Assert.Equal(42, j.IgdbId)); }
    [AvaloniaFact(DisplayName = "A12 | Coleção corrompida no início | Gravação bloqueada e original intacto")]
    public void ColecaoCorrompida() { using var t = new TelaTeste(json: "{"); Assert.False(t.Campo<Button>("BtnAdicionar").IsEnabled); t.Selecionar(); t.Clicar("BtnAdicionar"); Assert.Equal("{", File.ReadAllText(t.Pasta.Arquivo)); Assert.Empty(t.Exibidos); }
    [AvaloniaFact(DisplayName = "A13 | Review vazia e nota zero | Cadastro válido com texto padrão")]
    public void Defaults() { using var t = new TelaTeste(); t.Selecionar(); t.Campo<NumericUpDown>("NumNota").Value = 0; t.Clicar("BtnAdicionar"); var j = Assert.Single(t.Salvos); Assert.Equal(0, j.Nota); Assert.Equal("Nenhuma Review", j.Review); }

    [AvaloniaFact(DisplayName = "E01 | Editar registro legado | SteamAppId preservado e título somente leitura")]
    public void EditarLegado() { using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Editar(); Assert.True(t.Campo<TextBox>("TxtTitulo").IsReadOnly); t.Clicar("BtnAtualizar"); var j = Assert.Single(t.Salvos); Assert.Equal(1010, j.SteamAppId); Assert.Null(j.IgdbId); }
    [AvaloniaFact(DisplayName = "E02 | Editar registro IGDB | IgdbId preservado")]
    public void EditarIgdb() { var j = PastaTeste.Jogo(); j.SteamAppId = null; j.IgdbId = 99; using var t = new TelaTeste(new[] { j }); t.Editar(); t.Clicar("BtnAtualizar"); Assert.Equal(99, Assert.Single(t.Salvos).IgdbId); Assert.Null(Assert.Single(t.Salvos).SteamAppId); }
    [AvaloniaFact(DisplayName = "E03 | Registro com dois IDs | Ambos preservados sem reinterpretar")]
    public void DoisIds() { var j = PastaTeste.Jogo(); j.IgdbId = 99; using var t = new TelaTeste(new[] { j }); t.Editar(); t.Clicar("BtnAtualizar"); Assert.Equal(99, Assert.Single(t.Salvos).IgdbId); Assert.Equal(1010, Assert.Single(t.Salvos).SteamAppId); }
    [AvaloniaFact(DisplayName = "E04 | Registro sem IDs | IDs permanecem nulos")]
    public void SemIds() { var j = PastaTeste.Jogo(); j.SteamAppId = null; using var t = new TelaTeste(new[] { j }); t.Editar(); t.Clicar("BtnAtualizar"); Assert.Null(Assert.Single(t.Salvos).IgdbId); Assert.Null(Assert.Single(t.Salvos).SteamAppId); }
    [AvaloniaFact(DisplayName = "E05 | Editar nota mínima | Zero persistido")]
    public void NotaMinima() { using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Editar(); t.Campo<NumericUpDown>("NumNota").Value = 0; t.Clicar("BtnAtualizar"); Assert.Equal(0, Assert.Single(t.Salvos).Nota); }
    [AvaloniaFact(DisplayName = "E06 | Editar nota máxima | Cinco persistido")]
    public void NotaMaxima() { using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Editar(); t.Campo<NumericUpDown>("NumNota").Value = 5; t.Clicar("BtnAtualizar"); Assert.Equal(5, Assert.Single(t.Salvos).Nota); }
    [AvaloniaFact(DisplayName = "E07 | Categoria e status legados | Valores fora das opções preservados")]
    public void CategoriasLegadas() { var j = PastaTeste.Jogo(); j.Categoria = "Categoria antiga"; j.Status = "Status antigo"; using var t = new TelaTeste(new[] { j }); t.Editar(); t.Clicar("BtnAtualizar"); Assert.Equal(j.Categoria, Assert.Single(t.Salvos).Categoria); Assert.Equal(j.Status, Assert.Single(t.Salvos).Status); }
    [AvaloniaFact(DisplayName = "E08 | Campo JSON desconhecido na edição | Conteúdo adicional preservado")]
    public void CampoFuturo() { var j = PastaTeste.Jogo(); j.DadosAdicionais = new() { ["futuro"] = JsonSerializer.SerializeToElement(42) }; using var t = new TelaTeste(new[] { j }); t.Editar(); t.Clicar("BtnAtualizar"); Assert.Equal(42, Assert.Single(t.Salvos).DadosAdicionais!["futuro"].GetInt32()); }
    [AvaloniaFact(DisplayName = "E09 | Falha ao salvar edição | Nota anterior e formulário mantidos")]
    public void FalhaEditar()
    {
        using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Editar(); t.Campo<NumericUpDown>("NumNota").Value = 1;
        Directory.CreateDirectory(t.Pasta.Arquivo + ".bak"); t.Clicar("BtnAtualizar"); Assert.Equal(4.5, Assert.Single(t.Exibidos).Nota);
        Assert.Equal(4.5, Assert.Single(t.Salvos).Nota); Assert.Equal(1, t.Campo<NumericUpDown>("NumNota").Value); Assert.True(t.Campo<TextBox>("TxtTitulo").IsReadOnly);
    }
    [AvaloniaFact(DisplayName = "E10 | Atualizar sem item em edição | Coleção não alterada")]
    public void SemEdicao() { using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); var b = File.ReadAllBytes(t.Pasta.Arquivo); t.Clicar("BtnAtualizar"); Assert.Equal(b, File.ReadAllBytes(t.Pasta.Arquivo)); }
    [AvaloniaFact(DisplayName = "E11 | Nota vazia na edição | Arquivo anterior preservado")]
    public void NotaVaziaEditar() { using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Editar(); t.Campo<NumericUpDown>("NumNota").Value = null; var b = File.ReadAllBytes(t.Pasta.Arquivo); t.Clicar("BtnAtualizar"); Assert.Equal(b, File.ReadAllBytes(t.Pasta.Arquivo)); Assert.Contains("nota", t.Aviso); }
    [AvaloniaFact(DisplayName = "E12 | Review legada acima de 1000 caracteres | Edição de nota não trunca texto")]
    public void ReviewLonga() { var j = PastaTeste.Jogo(); j.Review = new string('x', 1200); using var t = new TelaTeste(new[] { j }); t.Editar(); t.Campo<NumericUpDown>("NumNota").Value = 1; t.Clicar("BtnAtualizar"); Assert.Equal(j.Review, Assert.Single(t.Salvos).Review); }
    [AvaloniaFact(DisplayName = "E13 | Review legada com espaços | Edição sem alterar review preserva texto exato")]
    public void ReviewEspacos() { var j = PastaTeste.Jogo(); j.Review = "  Texto antigo  "; using var t = new TelaTeste(new[] { j }); t.Editar(); t.Clicar("BtnAtualizar"); Assert.Equal(j.Review, Assert.Single(t.Salvos).Review); }
    [AvaloniaFact(DisplayName = "E14 | Review legada vazia | Edição mantém valor vazio original")]
    public void ReviewVazia() { var j = PastaTeste.Jogo(); j.Review = ""; using var t = new TelaTeste(new[] { j }); t.Editar(); t.Clicar("BtnAtualizar"); Assert.Equal("", Assert.Single(t.Salvos).Review); }

    [AvaloniaFact(DisplayName = "R01 | Remover item selecionado | Registro excluído da memória e do arquivo")]
    public void Remover() { using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; t.Clicar("BtnRemover"); Assert.Empty(t.Exibidos); Assert.Empty(t.Salvos); Assert.Contains("removido", t.Aviso); }
    [AvaloniaFact(DisplayName = "R02 | Remover sem seleção | Arquivo intacto")]
    public void RemoverSemSelecao() { using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); var b = File.ReadAllBytes(t.Pasta.Arquivo); t.Clicar("BtnRemover"); Assert.Equal(b, File.ReadAllBytes(t.Pasta.Arquivo)); Assert.Single(t.Exibidos); }
    [AvaloniaFact(DisplayName = "R03 | Remover registro com ID repetido | Somente o registro selecionado é removido")]
    public void RemoverDuplicado() { using var t = new TelaTeste(new[] { PastaTeste.Jogo("Um"), PastaTeste.Jogo("Dois") }); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 1; t.Clicar("BtnRemover"); Assert.Equal("Um", Assert.Single(t.Salvos).Titulo); }
    [AvaloniaFact(DisplayName = "R04 | Remoção atualiza estatísticas | Contadores refletem a coleção salva")]
    public void Contadores() { using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; t.Clicar("BtnRemover"); Assert.Equal("0", t.Campo<TextBlock>("TxtTotal").Text); Assert.Equal("0", t.Campo<TextBlock>("TxtContagemNav").Text); }
    [AvaloniaFact(DisplayName = "R05 | Remover item em edição | Formulário volta ao cadastro")]
    public void RemoverEditado() { using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Editar(); t.Clicar("BtnRemover"); Assert.False(t.Campo<TextBox>("TxtTitulo").IsReadOnly); Assert.Equal("", t.Campo<TextBox>("TxtTitulo").Text); Assert.Empty(t.Salvos); }
    [AvaloniaFact(DisplayName = "R06 | Remover outro item durante edição | Edição do registro restante preservada")]
    public void RemoverOutro() { using var t = new TelaTeste(new[] { PastaTeste.Jogo("Um"), PastaTeste.Jogo("Dois") }); t.Editar(); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 1; t.Clicar("BtnRemover"); Assert.True(t.Campo<TextBox>("TxtTitulo").IsReadOnly); Assert.Equal("Um", t.Campo<TextBox>("TxtTitulo").Text); Assert.Equal("Um", Assert.Single(t.Salvos).Titulo); }
    [AvaloniaFact(DisplayName = "R07 | Conflito externo na remoção | Memória e arquivo externo preservados")]
    public void ConflitoRemover() { using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; File.WriteAllText(t.Pasta.Arquivo, "[]"); t.Clicar("BtnRemover"); Assert.Single(t.Exibidos); Assert.Equal("[]", File.ReadAllText(t.Pasta.Arquivo)); Assert.Contains("não foram aplicadas", t.Aviso); }
    [AvaloniaFact(DisplayName = "R08 | Bloqueio na remoção | Nenhum registro desaparece da memória")]
    public void BloqueioRemover() { using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; using var l = new FileStream(t.Pasta.Arquivo + ".lock", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None); t.Clicar("BtnRemover"); Assert.Single(t.Exibidos); Assert.Single(t.Salvos); }
    [AvaloniaFact(DisplayName = "R09 | Falha no backup da remoção | Original preservado sem falso sucesso")]
    public void FalhaRemover() { using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; Directory.CreateDirectory(t.Pasta.Arquivo + ".bak"); t.Clicar("BtnRemover"); Assert.Single(t.Exibidos); Assert.Single(t.Salvos); Assert.Contains("não foram aplicadas", t.Aviso); }
    [AvaloniaFact(DisplayName = "R10 | Reiniciar após remoção | Apenas registros restantes são carregados")]
    public void RecargaRemover() { using var t = new TelaTeste(new[] { PastaTeste.Jogo("Um"), PastaTeste.Jogo("Dois") }); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; t.Clicar("BtnRemover"); using var outra = new TelaTeste(t.Salvos); Assert.Equal("Dois", Assert.Single(outra.Exibidos).Titulo); }
    [AvaloniaFact(DisplayName = "R11 | Remover vizinho de registro IGDB | Identificadores restantes intactos")]
    public void IdRestante() { var j = PastaTeste.Jogo("Mantido"); j.IgdbId = 77; using var t = new TelaTeste(new[] { PastaTeste.Jogo("Removido"), j }); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; t.Clicar("BtnRemover"); Assert.Equal(77, Assert.Single(t.Salvos).IgdbId); Assert.Equal(1010, Assert.Single(t.Salvos).SteamAppId); }
    [AvaloniaFact(DisplayName = "R12 | Remover último registro | JSON vazio válido e backup recuperável")]
    public void Ultimo() { using var t = new TelaTeste(new[] { PastaTeste.Jogo() }); var b = File.ReadAllBytes(t.Pasta.Arquivo); t.Campo<ListBox>("ListaBacklog").SelectedIndex = 0; t.Clicar("BtnRemover"); Assert.Equal("[]", File.ReadAllText(t.Pasta.Arquivo)); Assert.Equal(b, File.ReadAllBytes(t.Pasta.Arquivo + ".bak")); }
}
