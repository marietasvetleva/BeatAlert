using BeatAlert.Logic.Models;
namespace BeatAlert.Logic.Interfaces;

public interface IHomeService
{
    List<Album> GetAlbums();
}