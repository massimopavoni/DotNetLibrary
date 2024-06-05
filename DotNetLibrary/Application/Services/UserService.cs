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
            throw new UnauthorizedException("Invalid password");
        return new UserDTO(user);
    }

    public UserDTO Get(UserRole requesterRole, string requesterEmailAddress, string emailAddress)
    {
        if (!requesterRole.IsLibraryStaff() && requesterEmailAddress != emailAddress)
            throw new ForbiddenException(requesterRole, "get another user's info");
        var user = repository.GetByEmailAddress(emailAddress);
        if (user == null)
            throw new NotFoundException($"User {emailAddress}");
        return new UserDTO(user);
    }

    public ICollection<UserDTO> Get(UserRole requesterRole, int from, int num, out int total, string ordering = "",
        string emailAddress = "", UserRole? role = null, string firstName = "", string lastName = "")
    {
        if (!requesterRole.IsLibraryStaff())
            throw new ForbiddenException(requesterRole, "get other users' info");
        return repository.Get(from, num, out total, ordering switch
            {
                "emailAddress" => u => u.EmailAddress,
                "role" => u => u.Role,
                "firstName" => u => u.FirstName,
                "lastName" => u => u.LastName,
                _ => u => u.EmailAddress
            }, emailAddress, role, firstName, lastName)
            .Select(u => new UserDTO(u))
            .ToList();
    }

    public UserDTO Put(UserRole requesterRole, string requesterEmailAddress, UserDTO newUser)
    {
        var user = repository.GetByEmailAddress(requesterEmailAddress);
        if (user == null)
            throw new NotFoundException($"User {requesterEmailAddress}");
        if (user.EmailAddress != newUser.EmailAddress)
            throw new ForbiddenException(requesterRole, "change other users' info");
        repository.Update(newUser.ToEntity());
        repository.SaveChanges();
        return new UserDTO(newUser);
    }

    public void Delete(UserRole requesterRole, string requesterEmailAddress, string emailAddress)
    {
        if (!requesterRole.IsAdmin() && requesterEmailAddress != emailAddress)
            throw new ForbiddenException(requesterRole, "delete other users");
        if (!repository.Exists(emailAddress))
            throw new NotFoundException($"User {emailAddress}");
        repository.Delete(emailAddress);
        repository.SaveChanges();
    }
}