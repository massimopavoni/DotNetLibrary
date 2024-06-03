using Models.Entities;

namespace Models.Repositories;

public class CategoryRepository(LibraryContext context) : GenericRepository<Category, int>(context)
{
    public List<Category> Get() =>
        _context.Categories.ToList();

    public Category? Get(string name) =>
        _context.Categories.FirstOrDefault(c => c.Name == name);
}