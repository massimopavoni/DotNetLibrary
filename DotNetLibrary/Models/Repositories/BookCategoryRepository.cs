using Models.Entities;

namespace Models.Repositories;

public class BookCategoryRepository(LibraryContext context) : GenericRepository<BookCategory, string>(context)
{
    public List<Category> Get(string isbn) =>
        _context.BookCategories.Where(bc => bc.BookISBN == isbn)
            .Select(bc => bc.Category)
            .ToList();

    public List<Book> Get(long categoryID) =>
        _context.BookCategories.Where(bc => bc.CategoryID == categoryID)
            .Select(bc => bc.Book)
            .ToList();

    public override void Delete(string bookISBN) =>
        _context.BookCategories.RemoveRange(_context.BookCategories.Where(bc => bc.BookISBN == bookISBN));

    public void Delete(long categoryID) =>
        _context.BookCategories.RemoveRange(_context.BookCategories.Where(bc => bc.CategoryID == categoryID));
}