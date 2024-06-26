using System.Text.Json.Serialization;

namespace DotNetLibrary.Application.Models.Responses;

public class BaseResponse<T>(
    bool success,
    ICollection<string>? errors = null,
    int? total = null,
    int? offset = null,
    T? result = default,
    string? next = null,
    string? previous = null)
{
    public bool Success { get; } = success;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ICollection<string>? Errors { get; } = errors;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Total { get; } = total;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Offset { get; } = offset;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Result { get; } = result;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Next { get; } = next;
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Previous { get; } = previous;
    
}