namespace DotNetLibrary.Application.Models.Requests;

public class UpdateBookRequest(
    string? title,
    string? author,
    DateOnly? publicationDate,
    string? publisher,
    ICollection<string>? categoryNames)
{
    public string? Title { get; } = title;
    public string? Author { get; } = author;
    public DateOnly? PublicationDate { get; } = publicationDate;
    public string? Publisher { get; } = publisher;
    public ICollection<string>? CategoryNames { get; } = categoryNames;
}