using Application.Common.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApi.Filters;

namespace WebApi.Services;

public static class ConfigureWebApiServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddHttpClient();
        services.AddConfiguredCors();
        services.AddConfiguredControllers();
        services.AddSwaggerDocumentation();

        services.AddApplicationHealthChecks(connectionString);

        return services;
    }

    private static IServiceCollection AddConfiguredControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilterAttribute>();
        });

        return services;
    }

    private static IServiceCollection AddConfiguredCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }
    
    
    private static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
    
    private static IServiceCollection AddApplicationHealthChecks(this IServiceCollection services, string connectionString)
    {
        services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddNpgSql(connectionString, name: "ukid-api", tags: new[] { "ukid-api-db" });

        return services;
    }
}
