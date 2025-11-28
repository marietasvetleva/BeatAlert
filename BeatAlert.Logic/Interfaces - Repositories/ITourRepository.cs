using BeatAlert.Logic.Models;

namespace BeatAlert.Logic.Interfaces;

public interface ITourRepository
{
    List<Tour> GetToursByArtistId(int artistId);
    List<Tour> SearchToursByName(string queryText);
    Tour GetTourById(int id);
}