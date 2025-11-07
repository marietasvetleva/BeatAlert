using BeatAlert.Logic.Models;
using BeatAlert.Logic.Interfaces;

namespace BeatAlert.Logic.Services;


public class HomeService : IHomeService
{
    private readonly IAlbumRepository _albumRepo;

    public HomeService(IAlbumRepository albumRepo)
    {
        _albumRepo = albumRepo;
    }

    public List<Album> GetAlbums()
    {
        return _albumRepo.GetAllAlbums();
    }
}
