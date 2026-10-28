using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BacklogManager.Aparencia;

public enum TemaVisual { Moderno, Tecnologico, Acolhedor }
public enum ModoVisual { Claro, Escuro }
public enum ElementoCor
{
    FundoPrincipal, FundoSidebar, TextoSidebar, FundoPainel, TextoPainel,
    FundoCartao, TextoCartao, FundoBotao, TextoBotao, Destaque, TextoDestaque,
    TextoPrincipal, TextoSecundario
}

public sealed record PreferenciasAparencia
{
    public TemaVisual Tema { get; init; } = TemaVisual.Moderno;
    public ModoVisual Modo { get; init; } = ModoVisual.Claro;
    public IReadOnlyDictionary<string, IReadOnlyDictionary<ElementoCor, string>> Personalizacoes { get; init; }
        = new ReadOnlyDictionary<string, IReadOnlyDictionary<ElementoCor, string>>(new Dictionary<string, IReadOnlyDictionary<ElementoCor, string>>());
    public static string Chave(TemaVisual tema, ModoVisual modo) => $"{tema}/{modo}";
}

public sealed record LeituraAparencia(PreferenciasAparencia Preferencias, string? Aviso = null);
public interface IArmazenamentoAparencia
{
    LeituraAparencia Carregar();
    void Salvar(PreferenciasAparencia preferencias);
}
