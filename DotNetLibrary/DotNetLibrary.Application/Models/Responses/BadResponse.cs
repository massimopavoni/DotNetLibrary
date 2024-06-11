namespace DotNetLibrary.Application.Models.Responses;

public class BadResponse(ICollection<string>? errors = null) : BaseResponse<bool>(false, errors);