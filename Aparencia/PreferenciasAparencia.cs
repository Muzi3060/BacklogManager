using System;

namespace BacklogManager.Aparencia;

public enum TemaVisual { Moderno, Tecnologico, Acolhedor, DarkMode }
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
}

public sealed record LeituraAparencia(PreferenciasAparencia Preferencias, string? Aviso = null);
public interface IArmazenamentoAparencia
{
    LeituraAparencia Carregar();
    void Salvar(PreferenciasAparencia preferencias);
}
