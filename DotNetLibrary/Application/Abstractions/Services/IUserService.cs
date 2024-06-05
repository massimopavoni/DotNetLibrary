using DotNetLibrary.Application.Models.DTOs;
using DotNetLibrary.Models.Entities;

namespace DotNetLibrary.Application.Abstractions.Services;

public interface IUserService
{
    public UserDTO SignUp(UserDTO user);

    public UserDTO LogIn(string emailAddress, string password);

    public UserDTO Get(UserRole requesterRole, string requesterEmailAddress, string emailAddress);

    public ICollection<UserDTO> Get(UserRole requesterRole, int from, int num, out int total, string ordering = "",
        string emailAddress = "", UserRole? role = null, string firstName = "", string lastName = "");

    public UserDTO Put(UserRole requesterRole, string requesterEmailAddress, UserDTO user);

    public void Delete(UserRole requesterRole, string requesterEmailAddress, string emailAddress);
}