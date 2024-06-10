namespace DotNetLibrary.Application.Models.Requests;

public class CreateBookRequest(
    string isbn,
    string title,
    string author,
    DateOnly publicationDate,
    string publisher,
    ICollection<string> categoryNames)
{
    public string ISBN { get; } = isbn;
    public string Title { get; } = title;
    public string Author { get; } = author;
    public DateOnly PublicationDate { get; } = publicationDate;
    public string Publisher { get; } = publisher;
    public ICollection<string> CategoryNames { get; } = categoryNames;
}