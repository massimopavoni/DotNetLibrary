using BCrypt.Net;

namespace DotNetLibrary.Application.Utils;

public static class PasswordHashing
{
    private const int WorkFactor = 14;
    private const HashType HashType = BCrypt.Net.HashType.SHA384;

    public static string HashPassword(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password, WorkFactor, HashType);

    public static bool VerifyPassword(string password, string hash) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
}