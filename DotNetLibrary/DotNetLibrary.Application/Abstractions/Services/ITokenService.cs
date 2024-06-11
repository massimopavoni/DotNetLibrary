using DotNetLibrary.Application.Models.Requests;

namespace DotNetLibrary.Application.Abstractions.Services;

public interface ITokenService
{
    protected const int MinutesToExpire = 30;

    public string Post(CreateTokenRequest request);
}