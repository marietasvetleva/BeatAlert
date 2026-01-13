using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces___Repositories;
using BeatAlert.Logic.Interfaces___Service;
using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;


namespace BeatAlert.Logic.Services;

public class AlbumService : IAlbumService
{
    private readonly IAlbumRepository _albumRepository;

    public AlbumService(IAlbumRepository albumRepository)
    {
        _albumRepository = albumRepository;
    }

    public List<AlbumDTO> GetNewAlbums()
    {
        var cutoff = DateOnly.FromDateTime(DateTime.Today).AddDays(-60);
        var albums = _albumRepository.GetAllAlbums();
        
        var newAlbums = albums
            .Where(a => a.ReleaseDate >= cutoff)
            .OrderByDescending(a => a.ReleaseDate)
            .Select(a => new AlbumDTO
            {
                AlbumId = a.AlbumId,
                Title = a.Title,
                CoverImage = a.CoverImage,
                ReleaseDate = a.ReleaseDate,
                Duration = a.Duration,
                
            })
            .ToList();

        return newAlbums;
        
    }
    
    public AlbumDTO GetById(int id)
    {
        if (id <= 0)
        {
            return null;
        }

        var albumEntity = _albumRepository.GetAlbumById(id);

        if (albumEntity == null)
        {
            return null;
        }
        
        var cover = string.IsNullOrWhiteSpace(albumEntity.CoverImage)
            ? "/images/placeholders/album.png"
            : albumEntity.CoverImage;

        return new AlbumDTO
        {
            AlbumId = albumEntity.AlbumId,
            Title = albumEntity.Title,
            ReleaseDate = albumEntity.ReleaseDate,
            Duration = albumEntity.Duration,
            CoverImage = cover,
            ArtistName = albumEntity.Artists
                .Select(a => a.StageName)
                .FirstOrDefault() ?? "Unknown artist",
            Songs = albumEntity.Songs.Select(s => MapSong(s)).ToList()

        };
        
    }
    
    private SongDTO MapSong(Song song)
    {
        return new SongDTO
        {
            Title = song.Title,
            Duration = song.DurationSeconds,
            Lyrics = song.Lyrics,
            ReleaseDate = song.ReleaseDate
        };
    }

    private ArtistDetailsDTO MapArtist(Artist artist)
    {
        return new ArtistDetailsDTO
        {
            ArtistId = artist.ArtistId,
            StageName = artist.StageName,
            AvatarUrl = artist.AvatarUrl,
            BannerUrl = artist.BannerUrl,
            Biography = artist.Biography,
        };
    }
    
    public bool IsNewRelease(DateTime releaseDate)
    {
        return (DateTime.Now - releaseDate).TotalDays <= 30;
    }

    public List<Album> GetAllAlbums()
    {
        return _albumRepository.GetAllAlbums();
    }
    
}