using BeatAlert.Logic.Interfaces___Repositories;

namespace BeatAlert.Logic.Services;

public class FollowService
{
    private readonly IFollowRepository _followRepository;

    public FollowService(IFollowRepository followRepository)
    {
        _followRepository = followRepository;
    }

    // public bool IsFollowed(int music_lover_id, int artist_id)
    // {
    //     
    // }
    //
    // public void Follow(int music_lover_id, int artist_id)
    // {
    //     
    // }
    //
    // public void Unfollow(int music_lover_id, int artist_id)
    // {
    //     
    // }
}