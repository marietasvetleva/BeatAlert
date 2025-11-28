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
    
    // public MusicLover(string firstName, string lastName, string email, string username, string homeCity, string passwordHash)
    // {
    //     FirstName = firstName;
    //     LastName = lastName;
    //     Email = email;
    //     Username = username;
    //     HomeCity = homeCity;
    //     PasswordHash = passwordHash;
    // }

    public List<Artist> FollowedArtists { get; init; } = new();
    public List<Song> LikedSongs { get; init; } = new();
    public List<Show> SavedShows { get; init; } = new();
}
