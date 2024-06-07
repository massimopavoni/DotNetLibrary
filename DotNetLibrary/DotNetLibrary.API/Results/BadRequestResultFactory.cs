using DotNetLibrary.Application.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DotNetLibrary.API.Results;

public class BadRequestResultFactory(ActionContext context) :
    BadRequestObjectResult(new BadResponse(context.ModelState
        .Values.SelectMany(x =>
            x.Errors.Select(e => e.ErrorMessage)).ToList()));