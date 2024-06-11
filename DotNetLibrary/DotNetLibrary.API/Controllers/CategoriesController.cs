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
public class CategoriesController(ICategoryService categoryService) : LibraryBaseController
{
    [HttpPost]
    public IActionResult Post(CreateCategoryRequest request)
    {
        var userRole = User.UserRole();
        if (!userRole.IsLibraryStaff())
            return Forbidden(ResponseFactory.WithJustError(
                new ForbiddenException(userRole, "create categories")));
        var category = new CategoryDTO(request.Name, request.Description);
        try
        {
            category = categoryService.Post(category);
            return CreatedAtAction(nameof(Get), new { category.Name },
                ResponseFactory.WithSuccess(category));
        }
        catch (BadRequestException e)
        {
            return BadRequest(ResponseFactory.WithError(e,
                new CategoryDTO(category)));
        }
    }

    [HttpGet("{name}")]
    public IActionResult Get(string name)
    {
        try
        {
            return Ok(ResponseFactory.WithSuccess(categoryService.Get(name)));
        }
        catch (NotFoundException e)
        {
            return NotFound(ResponseFactory.WithError(e,
                new CategoryDTO(name)));
        }
    }

    [HttpGet]
    public IActionResult Get([FromQuery] int limit, [FromQuery] int offset, [FromQuery] string orderBy = "",
        [FromQuery] string name = "", [FromQuery] string description = "", [FromQuery] string bookISBNs = "")
    {
        ICollection<string> bookISBNsFilter = string.IsNullOrWhiteSpace(bookISBNs)
            ? []
            : bookISBNs.Split(',').ToList();
        limit = limit == default ? 10 : limit;
        var result = categoryService.Get(limit, offset, out var total,
            orderBy, name, description, bookISBNsFilter);
        var (nextOffset, previousOffset) = ComputePaginationOffsets(limit, offset);
        return Ok(ResponseFactory.WithSuccess(total, offset, result,
            nextOffset < total
                ? Url.Action(nameof(Get), new
                {
                    limit, offset = nextOffset, orderBy,
                    name, description, bookISBNs
                })
                : null,
            offset > 0
                ? Url.Action(nameof(Get), new
                {
                    limit, offset = previousOffset, orderBy,
                    name, description, bookISBNs
                })
                : null));
    }

    [HttpPatch("{name}")]
    public IActionResult Patch(string name, UpdateCategoryRequest request)
    {
        var userRole = User.UserRole();
        if (!userRole.IsLibraryStaff())
            return Forbidden(ResponseFactory.WithJustError(
                new ForbiddenException(userRole, "update categories")));
        try
        {
            return Ok(ResponseFactory.WithSuccess(categoryService.Patch(
                new CategoryDTO(name, request.Description))));
        }
        catch (NotFoundException e)
        {
            return NotFound(ResponseFactory.WithError(e,
                new CategoryDTO(name)));
        }
    }

    [HttpDelete("{name}")]
    public IActionResult Delete(string name)
    {
        var userRole = User.UserRole();
        if (!userRole.IsLibraryStaff())
            return Forbidden(ResponseFactory.WithJustError(
                new ForbiddenException(userRole, "delete categories")));
        try
        {
            categoryService.Delete(name);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(ResponseFactory.WithError(e,
                new CategoryDTO(name)));
        }
        catch (BadRequestException e)
        {
            return BadRequest(ResponseFactory.WithError(e,
                new CategoryDTO(name)));
        }
    }
}