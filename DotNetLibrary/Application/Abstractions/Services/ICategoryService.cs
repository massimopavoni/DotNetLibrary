using DotNetLibrary.Application.Models.DTOs;
using DotNetLibrary.Models.Entities;

namespace DotNetLibrary.Application.Abstractions.Services;

public interface ICategoryService
{
    public CategoryDTO Post(UserRole requesterRole, CategoryDTO category);

    public ICollection<CategoryDTO> Get(int from, int num, out int total, string ordering = "",
        string name = "", string description = "", ICollection<string>? bookISBNs = null);

    public CategoryDTO Put(UserRole requesterRole, CategoryDTO category);

    public void Delete(UserRole requesterRole, string name);
}