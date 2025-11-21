namespace BeatAlert.Logic.Models;

public class Show
{
    public int ShowId { get; init; }
    public DateTime StartDateTime { get; init; }
    public string TicketUrl { get; init; }
    
    public int TourId { get; init; }
    public int VenueId { get; init; }
    
    public Venue? Venue { get; init; }
    public Tour? Tour { get; init; }

}