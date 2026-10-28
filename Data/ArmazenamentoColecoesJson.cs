using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using BacklogManager.Contracts;
using BacklogManager.Models;
using BacklogManager.Services;

namespace BacklogManager.Data;

public sealed class ArmazenamentoColecoesJson : IArmazenamentoColecoes
{
    private readonly object _sincronia = new();
    private bool _carregado;
    private string? _hash;
    public string DescricaoOrigem { get; }
    public ArmazenamentoColecoesJson(string caminho) => DescricaoOrigem = Path.GetFullPath(caminho);

    public IReadOnlyList<ColecaoPersonalizada> Carregar()
    {
        lock (_sincronia)
        {
            _carregado = false;
            var bytes = Ler();
            IReadOnlyList<ColecaoPersonalizada> colecoes = Array.Empty<ColecaoPersonalizada>();
            if (bytes != null)
            {
                try
                {
                    using var memoria = new MemoryStream(bytes);
                    using var leitor = new StreamReader(memoria, Encoding.UTF8, true);
                    var dados = JsonSerializer.Deserialize<Documento>(leitor.ReadToEnd());
                    if (dados is not { Versao: 1, Colecoes: not null }) throw new InvalidDataException("Versão de coleções desconhecida ou arquivo inválido.");
                    if (dados.Colecoes.Any(c => c == null)) throw new InvalidDataException("Uma coleção do arquivo é nula.");
                    colecoes = dados.Colecoes.Select(c => new ColecaoPersonalizada(c.Id, c.Nome, c.Descricao,
                        c.Obras == null ? null! : Array.AsReadOnly(c.Obras))).ToArray();
                    RegrasColecoes.Validar(colecoes);
                }
                catch (Exception erro) when (erro is JsonException or ArgumentException)
                { throw new InvalidDataException("O arquivo de coleções é inválido. O original foi preservado.", erro); }
            }
            _hash = Hash(bytes);
            _carregado = true;
            return colecoes;
        }
    }

    public void Salvar(IReadOnlyList<ColecaoPersonalizada> colecoes)
    {
        lock (_sincronia)
        {
            if (!_carregado) throw new InvalidOperationException("Carregue as coleções com sucesso antes de salvar.");
            RegrasColecoes.Validar(colecoes);
            var dados = new Documento { Versao = 1, Colecoes = colecoes.Select(c => new Registro { Id = c.Id, Nome = c.Nome, Descricao = c.Descricao, Obras = c.Obras.ToArray() }).ToArray() };
            var bytes = JsonSerializer.SerializeToUtf8Bytes(dados, new JsonSerializerOptions { WriteIndented = true });
            using var bloqueio = new FileStream(DescricaoOrigem + ".lock", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            if (Hash(Ler()) != _hash) throw new ArquivoAlteradoException();
            var temporario = DescricaoOrigem + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                using (var arquivo = new FileStream(temporario, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                { arquivo.Write(bytes); arquivo.Flush(true); }
                if (_hash == null) File.Move(temporario, DescricaoOrigem, false);
                else File.Replace(temporario, DescricaoOrigem, DescricaoOrigem + ".bak");
                _hash = Hash(bytes);
            }
            finally
            {
                try { File.Delete(temporario); }
                catch (IOException) { }
                catch (UnauthorizedAccessException) { }
            }
        }
    }

    private byte[]? Ler()
    {
        try { return File.ReadAllBytes(DescricaoOrigem); }
        catch (FileNotFoundException) { return null; }
        catch (DirectoryNotFoundException) { return null; }
    }
    private static string? Hash(byte[]? bytes) => bytes == null ? null : Convert.ToHexString(SHA256.HashData(bytes));
    [JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
    private sealed class Documento
    {
        public required int Versao { get; init; }
        public required Registro[] Colecoes { get; init; }
    }
    [JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
    private sealed class Registro
    {
        public required Guid Id { get; init; }
        public required string Nome { get; init; }
        public required string Descricao { get; init; }
        public required Guid[] Obras { get; init; }
    }
}

internal sealed class ArmazenamentoColecoesMemoria : IArmazenamentoColecoes
{
    private IReadOnlyList<ColecaoPersonalizada> _colecoes = Array.Empty<ColecaoPersonalizada>();
    public string DescricaoOrigem => "coleções em memória";
    public IReadOnlyList<ColecaoPersonalizada> Carregar() => _colecoes;
    public void Salvar(IReadOnlyList<ColecaoPersonalizada> colecoes) => _colecoes = colecoes;
}
