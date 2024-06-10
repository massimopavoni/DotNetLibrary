namespace DotNetLibrary.Application.Models.Requests;

public class UpdateCategoryRequest(
    string? description)
{
    public string? Description { get; } = description;
}