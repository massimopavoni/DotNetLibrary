using DotNetLibrary.Models.Context;
using DotNetLibrary.Models.Entities;

namespace DotNetLibrary.Models.Repositories;

public class UserRepository(LibraryContext context) : GenericRepository<User, string>(context)
{
    public User? GetByEmailAddress(string emailAddress) =>
        Read(emailAddress);

    public ICollection<User> Get(int limit, int offset, out int total, Func<User, object> orderBy,
        string emailAddress = "", UserRole? role = null, string firstName = "", string lastName = "")
    {
        var users = Context.Users.AsQueryable();
        if (!string.IsNullOrWhiteSpace(emailAddress))
            users = users.Where(u => u.EmailAddress.Contains(emailAddress));
        if (role != null)
            users = users.Where(u => u.Role == role);
        if (!string.IsNullOrWhiteSpace(firstName))
            users = users.Where(u => u.FirstName != null && u.FirstName.Contains(firstName));
        if (!string.IsNullOrWhiteSpace(lastName))
            users = users.Where(u => u.LastName != null && u.LastName.Contains(lastName));
        total = users.Count();

        return users.AsEnumerable()
            .OrderBy(orderBy)
            .Skip(offset)
            .Take(limit)
            .ToList();
    }
}