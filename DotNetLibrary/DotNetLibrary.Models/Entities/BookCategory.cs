#pragma warning disable CS8618
namespace DotNetLibrary.Models.Entities;

public class BookCategory
{
    public string BookISBN { get; init; }
    public string CategoryName { get; init; }

    public Book? Book { get; set; }
    public Category Category { get; init; }
}