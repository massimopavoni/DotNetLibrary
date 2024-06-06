using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNetLibrary.Application.Abstractions.Services;
using DotNetLibrary.Application.Models.Requests;
using DotNetLibrary.Application.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DotNetLibrary.Application.Services;

public class TokenService(IUserService userService, IOptions<JwtAuthenticationOption> jwtAuthOption)
    : ITokenService
{
    private const int MinutesToExpire = 60;
    private readonly JwtAuthenticationOption _jwtAuthOption = jwtAuthOption.Value;

    public string CreateToken(CreateTokenRequest request)
    {
        var user = userService.LogIn(request.EmailAddress, request.Password);
        return new JwtSecurityTokenHandler().WriteToken(
            new JwtSecurityToken(_jwtAuthOption.Issuer, null, [
                    new Claim("EmailAddress", user.EmailAddress),
                    new Claim("Role", user.Role.ToString())
                ], expires: DateTime.Now.AddMinutes(MinutesToExpire),
                signingCredentials: GetSigningCredentials()));
    }

    private SigningCredentials GetSigningCredentials() =>
        new(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthOption.Key)), SecurityAlgorithms.HmacSha256);
}