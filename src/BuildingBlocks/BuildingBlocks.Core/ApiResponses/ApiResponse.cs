namespace BuildingBlocks.Core.ApiResponses;

public class ApiResponse<T> : IApiResponse
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
    public string? Message { get; set; }

    public static ApiResponse<T> Ok(T data, string? message = null)
        => new() { IsSuccess = true, Data = data, Message = message };

    public static ApiResponse<T> Fail(List<string> errors, string? message = null)
        => new() { IsSuccess = false, Errors = errors, Message = message };
}