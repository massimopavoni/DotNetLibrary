using DotNetLibrary.Application.Abstractions.Services;
using DotNetLibrary.Application.Exceptions;
using DotNetLibrary.Application.Models.DTOs;
using DotNetLibrary.Models.Entities;
using DotNetLibrary.Models.Repositories;

namespace DotNetLibrary.Application.Services;

public class BookService(
    BookRepository bookRepository,
    BookCategoryRepository bookCategoryRepository,
    CategoryRepository categoryRepository) : IBookService
{
    public BookDTO Post(UserRole requesterRole, BookDTO book)
    {
        if (!requesterRole.IsLibraryStaff())
            throw new ForbiddenException(requesterRole, "add books");
        if (bookRepository.Exists(book.ISBN))
            throw new BadRequestException($"Book {book.ISBN} already exists");
        var bookEntity = book.ToEntity();
        return UsingTransaction(() =>
        {
            bookRepository.Create(bookEntity);
            if (bookEntity.BookCategories.Count != 0)
                CreateBookCategories(bookEntity.BookCategories);
            return new BookDTO(bookEntity);
        });
    }

    public ICollection<BookDTO> Get(int from, int num, out int total, string ordering = "",
        string isbn = "", string title = "", string author = "", DateOnly publicationDate = default,
        string publisher = "", ICollection<string>? categoryNames = null) =>
        bookRepository.Get(from, num, out total, ordering switch
            {
                "isbn" => b => b.ISBN,
                "title" => b => b.Title,
                "author" => b => b.Author,
                "publicationDate" => b => b.PublicationDate.ToString(),
                "publisher" => b => b.Publisher,
                _ => b => b.ISBN
            }, isbn, title, author, publicationDate, publisher, categoryNames)
            .Select(b => new BookDTO(b))
            .ToList();

    public BookDTO Put(UserRole requesterRole, BookDTO book)
    {
        if (!requesterRole.IsLibraryStaff())
            throw new ForbiddenException(requesterRole, "modify books");
        if (!bookRepository.Exists(book.ISBN))
            throw new NotFoundException($"Book {book.ISBN}");
        var bookEntity = book.ToEntity();
        var categories = categoryRepository.Get().Select(c => c.Name).ToHashSet();
        foreach (var bc in bookEntity.BookCategories)
            if (!categories.Contains(bc.CategoryName))
                throw new NotFoundException($"Category {bc.CategoryName}");
        return UsingTransaction(() =>
        {
            bookRepository.Update(bookEntity);
            if (bookEntity.BookCategories.Count != 0)
            {
                bookCategoryRepository.DeleteByBook(bookEntity.ISBN);
                CreateBookCategories(bookEntity.BookCategories);
            }

            return new BookDTO(bookEntity);
        });
    }

    public void Delete(UserRole requesterRole, string isbn)
    {
        if (!requesterRole.IsLibraryStaff())
            throw new ForbiddenException(requesterRole, "delete books");
        if (!bookRepository.Exists(isbn))
            throw new NotFoundException($"Book {isbn}");
        UsingTransaction<object?>(() =>
        {
            bookCategoryRepository.DeleteByBook(isbn);
            bookRepository.Delete(isbn);
            return null;
        });
    }

    private T UsingTransaction<T>(Func<T> func)
    {
        using var transaction = bookRepository.BeginTransaction();
        try
        {
            var result = func.Invoke();
            transaction.Commit();
            return result;
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw new InternalServerErrorException("Database error", e);
        }
    }

    private void CreateBookCategories(ICollection<BookCategory> bookCategories)
    {
        foreach (var bc in bookCategories)
            bookCategoryRepository.Create(bc);
    }
}