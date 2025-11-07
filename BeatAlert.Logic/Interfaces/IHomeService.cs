using BeatAlert.Logic.Classes;
namespace BeatAlert.Logic.Interfaces;

public interface IHomeService
{
    List<Album> GetAlbums();
}