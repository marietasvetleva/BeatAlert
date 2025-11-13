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

    public bool IsNewRelease(DateTime releaseDate)
    {
        return (DateTime.Now - releaseDate).TotalDays <= 30;
    }

    public List<Album> GetAllAlbums()
    {
        return _albumRepository.GetAllAlbums();
    }
    
}