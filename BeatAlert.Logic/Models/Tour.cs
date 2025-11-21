namespace BeatAlert.Logic.Models;

public class Tour
{
    public int TourId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string PromoPic { get; init; }
    
    public int ArtistId { get; init; }
    public List<Show> Shows { get; init; } = new();
    public Artist? Artist { get; init; }
}