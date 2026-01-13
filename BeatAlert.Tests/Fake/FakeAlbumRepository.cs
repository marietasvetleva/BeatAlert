using BeatAlert.Logic.Interfaces___Repositories;
using BeatAlert.Logic.Models;

namespace BeatAlert.Tests.Fake;

public class FakeAlbumRepository : IAlbumRepository
{
    public List<Album> GetAllAlbums()
    {
        return new List<Album>
        {
            new Album
            {
                AlbumId = 1,
                Title = "Test Album 1",
                ReleaseDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
                CoverImage = "/images/placeholders/album.png",
                Songs = new List<Song>(),
                Artists = new List<Artist>()
            },
            new Album
            {
                AlbumId = 2,
                Title = "Test Album 2",
                ReleaseDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-50)),
                CoverImage = "/images/placeholders/album.png",
                Songs = new List<Song>(),
                Artists = new List<Artist>()
            }
        };
    }

    public Album? GetAlbumById(int id)
    {
        return new Album
        {
            AlbumId = id,
            Title = "Fake Album",
            ReleaseDate = DateOnly.FromDateTime(DateTime.Now),
            CoverImage = "/images/placeholders/album.png",
            Songs = new List<Song>(),
            Artists = new List<Artist>()
        };
    }

    public List<Album> SearchAlbum(string query)
    {
        return new List<Album>();
    }
}