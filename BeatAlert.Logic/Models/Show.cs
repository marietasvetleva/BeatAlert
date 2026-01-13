namespace BeatAlert.Logic.Models;

public class Show
{
    public int ShowId { get; init; }
    public DateTime StartDateTime { get; init; }
    public string TicketUrl { get; init; }
    
    public int TourId { get; init; }
    public int VenueId { get; init; }
    
    public string VenueName { get; init; }
    public string City { get; init; }
    public string Country { get; init; }
    
    public Venue? Venue { get; init; }
    public Tour? Tour { get; init; }

}