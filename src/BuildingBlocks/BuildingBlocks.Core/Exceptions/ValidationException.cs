namespace BuildingBlocks.Core.Exceptions;

public class ValidationException : AppException
{
    public ValidationException(List<string> errors)
        : base("Validation failed.", errors)
    {
    }

    public ValidationException(string message)
        : base(message)
    {
    }
}