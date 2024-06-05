using DotNetLibrary.Application.Abstractions;
using DotNetLibrary.Models.Entities;

namespace DotNetLibrary.Application.Models.DTOs;

public class CategoryDTO(string name, string description = "") : IDTO<Category>
{
    public CategoryDTO(CategoryDTO category) : this(category.Name, category.Description)
    {
    }

    public CategoryDTO(Category category) : this(category.Name, category.Description)
    {
    }

    public string Name { get; } = name;
    public string Description { get; } = description;

    public Category ToEntity() =>
        new()
        {
            Name = Name,
            Description = Description
        };
}