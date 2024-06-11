#pragma warning disable CS8618
namespace DotNetLibrary.Models.Entities;

public class User
{
    public string EmailAddress { get; init; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; init; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}