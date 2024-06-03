namespace Models.Entities;

public class BookCategory
{
    public string BookISBN { get; init; }
    public long CategoryID { get; init; }

    public virtual Book Book { get; init; }
    public virtual Category Category { get; init; }
}