using DotNetLibrary.Application.Abstractions.Services;
using DotNetLibrary.Application.Exceptions;
using DotNetLibrary.Application.Factories;
using DotNetLibrary.Application.Models.DTOs;
using DotNetLibrary.Application.Models.Requests;
using DotNetLibrary.Application.Models.Responses;
using DotNetLibrary.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DotNetLibrary.API.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class UserController(IUserService userService, ITokenService tokenService) : ControllerBase
{
    [HttpPost]
    public IActionResult SignUp(CreateUserRequest request)
    {
        var user = new UserDTO(request.EmailAddress, request.Password, UserRole.User,
            request.FirstName, request.LastName);
        try
        {
            user = userService.SignUp(user);
            return Ok(ResponseFactory.WithSuccess(user));
        }
        catch (BadRequestException e)
        {
            return BadRequest(ResponseFactory.WithError(e, new UserDTO(user)));
        }
    }

    [HttpPost]
    public IActionResult LogIn(CreateTokenRequest request)
    {
        try
        {
            return Ok(ResponseFactory.WithSuccess(
                new CreateTokenResponse(tokenService.CreateToken(request))));
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
}