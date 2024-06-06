using DotNetLibrary.Application.Models.Requests;

namespace DotNetLibrary.Application.Abstractions.Services;

public interface ITokenService
{
    public string CreateToken(CreateTokenRequest request);
}