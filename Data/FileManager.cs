using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace BacklogManager.Data;
using BacklogManager.Models;


public static class FileManager
{
    private const string CaminhoArquivo = "backlog.json";

    public static void Salvar(IEnumerable<BacklogItem> itens)
    {
        var json = JsonSerializer.Serialize(itens, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(CaminhoArquivo, json);
        
    }

    public static List<BacklogItem> Carregar()
    {
        if (File.Exists(CaminhoArquivo))
        {
            var json = File.ReadAllText(CaminhoArquivo);
            return JsonSerializer.Deserialize<List<BacklogItem>>(json) ?? new List<BacklogItem>();
        }
        
        return new List<BacklogItem>();
    }
}