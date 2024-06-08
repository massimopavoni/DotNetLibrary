namespace DotNetLibrary.Application.Models.Requests;

public class ModifyUserRequest(
    string? password,
    string? firstName,
    string? lastName)
{
    public string? Password { get; } = password;
    public string? FirstName { get; } = firstName;
    public string? LastName { get; } = lastName;
}