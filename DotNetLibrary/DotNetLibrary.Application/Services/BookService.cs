using DotNetLibrary.Application.Abstractions.Services;
using DotNetLibrary.Application.Exceptions;
using DotNetLibrary.Application.Extensions;
using DotNetLibrary.Application.Models.DTOs;
using DotNetLibrary.Models.Entities;
using DotNetLibrary.Models.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DotNetLibrary.Application.Services;

public class BookService(
    BookRepository bookRepository,
    BookCategoryRepository bookCategoryRepository,
    CategoryRepository categoryRepository) : IBookService
{
    public BookDTO Post(BookDTO book)
    {
        if (!ISBN.TryParse(book.ISBN, out var isbn))
            throw new BadRequestException($"Invalid ISBN {book.ISBN}");
        book.ISBN = isbn.ToDashedString();
        if (bookRepository.Exists(book.ISBN))
            throw new BadRequestException($"Book {book.ISBN} already exists");
        CheckCategoryNames(book.CategoryNames);
        var bookEntity = book.ToEntity();
        return UsingTransaction(() =>
        {
            bookRepository.Create(bookEntity);
            if (book.CategoryNames.Count != 0)
                CreateBookCategories(bookEntity.ISBN, book.CategoryNames);
            return new BookDTO(bookEntity);
        });
    }

    public BookDTO Get(string isbn)
    {
        var book = bookRepository.GetByISBN(ISBN.TryParse(isbn, out var isbnObj)
            ? isbnObj.ToDashedString()
            : isbn);
        if (book == null)
            throw new NotFoundException($"Book {isbn}");
        return new BookDTO(book);
    }

    public ICollection<BookDTO> Get(int limit, int offset, out int total, string orderBy = "",
        string isbn = "", string title = "", string author = "", DateTime publicationDate = default,
        string publisher = "", ICollection<string>? categoryNames = null) =>
        bookRepository.Get(limit, offset, out total, orderBy switch
                {
                    "isbn" => b => b.ISBN,
                    "title" => b => b.Title,
                    "author" => b => b.Author,
                    "publicationDate" => b => b.PublicationDate,
                    "publisher" => b => b.Publisher,
                    _ => b => b.ISBN
                }, ISBN.TryParse(isbn, out var isbnObj)
                    ? isbnObj.ToDashedString()
                    : isbn,
                title, author, publicationDate, publisher, categoryNames)
            .Select(b => new BookDTO(b))
            .ToList();

    public BookDTO Patch(BookDTO newBook)
    {
        if (!ISBN.TryParse(newBook.ISBN, out var isbn))
            throw new BadRequestException($"Invalid ISBN {newBook.ISBN}");
        newBook.ISBN = isbn.ToDashedString();
        var book = bookRepository.GetByISBN(newBook.ISBN);
        if (book == null)
            throw new NotFoundException($"Book {newBook.ISBN}");
        CheckCategoryNames(newBook.CategoryNames);
        if (!string.IsNullOrWhiteSpace(newBook.Title))
            book.Title = newBook.Title;
        if (!string.IsNullOrWhiteSpace(newBook.Author))
            book.Author = newBook.Author;
        if (newBook.PublicationDate != default)
            book.PublicationDate = newBook.PublicationDate.ToDateTime(TimeOnly.MinValue);
        if (!string.IsNullOrWhiteSpace(newBook.Publisher))
            book.Publisher = newBook.Publisher;
        return UsingTransaction(() =>
        {
            bookRepository.Update(book);
            if (newBook.CategoryNames.Count != 0)
            {
                bookCategoryRepository.DeleteByBook(book.ISBN);
                CreateBookCategories(book.ISBN, newBook.CategoryNames);
            }

            return new BookDTO(book);
        });
    }

    public void Delete(string isbn)
    {
        if (!bookRepository.Exists(isbn))
            throw new NotFoundException($"Book {isbn}");
        UsingTransaction<object?>(() =>
        {
            bookCategoryRepository.DeleteByBook(isbn);
            bookRepository.Delete(isbn);
            return null;
        });
    }

    private T UsingTransaction<T>(Func<T> func) =>
        bookRepository.CreateExecutionStrategy().Execute(() =>
        {
            using var transaction = bookRepository.BeginTransaction();
            try
            {
                var result = func.Invoke();
                transaction.Commit();
                bookRepository.SaveChanges();
                return result;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw new InternalServerErrorException("Database error", e);
            }
        });

    private void CheckCategoryNames(ICollection<string> categoryNames)
    {
        var categories = categoryRepository.Get().Select(c => c.Name).ToHashSet();
        foreach (var cn in categoryNames)
            if (!categories.Contains(cn))
                throw new NotFoundException($"Category {cn}");
    }

    private void CreateBookCategories(string isbn, ICollection<string> categoryNames)
    {
        foreach (var cn in categoryNames)
            bookCategoryRepository.Create(new BookCategory
            {
                BookISBN = isbn,
                CategoryName = cn
            });
    }
}