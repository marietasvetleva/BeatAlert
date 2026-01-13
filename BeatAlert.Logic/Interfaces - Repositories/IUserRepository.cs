using BeatAlert.Logic.Models;

namespace BeatAlert.Logic.Interfaces;

public interface IUserRepository
{
    int CreateUser(MusicLover musicLover);
    MusicLover GetByEmail(string email);
    MusicLover GetByUsername(string username);
}