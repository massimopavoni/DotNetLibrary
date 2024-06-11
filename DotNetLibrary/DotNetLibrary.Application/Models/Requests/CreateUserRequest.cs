namespace DotNetLibrary.Application.Models.Requests;

public class CreateUserRequest(
    string emailAddress,
    string password,
    string? firstName,
    string? lastName)
{
    public string EmailAddress { get; } = emailAddress;
    public string Password { get; } = password;
    public string? FirstName { get; } = firstName;
    public string? LastName { get; } = lastName;
}