using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BacklogManager.Backup;

// Conteúdo codificado em Base64 pelo System.Text.Json preserva exatamente bytes e BOM de cada componente.
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed record ComponenteBackup
{
    public required byte[]? Conteudo { get; init; }
    public required string Sha256 { get; init; }
}
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public sealed record DocumentoBackup
{
    public required string Formato { get; init; }
    public required int Versao { get; init; }
    public required DateTimeOffset CriadoEm { get; init; }
    public required Dictionary<string, ComponenteBackup> Componentes { get; init; }
}
public sealed record PreviaRestauracao(string Arquivo, string HashArquivo, int Obras, int Colecoes, int Vinculos,
    int ReferenciasAusentes, int ObrasAtuais, int ColecoesAtuais, bool PreferenciasValidas,
    bool TemPreferencias, string Aviso, string ResumoPorMidia, DateTimeOffset CriadoEm, string HashEstadoAtual,
    string PreferenciasDisponiveis, bool TemAparencia, bool TemVisualizacao);
internal sealed record DadosValidados(int Obras, int Colecoes, int Vinculos, int Ausentes,
    bool PreferenciasValidas, string Aviso, string PorMidia);
