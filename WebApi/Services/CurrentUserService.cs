using System.Security.Claims;
using Application.Common.Interfaces;

namespace WebApi.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public int? UserId
    {
        get
        {
            var userClaim = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userClaim is null)
            {
                return null;
            }

            bool success = int.TryParse(userClaim, out var id);

            return success ? id : null;
        }
    }

    public bool IsAuthenticated => UserId != null;
}
