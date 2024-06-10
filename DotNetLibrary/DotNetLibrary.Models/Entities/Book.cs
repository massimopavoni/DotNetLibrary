#pragma warning disable CS8618
namespace DotNetLibrary.Models.Entities;

public class Book
{
    public string ISBN { get; init; }
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime PublicationDate { get; set; }
    public string Publisher { get; set; }

    public virtual ICollection<BookCategory> BookCategories { get; set; }
}