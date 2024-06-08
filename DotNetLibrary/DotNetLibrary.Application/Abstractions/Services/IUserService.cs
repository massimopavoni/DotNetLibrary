using DotNetLibrary.Application.Models.DTOs;
using DotNetLibrary.Models.Entities;

namespace DotNetLibrary.Application.Abstractions.Services;

public interface IUserService
{
    public UserDTO SignUp(UserDTO user);

    public UserDTO LogIn(string emailAddress, string password);

    public UserDTO Get(string emailAddress);

    public ICollection<UserDTO> Get(int limit, int offset, out int total, string orderBy = "",
        string emailAddress = "", UserRole? role = null, string firstName = "", string lastName = "");

    public UserDTO Patch(UserDTO newUser);

    public void Delete(string emailAddress);
}