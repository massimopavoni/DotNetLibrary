#pragma warning disable CS8618
namespace DotNetLibrary.Models.Entities;

public class BookCategory
{
    public string BookISBN { get; init; }
    public string CategoryName { get; init; }

    public virtual Book Book { get; init; }
    public virtual Category Category { get; init; }
}