using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Models;
namespace BeatAlert.Logic.Interfaces;

public interface IHomeService
{
    List<AlbumDTO> GetAlbums();
}