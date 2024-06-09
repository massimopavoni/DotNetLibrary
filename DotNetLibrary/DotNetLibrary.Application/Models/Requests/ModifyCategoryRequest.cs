namespace DotNetLibrary.Application.Models.Requests;

public class ModifyCategoryRequest(
    string? description)
{
    public string? Description { get; } = description;
}