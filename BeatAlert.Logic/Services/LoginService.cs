using BeatAlert.Logic.DTOs;
using BeatAlert.Logic.Interfaces;
using BeatAlert.Logic.Models;

namespace BeatAlert.Logic.Services;

public class LoginService
{
   private readonly IUserRepository _userRepository;

   public LoginService(IUserRepository userRepository)
   {
      _userRepository = userRepository;
   }

   public LoginResultDTO Login(LoginRequestDTO request)
   {
       if (request == null)
       {
           return new LoginResultDTO
           {
               Success = false,
               ErrorMessage = "Invalid login request."
           };
       }
       
       request.Identifier = (request.Identifier ?? string.Empty).Trim();
       request.Password = (request.Password ?? string.Empty).Trim();
       
       
       if (string.IsNullOrEmpty(request.Identifier) ||
           string.IsNullOrEmpty(request.Password))
       {
           return new LoginResultDTO { Success = false, ErrorMessage = "Please fill in all fields." };
       }

       MusicLover? user = null;
       
       if (request.Identifier.Contains("@"))
       {
           user = _userRepository.GetByEmail(request.Identifier);
       }
       else
       {
           user = _userRepository.GetByUsername(request.Identifier);
       }

       if (user is null)
       {
           return new LoginResultDTO { Success = false, ErrorMessage = "Invalid email/username or password." };
       }

       bool isPasswordMatched = (BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash));
       
       if (isPasswordMatched == false)
       {
           return new LoginResultDTO { Success = false, ErrorMessage = "Invalid email/username or password." };
       }

       else
       {
           return new LoginResultDTO { Success = true, ErrorMessage = null, UserId = user.MusicLoverId, FirstName = user.FirstName };
       }
       
       
   }
}