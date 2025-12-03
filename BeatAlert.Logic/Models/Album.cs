namespace BeatAlert.Logic.Models;

public class Album
{
    public int AlbumId { get; init; }
    public string Title { get; init; }
    public DateTime ReleaseDate { get; init; }
    public int Duration { get; init; }
    public string CoverImage { get; init; }
    
    public List<Artist> Artists { get; init; } = new();
    public List<Song> Songs { get; init; } = new();
    
}