namespace BeatAlert.Logic.Models;

public class Notification
{
    public int NotificationId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime DeliveredAt { get; init; }
    public bool IsRead { get; set; }

}