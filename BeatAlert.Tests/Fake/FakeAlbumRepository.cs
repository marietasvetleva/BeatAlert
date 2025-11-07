using BeatAlert.Logic.Models;
using BeatAlert.Logic.Interfaces;

namespace BeatAlert.Tests;

public class FakeAlbumRepository : IAlbumRepository
{
    public List<Album> GetAllAlbums()
    {
        // fake data
        return new List<Album>
        {
            new Album { Id = 1, Title = "Test Album 1", ReleaseDate = DateTime.Now.AddDays(-10) },
            new Album { Id = 2, Title = "Test Album 2", ReleaseDate = DateTime.Now.AddDays(-50) }
        };
    }

    public Album GetAlbumById(int id)
    {
        return new Album { Id = id, Title = "Fake Album", ReleaseDate = DateTime.Now };
    }
}
