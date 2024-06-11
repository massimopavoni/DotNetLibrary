using DotNetLibrary.Application.Models.Responses;

namespace DotNetLibrary.Application.Factories;

public static class ResponseFactory
{
    public static BaseResponse<T> WithSuccess<T>(T result) =>
        new(true, result: result);

    public static BaseResponse<T> WithSuccess<T>(int total, int offset, T result, string? next, string? previous) =>
        new(true, total: total, offset: offset, result: result, next:next, previous:previous);

    public static BaseResponse<object> WithJustError(Exception exception) =>
        new(false, new List<string> { exception.Message });

    public static BaseResponse<T> WithError<T>(T result) =>
        new(false, result: result);

    public static BaseResponse<T> WithError<T>(Exception exception, T result) =>
        new(false, new List<string> { exception.Message }, result: result);
}