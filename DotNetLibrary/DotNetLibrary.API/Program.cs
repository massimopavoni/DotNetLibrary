using DotNetLibrary.API.Extensions;
using DotNetLibrary.Application.Extensions;
using DotNetLibrary.Models.Extensions;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddWebServices()
    .AddSwaggerServices()
    .AddSecurityServices(builder.Configuration)
    .AddModelServices(builder.Configuration)
    .AddApplicationServices();
var app = builder.Build();
app.AddWebMiddleware()
    .Run();