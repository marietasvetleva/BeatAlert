using BeatAlert.Logic.Classes;


namespace BeatAlert.Logic.Interfaces;

public interface IAlbumRepository
{
    List<Album> GetAllAlbums();
    Album GetAlbumById(int id);
    

}