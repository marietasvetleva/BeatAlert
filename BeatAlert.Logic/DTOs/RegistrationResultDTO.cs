namespace BeatAlert.Logic.DTOs;

public class RegistrationResultDTO
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public int? NewUserId { get; set; }
    
}