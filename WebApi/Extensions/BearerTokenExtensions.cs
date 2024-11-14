using Domain.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Extensions;

public static class BearerTokenExtensions
{
    public static void AddBearerConfigurations(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services
            .AddAuthentication(
                options =>
                {
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = AccessTokenConfig.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            })
            .AddJwtBearer(RefreshTokenConfig.SchemeName, options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidIssuer = RefreshTokenConfig.Issuer,
                    ValidateLifetime = true,
                    IssuerSigningKey = RefreshTokenConfig.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            })
            .AddCookie(options =>
            {
                var isRunningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") != null;
                options.Cookie.HttpOnly = false;
                options.Cookie.Name = "ukid-api";
                options.Cookie.SameSite = isRunningInContainer ? SameSiteMode.None : SameSiteMode.Lax;
                options.Cookie.SecurePolicy = isRunningInContainer ? CookieSecurePolicy.Always : default;
                options.Cookie.MaxAge = TimeSpan.FromDays(2);
            
                // Do not redirect to /account/login, but return 401
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.Headers["Location"] = context.RedirectUri;
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };

                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.Headers["Location"] = context.RedirectUri;
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                };   
            })
            ;
    }
}