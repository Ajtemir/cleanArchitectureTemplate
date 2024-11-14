using System.Globalization;
using Application;
using HealthChecks.UI.Client;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using WebApi.Extensions;
using WebApi.Services;

namespace WebApi;

// Needs to be public to be accessed from test projects
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName.ToLower()}.json", optional: true, true) // parse from ASPNETCORE_ENVIRONMENT
            .AddEnvironmentVariables()
            .AddCommandLine(args);

        var cultureInfo = new CultureInfo("ru-RU");
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Could not bind the connection string 'DefaultConnection'");
        }

        builder.Logging.AddConfiguredSerilog(builder.Configuration);
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(connectionString);
        builder.Services.AddWebApiServices(connectionString);


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // Apply migrations when the path "/ApplyDatabaseMigrations" is hit.
            app.UseMigrationsEndPoint();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHealthChecks("/hc",
                new HealthCheckOptions
                {
                    Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                });

            // app.UseDeveloperExceptionPage();
        }

        using (var scope = app.Services.CreateScope())
        {
            var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
            await initializer.InitializeAsync();
            await initializer.InitializeRolesAndUsers();
        }

        app.UseHttpsRedirection();

        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapGet("/", () => "Ok");

        await app.RunAsync();
    }
}
