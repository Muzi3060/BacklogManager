namespace BacklogManager.Models;

public class SteamGame
{
    public int AppId { get; set; }
    public string Nome { get; set; }

    public SteamGame(int appId, string nome)
    {
        AppId = appId;
        Nome = nome;
    }
}