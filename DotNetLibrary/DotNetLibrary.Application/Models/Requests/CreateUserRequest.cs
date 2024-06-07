using System.ComponentModel.DataAnnotations;

namespace DotNetLibrary.Application.Models.Requests;

public class CreateUserRequest(
    string emailAddress,
    string password,
    string? firstName,
    string? lastName)
{
    [Required] public string EmailAddress { get; } = emailAddress;

    [Required] public string Password { get; } = password;
    public string? FirstName { get; } = firstName;
    public string? LastName { get; } = lastName;
}