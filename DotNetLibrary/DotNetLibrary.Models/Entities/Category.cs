namespace DotNetLibrary.Models.Entities;

public class Category
{
    public string Name { get; init; }
    public string Description { get; init; }

    public virtual ICollection<BookCategory> CategoryBooks { get; init; } = [];
}