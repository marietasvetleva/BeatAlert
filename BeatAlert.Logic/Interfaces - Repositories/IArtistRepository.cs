using BeatAlert.Logic.Models;

namespace BeatAlert.Logic.Interfaces;

public interface IArtistRepository
{
    List<Artist> GetAllArtists();
    Artist GetArtistById(int id);
    List<Artist> SearchArtist(string query);
}