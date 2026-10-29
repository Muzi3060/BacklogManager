using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace BacklogManager.Aparencia;

public static class CoresAparencia
{
    public static bool TentarNormalizar(string? texto, out string cor)
    {
        cor = texto?.Trim().ToUpperInvariant() ?? string.Empty;
        return Regex.IsMatch(cor, "^#[0-9A-F]{6}$", RegexOptions.CultureInvariant);
    }
    public static string Normalizar(string cor) => TentarNormalizar(cor, out var normalizada)
        ? normalizada : throw new ArgumentException("Use #RRGGBB, como #3366CC.");

    public static double Contraste(string a, string b)
    {
        var x = Luminancia(a); var y = Luminancia(b);
        return (Math.Max(x, y) + .05) / (Math.Min(x, y) + .05);
    }
    private static double Luminancia(string cor)
    {
        var c = Normalizar(cor);
        double Canal(int inicio)
        {
            var v = int.Parse(c.AsSpan(inicio, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture) / 255d;
            return v <= .04045 ? v / 12.92 : Math.Pow((v + .055) / 1.055, 2.4);
        }
        return .2126 * Canal(1) + .7152 * Canal(3) + .0722 * Canal(5);
    }
    // A escolha solicitada é mantida no modelo. Apenas a cor efetiva é corrigida.
    public static string TextoSeguro(string solicitado, string fundo, double minimo = 4.5)
        => Contraste(solicitado, fundo) >= minimo ? Normalizar(solicitado) :
            Contraste("#000000", fundo) >= Contraste("#FFFFFF", fundo) ? "#000000" : "#FFFFFF";

    public static string BordaSegura(string fundo) => TextoSeguro("#777777", fundo, 3);

    public static string IndicadorSeguro(string solicitado, params string[] fundos)
    {
        if (fundos.All(f => Contraste(solicitado, f) >= 3)) return Normalizar(solicitado);
        return new[] { "#000000", "#FFFFFF" }.OrderByDescending(c => fundos.Min(f => Contraste(c, f))).First();
    }

    public static string Misturar(string origem, string destino, double proporcao)
    {
        origem = Normalizar(origem); destino = Normalizar(destino);
        var canais = Enumerable.Range(0, 3).Select(n =>
            (int)Math.Round(Convert.ToInt32(origem.Substring(n * 2 + 1, 2), 16) * (1 - proporcao) +
                            Convert.ToInt32(destino.Substring(n * 2 + 1, 2), 16) * proporcao));
        return "#" + string.Concat(canais.Select(c => c.ToString("X2", CultureInfo.InvariantCulture)));
    }
}

public static class TemasPredefinidos
{
    public static string Nome(ElementoCor elemento) => elemento switch
    {
        ElementoCor.FundoPrincipal => "Fundo principal", ElementoCor.FundoSidebar => "Fundo da barra lateral",
        ElementoCor.TextoSidebar => "Texto da barra lateral", ElementoCor.FundoPainel => "Fundo dos painéis",
        ElementoCor.TextoPainel => "Texto dos painéis", ElementoCor.FundoCartao => "Fundo dos cartões",
        ElementoCor.TextoCartao => "Texto dos cartões", ElementoCor.FundoBotao => "Fundo dos botões",
        ElementoCor.TextoBotao => "Texto dos botões", ElementoCor.Destaque => "Destaque e seleção",
        ElementoCor.TextoDestaque => "Texto sobre o destaque", ElementoCor.TextoPrincipal => "Texto principal",
        _ => "Texto secundário"
    };
    public static IReadOnlyDictionary<ElementoCor, string> Obter(TemaVisual tema, ModoVisual modo)
    {
        if (!Enum.IsDefined(tema) || !Enum.IsDefined(modo)) throw new ArgumentOutOfRangeException(nameof(tema));
        var escuro = modo == ModoVisual.Escuro;
        var (pagina, lateral, painel, cartao, botao, destaque) = (tema, escuro) switch
        {
            (TemaVisual.Moderno, false) => ("#F3F4FA", "#E5E8F5", "#FFFFFF", "#FFFFFF", "#59349A", "#59349A"),
            (TemaVisual.Moderno, true) => ("#191C2C", "#121524", "#24283C", "#292E45", "#AE94ED", "#AE94ED"),
            (TemaVisual.Tecnologico, false) => ("#F1F6F5", "#DFEBE8", "#FFFFFF", "#FFFFFF", "#08675F", "#08675F"),
            (TemaVisual.Tecnologico, true) => ("#131A1B", "#0A1011", "#202A2C", "#273336", "#70D6CB", "#70D6CB"),
            (TemaVisual.Acolhedor, false) => ("#FBF4EA", "#EFE0CE", "#FFFCF6", "#FFFCF6", "#7B3049", "#7B3049"),
            (TemaVisual.Acolhedor, true) => ("#251B21", "#1A1217", "#352630", "#402E38", "#DAAF59", "#DAAF59"),
            (TemaVisual.DarkMode, false) => ("#F2F2F2", "#E4E4E4", "#FFFFFF", "#FAFAFA", "#383838", "#383838"),
            (TemaVisual.DarkMode, true) => ("#121212", "#0A0A0A", "#202020", "#282828", "#D0D0D0", "#D0D0D0"),
            _ => throw new ArgumentOutOfRangeException(nameof(tema))
        };
        var texto = tema == TemaVisual.DarkMode ? (escuro ? "#F5F5F5" : "#252525") : escuro ? "#F6F3F8" : "#252330";
        var secundario = tema == TemaVisual.DarkMode ? (escuro ? "#BBBBBB" : "#595959") : escuro ? "#C0BCCD" : "#585568";
        return new Dictionary<ElementoCor, string>
        {
            [ElementoCor.FundoPrincipal] = pagina, [ElementoCor.FundoSidebar] = lateral,
            [ElementoCor.TextoSidebar] = texto, [ElementoCor.FundoPainel] = painel, [ElementoCor.TextoPainel] = texto,
            [ElementoCor.FundoCartao] = cartao, [ElementoCor.TextoCartao] = texto, [ElementoCor.FundoBotao] = botao,
            [ElementoCor.TextoBotao] = CoresAparencia.TextoSeguro(escuro ? "#17131E" : "#FFFFFF", botao),
            [ElementoCor.Destaque] = destaque, [ElementoCor.TextoDestaque] = CoresAparencia.TextoSeguro("#FFFFFF", destaque),
            [ElementoCor.TextoPrincipal] = texto, [ElementoCor.TextoSecundario] = secundario
        };
    }
}
