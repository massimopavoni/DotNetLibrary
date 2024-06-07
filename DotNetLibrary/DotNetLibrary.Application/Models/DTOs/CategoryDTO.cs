using System.Text.Json.Serialization;
using DotNetLibrary.Application.Abstractions;
using DotNetLibrary.Models.Entities;

namespace DotNetLibrary.Application.Models.DTOs;

public class CategoryDTO(
    string name,
    string? description = null) : IDTO<Category>
{
    public CategoryDTO(CategoryDTO category) : this(category.Name, category.Description)
    {
    }

    public CategoryDTO(Category category) : this(category.Name, category.Description)
    {
    }

    public string Name { get; } = name;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; } = description;

    public Category ToEntity() =>
        new()
        {
            Name = Name,
            Description = Description
        };
}