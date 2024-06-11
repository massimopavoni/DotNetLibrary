using System.Security.Claims;
using DotNetLibrary.Models.Entities;

namespace DotNetLibrary.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static UserRole UserRole(this ClaimsPrincipal principal) =>
        Enum.Parse<UserRole>(principal.Claims.First(c => c.Type == "Role").Value);

    public static string EmailAddress(this ClaimsPrincipal principal) =>
        principal.Claims.First(c => c.Type == "EmailAddress").Value;
}