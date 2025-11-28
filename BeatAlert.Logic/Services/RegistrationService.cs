using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;
using BCryptNet = BCrypt.Net.BCrypt;

namespace BeatAlert.Logic.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IUserRepository _userRepository;

    public RegistrationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public RegistrationResultDTO Register(MusicLoverDTO request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName))
        {
            return new RegistrationResultDTO { Success = false, ErrorMessage = "First Name is required." };

        }
        
        if (string.IsNullOrWhiteSpace(request.LastName))
        {
            return new RegistrationResultDTO { Success = false, ErrorMessage = "Last Name is required." };

        }
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return new RegistrationResultDTO { Success = false, ErrorMessage = "Email is required." };

        }
        
        if (string.IsNullOrWhiteSpace(request.Username))
        {
            return new RegistrationResultDTO { Success = false, ErrorMessage = "Username is required." };

        }

        var existingEmail = _userRepository.GetByEmail(request.Email);

        if (existingEmail != null)
        {
            return new RegistrationResultDTO
            {
                Success = false,
                ErrorMessage = "This email address is already in use.",
                NewUserId = null
            };
        }

        var existingusername = _userRepository.GetByUsername(request.Username);

        if (existingusername != null)
        {
            return new RegistrationResultDTO
            {
                Success = false,
                ErrorMessage = "This username address is already in use.",
                NewUserId = null
            };
        }

        var passwordHash = BCryptNet.HashPassword(request.Password);

        var musicLover = new MusicLover
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Username = request.Username,
            HomeCity = request.HomeCity,
            PasswordHash = passwordHash,
            AvatarUrl = "/images/default-avatar.jpg"
        };

        var newUserId = _userRepository.CreateUser(musicLover);

        return new RegistrationResultDTO
        {
            Success = true,
            ErrorMessage = null,
            NewUserId = newUserId
        };


    }
}