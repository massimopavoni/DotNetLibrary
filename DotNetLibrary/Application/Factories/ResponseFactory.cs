using DotNetLibrary.Application.Models.Responses;

namespace DotNetLibrary.Application.Factories;

public static class ResponseFactory
{
    public static BaseResponse<T> WithSuccess<T>(T result) =>
        new()
        {
            Success = true,
            Result = result
        };

    public static BaseResponse<T> WithError<T>(T result) =>
        new()
        {
            Success = false,
            Result = result
        };

    public static BaseResponse<string> WithError(Exception exception) =>
        new()
        {
            Success = false,
            Errors = new List<string>
            {
                exception.Message
            }
        };
}