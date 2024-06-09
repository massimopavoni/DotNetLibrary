#pragma warning disable CS8618
namespace DotNetLibrary.Models.Entities;

public class Category
{
    public string Name { get; init; }
    public string? Description { get; set; }

    public ICollection<BookCategory>? CategoryBooks { get; init; }
}