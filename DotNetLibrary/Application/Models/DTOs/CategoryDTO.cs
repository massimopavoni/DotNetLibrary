using Application.Abstractions;
using Models.Entities;

namespace Application.Models.DTOs;

public class CategoryDTO(string name) : IDTO<Category>
{
    public CategoryDTO(Category category) : this(category.Name)
    {
    }

    public long ID { get; }
    public string Name { get; } = name;

    public Category ToEntity() => new()
    {
        ID = ID,
        Name = Name
    };
}