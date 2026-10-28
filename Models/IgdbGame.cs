using System.Text.Json.Serialization;

namespace BacklogManager.Models;

public class IgdbGame
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IgdbCover? Cover { get; set; }
    public IgdbNome[]? Genres { get; set; }
    public IgdbNome[]? Platforms { get; set; }

    public IgdbGame(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public IgdbGame()
    {

    }
}

public sealed class IgdbCover
{
    [JsonPropertyName("image_id")]
    public string? ImageId { get; set; }
    public string? Url { get; set; }
}

public sealed class IgdbNome
{
    public string? Name { get; set; }
}
