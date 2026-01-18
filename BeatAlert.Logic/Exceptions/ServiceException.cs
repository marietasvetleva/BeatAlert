namespace BeatAlert.Logic.Services;

public class ServiceException : Exception
{
    public ServiceException(string message, Exception InnerException)
        : base(message, InnerException)
    {
        
    }
}