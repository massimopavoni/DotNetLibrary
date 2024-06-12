using DotNetLibrary.Application.Abstractions;
using DotNetLibrary.Models.Entities;

namespace DotNetLibrary.Application.Models.DTOs;

public class BookDTO(
    string isbn,
    string title,
    string author,
    DateOnly publicationDate,
    string publisher,
    ICollection<string> categoryNames) : IDTO<Book>
{
    public BookDTO(BookDTO book) : this(book.ISBN, book.Title, book.Author,
        book.PublicationDate, book.Publisher, book.CategoryNames)
    {
    }

    public BookDTO(Book book) : this(book.ISBN, book.Title, book.Author,
        DateOnly.FromDateTime(book.PublicationDate), book.Publisher,
        book.BookCategories.Select(bc => bc.CategoryName).ToList())
    {
    }

    public string ISBN { get; set; } = isbn;
    public string Title { get; } = title;
    public string Author { get; } = author;
    public DateOnly PublicationDate { get; } = publicationDate;
    public string Publisher { get; } = publisher;
    public ICollection<string> CategoryNames { get; } = categoryNames;

    public Book ToEntity()
    {
        Book book = new()
        {
            ISBN = ISBN,
            Title = Title,
            Author = Author,
            PublicationDate = PublicationDate.ToDateTime(TimeOnly.MinValue),
            Publisher = Publisher
        };
        return book;
    }
}