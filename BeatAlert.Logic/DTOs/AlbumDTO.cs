namespace BeatAlert.Logic.DTOs;

public class AlbumDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int Duration { get; set; }
    public string CoverImage { get; set; }
}