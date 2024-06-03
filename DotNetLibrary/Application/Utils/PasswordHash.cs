using BCrypt.Net;

namespace Application.Utils;

public static class PasswordHash
{
    private const int WorkFactor = 18;
    private const HashType HashType = BCrypt.Net.HashType.SHA384;

    public static string NewPassword(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password, WorkFactor, HashType);

    public static bool VerifyPassword(string password, string hash) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
}