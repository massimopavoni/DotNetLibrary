namespace DotNetLibrary.Application.Models.Requests;

public class CreateCategoryRequest(
    string name,
    string? description)
{
    public string Name { get; } = name;
    public string? Description { get; } = description;
}