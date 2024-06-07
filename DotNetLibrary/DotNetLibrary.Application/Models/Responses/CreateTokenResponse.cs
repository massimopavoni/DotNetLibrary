namespace DotNetLibrary.Application.Models.Responses;

public class CreateTokenResponse(string token)
{
    public string Token { get; } = token;
}