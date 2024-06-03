using Models.Entities;

namespace Models.Repositories;

public class BookRepository(LibraryContext context) : GenericRepository<Book, string>(context)
{
    public Book? Get(string isbn) => Read(isbn);

    public List<Book> Get(int from, int num, out int total,
        string title = "", string author = "",
        DateOnly publicationDate = default, string publisher = "", List<Category>? categories = null,
        Func<Book, object>? ordering = null)
    {
        var books = _context.Books.AsQueryable();
        total = books.Count();
        if (!string.IsNullOrWhiteSpace(title))
            books = books.Where(b => b.Title.Contains(title));
        if (!string.IsNullOrWhiteSpace(author))
            books = books.Where(b => b.Author.Contains(author));
        if (publicationDate != default)
            books = books.Where(b => b.PublicationDate == publicationDate);
        if (!string.IsNullOrWhiteSpace(publisher))
            books = books.Where(b => b.Publisher.Contains(publisher));
        if (categories is { Count: > 0 })
        {
            var categoryIDs = categories.Select(c => c.ID).ToList();
            books = books.Where(b => b.Categories.Any(c => categoryIDs.Contains(c.CategoryID)));
        }

        return books.AsEnumerable()
            .OrderBy(ordering ?? (b => b.Title))
            .Skip(from)
            .Take(num)
            .ToList();
    }
}