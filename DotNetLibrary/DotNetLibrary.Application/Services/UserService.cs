using DotNetLibrary.Application.Abstractions.Services;
using DotNetLibrary.Application.Exceptions;
using DotNetLibrary.Application.Models.DTOs;
using DotNetLibrary.Application.Utils;
using DotNetLibrary.Models.Entities;
using DotNetLibrary.Models.Repositories;

namespace DotNetLibrary.Application.Services;

public class UserService(UserRepository repository)
    : IUserService
{
    public UserDTO SignUp(UserDTO user)
    {
        if (repository.Exists(user.EmailAddress))
            throw new BadRequestException($"User {user.EmailAddress} already exists");
        repository.Create(user.ToEntity());
        repository.SaveChanges();
        return new UserDTO(user);
    }

    public UserDTO LogIn(string emailAddress, string password)
    {
        var user = repository.GetByEmailAddress(emailAddress);
        if (user == null)
            throw new NotFoundException($"User {emailAddress}");
        if (!PasswordHashing.VerifyPassword(password, user.PasswordHash))
            throw new UnauthorizedException("Wrong password");
        return new UserDTO(user);
    }

    public UserDTO Get(string emailAddress)
    {
        var user = repository.GetByEmailAddress(emailAddress);
        if (user == null)
            throw new NotFoundException($"User {emailAddress}");
        return new UserDTO(user);
    }

    public ICollection<UserDTO> Get(int limit, int offset, out int total, string orderBy = "",
        string emailAddress = "", UserRole? role = null, string firstName = "", string lastName = "") =>
        repository.Get(limit, offset, out total, orderBy switch
            {
                "emailAddress" => u => u.EmailAddress,
                "role" => u => u.Role,
                "firstName" => u => u.FirstName,
                "lastName" => u => u.LastName,
                _ => u => u.EmailAddress
            }, emailAddress, role, firstName, lastName)
            .Select(u => new UserDTO(u))
            .ToList();

    public UserDTO Patch(UserDTO newUser)
    {
        var user = repository.GetByEmailAddress(newUser.EmailAddress);
        if (user == null)
            throw new NotFoundException($"User {newUser.EmailAddress}");
        if (!string.IsNullOrWhiteSpace(newUser.Password))
            user.PasswordHash = PasswordHashing.HashPassword(newUser.Password);
        if (newUser.FirstName != null)
            user.FirstName = newUser.FirstName;
        if (newUser.LastName != null)
            user.LastName = newUser.LastName;
        repository.Update(user);
        repository.SaveChanges();
        return new UserDTO(newUser);
    }

    public void Delete(string emailAddress)
    {
        if (!repository.Exists(emailAddress))
            throw new NotFoundException($"User {emailAddress}");
        repository.Delete(emailAddress);
        repository.SaveChanges();
    }
}