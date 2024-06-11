namespace DotNetLibrary.Models.Entities;

public enum UserRole
{
    User = 0,
    Librarian = 1,
    Admin = 2
}

public static class UserRoleExtensions
{
    public static bool IsOneOf(this UserRole role, params UserRole[] roles) =>
        roles.Contains(role);

    public static bool IsAdmin(this UserRole role) =>
        role == UserRole.Admin;

    public static bool IsLibraryStaff(this UserRole role) =>
        role.IsOneOf(UserRole.Librarian, UserRole.Admin);
}