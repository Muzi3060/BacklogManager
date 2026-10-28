using System.Text.Json;
using BacklogManager.Data;
using BacklogManager.Models;
using Xunit;

namespace BacklogManager.Tests.Fase3;

public sealed class PreferenciasTests
{
    private static string Caminho(PastaTeste p) => Path.Combine(p.Pasta, "biblioteca.preferencias.json");
    [Fact(DisplayName = "PV01 | Preferência inexistente | Grade é o padrão e nenhum arquivo é criado")]
    public void Padrao() { using var p = new PastaTeste(); var a = new ArmazenamentoPreferenciasBiblioteca(Caminho(p)); Assert.Equal(VisualizacaoBiblioteca.Grade, a.Carregar()); Assert.False(File.Exists(a.CaminhoArquivo)); }
    [Fact(DisplayName = "PV02 | Salvar lista e reiniciar | Lista é recuperada do arquivo separado")]
    public void Lista() { using var p = new PastaTeste(); new ArmazenamentoPreferenciasBiblioteca(Caminho(p)).Salvar(VisualizacaoBiblioteca.Lista); Assert.Equal(VisualizacaoBiblioteca.Lista, new ArmazenamentoPreferenciasBiblioteca(Caminho(p)).Carregar()); }
    [Fact(DisplayName = "PV03 | Salvar grade após lista | Preferência atualizada sem temporários restantes")]
    public void Grade() { using var p = new PastaTeste(); var a = new ArmazenamentoPreferenciasBiblioteca(Caminho(p)); a.Salvar(VisualizacaoBiblioteca.Lista); a.Salvar(VisualizacaoBiblioteca.Grade); Assert.Equal(VisualizacaoBiblioteca.Grade, a.Carregar()); Assert.Single(Directory.GetFiles(p.Pasta)); }
    [Fact(DisplayName = "PV04 | JSON truncado | Erro controlado e conteúdo original preservado")]
    public void Corrompido() { using var p = new PastaTeste(); File.WriteAllText(Caminho(p), "{"); Assert.Throws<JsonException>(() => new ArmazenamentoPreferenciasBiblioteca(Caminho(p)).Carregar()); Assert.Equal("{", File.ReadAllText(Caminho(p))); }
    [Fact(DisplayName = "PV05 | Versão desconhecida | Preferência recusada sem sobrescrita")]
    public void Versao() { using var p = new PastaTeste(); File.WriteAllText(Caminho(p), "{\"Versao\":2,\"Visualizacao\":1}"); Assert.Throws<InvalidDataException>(() => new ArmazenamentoPreferenciasBiblioteca(Caminho(p)).Carregar()); }
    [Fact(DisplayName = "PV06 | Enum inválido no disco | Preferência recusada")]
    public void EnumDisco() { using var p = new PastaTeste(); File.WriteAllText(Caminho(p), "{\"Versao\":1,\"Visualizacao\":99}"); Assert.Throws<InvalidDataException>(() => new ArmazenamentoPreferenciasBiblioteca(Caminho(p)).Carregar()); }
    [Fact(DisplayName = "PV07 | Campos obrigatórios ausentes | Leitura recusa preferência incompleta")]
    public void Ausentes() { using var p = new PastaTeste(); File.WriteAllText(Caminho(p), "{}"); Assert.Throws<JsonException>(() => new ArmazenamentoPreferenciasBiblioteca(Caminho(p)).Carregar()); }
    [Fact(DisplayName = "PV08 | Arquivo contendo null | Preferência recusada")]
    public void Nulo() { using var p = new PastaTeste(); File.WriteAllText(Caminho(p), "null"); Assert.Throws<InvalidDataException>(() => new ArmazenamentoPreferenciasBiblioteca(Caminho(p)).Carregar()); }
    [Fact(DisplayName = "PV09 | Enum inválido ao salvar | Nenhum arquivo é criado")]
    public void EnumSalvar() { using var p = new PastaTeste(); Assert.Throws<ArgumentOutOfRangeException>(() => new ArmazenamentoPreferenciasBiblioteca(Caminho(p)).Salvar((VisualizacaoBiblioteca)99)); Assert.Empty(Directory.GetFiles(p.Pasta)); }
    [Fact(DisplayName = "PV10 | Falha real no destino | Preferência anterior permanece e temporário é removido")]
    public void FalhaDestino() { using var p = new PastaTeste(); Directory.CreateDirectory(Caminho(p)); var a = new ArmazenamentoPreferenciasBiblioteca(Caminho(p)); Assert.ThrowsAny<IOException>(() => a.Salvar(VisualizacaoBiblioteca.Lista)); Assert.True(Directory.Exists(Caminho(p))); Assert.Empty(Directory.GetFiles(p.Pasta)); }
    [Fact(DisplayName = "PV11 | Recuperar após falha | Nova gravação funciona quando o destino é corrigido")]
    public void Recuperacao() { using var p = new PastaTeste(); Directory.CreateDirectory(Caminho(p)); var a = new ArmazenamentoPreferenciasBiblioteca(Caminho(p)); Assert.ThrowsAny<IOException>(() => a.Salvar(VisualizacaoBiblioteca.Lista)); Directory.Delete(Caminho(p)); a.Salvar(VisualizacaoBiblioteca.Lista); Assert.Equal(VisualizacaoBiblioteca.Lista, a.Carregar()); }
    [Fact(DisplayName = "PV12 | Mudar preferência com biblioteca presente | Bytes da biblioteca não são tocados")]
    public void Separacao() { using var p = new PastaTeste(); File.WriteAllText(p.Arquivo, "biblioteca simulada imutável"); new ArmazenamentoPreferenciasBiblioteca(Caminho(p)).Salvar(VisualizacaoBiblioteca.Lista); Assert.Equal("biblioteca simulada imutável", File.ReadAllText(p.Arquivo)); }
    [Fact(DisplayName = "PV13 | Armazenamento em memória | Janelas de teste não escrevem no repositório")]
    public void Memoria() { var a = new ArmazenamentoPreferenciasBiblioteca(); Assert.Null(a.CaminhoArquivo); a.Salvar(VisualizacaoBiblioteca.Lista); Assert.Equal(VisualizacaoBiblioteca.Lista, a.Carregar()); Assert.Equal(VisualizacaoBiblioteca.Grade, new ArmazenamentoPreferenciasBiblioteca().Carregar()); }
    [Fact(DisplayName = "PV14 | Caminho relativo explícito | Caminho é documentável como absoluto")]
    public void Absoluto() { using var p = new PastaTeste(); var a = new ArmazenamentoPreferenciasBiblioteca(Path.GetRelativePath(Environment.CurrentDirectory, Caminho(p))); Assert.Equal(Caminho(p), a.CaminhoArquivo); }
    [Fact(DisplayName = "PV15 | Preferência inválida substituída explicitamente | Escolha válida pode recuperar o arquivo")]
    public void Substituir() { using var p = new PastaTeste(); File.WriteAllText(Caminho(p), "inválida"); var a = new ArmazenamentoPreferenciasBiblioteca(Caminho(p)); a.Salvar(VisualizacaoBiblioteca.Lista); Assert.Equal(VisualizacaoBiblioteca.Lista, a.Carregar()); }
    [Fact(DisplayName = "PV16 | Composição com biblioteca de mesmo nome da preferência | Caminhos diferentes impedem sobrescrita de dados")]
    public void ColisaoNome() { using var p = new PastaTeste(); var caminho = Path.Combine(p.Pasta, "biblioteca.preferencias.json"); File.WriteAllText(caminho, "[]"); var dependencias = BacklogManager.Configuracao.ComposicaoAplicativo.Criar(caminho, _ => null); Assert.NotEqual(Path.GetFullPath(caminho), dependencias.Preferencias!.CaminhoArquivo); dependencias.Preferencias.Salvar(VisualizacaoBiblioteca.Lista); Assert.Equal("[]", File.ReadAllText(caminho)); }
}
