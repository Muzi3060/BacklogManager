using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using BacklogManager.Data;

namespace BacklogManager.Aparencia;

public sealed class ServicoAparencia
{
    private readonly IArmazenamentoAparencia _armazenamento;
    public PreferenciasAparencia Preferencias { get; private set; } = new();
    public string? Aviso { get; private set; }
    public bool PendenteSalvar { get; private set; }
    public ServicoAparencia(IArmazenamentoAparencia armazenamento) => _armazenamento = armazenamento;
    public void Carregar()
    {
        try { var leitura = _armazenamento.Carregar(); Preferencias = leitura.Preferencias; Aviso = leitura.Aviso; }
        catch (Exception e) when (Falha(e))
        { Preferencias = new(); Aviso = "A aparência salva não pôde ser lida. Usando Moderno claro; os dados pessoais estão preservados."; }
    }
    public void Selecionar(TemaVisual tema, ModoVisual modo)
    {
        if (!Enum.IsDefined(tema) || !Enum.IsDefined(modo)) throw new ArgumentOutOfRangeException(nameof(tema));
        Aplicar(Preferencias with { Tema = tema, Modo = modo });
    }
    public void Personalizar(ElementoCor elemento, string cor)
    {
        if (!Enum.IsDefined(elemento)) throw new ArgumentOutOfRangeException(nameof(elemento));
        cor = CoresAparencia.Normalizar(cor);
        var conjuntos = Copiar(); var chave = PreferenciasAparencia.Chave(Preferencias.Tema, Preferencias.Modo);
        var cores = conjuntos.TryGetValue(chave, out var existentes) ? existentes.ToDictionary(p => p.Key, p => p.Value) : new();
        cores[elemento] = cor;
        conjuntos[chave] = new ReadOnlyDictionary<ElementoCor, string>(cores);
        Aplicar(Preferencias with { Personalizacoes = new ReadOnlyDictionary<string, IReadOnlyDictionary<ElementoCor, string>>(conjuntos) });
    }
    public void RestaurarAtivo()
    {
        var conjuntos = Copiar(); conjuntos.Remove(PreferenciasAparencia.Chave(Preferencias.Tema, Preferencias.Modo));
        Aplicar(Preferencias with { Personalizacoes = new ReadOnlyDictionary<string, IReadOnlyDictionary<ElementoCor, string>>(conjuntos) });
    }
    public void RestaurarTudo() => Aplicar(new());
    public void TentarSalvarNovamente() => Aplicar(Preferencias);
    private Dictionary<string, IReadOnlyDictionary<ElementoCor, string>> Copiar() => Preferencias.Personalizacoes.ToDictionary(p => p.Key, p => p.Value);
    private void Aplicar(PreferenciasAparencia preferencias)
    {
        Preferencias = preferencias;
        try { _armazenamento.Salvar(preferencias); Aviso = null; PendenteSalvar = false; }
        catch (Exception e) when (Falha(e))
        {
            PendenteSalvar = true;
            Aviso = e is ArquivoAlteradoException
                ? "A aparência mudou apenas nesta sessão. O arquivo foi alterado fora desta janela; reabra o aplicativo antes de salvar."
                : e is InvalidOperationException
                    ? "A aparência mudou apenas nesta sessão. Reabra o arquivo numa versão compatível antes de salvar."
                    : "A aparência mudou nesta sessão, mas não pôde ser salva. Confira o acesso ao arquivo e tente salvar novamente.";
        }
    }
    internal static bool Falha(Exception e) => e is IOException or UnauthorizedAccessException or System.Security.SecurityException or InvalidOperationException or System.Text.Json.JsonException or NotSupportedException;
}

internal sealed class ArmazenamentoAparenciaMemoria : IArmazenamentoAparencia
{
    private PreferenciasAparencia _preferencias = new();
    public LeituraAparencia Carregar() => new(_preferencias);
    public void Salvar(PreferenciasAparencia preferencias) => _preferencias = preferencias;
}
