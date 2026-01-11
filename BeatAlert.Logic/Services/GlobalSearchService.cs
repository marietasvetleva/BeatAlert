using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces___Repositories;
using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;

namespace BeatAlert.Logic.Services;

public class GlobalSearchService : IGlobalSearchService
{
    private readonly IArtistRepository _artistRepository;
    private readonly IAlbumRepository _albumRepository;
    // private readonly IShowRepository _showRepository;
    private readonly ISongRepository _songRepository;
    private readonly ITourRepository _tourRepository;

    public GlobalSearchService
    (
        IArtistRepository artistRepository,
        IAlbumRepository albumRepository,
        ISongRepository songRepository,
        ITourRepository tourRepository
    )
    {
        _artistRepository = artistRepository;
        _albumRepository = albumRepository;
        _songRepository = songRepository;
        _tourRepository = tourRepository;
    }


    public GlobalSearchResultsDto Search(string? query)
    {
         var normalized = (query ?? string.Empty).Trim();

        var result = new GlobalSearchResultsDto
        {
            Query = normalized
        };

        if (string.IsNullOrWhiteSpace(normalized))
        {
            return result;
        }

        var artists = _artistRepository.SearchArtist(normalized);
        var albums  = _albumRepository.SearchAlbum(normalized);
        var songs   = _songRepository.SearchSong(normalized);
        var tours   = _tourRepository.SearchToursByName(normalized);

        result.Artists = artists.Select(MapArtistToDto).ToList();
        result.Albums  = albums.Select(MapAlbumToDto).ToList();
        result.Songs   = songs.Select(MapSongToDto).ToList();
        result.Tours   = tours.Select(MapTourToDto).ToList();

        return result;
    }

    private static ArtistSearchResultDto MapArtistToDto(Artist artist)
    {
        var imageUrl = string.IsNullOrWhiteSpace(artist.AvatarUrl)
            ? "/images/placeholders/artist.png"
            : artist.AvatarUrl;

        return new ArtistSearchResultDto
        {
            ArtistId = artist.ArtistId,
            StageName = artist.StageName,
            AvatarUrl = imageUrl
        };
    }

    private static AlbumSearchResultDto MapAlbumToDto(Album album)
    {
        var cover = string.IsNullOrWhiteSpace(album.CoverImage)
            ? "/images/placeholders/album.png"
            : album.CoverImage;

        return new AlbumSearchResultDto
        {
            AlbumId = album.AlbumId,
            Title = album.Title,
            CoverImageUrl = cover
        };
    }

    private static SongSearchResultDto MapSongToDto(Song song)
    {
        return new SongSearchResultDto
        {
            SongId = song.SongId,
            Title = song.Title
        };
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