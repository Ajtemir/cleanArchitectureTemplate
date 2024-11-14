using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Constants
{
    public static class RefreshTokenConfig
    {
        public const string Issuer = "MyAuthServer"; 
        public const string Audience = "MyAuthClient";
        private const string Key = "myRefreshToken secret key for authentication";  
        public const int LifetimeInMinutes = 1800;
        public const string Algorithm = SecurityAlgorithms.HmacSha256;
        public const string SchemeName = "RefreshToken";
        public const string UserIdClaim = ClaimTypes.Name;
        public const string UserRoleClaim = ClaimTypes.Role;
        public static readonly Func<ApplicationUser, string> GetPropertyAsIdentifier = user => user.Id.ToString();
        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new (Encoding.ASCII.GetBytes(Key));
        
    }
}