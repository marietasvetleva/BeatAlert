using BeatAlert.Logic.Models;

namespace BeatAlert.Logic.Interfaces___Repositories;

public interface IAlbumRepository
{
    List<Album> GetAllAlbums();
    Album GetAlbumById(int id);
    List<Album> SearchAlbum(string query);


}