using Application.Abstractions;
using Application.Utils;
using Models.Entities;

namespace Application.Models.DTOs;

public class UserDTO(string emailAddress, string firstName, string lastName, string password, bool isAdmin)
    : IDTO<User>
{
    public UserDTO(User user) : this(user.EmailAddress, user.FirstName, user.LastName, "", user.IsAdmin)
    {
    }

    public string EmailAddress { get; } = emailAddress;
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
    public string Password { get; } = password;
    public bool IsAdmin { get; } = isAdmin;

    public User ToEntity() => new()
    {
        EmailAddress = EmailAddress,
        FirstName = FirstName,
        LastName = LastName,
        Password = PasswordHash.NewPassword(Password),
        IsAdmin = IsAdmin
    };
}