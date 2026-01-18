using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces___Repositories;
using BeatAlert.Logic.Interfaces___Service;
using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;

namespace BeatAlert.Logic.Services;

public class GlobalSearchService : IGlobalSearchService
{
    private readonly IEnumerable<IGlobalSearchProvider> _providers;

    public GlobalSearchService(IEnumerable<IGlobalSearchProvider> providers)
    {
        _providers = providers;
    }

    public GlobalSearchResultsDto Search(string? query)
    {
        var normalized = (query ?? string.Empty).Trim();

        var result = new GlobalSearchResultsDto
        {
            Query = normalized
        };

        if (string.IsNullOrWhiteSpace(normalized))
            return result;

        try
        {
            foreach (IGlobalSearchProvider provider in _providers)
                provider.AddResults(result, normalized);

            return result;
        }
        catch (Exception ex)
        {
            throw new ServiceException("Search is currently unavailable. Please try again later.", ex);
        }
    }
}
