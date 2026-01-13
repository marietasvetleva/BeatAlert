using BeatAlert.Logic.DTOs;

namespace BeatAlert.Logic.Interfaces;

public interface IGlobalSearchService
{
    GlobalSearchResultsDto Search(string? query);
}