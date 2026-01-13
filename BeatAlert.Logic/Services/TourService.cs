using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;

namespace BeatAlert.Logic.Services;

public class TourService
{
    private readonly ITourRepository _tourRepository;

    public TourService(ITourRepository tourRepository)
    {
        _tourRepository = tourRepository;
    }

    public TourDetailsDto GetById(int id)
    {
        if (id <= 0)
        {
            return null;
        }

        try
        {
            var tourEntity = _tourRepository.GetTourById(id);

            if (tourEntity == null)
            {
                return null;
            }

            var promopic = string.IsNullOrEmpty(tourEntity.PromoPic)
                ? "/images/placeholders/tour.png"
                : tourEntity.PromoPic;

            return new TourDetailsDto
            {
                TourId = tourEntity.TourId,
                Name = tourEntity.Name,
                Description = tourEntity.Description,
                PromoPic = promopic,
                Shows = tourEntity.Shows.Select(s => MapShow(s)).ToList()

            };
        }

        catch (Exception exception)
        {
            throw new ServiceException("Could not load tour details. Please try again later.", exception);
        }

    }

    private ShowDTO MapShow(Show show)
    {
        return new ShowDTO
        {
            StartDateTime = show.StartDateTime,
            TicketUrl = show.TicketUrl,
            VenueName = show.VenueName,
            City = show.City,
            Country = show.Country
        };
    }
}