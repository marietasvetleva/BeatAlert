namespace BeatAlert.Logic.Interfaces___Repositories;

public interface IFollowRepository
{
    bool Exists(int music_lover_id, int artist_id);
    void Add(int music_lover_id, int artist_id);
    void Remove(int music_lover_id, int artist_id);
}