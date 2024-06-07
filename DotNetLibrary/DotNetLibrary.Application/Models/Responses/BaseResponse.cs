using System.Text.Json.Serialization;

namespace DotNetLibrary.Application.Models.Responses;

public class BaseResponse<T>(bool success, ICollection<string>? errors = null, T? result = default)
{
    public bool Success { get; } = success;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ICollection<string>? Errors { get; } = errors;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Result { get; } = result;
}