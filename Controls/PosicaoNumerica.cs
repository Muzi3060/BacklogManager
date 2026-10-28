using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Automation;

namespace BacklogManager.Controls;

// Posições não são contagens: começam em 1, mas null continua sendo ausência.
public sealed class PosicaoNumerica : NumericUpDown
{
    private bool _carregando;
    private bool _normalizando;
    private bool _zeroLegado;
    private TextBox? _editor;
    protected override Type StyleKeyOverride => typeof(NumericUpDown);

    public PosicaoNumerica()
    {
        Minimum = 1;
        Maximum = int.MaxValue;
        Increment = 1;
        FormatString = "0";
        ParsingNumberStyle = NumberStyles.Integer;
        PlaceholderText = "Não informado";
        AtualizarOrientacao();
    }

    public void CarregarValorLegado(int? valor)
    {
        _carregando = true;
        try
        {
            _zeroLegado = valor == 0;
            Minimum = _zeroLegado ? 0 : 1;
            SetCurrentValue(ValueProperty, valor is { } numero ? (decimal?)numero : null);
        }
        finally { _carregando = false; }
        AtualizarOrientacao();
    }

    protected override decimal? OnCoerceValue(decimal? valor)
    {
        if (valor == null) return null;
        if (valor == 0 && (_carregando || _zeroLegado && Value == 0)) return 0;
        if (valor != decimal.Truncate(valor.Value)) return Value;
        return Math.Clamp(valor.Value, 1, int.MaxValue);
    }

    protected override void OnValueChanged(decimal? anterior, decimal? atual)
    {
        if (!_carregando && atual != 0)
        {
            _zeroLegado = false;
            Minimum = 1;
        }
        base.OnValueChanged(anterior, atual);
        AtualizarOrientacao();
    }

    protected override void OnTextChanged(string? anterior, string? atual)
    {
        if (_normalizando || _carregando) { base.OnTextChanged(anterior, atual); return; }
        var normalizado = Normalizar(atual);
        if (atual == normalizado) { base.OnTextChanged(anterior, atual); return; }
        _normalizando = true;
        try
        {
            SetCurrentValue(TextProperty, normalizado);
            if (_editor != null) _editor.Text = normalizado;
        }
        finally { _normalizando = false; }
    }

    private string? Normalizar(string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) return string.Empty;
        if (!decimal.TryParse(texto, NumberStyles.Integer, NumberFormat ?? CultureInfo.CurrentCulture.NumberFormat, out var numero))
            return Value?.ToString("0", NumberFormat) ?? string.Empty;
        if (_zeroLegado && Value == 0 && numero == 0) return "0";
        return Math.Clamp(numero, 1, int.MaxValue).ToString("0", NumberFormat);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _editor = e.NameScope.Find<TextBox>("PART_TextBox");
    }

    protected override void OnSpin(SpinEventArgs e)
    {
        // O NumericUpDown padrão escolhe Maximum ao diminuir um campo vazio.
        if (Value == null && e.Direction == SpinDirection.Decrease) return;
        base.OnSpin(e);
    }

    private void AtualizarOrientacao()
    {
        var texto = _zeroLegado ? "Zero legado preservado. Aumente para 1 ou limpe o campo para não informar progresso." :
            "Use inteiros a partir de 1 ou deixe vazio. Zero e negativos viram 1; texto e frações são rejeitados.";
        ToolTip.SetTip(this, texto);
        AutomationProperties.SetHelpText(this, texto);
    }
}
