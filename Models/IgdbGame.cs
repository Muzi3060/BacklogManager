using System.Collections.Generic;
using System.Threading.Tasks;

namespace BacklogManager.Models;

public class IgdbGame
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public IgdbGame(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public IgdbGame()
    {
        
    }
    
    
    
}