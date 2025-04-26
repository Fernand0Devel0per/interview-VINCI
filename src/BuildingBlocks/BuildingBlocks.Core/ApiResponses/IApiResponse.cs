namespace BuildingBlocks.Core.ApiResponses;

public interface IApiResponse
{
    bool Success { get; }
    string? Message { get; }
    List<string>? Errors { get; }
}