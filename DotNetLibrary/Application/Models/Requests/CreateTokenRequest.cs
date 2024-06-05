namespace DotNetLibrary.Application.Models.Requests;

public class CreateTokenRequest
{
    public string EmailAddress { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}