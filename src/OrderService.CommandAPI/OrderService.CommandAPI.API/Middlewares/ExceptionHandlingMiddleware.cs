using System.Net;
using BuildingBlocks.Core.ApiResponses;
using BuildingBlocks.Core.Exceptions;
using System.Text.Json;

namespace OrderService.CommandAPI.API.Middlewares;

public class ExceptionHandlingMiddleware
{
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            IApiResponse response;

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    response = ApiResponse<string>.Fail(validationException.Errors, validationException.Message);
                    break;

                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    response = ApiResponse<string>.Fail(notFoundException.Errors, notFoundException.Message);
                    break;

                case AppException appException:
                    statusCode = HttpStatusCode.BadRequest;
                    response = ApiResponse<string>.Fail(appException.Errors, appException.Message);
                    break;

                default:
                    response = ApiResponse<string>.Fail(
                        new List<string> { "An unexpected error occurred." },
                        "Internal Server Error"
                    );
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            var result = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(result);
        }
}