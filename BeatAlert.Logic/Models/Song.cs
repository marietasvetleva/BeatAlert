namespace BeatAlert.Logic.Models;

public class Song
{
    public int SongId { get; init; }
    public string Title { get; init; }
    public int DurationSeconds { get; init; }
    public DateOnly? ReleaseDate { get; init; }
    public string Lyrics { get; init; }
    
    public int AlbumId { get; init; }
    
    public List<Artist> Artists { get; init; } = new();
}