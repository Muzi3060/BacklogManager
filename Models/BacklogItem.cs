namespace BacklogManager.Models;

public class BacklogItem
{
    public string Titulo { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public double Nota { get; set; }
    
    public string Review { get; set; } = string.Empty;
    
    public int? SteamAppId { get; set; } 
    
    public BacklogItem()
    {

    }
    
    public BacklogItem(string titulo, string categoria, string status, double nota, string review, int? steamAppID)
    {
        Titulo = titulo;
        Categoria = categoria;
        Status = status;
        Nota = nota;
        Review = review;
        SteamAppId = steamAppID;
    }

    public override string ToString()
    {
        return $"{Titulo} - {Categoria} - {Status} - {Nota} - {Review}";
    }
}