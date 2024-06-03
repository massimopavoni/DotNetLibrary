namespace Models.Entities;

public class User
{
    public string EmailAddress { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Password { get; init; }
    public bool IsAdmin { get; init; }
}