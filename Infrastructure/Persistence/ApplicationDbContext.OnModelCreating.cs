using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public partial class ApplicationDbContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Add the Postgres Extension for UUID generation
        builder.HasPostgresExtension("uuid-ossp");

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        IdentitySettings(builder);
        builder.AllSeed();
    }
    
    private void IdentitySettings(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>()
            .HasMany(e => e.UserRoles)
            .WithOne()
            .HasForeignKey(e => e.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<ApplicationUserRole>()
            .HasOne(e => e.User)
            .WithMany(e => e.UserRoles)
            .HasForeignKey(e => e.UserId);
        
        builder.Entity<ApplicationUserRole>()
            .HasOne(e => e.Role)
            .WithMany(e => e.Users)
            .HasForeignKey(e => e.RoleId);
    }
}