using BacklogManager.Models;
using BacklogManager.Services;
using BacklogManager.Tests.Fase2;
using Xunit;
namespace BacklogManager.Tests.AjustesFase3;

public sealed class PosicoesTests
{
    private static ProgressoPessoal Progresso(string campo, int? valor) => campo switch
    {
        "Pagina" => new() { Pagina = valor },
        "Capitulo" => new() { Capitulo = valor },
        _ => new() { Volume = valor }
    };
    private static int? Valor(ProgressoPessoal? p, string campo) => campo switch { "Pagina" => p?.Pagina, "Capitulo" => p?.Capitulo, _ => p?.Volume };
    private static ItemBiblioteca Preparar(string campo, int? valor, ItemBiblioteca? antigo = null, TipoMidia? tipo = null)
    {
        var t = tipo ?? (campo == "Volume" ? TipoMidia.Manga : TipoMidia.Livro); var c = ApoioMultimidia.Catalogo(t);
        return FormularioObra.Preparar(new(c.Titulo, "Fantasia", "Na Fila", 4m, "Review", Progresso(campo, valor)), c, c.Titulo, antigo, t);
    }
    private static ItemBiblioteca Antigo(string campo, int valor) => ApoioMultimidia.Item(campo == "Volume" ? TipoMidia.Manga : TipoMidia.Livro, Progresso(campo, valor));
    [Fact(DisplayName = "NP01 | Pagina: Posição inicial um | Primeira posição aceita")]
    public void UmNP() { var campo = "Pagina"; Assert.Equal(1, Valor(Preparar(campo, 1).Pessoal.Progresso, campo)); }
    [Fact(DisplayName = "NP02 | Pagina: Maior inteiro permitido | Valor mantido sem overflow")]
    public void MaximoNP() { var campo = "Pagina"; Assert.Equal(int.MaxValue, Valor(Preparar(campo, int.MaxValue).Pessoal.Progresso, campo)); }
    [Fact(DisplayName = "NP03 | Pagina: Posição zero nova | Cadastro recusado com mensagem de progresso inválido")]
    public void ZeroNP() { var campo = "Pagina"; Assert.Equal("Progresso inválido", Assert.Throws<ValidacaoFormularioException>(() => Preparar(campo, 0)).Titulo); }
    [Fact(DisplayName = "NP04 | Pagina: Posição negativa nova | Cadastro recusado sem conversão para positivo")]
    public void NegativoNP() { var campo = "Pagina"; Assert.Throws<ValidacaoFormularioException>(() => Preparar(campo, -1)); }
    [Fact(DisplayName = "NP05 | Pagina: Progresso não informado | Ausência preservada como null")]
    public void AusenteNP() { var campo = "Pagina"; Assert.Null(Preparar(campo, null).Pessoal.Progresso); }
    [Fact(DisplayName = "NP06 | Pagina: Posição zero antiga | Leitura e edição de dados pessoais mantêm zero já existente")]
    public void LegadoNP() { var campo = "Pagina"; var a = Antigo(campo, 0); var r = Preparar(campo, 0, a); Assert.Equal(0, Valor(ApoioMultimidia.Roundtrip(r).Pessoal.Progresso, campo)); }
    [Fact(DisplayName = "NP07 | Pagina: Corrigir zero antigo para positivo | Nova posição persiste")]
    public void CorrigirNP() { var campo = "Pagina"; var r = Preparar(campo, 2, Antigo(campo, 0)); Assert.Equal(2, Valor(ApoioMultimidia.Roundtrip(r).Pessoal.Progresso, campo)); }
    [Fact(DisplayName = "NP08 | Pagina: Limpar posição antiga | Progresso volta a não informado")]
    public void LimparNP() { var campo = "Pagina"; Assert.Null(Preparar(campo, null, Antigo(campo, 0)).Pessoal.Progresso); }
    [Fact(DisplayName = "NP09 | Pagina: Alterar posição positiva para zero | Modificação inválida recusada")]
    public void RegredirZeroNP() { var campo = "Pagina"; Assert.Throws<ValidacaoFormularioException>(() => Preparar(campo, 0, Antigo(campo, 3))); }
    [Fact(DisplayName = "NP10 | Pagina: Salvar posição positiva | Posição recarrega corretamente")]
    public void RoundtripNP() { var campo = "Pagina"; Assert.Equal(25, Valor(ApoioMultimidia.Roundtrip(Preparar(campo, 25)).Pessoal.Progresso, campo)); }
    [Fact(DisplayName = "NP11 | Pagina: Carregar dado antigo sem alteração | Zero não é migrado automaticamente")]
    public void SemMigrarNP() { var campo = "Pagina"; Assert.Equal(0, Valor(ApoioMultimidia.Roundtrip(Antigo(campo, 0)).Pessoal.Progresso, campo)); }
    [Fact(DisplayName = "NP12 | Pagina: Adicionar posição zero a registro sem progresso | Edição recusa novo zero")]
    public void NaoCriarZeroNP() { var campo = "Pagina"; var a = Antigo(campo, 0) with { Pessoal = Antigo(campo, 0).Pessoal with { Progresso = null } }; Assert.Throws<ValidacaoFormularioException>(() => Preparar(campo, 0, a)); }
    [Fact(DisplayName = "NC01 | Capitulo: Posição inicial um | Primeira posição aceita")]
    public void UmNC() { var campo = "Capitulo"; Assert.Equal(1, Valor(Preparar(campo, 1).Pessoal.Progresso, campo)); }
    [Fact(DisplayName = "NC02 | Capitulo: Maior inteiro permitido | Valor mantido sem overflow")]
    public void MaximoNC() { var campo = "Capitulo"; Assert.Equal(int.MaxValue, Valor(Preparar(campo, int.MaxValue).Pessoal.Progresso, campo)); }
    [Fact(DisplayName = "NC03 | Capitulo: Posição zero nova | Cadastro recusado com mensagem de progresso inválido")]
    public void ZeroNC() { var campo = "Capitulo"; Assert.Equal("Progresso inválido", Assert.Throws<ValidacaoFormularioException>(() => Preparar(campo, 0)).Titulo); }
    [Fact(DisplayName = "NC04 | Capitulo: Posição negativa nova | Cadastro recusado sem conversão para positivo")]
    public void NegativoNC() { var campo = "Capitulo"; Assert.Throws<ValidacaoFormularioException>(() => Preparar(campo, -1)); }
    [Fact(DisplayName = "NC05 | Capitulo: Progresso não informado | Ausência preservada como null")]
    public void AusenteNC() { var campo = "Capitulo"; Assert.Null(Preparar(campo, null).Pessoal.Progresso); }
    [Fact(DisplayName = "NC06 | Capitulo: Posição zero antiga | Leitura e edição de dados pessoais mantêm zero já existente")]
    public void LegadoNC() { var campo = "Capitulo"; var a = Antigo(campo, 0); var r = Preparar(campo, 0, a); Assert.Equal(0, Valor(ApoioMultimidia.Roundtrip(r).Pessoal.Progresso, campo)); }
    [Fact(DisplayName = "NC07 | Capitulo: Corrigir zero antigo para positivo | Nova posição persiste")]
    public void CorrigirNC() { var campo = "Capitulo"; var r = Preparar(campo, 2, Antigo(campo, 0)); Assert.Equal(2, Valor(ApoioMultimidia.Roundtrip(r).Pessoal.Progresso, campo)); }
    [Fact(DisplayName = "NC08 | Capitulo: Limpar posição antiga | Progresso volta a não informado")]
    public void LimparNC() { var campo = "Capitulo"; Assert.Null(Preparar(campo, null, Antigo(campo, 0)).Pessoal.Progresso); }
    [Fact(DisplayName = "NC09 | Capitulo: Alterar posição positiva para zero | Modificação inválida recusada")]
    public void RegredirZeroNC() { var campo = "Capitulo"; Assert.Throws<ValidacaoFormularioException>(() => Preparar(campo, 0, Antigo(campo, 3))); }
    [Fact(DisplayName = "NC10 | Capitulo: Salvar posição positiva | Posição recarrega corretamente")]
    public void RoundtripNC() { var campo = "Capitulo"; Assert.Equal(25, Valor(ApoioMultimidia.Roundtrip(Preparar(campo, 25)).Pessoal.Progresso, campo)); }
    [Fact(DisplayName = "NC11 | Capitulo: Carregar dado antigo sem alteração | Zero não é migrado automaticamente")]
    public void SemMigrarNC() { var campo = "Capitulo"; Assert.Equal(0, Valor(ApoioMultimidia.Roundtrip(Antigo(campo, 0)).Pessoal.Progresso, campo)); }
    [Fact(DisplayName = "NC12 | Capitulo: Adicionar posição zero a registro sem progresso | Edição recusa novo zero")]
    public void NaoCriarZeroNC() { var campo = "Capitulo"; var a = Antigo(campo, 0) with { Pessoal = Antigo(campo, 0).Pessoal with { Progresso = null } }; Assert.Throws<ValidacaoFormularioException>(() => Preparar(campo, 0, a)); }
    [Fact(DisplayName = "NV01 | Volume: Posição inicial um | Primeira posição aceita")]
    public void UmNV() { var campo = "Volume"; Assert.Equal(1, Valor(Preparar(campo, 1).Pessoal.Progresso, campo)); }
    [Fact(DisplayName = "NV02 | Volume: Maior inteiro permitido | Valor mantido sem overflow")]
    public void MaximoNV() { var campo = "Volume"; Assert.Equal(int.MaxValue, Valor(Preparar(campo, int.MaxValue).Pessoal.Progresso, campo)); }
    [Fact(DisplayName = "NV03 | Volume: Posição zero nova | Cadastro recusado com mensagem de progresso inválido")]
    public void ZeroNV() { var campo = "Volume"; Assert.Equal("Progresso inválido", Assert.Throws<ValidacaoFormularioException>(() => Preparar(campo, 0)).Titulo); }
    [Fact(DisplayName = "NV04 | Volume: Posição negativa nova | Cadastro recusado sem conversão para positivo")]
    public void NegativoNV() { var campo = "Volume"; Assert.Throws<ValidacaoFormularioException>(() => Preparar(campo, -1)); }
    [Fact(DisplayName = "NV05 | Volume: Progresso não informado | Ausência preservada como null")]
    public void AusenteNV() { var campo = "Volume"; Assert.Null(Preparar(campo, null).Pessoal.Progresso); }
    [Fact(DisplayName = "NV06 | Volume: Posição zero antiga | Leitura e edição de dados pessoais mantêm zero já existente")]
    public void LegadoNV() { var campo = "Volume"; var a = Antigo(campo, 0); var r = Preparar(campo, 0, a); Assert.Equal(0, Valor(ApoioMultimidia.Roundtrip(r).Pessoal.Progresso, campo)); }
    [Fact(DisplayName = "NV07 | Volume: Corrigir zero antigo para positivo | Nova posição persiste")]
    public void CorrigirNV() { var campo = "Volume"; var r = Preparar(campo, 2, Antigo(campo, 0)); Assert.Equal(2, Valor(ApoioMultimidia.Roundtrip(r).Pessoal.Progresso, campo)); }
    [Fact(DisplayName = "NV08 | Volume: Limpar posição antiga | Progresso volta a não informado")]
    public void LimparNV() { var campo = "Volume"; Assert.Null(Preparar(campo, null, Antigo(campo, 0)).Pessoal.Progresso); }
    [Fact(DisplayName = "NV09 | Volume: Alterar posição positiva para zero | Modificação inválida recusada")]
    public void RegredirZeroNV() { var campo = "Volume"; Assert.Throws<ValidacaoFormularioException>(() => Preparar(campo, 0, Antigo(campo, 3))); }
    [Fact(DisplayName = "NV10 | Volume: Salvar posição positiva | Posição recarrega corretamente")]
    public void RoundtripNV() { var campo = "Volume"; Assert.Equal(25, Valor(ApoioMultimidia.Roundtrip(Preparar(campo, 25)).Pessoal.Progresso, campo)); }
    [Fact(DisplayName = "NV11 | Volume: Carregar dado antigo sem alteração | Zero não é migrado automaticamente")]
    public void SemMigrarNV() { var campo = "Volume"; Assert.Equal(0, Valor(ApoioMultimidia.Roundtrip(Antigo(campo, 0)).Pessoal.Progresso, campo)); }
    [Fact(DisplayName = "NV12 | Volume: Adicionar posição zero a registro sem progresso | Edição recusa novo zero")]
    public void NaoCriarZeroNV() { var campo = "Volume"; var a = Antigo(campo, 0) with { Pessoal = Antigo(campo, 0).Pessoal with { Progresso = null } }; Assert.Throws<ValidacaoFormularioException>(() => Preparar(campo, 0, a)); }
    [Fact(DisplayName = "NC13 | Capítulo de mangá zero | Mesma regra positiva aplicada ao mangá")]
    public void MangaZero() => Assert.Throws<ValidacaoFormularioException>(() => Preparar("Capitulo", 0, tipo: TipoMidia.Manga));
    [Fact(DisplayName = "NC14 | Capítulo de mangá positivo | Cadastro e persistência aceitos")]
    public void MangaPositivo() => Assert.Equal(1, ApoioMultimidia.Roundtrip(Preparar("Capitulo", 1, tipo: TipoMidia.Manga)).Pessoal.Progresso!.Capitulo);
    [Fact(DisplayName = "NC15 | Capítulo e volume de mangá não informados | Nenhum zero é preenchido automaticamente")]
    public void MangaSemProgresso() => Assert.Null(Preparar("Capitulo", null, tipo: TipoMidia.Manga).Pessoal.Progresso);
}
