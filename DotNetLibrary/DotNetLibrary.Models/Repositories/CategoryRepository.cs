using DotNetLibrary.Models.Context;
using DotNetLibrary.Models.Entities;

namespace DotNetLibrary.Models.Repositories;

public class CategoryRepository(LibraryContext context) : GenericRepository<Category, string>(context)
{
    public Category? GetByName(string name) =>
        Read(name);

    public ICollection<Category> Get() =>
        Context.Categories.ToList();

    public ICollection<Category> Get(int limit, int offset, out int total, Func<Category, object> orderBy,
        string name = "", string description = "", ICollection<string>? bookISBNs = null)
    {
        var categories = Context.Categories.AsQueryable();
        if (!string.IsNullOrWhiteSpace(name))
            categories = categories.Where(c => c.Name.Contains(name));
        if (!string.IsNullOrWhiteSpace(description))
            categories = categories.Where(c => c.Description != null && c.Description.Contains(description));
        if (bookISBNs is { Count: > 0 })
            categories = categories.Where(c => c.CategoryBooks.Any(bc => bookISBNs.Contains(bc.BookISBN)));
        total = categories.Count();

        return categories.AsEnumerable()
            .OrderBy(orderBy)
            .Skip(offset)
            .Take(limit)
            .ToList();
    }
}