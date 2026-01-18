using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces___Repositories;
using BeatAlert.Logic.Interfaces___Service;
using BeatAlert.Logic.Models;

namespace BeatAlert.Logic.Services.SearchProviders;

public class AlbumSearchProvider : IGlobalSearchProvider
{
    private readonly IAlbumRepository _albumRepository;

    public AlbumSearchProvider(IAlbumRepository albumRepository)
    {
        _albumRepository = albumRepository;
    }

    public void AddResults(GlobalSearchResultsDto result, string query)
    {
        var albums = _albumRepository.SearchAlbum(query);
        result.Albums = albums.Select(MapAlbumToDto).ToList();
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
}