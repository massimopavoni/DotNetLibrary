using DotNetLibrary.Application.Abstractions;
using DotNetLibrary.Application.Utils;
using DotNetLibrary.Models.Entities;

namespace DotNetLibrary.Application.Models.DTOs;

public class UserDTO(string emailAddress, string password, UserRole role, string firstName = "", string lastName = "")
    : IDTO<User>
{
    public UserDTO(UserDTO user) : this(user.EmailAddress, "", user.Role, user.FirstName, user.LastName)
    {
    }

    public UserDTO(User user) : this(user.EmailAddress, "", user.Role, user.FirstName, user.LastName)
    {
    }

    public string EmailAddress { get; } = emailAddress;
    public string Password { get; } = password;
    public UserRole Role { get; } = role;
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;

    public User ToEntity() =>
        new()
        {
            EmailAddress = EmailAddress,
            PasswordHash = PasswordHashing.HashPassword(Password),
            Role = Role,
            FirstName = FirstName,
            LastName = LastName
        };
}