using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BacklogManager.Models;

namespace BacklogManager.Data;

public sealed class FileManager
{
    private static readonly JsonSerializerOptions OpcoesJson = new() { WriteIndented = true };
    private readonly object _sincronizacao = new();
    private bool _carregado;
    private string? _hashCarregado;

    // Mantém backlog.json no diretório de trabalho, sem migrar dados.
    // Resolver uma vez impede que o caminho mude durante a sessão.
    public string CaminhoArquivo { get; }

    public FileManager(string caminhoArquivo = "backlog.json")
    {
        CaminhoArquivo = Path.GetFullPath(caminhoArquivo);
    }

    public List<BacklogItem> Carregar()
    {
        lock (_sincronizacao)
        {
            _carregado = false;
            var bytes = LerArquivoExistente();
            var itens = bytes == null ? new List<BacklogItem>() : Desserializar(bytes);
            _hashCarregado = CalcularHash(bytes);
            _carregado = true;
            return itens;
        }
    }

    internal void ConferirEstadoAtual()
    {
        if (!_carregado || CalcularHash(LerArquivoExistente()) != _hashCarregado) throw new ArquivoAlteradoException();
    }

    public void Salvar(IEnumerable<BacklogItem> itens)
    {
        lock (_sincronizacao)
        {
            if (!_carregado)
                throw new InvalidOperationException("Carregue a coleção com sucesso antes de salvar.");

            var lista = itens.ToList();
            Validar(lista);
            var bytes = JsonSerializer.SerializeToUtf8Bytes(lista, OpcoesJson);
            var temporario = CaminhoArquivo + "." + Guid.NewGuid().ToString("N") + ".tmp";

            // Outras instâncias deste aplicativo respeitam o bloqueio; editores externos podem não respeitá-lo.
            using var bloqueio = new FileStream(CaminhoArquivo + ".lock", FileMode.OpenOrCreate,
                FileAccess.ReadWrite, FileShare.None);
            BacklogManager.Backup.CaminhosDadosLocais.VerificarGravacao(CaminhoArquivo);
            if (CalcularHash(LerArquivoExistente()) != _hashCarregado)
                throw new ArquivoAlteradoException();

            try
            {
                using (var arquivo = new FileStream(temporario, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    arquivo.Write(bytes);
                    arquivo.Flush(flushToDisk: true);
                }

                if (_hashCarregado == null)
                    File.Move(temporario, CaminhoArquivo, overwrite: false);
                else
                    File.Replace(temporario, CaminhoArquivo, CaminhoArquivo + ".bak");

                _hashCarregado = CalcularHash(bytes);
            }
            finally
            {
                // Uma falha na limpeza não deve esconder o resultado da gravação.
                try { File.Delete(temporario); }
                catch (IOException) { }
                catch (UnauthorizedAccessException) { }
            }
        }
    }

    private byte[]? LerArquivoExistente()
    {
        try { return File.ReadAllBytes(CaminhoArquivo); }
        catch (FileNotFoundException) { return null; }
        catch (DirectoryNotFoundException) { return null; }
    }

    private static string? CalcularHash(byte[]? bytes) =>
        bytes == null ? null : Convert.ToHexString(SHA256.HashData(bytes));

    private static List<BacklogItem> Desserializar(byte[] bytes)
    {
        try
        {
            // Como File.ReadAllText fazia antes, reconhece BOM UTF-8/UTF-16/UTF-32.
            // O hash e o backup continuam usando os bytes originais.
            using var memoria = new MemoryStream(bytes);
            using var leitor = new StreamReader(memoria, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
            var itens = JsonSerializer.Deserialize<List<BacklogItem>>(leitor.ReadToEnd())
                ?? throw new InvalidDataException("O JSON deve conter uma lista de jogos, não null.");
            Validar(itens);
            return itens;
        }
        catch (JsonException)
        {
            throw new InvalidDataException("O arquivo da coleção contém JSON inválido. O original foi preservado.");
        }
    }

    private static void Validar(List<BacklogItem> itens)
    {
        var identidades = new HashSet<Guid>();
        foreach (var item in itens)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Titulo) ||
                (string.IsNullOrWhiteSpace(item.Categoria) && !(item.Categoria == string.Empty && item.IgdbId > 0)) || string.IsNullOrWhiteSpace(item.Status) ||
                item.Review == null || !double.IsFinite(item.Nota) || item.Nota < 0 || item.Nota > 5 ||
                item.IgdbId is <= 0)
                throw new InvalidDataException("A coleção contém um registro inválido. Nenhum registro foi descartado.");
            if (item.IdInterno is { } id && (id == Guid.Empty || !identidades.Add(id)))
                throw new InvalidDataException("A biblioteca contém identificadores internos vazios ou repetidos. Nenhum registro foi descartado.");
            if (item.Multimidia != null) ConversorBibliotecaJson.ValidarMultimidia(item);
            if (item.DadosAdicionais?.ContainsKey("Multimidia") == true)
                throw new InvalidDataException("O campo Multimidia está duplicado nos dados adicionais.");
            if (item.DadosAdicionais?.ContainsKey("IdInterno") == true)
                throw new InvalidDataException("O campo IdInterno está duplicado nos dados adicionais.");
        }
    }
}

public sealed class ArquivoAlteradoException : IOException
{
    public ArquivoAlteradoException() : base("O arquivo mudou fora desta janela. Reabra o aplicativo antes de salvar.") { }
}
