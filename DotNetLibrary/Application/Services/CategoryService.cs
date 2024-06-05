using DotNetLibrary.Application.Abstractions.Services;
using DotNetLibrary.Application.Exceptions;
using DotNetLibrary.Application.Models.DTOs;
using DotNetLibrary.Models.Entities;
using DotNetLibrary.Models.Repositories;

namespace DotNetLibrary.Application.Services;

public class CategoryService(CategoryRepository categoryRepository, BookCategoryRepository bookCategoryRepository)
    : ICategoryService
{
    public CategoryDTO Post(UserRole requesterRole, CategoryDTO category)
    {
        if (!requesterRole.IsLibraryStaff())
            throw new ForbiddenException(requesterRole, "add categories");
        if (categoryRepository.Exists(category.Name))
            throw new BadRequestException($"Category {category.Name} already exists");
        categoryRepository.Create(category.ToEntity());
        categoryRepository.SaveChanges();
        return new CategoryDTO(category);
    }

    public ICollection<CategoryDTO> Get(int from, int num, out int total, string ordering = "",
        string name = "", string description = "", ICollection<string>? bookISBNs = null) =>
        categoryRepository.Get(from, num, out total, ordering switch
            {
                "name" => c => c.Name,
                "description" => c => c.Description,
                _ => c => c.Name
            }, name, description, bookISBNs)
            .Select(c => new CategoryDTO(c))
            .ToList();

    public CategoryDTO Put(UserRole requesterRole, CategoryDTO category)
    {
        if (!requesterRole.IsLibraryStaff())
            throw new ForbiddenException(requesterRole, "modify categories");
        if (!categoryRepository.Exists(category.Name))
            throw new NotFoundException($"Category {category.Name}");
        categoryRepository.Update(category.ToEntity());
        categoryRepository.SaveChanges();
        return new CategoryDTO(category);
    }

    public void Delete(UserRole requesterRole, string name)
    {
        if (!requesterRole.IsLibraryStaff())
            throw new ForbiddenException(requesterRole, "delete categories");
        if (!categoryRepository.Exists(name))
            throw new NotFoundException($"Category {name}");
        if (bookCategoryRepository.GetByCategory(name).Any())
            throw new BadRequestException($"Category {name} is used by books");
        categoryRepository.Delete(name);
        categoryRepository.SaveChanges();
    }
}