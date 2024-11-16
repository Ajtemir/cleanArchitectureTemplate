using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserAccountService : IUserAccountService
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserAccountService(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<ApplicationUser> Login(string username, string password, CancellationToken cancellationToken = default)
    {
        var user = await _signInManager.UserManager.Users
            .Include(x => x.UserRoles)
            .FirstOrDefaultAsync(x => x.UserName == username, cancellationToken: cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"Пользователь с логином '{username}' не существует.");
        }

        var isValidPassword = await _signInManager.UserManager.CheckPasswordAsync(user, password);
        if (!isValidPassword)
        {
            throw new BadRequestException("Не удалось выполнить вход с предоставленными учетными данными.");
        }
        
        await _signInManager.SignInAsync(user, true);
        return user;
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<ApplicationUser> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _signInManager.UserManager.Users
            .Include(x => x.UserRoles)
            .ThenInclude(x=>x.Role)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken: cancellationToken);
        if (user == null)
        {
            throw new NotFoundException($"Пользователь с идентификатором '{userId}' не существует.");
        }

        return user;
    }
}
