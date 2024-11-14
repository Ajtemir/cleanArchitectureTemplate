using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ukid.Domain.Enums;

namespace Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger,
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitializeAsync()
    {
        try
        {
            if (_context.Database.IsNpgsql())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while initializing the database: {Message}.", ex.Message);
            throw;
        }
    }

    public async Task InitializeRolesAndUsers()
    {
        try
        {
            await TryInitializeAuth();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while initializing roles and users: {Message}.", ex.Message);
            throw;
        }
    }

    private async Task TryInitializeAuth()
    {
        var domainRoles = Enumeration.GetAll<DomainRole>();
        var existingRoles = await _roleManager.Roles.ToListAsync();

        var needToCreateRoles = domainRoles.ExceptBy(
            existingRoles.Select(er => er.Id),
            domainRole => domainRole.Id);

        foreach (var role in needToCreateRoles)
        {
            await _roleManager.CreateAsync(new ApplicationRole(role.Name, role.Note));
        }

        await CreateIfNotExistAdmin();
        await CreateIfNotExistManager();
        await CreateIfNotExistUser();
    }

    private async Task CreateIfNotExistAdmin()
    {
        var admin = await _userManager.FindByNameAsync("admin");
        if (admin is null)
        {
            admin = new ApplicationUser("admin", "John", "Admin", email: "admin@example.com");
            await _userManager.CreateAsync(admin, "Admin123!");
            await _userManager.AddToRolesAsync(admin, new [] {DomainRole.Administrator.Name, DomainRole.Manager.Name, DomainRole.User.Name});
        }
    }
    
    private async Task CreateIfNotExistManager()
    {
        var manager = await _userManager.FindByNameAsync("manager");
        if (manager is null)
        {
            manager = new ApplicationUser("manager", "John", "Manager", email: "manager@example.com");
            await _userManager.CreateAsync(manager, "Manager123!");
            await _userManager.AddToRolesAsync(manager, new[] { DomainRole.Manager.Name, DomainRole.User.Name });
        }
    }
    
    private async Task CreateIfNotExistUser()
    {
        var user = await _userManager.FindByNameAsync("user");
        if (user is null)
        {
            user = new ApplicationUser("user", "John", "User", email: "user@example.com");
            await _userManager.CreateAsync(user, "User123!");
            await _userManager.AddToRoleAsync(user, DomainRole.User.Name);
        }
    }
}
