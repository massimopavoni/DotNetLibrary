namespace Models.Entities;

public class Category
{
    public long ID { get; init; }
    public string Name { get; init; }

    public virtual ICollection<BookCategory> Books { get; } = [];
}