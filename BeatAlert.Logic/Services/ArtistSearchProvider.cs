using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces___Service;
using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;

namespace BeatAlert.Logic.Services;

public class ArtistSearchProvider : IGlobalSearchProvider
{
    private readonly IArtistRepository _artistRepository;
    
    public ArtistSearchProvider(IArtistRepository artistRepository)
    {
        _artistRepository = artistRepository;
    }
    
    public void AddResults(GlobalSearchResultsDto result, string query)
    {
        var artists = _artistRepository.SearchArtist(query);
        result.Artists = artists.Select(MapArtistToDto).ToList();
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
    
}