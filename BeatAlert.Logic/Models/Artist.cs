namespace BeatAlert.Logic.Models;

public class Artist
{
    public int ArtistId { get; init; }
    public string StageName { get; init; }
    public string AvatarUrl { get; init; }
    public string BannerUrl { get; init; }
    public string Biography { get; init; }
    
    public List<Album> Albums { get; set; } = new();
    
    public List<Song> Songs { get; init; } = new();
    
    public List<Tour> Tours { get; init; } = new();
    
}
