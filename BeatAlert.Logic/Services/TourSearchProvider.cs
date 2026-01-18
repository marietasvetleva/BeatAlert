using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces___Repositories;
using BeatAlert.Logic.Interfaces___Service;
using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;

namespace BeatAlert.Logic.Services.SearchProviders;

public class TourSearchProvider : IGlobalSearchProvider
{
    private readonly ITourRepository _tourRepository;

    public TourSearchProvider(ITourRepository tourRepository)
    {
        _tourRepository = tourRepository;
    }

    public void AddResults(GlobalSearchResultsDto result, string query)
    {
        var tours = _tourRepository.SearchToursByName(query);
        result.Tours = tours.Select(MapTourToDto).ToList();
    }

    private static TourSearchResultDto MapTourToDto(Tour tour)
    {
        var promo = string.IsNullOrWhiteSpace(tour.PromoPic)
            ? "/images/placeholders/tour.png"
            : tour.PromoPic;

        return new TourSearchResultDto
        {
            TourId = tour.TourId,
            TourName = tour.Name,
            PromoImageUrl = promo
        };
    }
}