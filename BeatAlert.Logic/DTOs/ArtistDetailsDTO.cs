namespace BeatAlert.Logic.DTOs;

public class ArtistDetailsDTO
{
    public int ArtistId { get; set; }
    public string StageName { get; set; }
    public string AvatarUrl { get; set; }
    public string BannerUrl { get; set; }
    public string Biography { get; set; }
    
    public List<AlbumDTO> Albums { get; set; }
    public List<TourDetailsDto> Tours { get; set; }
    
}