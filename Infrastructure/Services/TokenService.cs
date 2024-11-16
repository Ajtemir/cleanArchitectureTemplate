using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class TokenService : ITokenService
{
    public string GetAccessToken(ApplicationUser user)
    {
        var now = DateTime.UtcNow;
        var claims = new List<Claim>();
        var roles = AccessTokenConfig.GetPropertyAsRoles(user)
            .Select(x => new Claim(AccessTokenConfig.UserRoleClaim, x)).ToList();
        var idClaim = new Claim(AccessTokenConfig.UserIdClaim, AccessTokenConfig.GetPropertyAsIdentifier(user));
        claims.Add(idClaim);
        claims.AddRange(roles);
        
        var jwt = new JwtSecurityToken(
            issuer: AccessTokenConfig.Issuer,
            audience: AccessTokenConfig.Audience,
            notBefore: now,
            claims: claims,
            expires: now.Add(TimeSpan.FromMinutes(AccessTokenConfig.LifetimeInMinutes)),
            signingCredentials: new SigningCredentials(AccessTokenConfig.GetSymmetricSecurityKey(), AccessTokenConfig.Algorithm));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return "bearer " + encodedJwt;
    }

    public string GetRefreshToken(ApplicationUser user)
    {
        var now = DateTime.UtcNow;
        var claims = new List<Claim>();
        var roles = RefreshTokenConfig.GetPropertyAsRoles(user)
            .Select(x => new Claim(RefreshTokenConfig.UserRoleClaim, x)).ToList();
        var idClaim = new Claim(RefreshTokenConfig.UserIdClaim, RefreshTokenConfig.GetPropertyAsIdentifier(user));
        claims.Add(idClaim);
        claims.AddRange(roles);
        var jwt = new JwtSecurityToken(
            issuer: RefreshTokenConfig.Issuer,
            audience: RefreshTokenConfig.Audience,
            notBefore: now,
            claims: claims,
            expires: now.Add(TimeSpan.FromMinutes(RefreshTokenConfig.LifetimeInMinutes)),
            signingCredentials: new SigningCredentials(RefreshTokenConfig.GetSymmetricSecurityKey(), RefreshTokenConfig.Algorithm));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return "bearer " + encodedJwt;
    }
}