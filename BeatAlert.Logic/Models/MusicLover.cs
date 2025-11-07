namespace BeatAlert.Logic.Models;

public class MusicLover
{
    public int Id { get; init; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string HomeCity { get; set; }
    public string AvatarUrl { get; set; }
    
    private string PasswordHash { get; set; }
    
    
}
