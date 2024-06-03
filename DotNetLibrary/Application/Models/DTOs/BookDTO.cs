using Application.Abstractions;
using Models.Entities;

namespace Application.Models.DTOs;

public class BookDTO(string isbn, string title, string author, DateOnly publicationDate, string publisher) : IDTO<Book>
{
    public BookDTO(Book book) : this(book.ISBN, book.Title, book.Author, book.PublicationDate, book.Publisher)
    {
    }

    public string ISBN { get; } = isbn;
    public string Title { get; } = title;
    public string Author { get; } = author;
    public DateOnly PublicationDate { get; } = publicationDate;
    public string Publisher { get; } = publisher;

    public Book ToEntity() => new()
    {
        ISBN = ISBN,
        Title = Title,
        Author = Author,
        PublicationDate = PublicationDate,
        Publisher = Publisher
    };
}