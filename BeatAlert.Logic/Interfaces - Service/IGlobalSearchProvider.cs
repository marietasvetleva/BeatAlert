using BeatAlert.Logic.DTOs;

namespace BeatAlert.Logic.Interfaces___Service;

public interface IGlobalSearchProvider
{
    void AddResults(GlobalSearchResultsDto result, string query);
}