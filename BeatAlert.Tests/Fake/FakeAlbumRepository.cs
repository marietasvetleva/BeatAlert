using BeatAlert.Logic.Models;
using BeatAlert.Logic.Interfaces;

namespace BeatAlert.Tests.Fake;

public class FakeAlbumRepository : IAlbumRepository
{
    public List<Album> GetAllAlbums()
    {
        // fake data
        return new List<Album>
        {
            new Album { AlbumId = 1, Title = "Test Album 1", ReleaseDate = DateTime.Now.AddDays(-10) },
            new Album { AlbumId = 2, Title = "Test Album 2", ReleaseDate = DateTime.Now.AddDays(-50) }
        };
    }

    public Album GetAlbumById(int id)
    {
        return new Album { AlbumId = id, Title = "Fake Album", ReleaseDate = DateTime.Now };
    }
    
    public List<Album> SearchAlbum(string query)
    {
        // This is not used in your tests → dummy data is fine
        return new List<Album>();
    }
}


