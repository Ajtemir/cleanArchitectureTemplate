namespace Application.Common.Exceptions;

public class InternalServerException : Exception
{
    public InternalServerException() : base("Internal Server Error")
    {
    }

    public InternalServerException(string? message) : base(message)
    {
    }

    public InternalServerException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
