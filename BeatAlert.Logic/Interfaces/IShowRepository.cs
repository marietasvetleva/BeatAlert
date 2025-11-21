using BeatAlert.Logic.Models;

namespace BeatAlert.Logic.Interfaces;

public interface IShowRepository
{
    List<Show> SearchShowsByName(string queryText);
    List<Show> GetShowsByTourId(int tourId);
    Show GetShowById(int id);

}