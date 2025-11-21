namespace BeatAlert.Logic.DTOs;

public class GlobalSearchResultsDto
{
    public string Query { get; init; }
    public string Result { get; init; }

    public List<ArtistSearchResultDto> Artists { get; set; } = new();
    public List<AlbumSearchResultDto> Albums { get; set; } = new();
    public List<SongSearchResultDto> Songs { get; set; } = new();
    public List<TourSearchResultDto> Tours { get; set; } = new();
}