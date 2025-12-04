namespace BeatAlert.Logic.DTOs;

public class LoginResultDTO
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public int? UserId { get; set; }
}