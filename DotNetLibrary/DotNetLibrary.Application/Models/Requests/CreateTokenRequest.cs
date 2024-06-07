using System.ComponentModel.DataAnnotations;

namespace DotNetLibrary.Application.Models.Requests;

public class CreateTokenRequest(
    string emailAddress,
    string password)
{
    [Required] public string EmailAddress { get; } = emailAddress;

    [Required] public string Password { get; } = password;
}