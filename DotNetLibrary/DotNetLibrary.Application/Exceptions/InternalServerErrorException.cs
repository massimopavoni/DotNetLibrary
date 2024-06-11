namespace DotNetLibrary.Application.Exceptions;

public class InternalServerErrorException(string message, Exception innerException)
    : Exception($"{message} ({innerException.Message})");