using DotNetLibrary.Application.Abstractions.Services;
using DotNetLibrary.Application.Exceptions;
using DotNetLibrary.Application.Extensions;
using DotNetLibrary.Application.Models.DTOs;
using DotNetLibrary.Models.Repositories;

namespace DotNetLibrary.Application.Services;

public class CategoryService(CategoryRepository categoryRepository, BookCategoryRepository bookCategoryRepository)
    : ICategoryService
{
    public CategoryDTO Post(CategoryDTO category)
    {
        if (categoryRepository.Exists(category.Name))
            throw new BadRequestException($"Category {category.Name} already exists");
        categoryRepository.Create(category.ToEntity());
        categoryRepository.SaveChanges();
        return new CategoryDTO(category);
    }

    public CategoryDTO Get(string name)
    {
        var category = categoryRepository.GetByName(name);
        if (category == null)
            throw new NotFoundException($"Category {name}");
        return new CategoryDTO(category);
    }

    public ICollection<CategoryDTO> Get(int limit, int offset, out int total, string orderBy = "",
        string name = "", string description = "", ICollection<string>? bookISBNs = null) =>
        categoryRepository.Get(limit, offset, out total, orderBy switch
            {
                "name" => c => c.Name,
                "description" => c => c.Description,
                _ => c => c.Name
            }, name, description, bookISBNs
                .Select(bi => ISBN.TryParse(bi, out var isbnObj)
                    ? isbnObj.ToDashedString()
                    : bi).ToList())
            .Select(c => new CategoryDTO(c))
            .ToList();

    public CategoryDTO Patch(CategoryDTO newCategory)
    {
        var category = categoryRepository.GetByName(newCategory.Name);
        if (category == null)
            throw new NotFoundException($"Category {newCategory.Name}");
        if (newCategory.Description != null)
            category.Description = newCategory.Description;
        categoryRepository.Update(category);
        categoryRepository.SaveChanges();
        return new CategoryDTO(newCategory);
    }

    public void Delete(string name)
    {
        if (!categoryRepository.Exists(name))
            throw new NotFoundException($"Category {name}");
        if (bookCategoryRepository.GetByCategory(name).Count != 0)
            throw new BadRequestException($"Category {name} is used by some books");
        categoryRepository.Delete(name);
        categoryRepository.SaveChanges();
    }
}