namespace BeatAlert.Logic.DTOs;

public class AlbumDTO
{
    public int AlbumId { get; set; }
    public string Title { get; set; }
    public DateOnly? ReleaseDate { get; set; }
    public int Duration { get; set; }
    public string CoverImage { get; set; }
    
    public List<SongDTO> Songs { get; set; }
}