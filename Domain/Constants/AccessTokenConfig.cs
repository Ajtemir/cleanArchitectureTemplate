using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Constants;

public static class AccessTokenConfig
{
    public const string Issuer = "MyAuthServer"; 
    public const string Audience = "MyAuthClient";
    private const string Key = "myAccessToken secret key for authentication";  
    public const string SchemeName = "AccessToken";
    public const int LifetimeInMinutes = 60;
    public const string Algorithm = SecurityAlgorithms.HmacSha256;
    public const string UserIdClaim = ClaimTypes.Name;
    public const string UserRoleClaim = ClaimTypes.Role;
    public static readonly Func<ApplicationUser, string> GetPropertyAsIdentifier = user => user.Id.ToString();
    public static readonly Func<ApplicationUser, List<string?>> GetPropertyAsRoles = user => user.UserRoles.Select(x=>x.Role.Name).ToList();
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new (Encoding.ASCII.GetBytes(Key));
}