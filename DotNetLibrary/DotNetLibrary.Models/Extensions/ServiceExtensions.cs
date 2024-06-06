using DotNetLibrary.Models.Context;
using DotNetLibrary.Models.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetLibrary.Models.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddModelServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LibraryContext>(conf =>
            conf.UseMySQL(configuration.GetConnectionString("LibraryContext")!)
        );
        services.AddScoped<UserRepository>();
        services.AddScoped<BookRepository>();
        services.AddScoped<CategoryRepository>();
        services.AddScoped<BookCategoryRepository>();
        return services;
    }
}