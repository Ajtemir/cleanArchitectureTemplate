using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string> GetUsernameAsync(int userId);
    Task<ApplicationUser?> GetUser(int userId);

    Task<ApplicationUser?> GetCurrentUser(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates the password of the user.
    /// </summary>
    /// <param name="userId">The id of the user.</param>
    /// <param name="currentPassword">Current password of the user.</param>
    /// <param name="newPassword">New password, must match the password requirements.</param>
    /// <exception cref="UnauthorizedAccessException">Could not update the password.</exception>
    Task UpdateUserPassword(int userId, string currentPassword, string newPassword);

    Task<bool> IsInRoleAsync(ApplicationUser user, string role);

    /// <summary>
    /// Get user roles.
    /// </summary>
    Task<IList<string>> GetRolesForUser(ApplicationUser user);
}
