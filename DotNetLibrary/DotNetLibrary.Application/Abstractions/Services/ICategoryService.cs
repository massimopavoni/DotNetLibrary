using DotNetLibrary.Application.Models.DTOs;

namespace DotNetLibrary.Application.Abstractions.Services;

public interface ICategoryService
{
    public CategoryDTO Post(CategoryDTO category);

    public CategoryDTO Get(string name);

    public ICollection<CategoryDTO> Get(int limit, int offset, out int total, string orderBy = "",
        string name = "", string description = "", ICollection<string>? bookISBNs = null);

    public CategoryDTO Patch(CategoryDTO newCategory);

    public void Delete(string name);
}