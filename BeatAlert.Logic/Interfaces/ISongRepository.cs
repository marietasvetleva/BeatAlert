using BeatAlert.Logic.Models;

namespace BeatAlert.Logic.Interfaces;

public interface ISongRepository
{
    List<Song> GetAllSongs();
    Song GetSongById(int id);
    List<Song> SearchSong(string query);
}