using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces___Repositories;
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

    public List<AlbumDTO> GetAlbums()
    {
        var albums = _albumRepo.GetAllAlbums();
        return albums.Select(a => new AlbumDTO
        {
            Title = a.Title,
            ReleaseDate = a.ReleaseDate,
            Duration = a.Duration,
            CoverImage = a. CoverImage
        }).ToList();
    }

}
