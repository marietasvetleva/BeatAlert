using BeatAlert.Logic.Interfaces___Repositories;

namespace BeatAlert.Logic.Services;

public class FollowService
{
    private readonly IFollowRepository _followRepository;

    public FollowService(IFollowRepository followRepository)
    {
        _followRepository = followRepository;
    }

    public bool IsFollowed(int music_lover_id, int artist_id)
    {
        return _followRepository.Exists(music_lover_id, artist_id);
    }

    public bool ToggleFollow(int music_lover_id, int artist_id)
    {
        if (_followRepository.Exists(music_lover_id, artist_id))
        {
            _followRepository.Remove(music_lover_id, artist_id);
            return false;
        }
        
        _followRepository.Add(music_lover_id, artist_id);
            return true;
        
    }
}