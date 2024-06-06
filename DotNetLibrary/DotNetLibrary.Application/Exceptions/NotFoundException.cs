namespace DotNetLibrary.Application.Exceptions;

public class NotFoundException(string subjectItem) : Exception($"{subjectItem} not found");