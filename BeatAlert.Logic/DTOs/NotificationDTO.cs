namespace BeatAlert.Logic.DTOs;

public class NotificationDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DeliveredAt { get; set; }
    public bool IsRead { get; set; }
}