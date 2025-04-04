namespace HolesNBalls.Exceptions.Validation;

public class BoardValidationException : Exception
{
    public BoardValidationException(string message) : base(message) { }
}