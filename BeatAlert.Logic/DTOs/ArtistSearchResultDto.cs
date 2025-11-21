namespace BeatAlert.Logic.DTOs;

public class ArtistSearchResultDto
{
    public int ArtistId { get; init; }
    public string StageName { get; init; } = null!;
    public string ImageUrl { get; init; } = null!;
}