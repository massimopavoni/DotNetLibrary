using DotNetLibrary.API.Extensions;
using DotNetLibrary.Application.Abstractions.Services;
using DotNetLibrary.Application.Exceptions;
using DotNetLibrary.Application.Factories;
using DotNetLibrary.Application.Models.DTOs;
using DotNetLibrary.Application.Models.Requests;
using DotNetLibrary.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetLibrary.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BooksController(IBookService bookService) : LibraryBaseController
{
    [HttpPost]
    public IActionResult Post(CreateBookRequest request)
    {
        var userRole = User.UserRole();
        if (!userRole.IsLibraryStaff())
            return Forbidden(ResponseFactory.WithJustError(
                new ForbiddenException(userRole, "create books")));
        var book = new BookDTO(request.ISBN, request.Title, request.Author,
            request.PublicationDate, request.Publisher, request.CategoryNames);
        try
        {
            book = bookService.Post(book);
            return CreatedAtAction(nameof(Get), new { book.ISBN },
                ResponseFactory.WithSuccess(book));
        }
        catch (BadRequestException e)
        {
            return BadRequest(ResponseFactory.WithError(e,
                new BookDTO(book)));
        }
    }

    [HttpGet("{isbn}")]
    public IActionResult Get(string isbn)
    {
        try
        {
            return Ok(ResponseFactory.WithSuccess(bookService.Get(isbn)));
        }
        catch (NotFoundException e)
        {
            return NotFound(ResponseFactory.WithError(e,
                new BookDTO(isbn, "", "", DateOnly.FromDateTime(DateTime.Today), "", [])));
        }
    }

    [HttpGet]
    public IActionResult Get([FromQuery] int limit, [FromQuery] int offset, [FromQuery] string orderBy = "",
        [FromQuery] string isbn = "", [FromQuery] string title = "", [FromQuery] string author = "",
        [FromQuery] DateOnly publicationDate = default, [FromQuery] string publisher = "",
        [FromQuery] string categoryNames = "")
    {
        ICollection<string> categoryNamesFilter = string.IsNullOrWhiteSpace(categoryNames)
            ? []
            : categoryNames.Split(',').ToList();
        limit = limit == default ? 10 : limit;
        var result = bookService.Get(limit, offset, out var total,
            orderBy, isbn, title, author, publicationDate.ToDateTime(TimeOnly.MinValue), publisher,
            categoryNamesFilter);
        var (nextOffset, previousOffset) = ComputePaginationOffsets(limit, offset);
        return Ok(ResponseFactory.WithSuccess(total, offset, result,
            nextOffset < total
                ? Url.Action(nameof(Get), new
                {
                    limit, offset = nextOffset, orderBy,
                    isbn, title, author, publicationDate, publisher, categoryNames
                })
                : null,
            offset > 0
                ? Url.Action(nameof(Get), new
                {
                    limit, offset = previousOffset, orderBy,
                    isbn, title, author, publicationDate, publisher, categoryNames
                })
                : null));
    }

    [HttpPatch("{isbn}")]
    public IActionResult Patch(string isbn, UpdateBookRequest request)
    {
        var userRole = User.UserRole();
        if (!userRole.IsLibraryStaff())
            return Forbidden(ResponseFactory.WithJustError(
                new ForbiddenException(userRole, "update books")));
        try
        {
            return Ok(ResponseFactory.WithSuccess(bookService.Patch(
                new BookDTO(isbn, request.Title ?? "", request.Author ?? "",
                    request.PublicationDate ?? default, request.Publisher ?? "", request.CategoryNames ?? []))));
        }
        catch (BadRequestException e)
        {
            return BadRequest(ResponseFactory.WithError(e,
                new BookDTO(isbn, request.Title ?? "", request.Author ?? "",
                    request.PublicationDate ?? default, request.Publisher ?? "", request.CategoryNames ?? [])));
        }
        catch (NotFoundException e)
        {
            return NotFound(ResponseFactory.WithError(e,
                new BookDTO(isbn, "", "", DateOnly.FromDateTime(DateTime.Today), "", [])));
        }
    }

    [HttpDelete("{isbn}")]
    public IActionResult Delete(string isbn)
    {
        var userRole = User.UserRole();
        if (!userRole.IsLibraryStaff())
            return Forbidden(ResponseFactory.WithJustError(
                new ForbiddenException(userRole, "delete books")));
        try
        {
            bookService.Delete(isbn);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(ResponseFactory.WithError(e,
                new BookDTO(isbn, "", "", DateOnly.FromDateTime(DateTime.Today), "", [])));
        }
    }
}