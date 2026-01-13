using BeatAlert.Logic.DTOs;

namespace BeatAlert.Logic.Interfaces;

public interface IRegistrationService
{
    RegistrationResultDTO Register(MusicLoverDTO request);
}