using DotNetLibrary.Application.Models.DTOs;

namespace DotNetLibrary.Application.Abstractions.Services;

public interface IBookService
{
    public BookDTO Post(BookDTO book);

    public BookDTO Get(string isbn);

    public ICollection<BookDTO> Get(int limit, int offset, out int total, string orderBy = "",
        string isbn = "", string title = "", string author = "", DateTime publicationDate = default,
        string publisher = "", ICollection<string>? categoryNames = null);

    public BookDTO Patch(BookDTO newBook);

    public void Delete(string isbn);
}