using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ITokenService
{
    string GetAccessToken(ApplicationUser user);
    string GetRefreshToken(ApplicationUser user);
}