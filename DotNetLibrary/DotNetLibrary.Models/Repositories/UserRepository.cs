using DotNetLibrary.Models.Context;
using DotNetLibrary.Models.Entities;

namespace DotNetLibrary.Models.Repositories;

public class UserRepository(LibraryContext context) : GenericRepository<User, string>(context)
{
    public User? GetByEmailAddress(string emailAddress) =>
        Read(emailAddress);

    public ICollection<User> Get(int from, int num, out int total, Func<User, object> ordering,
        string emailAddress = "", UserRole? role = null, string firstName = "", string lastName = "")
    {
        var users = Context.Users.AsQueryable();
        total = users.Count();
        if (!string.IsNullOrWhiteSpace(emailAddress))
            users = users.Where(u => u.EmailAddress.Contains(emailAddress));
        if (role != null)
            users = users.Where(u => u.Role == role);
        if (!string.IsNullOrWhiteSpace(firstName))
            users = users.Where(u => u.FirstName.Contains(firstName));
        if (!string.IsNullOrWhiteSpace(lastName))
            users = users.Where(u => u.LastName.Contains(lastName));

        return users.AsEnumerable()
            .OrderBy(ordering)
            .Skip(from)
            .Take(num)
            .ToList();
    }
}