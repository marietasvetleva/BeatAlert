namespace BeatAlert.Logic.Models;

public class MusicLover
{
    public int MusicLoverId { get; init; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string HomeCity { get; set; }
    public string AvatarUrl { get; set; }
    
    public string PasswordHash { get; set; }
    

    public List<Artist> FollowedArtists { get; init; } = new();
    public List<Song> LikedSongs { get; init; } = new();
    public List<Show> SavedShows { get; init; } = new();
}
