using DotNetLibrary.Application.Models.DTOs;
using DotNetLibrary.Models.Entities;

namespace DotNetLibrary.Application.Abstractions.Services;

public interface IBookService
{
    public BookDTO Post(UserRole requesterRole, BookDTO book);

    public ICollection<BookDTO> Get(int from, int num, out int total, string ordering = "",
        string isbn = "", string title = "", string author = "", DateOnly publicationDate = default,
        string publisher = "", ICollection<string>? categoryNames = null);

    public BookDTO Put(UserRole requesterRole, BookDTO book);

    public void Delete(UserRole requesterRole, string isbn);
}