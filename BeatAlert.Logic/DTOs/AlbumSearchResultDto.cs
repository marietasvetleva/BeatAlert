namespace BeatAlert.Logic.DTOs;

public class AlbumSearchResultDto
{
    public int AlbumId { get; init; }
    public string Title { get; init; } = null!;
    public string CoverImageUrl { get; init; } = null!;
}