using System;
using System.Collections.Generic;
using System.Linq;
using BacklogManager.Models;

namespace BacklogManager.Services;

// Projeção de leitura: não conhece arquivos, coleções ou APIs.
public sealed class ServicoDashboard
{
    private readonly Func<DateTimeOffset> _agora;
    private readonly TimeZoneInfo _fuso;
    public ServicoDashboard(Func<DateTimeOffset>? agora = null, TimeZoneInfo? fuso = null)
    {
        _agora = agora ?? (() => DateTimeOffset.UtcNow);
        _fuso = fuso ?? TimeZoneInfo.Local;
    }
    public ResumoDashboard Calcular(IReadOnlyList<ItemBiblioteca> obras, int dias = 30)
    {
        ArgumentNullException.ThrowIfNull(obras);
        if (dias is not (7 or 30)) throw new ArgumentOutOfRangeException(nameof(dias));
        var itens = obras.ToArray();
        var agora = _agora();
        var inicio = TimeZoneInfo.ConvertTime(agora, _fuso).Date.AddDays(-(dias - 1));
        bool NoPeriodo(ItemBiblioteca i) => i.DataAdicao is { } d && d <= agora &&
            TimeZoneInfo.ConvertTime(d, _fuso).Date >= inicio;
        var recentes = itens.Where(NoPeriodo).OrderByDescending(i => i.DataAdicao).ToArray();
        var andamento = itens.Where(i => i.Pessoal.Status == "Jogando").ToArray();
        var concluidas = itens.Count(i => i.Pessoal.Status == "Zerado");
        double Percentual(int n) => itens.Length == 0 ? 0 : 100d * n / itens.Length;
        var midias = Enum.GetValues<TipoMidia>().Select(t =>
        {
            var n = itens.Count(i => i.Catalogo.Tipo == t);
            return new DistribuicaoDashboard(TextosMidia.Plural(t), n, Percentual(n));
        }).ToArray();
        var estados = new[] { ("Na Fila", "Na fila"), ("Jogando", "Em andamento"), ("Zerado", "Concluídas"),
            ("Pausado", "Pausadas"), ("Abandonado", "Abandonadas") };
        var status = estados.Select(e =>
        {
            var n = itens.Count(i => i.Pessoal.Status == e.Item1);
            return new DistribuicaoDashboard(e.Item2, n, Percentual(n));
        }).ToList();
        var outros = itens.Count(i => !estados.Any(e => e.Item1 == i.Pessoal.Status));
        status.Add(new("Outros estados", outros, Percentual(outros)));
        EntradaDashboard Entrada(ItemBiblioteca i) => new(i,
            i.DataAdicao is { } d ? TimeZoneInfo.ConvertTime(d, _fuso).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.GetCultureInfo("pt-BR")) : "Data não registrada",
            i.TemProgresso ? i.ProgressoExibido : "Progresso não informado");
        return new(itens.Length, andamento.Length, concluidas, itens.Count(i => i.Pessoal.Status == "Na Fila"),
            itens.Count(i => i.DataAdicao == null), itens.Count(i => i.DataAdicao > agora), recentes.Length,
            Percentual(concluidas), agora, inicio, TimeZoneInfo.ConvertTime(agora, _fuso).Date,
            Array.AsReadOnly(midias), status.AsReadOnly(),
            Array.AsReadOnly(recentes.Take(6).Select(Entrada).ToArray()),
            Array.AsReadOnly(andamento.OrderBy(i => i.DataAdicao == null).ThenByDescending(i => i.DataAdicao).Take(6).Select(Entrada).ToArray()),
            Array.AsReadOnly(itens.Where(i => i.DataAdicao == null).Reverse().Take(3).ToArray()));
    }
}
