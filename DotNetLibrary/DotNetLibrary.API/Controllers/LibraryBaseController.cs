using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace DotNetLibrary.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class LibraryBaseController : ControllerBase
{
    [NonAction]
    protected virtual ForbiddenObjectResult Forbidden([ActionResultObjectValue] object? value) =>
        new(value);

    [NonAction]
    protected virtual (int, int) ComputePaginationOffsets(int limit, int offset)
    {
        var previousOffset = offset - limit;
        return (offset + limit, previousOffset < 0 ? 0 : previousOffset);
    }
}

[DefaultStatusCode(StatusCodes.Status403Forbidden)]
public class ForbiddenObjectResult : ObjectResult
{
    public ForbiddenObjectResult([ActionResultObjectValue] object? value) : base(value) =>
        StatusCode = StatusCodes.Status403Forbidden;
}