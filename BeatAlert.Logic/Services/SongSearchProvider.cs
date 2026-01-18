using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces___Repositories;
using BeatAlert.Logic.Interfaces___Service;
using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;

namespace BeatAlert.Logic.Services.SearchProviders;

public class SongSearchProvider : IGlobalSearchProvider
{
    private readonly ISongRepository _songRepository;

    public SongSearchProvider(ISongRepository songRepository)
    {
        _songRepository = songRepository;
    }

    public void AddResults(GlobalSearchResultsDto result, string query)
    {
        var songs = _songRepository.SearchSong(query);
        result.Songs = songs.Select(MapSongToDto).ToList();
    }

    private static SongSearchResultDto MapSongToDto(Song song)
    {
        return new SongSearchResultDto
        {
            SongId = song.SongId,
            Title = song.Title
        };
    }
}