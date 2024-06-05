using System.Text;
using DotNetLibrary.Application.Options;
using DotNetLibrary.Web.Results;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace DotNetLibrary.Web.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddControllers().ConfigureApiBehaviorOptions(opt =>
            opt.InvalidModelStateResponseFactory = context => new BadRequestResultFactory(context)
        );
        services.AddEndpointsApiExplorer();
        return services;
    }

    public static IServiceCollection AddSwaggerServices(this IServiceCollection services) =>
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DotNetLibrary",
                Version = "v1"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = """
                              JWT Authorization header using the Bearer scheme.

                              Enter 'Bearer' [space] and then your token in the text input below.

                              Example: \"Bearer k0MfZ1LOE0wq7XAx\""
                              """
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

    public static IServiceCollection AddSecurityServices(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtAuthOption = new JwtAuthenticationOption();
        configuration.GetSection("JwtAuthentication").Bind(jwtAuthOption);
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var key = jwtAuthOption.Key;
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtAuthOption.Issuer,
                    IssuerSigningKey = securityKey
                };
            });
        services.Configure<JwtAuthenticationOption>(configuration.GetSection("JwtAuthentication"));
        return services;
    }
}