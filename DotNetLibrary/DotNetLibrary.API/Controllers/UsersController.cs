using DotNetLibrary.API.Extensions;
using DotNetLibrary.Application.Abstractions.Services;
using DotNetLibrary.Application.Exceptions;
using DotNetLibrary.Application.Factories;
using DotNetLibrary.Application.Models.DTOs;
using DotNetLibrary.Application.Models.Requests;
using DotNetLibrary.Application.Models.Responses;
using DotNetLibrary.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetLibrary.API.Controllers;

public class UsersController(IUserService userService, ITokenService tokenService) : LibraryBaseController
{
    [HttpPost]
    [Route("signup")]
    public IActionResult SignUp(CreateUserRequest request)
    {
        var user = new UserDTO(request.EmailAddress, request.Password, UserRole.User,
            request.FirstName, request.LastName);
        try
        {
            user = userService.SignUp(user);
            return CreatedAtAction(nameof(Get), new { user.EmailAddress },
                ResponseFactory.WithSuccess(user));
        }
        catch (BadRequestException e)
        {
            return BadRequest(ResponseFactory.WithError(e,
                new UserDTO(user)));
        }
    }

    [HttpPost]
    [Route("login")]
    public IActionResult LogIn(CreateTokenRequest request)
    {
        try
        {
            return Ok(ResponseFactory.WithSuccess(
                new CreateTokenResponse(tokenService.Post(request))));
        }
        catch (NotFoundException e)
        {
            return NotFound(ResponseFactory.WithError(e,
                new UserDTO(request.EmailAddress, "", UserRole.User)));
        }
        catch (UnauthorizedException e)
        {
            return Unauthorized(ResponseFactory.WithError(e,
                new UserDTO(request.EmailAddress, "", UserRole.User)));
        }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("{emailAddress}")]
    public IActionResult Get(string emailAddress)
    {
        var userRole = User.UserRole();
        if (!userRole.IsLibraryStaff() && User.EmailAddress() != emailAddress)
            return Forbidden(ResponseFactory.WithJustError(
                new ForbiddenException(userRole, "read another user's info")));
        try
        {
            return Ok(ResponseFactory.WithSuccess(userService.Get(emailAddress)));
        }
        catch (NotFoundException e)
        {
            return NotFound(ResponseFactory.WithError(e,
                new UserDTO(emailAddress, "", UserRole.User)));
        }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet]
    public IActionResult Get([FromQuery] int limit, [FromQuery] int offset, [FromQuery] string orderBy = "",
        [FromQuery] string emailAddress = "", [FromQuery] string role = "",
        [FromQuery] string firstName = "", [FromQuery] string lastName = "")
    {
        var userRole = User.UserRole();
        if (!userRole.IsLibraryStaff())
            return Forbidden(ResponseFactory.WithJustError(
                new ForbiddenException(userRole, "read other users' info")));
        UserRole? roleFilter =
            string.IsNullOrWhiteSpace(role) || !Enum.TryParse<UserRole>(role, out var r)
                ? null
                : r;
        var result = userService.Get(limit == default ? 10 : limit, offset, out var total,
            orderBy, emailAddress, roleFilter, firstName, lastName);
        return Ok(ResponseFactory.WithSuccess(total, offset, result));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPatch("{emailAddress}")]
    public IActionResult Patch(string emailAddress, UpdateUserRequest request)
    {
        var userRole = User.UserRole();
        if (User.EmailAddress() != emailAddress)
            return Forbidden(ResponseFactory.WithJustError(
                new ForbiddenException(userRole, "update other users' info")));
        try
        {
            return Ok(ResponseFactory.WithSuccess(userService.Patch(
                new UserDTO(emailAddress, request.Password ?? "", UserRole.User,
                    request.FirstName, request.LastName))));
        }
        catch (NotFoundException e)
        {
            return NotFound(ResponseFactory.WithError(e,
                new UserDTO(emailAddress, "", UserRole.User)));
        }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete("{emailAddress}")]
    public IActionResult Delete(string emailAddress)
    {
        var userRole = User.UserRole();
        if (!userRole.IsAdmin() && User.EmailAddress() != emailAddress)
            return Forbidden(ResponseFactory.WithJustError(
                new ForbiddenException(userRole, "delete other users")));
        try
        {
            userService.Delete(emailAddress);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(ResponseFactory.WithError(e,
                new UserDTO(emailAddress, "", UserRole.User)));
        }
    }
}