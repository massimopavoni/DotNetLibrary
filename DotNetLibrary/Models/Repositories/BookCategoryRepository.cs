using DotNetLibrary.Models.Context;
using DotNetLibrary.Models.Entities;

namespace DotNetLibrary.Models.Repositories;

public class BookCategoryRepository(LibraryContext context) : GenericRepository<BookCategory, string>(context)
{
    public ICollection<Category> GetByBook(string bookISBN) =>
        Context.BookCategories.Where(bc => bc.BookISBN == bookISBN)
            .Select(bc => bc.Category).ToList();

    public ICollection<Book> GetByCategory(string categoryName) =>
        Context.BookCategories.Where(bc => bc.CategoryName == categoryName)
            .Select(bc => bc.Book).ToList();

    public void DeleteByBook(string bookISBN) =>
        Context.BookCategories.RemoveRange(Context.BookCategories.Where(bc => bc.BookISBN == bookISBN));

    public void DeleteByCategory(string categoryName) =>
        Context.BookCategories.RemoveRange(Context.BookCategories.Where(bc => bc.CategoryName == categoryName));
}