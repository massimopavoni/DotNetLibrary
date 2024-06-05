using DotNetLibrary.Models.Context;
using DotNetLibrary.Models.Entities;

namespace DotNetLibrary.Models.Repositories;

public class CategoryRepository(LibraryContext context) : GenericRepository<Category, string>(context)
{
    public ICollection<Category> Get() =>
        Context.Categories.ToList();

    public ICollection<Category> Get(int from, int num, out int total, Func<Category, object> ordering,
        string name = "", string description = "", ICollection<string>? bookISBNs = null)
    {
        var categories = Context.Categories.AsQueryable();
        total = categories.Count();
        if (!string.IsNullOrWhiteSpace(name))
            categories = categories.Where(c => c.Name.Contains(name));
        if (!string.IsNullOrWhiteSpace(description))
            categories = categories.Where(c => c.Description.Contains(description));
        if (bookISBNs is { Count: > 0 })
            categories = categories.Where(c => c.CategoryBooks.Any(bc => bookISBNs.Contains(bc.BookISBN)));

        return categories.AsEnumerable()
            .OrderBy(ordering)
            .Skip(from)
            .Take(num)
            .ToList();
    }
}