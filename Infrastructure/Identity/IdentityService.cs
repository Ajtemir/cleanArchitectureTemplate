using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<IdentityService> _logger;
    private readonly ICurrentUserService _currentUserService;

    public IdentityService(UserManager<ApplicationUser> userManager, ILogger<IdentityService> logger, ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<string> GetUsernameAsync(int userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return user?.UserName ?? "";
    }

    public async Task<ApplicationUser?> GetUser(int userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return user;
    }

    public async Task<ApplicationUser?> GetCurrentUser(CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;
        if (userId is null)
        {
            return null;
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId.Value, cancellationToken: cancellationToken);
        return user;
    }

    public Task<bool> IsInRoleAsync(ApplicationUser user, string role)
    {
        return _userManager.IsInRoleAsync(user, role);
    }

    public async Task<IList<string>> GetRolesForUser(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return roles;
    }

    public async Task UpdateUserPassword(int userId, string currentPassword, string newPassword)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
        {
            throw new UnauthorizedAccessException();
        }
        
        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (!result.Succeeded)
        {
            _logger.LogWarning("Не получилось обновить пароль пользователя с Id {UserId}.", userId);
            throw new BadRequestException("Не получилось обновить пароль пользователя.");
        }
    }
}
