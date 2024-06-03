using Models.Entities;

namespace Models.Repositories;

public class UserRepository(LibraryContext context) : GenericRepository<User, string>(context)
{
    public User? GetByEmail(string emailAddress) =>
        Read(emailAddress);

    public User? GetByFirstName(string firstName) =>
        _context.Users.FirstOrDefault(u => u.FirstName == firstName);

    public User? GetByLastName(string lastName) =>
        _context.Users.FirstOrDefault(u => u.LastName == lastName);

    public bool Exists(string emailAddress) =>
        GetByEmail(emailAddress) != null;
}