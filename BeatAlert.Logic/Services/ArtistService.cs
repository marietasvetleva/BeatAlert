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

        try
        {
            var artistEntity = _artistRepository.GetArtistById(id);

            if (artistEntity == null)
            {
                return null;
            }

            var avatar = string.IsNullOrWhiteSpace(artistEntity.AvatarUrl)
                ? "/images/placeholders/artist.png"
                : artistEntity.AvatarUrl;

            var banner = string.IsNullOrWhiteSpace(artistEntity.BannerUrl)
                ? "/images/placeholders/artist.png"
                : artistEntity.BannerUrl;

            return new ArtistDetailsDTO
            {
                ArtistId = artistEntity.ArtistId,
                StageName = artistEntity.StageName,
                AvatarUrl = avatar,
                BannerUrl = banner,
                Biography = artistEntity.Biography,
                Albums = artistEntity.Albums.Select(a => MapAlbum(a)).ToList(),
                Tours = artistEntity.Tours.Select(t => MapTour(t)).ToList()

            };
        }

        catch (Exception exception)
        {
            throw new ServiceException("Could not load artist details.", exception);
        }
    }
    
    private AlbumDTO MapAlbum(Album album)
    {
        var cover = string.IsNullOrWhiteSpace(album.CoverImage)
            ? "/images/placeholders/album.png"
            : album.CoverImage;
        
        return new AlbumDTO
        {
            AlbumId = album.AlbumId,
            Title = album.Title,
            CoverImage = cover,
            ReleaseDate = album.ReleaseDate,
            Duration = album.Duration
                
        };
    }

    private TourDetailsDto MapTour(Tour tour)
    {
        var promo = string.IsNullOrWhiteSpace(tour.PromoPic)
            ? "/images/placeholders/tour.png"
            : tour.PromoPic;
        
        return new TourDetailsDto()
        {
            TourId = tour.TourId,
            PromoPic = promo,
            Name = tour.Name,
            Description = tour.Description,
                
        };
    }
}