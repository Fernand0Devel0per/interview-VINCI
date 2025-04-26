namespace BuildingBlocks.Core.Exceptions;

public class AppException : Exception
{
    public List<string>? Errors { get; }

    public AppException(string message, List<string>? errors = null)
        : base(message)
    {
        Errors = errors;
    }
}