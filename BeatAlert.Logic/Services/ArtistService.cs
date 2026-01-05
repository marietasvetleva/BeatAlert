using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces___Repositories;
using BeatAlert.Logic.Models;

namespace BeatAlert.Logic.Services;

public class ArtistService
{
    private readonly IArtistRepository _artistRepository;

    public ArtistService(IArtistRepository artistRepository)
    {
        _artistRepository = artistRepository;
        
    }

    public ArtistDetailsDTO GetById(int id)
    {
        if (id <= 0)
        {
            return null;
        }
        
        
        var artistEntity = _artistRepository.GetArtistById(id);

        if (artistEntity == null)
        {
            return null;
        }

        return new ArtistDetailsDTO
        {
            ArtistId = artistEntity.ArtistId,
            StageName = artistEntity.StageName,
            AvatarUrl = artistEntity.AvatarUrl,
            BannerUrl = artistEntity.BannerUrl,
            Biography = artistEntity.Biography,
            Albums = artistEntity.Albums.Select(a => MapAlbum(a)).ToList(),
            Tours = artistEntity.Tours.Select(t => MapTour(t)).ToList()

        };
        
    }
    
    private AlbumDTO MapAlbum(Album album)
    {
        return new AlbumDTO
        {
            AlbumId = album.AlbumId,
            Title = album.Title,
            ReleaseDate = album.ReleaseDate,
            Duration = album.Duration
                
        };
    }

    private TourDTO MapTour(Tour tour)

    {
        return new TourDTO
        {
            TourId = tour.TourId,
            Name = tour.Name,
            Description = tour.Description,
                
        };
    }
}