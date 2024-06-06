namespace DotNetLibrary.Models.Entities;

public class BookCategory
{
    public string BookISBN { get; init; }
    public string CategoryName { get; init; }

    public virtual Book Book { get; set; }
    public virtual Category Category { get; set; }
}