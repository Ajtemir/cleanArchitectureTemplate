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
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
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
            ;
    }
}