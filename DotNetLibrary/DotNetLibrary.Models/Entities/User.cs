namespace DotNetLibrary.Models.Entities;

public class User
{
    public string EmailAddress { get; init; }
    public string PasswordHash { get; init; }
    public UserRole Role { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
}