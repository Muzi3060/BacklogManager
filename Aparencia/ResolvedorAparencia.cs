using System;
using System.Collections.Generic;
using System.Linq;

namespace BacklogManager.Aparencia;

public sealed record AparenciaResolvida(IReadOnlyDictionary<ElementoCor, string> Solicitadas,
    IReadOnlyDictionary<string, string> Recursos, IReadOnlyDictionary<ElementoCor, string> Avisos);

public static class ResolvedorAparencia
{
    public static AparenciaResolvida Resolver(PreferenciasAparencia preferencias)
    {
        var c = TemasPredefinidos.Obter(preferencias.Tema, preferencias.Modo).ToDictionary(p => p.Key, p => p.Value);
        var r = new Dictionary<string, string>(); var avisos = new Dictionary<ElementoCor, string>();
        string Cor(ElementoCor e) => c[e];
        void Par(ElementoCor fundo, ElementoCor texto, string chaveFundo, string chaveTexto)
        {
            r[chaveFundo] = Cor(fundo); r[chaveTexto] = CoresAparencia.TextoSeguro(Cor(texto), Cor(fundo));
            if (r[chaveTexto] != Cor(texto))
            {
                var mensagem = $"Contraste insuficiente entre {TemasPredefinidos.Nome(texto).ToLowerInvariant()} e seu fundo. Texto ajustado para {r[chaveTexto]}; sua escolha {Cor(texto)} foi preservada.";
                avisos[texto] = mensagem; avisos[fundo] = mensagem;
            }
        }
        Par(ElementoCor.FundoPrincipal, ElementoCor.TextoPrincipal, "PageBackground", "PageForeground");
        Par(ElementoCor.FundoSidebar, ElementoCor.TextoSidebar, "SidebarBackground", "SidebarForeground");
        Par(ElementoCor.FundoPainel, ElementoCor.TextoPainel, "PanelBackground", "PanelForeground");
        Par(ElementoCor.FundoCartao, ElementoCor.TextoCartao, "CardBackground", "CardForeground");
        Par(ElementoCor.FundoBotao, ElementoCor.TextoBotao, "ButtonBackground", "ButtonForeground");
        Par(ElementoCor.Destaque, ElementoCor.TextoDestaque, "AccentBrush", "AccentForeground");
        r["MutedForeground"] = CoresAparencia.TextoSeguro(Cor(ElementoCor.TextoSecundario), r["PageBackground"]);
        if (r["MutedForeground"] != Cor(ElementoCor.TextoSecundario)) avisos[ElementoCor.TextoSecundario] = "Texto secundário ajustado para manter contraste; a escolha solicitada foi preservada.";
        foreach (var superficie in new[] { "Page", "Sidebar", "Panel", "Card" })
        {
            var fundo = r[superficie + "Background"];
            r[superficie + "Muted"] = CoresAparencia.TextoSeguro(Cor(ElementoCor.TextoSecundario), fundo);
            if (r[superficie + "Muted"] != Cor(ElementoCor.TextoSecundario)) avisos[ElementoCor.TextoSecundario] = "Texto secundário ajustado nas superfícies em que o contraste é insuficiente; sua escolha foi preservada.";
            r[superficie + "Accent"] = CoresAparencia.TextoSeguro(r["AccentBrush"], fundo);
            r[superficie + "Border"] = CoresAparencia.TextoSeguro("#777777", fundo, 3);
            r[superficie + "Hover"] = CoresAparencia.Misturar(fundo, r[superficie + "Foreground"], .07);
            r[superficie + "HoverText"] = CoresAparencia.TextoSeguro(r[superficie + "Foreground"], r[superficie + "Hover"]);
        }
        r["ControlBackground"] = r["PanelBackground"]; r["ControlForeground"] = r["PanelForeground"];
        r["ControlBorder"] = r["PanelBorder"]; r["FocusBrush"] = CoresAparencia.TextoSeguro(r["AccentBrush"], r["ControlBackground"], 3);
        r["SelectionText"] = CoresAparencia.TextoSeguro(r["AccentBrush"], r["CardBackground"]);
        r["SelectionBrush"] = CoresAparencia.TextoSeguro(r["AccentBrush"], r["CardBackground"], 3);
        r["SelectionBackground"] = r["AccentBrush"]; r["SelectionForeground"] = r["AccentForeground"];
        r["ButtonHover"] = CoresAparencia.Misturar(r["ButtonBackground"], r["ButtonForeground"], .10);
        r["ButtonPressed"] = CoresAparencia.Misturar(r["ButtonBackground"], r["ButtonForeground"], .18);
        r["ButtonFocus"] = CoresAparencia.IndicadorSeguro(r["AccentBrush"], r["ButtonBackground"], r["ButtonHover"], r["ButtonPressed"]);
        r["ButtonHoverText"] = CoresAparencia.TextoSeguro(r["ButtonForeground"], r["ButtonHover"]);
        r["ButtonPressedText"] = CoresAparencia.TextoSeguro(r["ButtonForeground"], r["ButtonPressed"]);
        r["DisabledBackground"] = r["PanelHover"]; r["DisabledForeground"] = r["PanelHoverText"];
        var escuro = preferencias.Modo == ModoVisual.Escuro;
        r["SuccessBackground"] = escuro ? "#17392F" : "#DDF4E9";
        r["SuccessBrush"] = escuro ? "#82E3BB" : "#14543C";
        r["ErrorBackground"] = escuro ? "#44242D" : "#FCE8EB";
        r["ErrorBrush"] = escuro ? "#FFB5C0" : "#8B2036";
        r["WarningBackground"] = escuro ? "#3A301B" : "#FFF1CF";
        r["WarningBrush"] = escuro ? "#F0D18B" : "#684A0B";
        r["WarningOnPage"] = CoresAparencia.TextoSeguro(r["WarningBrush"], r["PageBackground"]);
        r["WarningOnCard"] = CoresAparencia.TextoSeguro(r["WarningBrush"], r["CardBackground"]);
        r["WarningOnPanel"] = CoresAparencia.TextoSeguro(r["WarningBrush"], r["PanelBackground"]);
        r["ErrorOnPage"] = CoresAparencia.TextoSeguro(r["ErrorBrush"], r["PageBackground"]);
        r["ErrorOnPanel"] = CoresAparencia.TextoSeguro(r["ErrorBrush"], r["PanelBackground"]);
        r["ErrorOnButton"] = CoresAparencia.TextoSeguro(r["ErrorBrush"], r["ButtonBackground"]);
        r["SuccessOnPanel"] = CoresAparencia.TextoSeguro(r["SuccessBrush"], r["PanelBackground"]);
        return new(c, r, avisos);
    }
}
