namespace DotNetLibrary.Models.Entities;

public class Book
{
    public string ISBN { get; init; }
    public string Title { get; init; }
    public string Author { get; init; }
    public DateOnly PublicationDate { get; init; }
    public string Publisher { get; init; }

    public virtual ICollection<BookCategory> BookCategories { get; init; } = [];
}