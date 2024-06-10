using DotNetLibrary.Models.Context;
using DotNetLibrary.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotNetLibrary.Models.Repositories;

public class BookRepository(LibraryContext context) : GenericRepository<Book, string>(context)
{
    public Book? GetByISBN(string isbn) =>
        Context.Books.Include(b => b.BookCategories).FirstOrDefault(b => b.ISBN == isbn);

    public ICollection<Book> Get(int limit, int offset, out int total, Func<Book, object> orderBy,
        string isbn = "", string title = "", string author = "",
        DateTime publicationDate = default, string publisher = "", ICollection<string>? categoryNames = null)
    {
        var books = Context.Books.Include(b => b.BookCategories).AsQueryable();
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
        total = books.Count();

        return books.AsEnumerable()
            .OrderBy(orderBy)
            .Skip(offset)
            .Take(limit)
            .ToList();
    }
}