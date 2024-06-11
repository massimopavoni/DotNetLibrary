namespace DotNetLibrary.Application.Models.Requests;

public class CreateTokenRequest(
    string emailAddress,
    string password)
{
    public string EmailAddress { get; } = emailAddress;

    public string Password { get; } = password;
}