namespace BeatAlert.Logic.Interfaces;

public interface IAlbumService
{
    bool IsNewRelease(DateTime releaseDate);
    
}