using DotNetLibrary.Application.Abstractions;
using DotNetLibrary.Models.Entities;

namespace DotNetLibrary.Application.Models.DTOs;

public class BookDTO(
    string isbn,
    string title,
    string author,
    DateOnly publicationDate,
    string publisher,
    ICollection<CategoryDTO> bookCategories) : IDTO<Book>
{
    public BookDTO(Book book) : this(book.ISBN, book.Title, book.Author, book.PublicationDate, book.Publisher,
        book.BookCategories.Select(bc => new CategoryDTO(bc.Category)).ToList())
    {
    }

    public string ISBN { get; } = isbn;
    public string Title { get; } = title;
    public string Author { get; } = author;
    public DateOnly PublicationDate { get; } = publicationDate;
    public string Publisher { get; } = publisher;
    public ICollection<CategoryDTO> BookCategories { get; } = bookCategories;

    public Book ToEntity()
    {
        Book book = new()
        {
            ISBN = ISBN,
            Title = Title,
            Author = Author,
            PublicationDate = PublicationDate,
            Publisher = Publisher,
            BookCategories = BookCategories.Select(c => new BookCategory
            {
                BookISBN = ISBN,
                CategoryName = c.Name
            }).ToList()
        };
        foreach (var bc in book.BookCategories)
            bc.Book = book;
        return book;
    }
}