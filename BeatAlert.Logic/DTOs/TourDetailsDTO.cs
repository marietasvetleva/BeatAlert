namespace BeatAlert.Logic.DTOs;

public class TourDetailsDto
{
    public int TourId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PromoPic { get; set; }
    
    public List<ShowDTO> Shows { get; set; }
    
}