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
}

[DefaultStatusCode(StatusCodes.Status403Forbidden)]
public class ForbiddenObjectResult : ObjectResult
{
    public ForbiddenObjectResult([ActionResultObjectValue] object? value) : base(value) =>
        StatusCode = StatusCodes.Status403Forbidden;
}