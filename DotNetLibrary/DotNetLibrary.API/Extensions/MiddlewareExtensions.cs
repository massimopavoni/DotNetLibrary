using System.Net;
using DotNetLibrary.Application.Factories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace DotNetLibrary.API.Extensions;

public static class MiddlewareExtensions
{
    public static WebApplication AddWebMiddleware(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseAuthentication();
        app.UseExceptionHandler(appError =>
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                    await context.Response.WriteAsJsonAsync(ResponseFactory.WithError(contextFeature.Error));
            })
        );
        app.MapControllers();
        return app;
    }
}