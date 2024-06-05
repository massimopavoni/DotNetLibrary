using DotNetLibrary.Application.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DotNetLibrary.Web.Results;

public class BadRequestResultFactory : BadRequestObjectResult
{
    public BadRequestResultFactory(ActionContext context) : base(new BadResponse())
    {
        var resultErrors = context.ModelState.Values.SelectMany(x =>
            x.Errors.Select(e => e.ErrorMessage)).ToList();
        var response = (BadResponse)Value!;
        response.Errors = resultErrors;
    }
}