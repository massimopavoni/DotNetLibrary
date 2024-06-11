namespace DotNetLibrary.Application.Extensions;

public static class ISBNExtensions
{
    public static string ToDashedString(this ISBN isbn) =>
        $"{isbn.CanonicalNumber[..3]}-{isbn.Group}-{isbn.Publisher}-{isbn.Article}-{isbn.CanonicalNumber[^1]}";
}