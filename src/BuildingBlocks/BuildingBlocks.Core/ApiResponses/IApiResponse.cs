namespace BuildingBlocks.Core.ApiResponses;

public interface IApiResponse
{
    bool IsSuccess { get; }
    string? Message { get; }
    List<string>? Errors { get; }
}