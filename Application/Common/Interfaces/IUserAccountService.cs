using Application.Common.Exceptions;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IUserAccountService
{
    /// <summary>
    /// Checks the credentials and sets the authentication cookie.
    /// </summary>
    /// <exception cref="NotFoundException">Could not find the user with specified <paramref name="username"/>.</exception>
    /// <exception cref="BadRequestException">Failed to sign in with provided credentials.</exception>
    Task<ApplicationUser> Login(string username, string password, CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs the user out and removes the authentication cookie.
    /// </summary>
    Task Logout();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ApplicationUser> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default);
}
