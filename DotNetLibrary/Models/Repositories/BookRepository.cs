using DotNetLibrary.Models.Context;
using DotNetLibrary.Models.Entities;

namespace DotNetLibrary.Models.Repositories;

public class BookRepository(LibraryContext context) : GenericRepository<Book, string>(context)
{
    public Book? Get(string isbn) =>
        Read(isbn);

    public ICollection<Book> Get(int from, int num, out int total, Func<Book, object> ordering,
        string isbn = "", string title = "", string author = "",
        DateOnly publicationDate = default, string publisher = "", ICollection<string>? categoryNames = null)
    {
        var books = Context.Books.AsQueryable();
        total = books.Count();
        if (!string.IsNullOrWhiteSpace(isbn))
            books = books.Where(b => b.ISBN.Contains(isbn));
        if (!string.IsNullOrWhiteSpace(title))
            books = books.Where(b => b.Title.Contains(title));
        if (!string.IsNullOrWhiteSpace(author))
            books = books.Where(b => b.Author.Contains(author));
        if (publicationDate != default)
            books = books.Where(b => b.PublicationDate == publicationDate);
        if (!string.IsNullOrWhiteSpace(publisher))
            books = books.Where(b => b.Publisher.Contains(publisher));
        if (categoryNames is { Count: > 0 })
            books = books.Where(b => b.BookCategories.Any(bc => categoryNames.Contains(bc.CategoryName)));

        return books.AsEnumerable()
            .OrderBy(ordering)
            .Skip(from)
            .Take(num)
            .ToList();
    }
}