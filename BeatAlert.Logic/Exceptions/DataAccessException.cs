namespace BeatAlert.Logic.Services;

public class DataAccessException : Exception
{
    public DataAccessException (string message, Exception InnerException)
        : base(message, InnerException)
    {}
    
}