using BeatAlert.Logic.Interfaces;

namespace BeatAlert.Logic.Services;

public class AlbumService : IAlbumService
{
    private readonly IAlbumRepository _repo;
    public AlbumService(IAlbumRepository repo) => _repo = repo;

    public bool IsNewRelease(DateTime releaseDate)
        => (DateTime.Now - releaseDate).Days <= 30;
    
}