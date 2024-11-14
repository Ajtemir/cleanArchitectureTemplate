using System.Reflection;
using Application.Common.Extensions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static partial class Seed
{
    private static void UserSeed(this ModelBuilder builder)
    {
        var password = new PasswordHasher<ApplicationUser>();
        var hashed = password.HashPassword(Admin,"password");
        Admin.PasswordHash = hashed;
        builder.Entity<ApplicationUser>().HasData(
            Admin
        );
    }
    
    private static ApplicationUser Admin => new()
    {
        Id = 1,
        UserName = "Admin",
        Email = "Admin@test.ru",
        LockoutEnabled = false,
        PhoneNumber = "996111222333",
        FirstName = "Админ",
        LastName = "Админов",
        NormalizedUserName = "ADMIN",
        NormalizedEmail = "ADMIN@TEST.RU",
        SecurityStamp = Guid.NewGuid().ToString(),
    };
}