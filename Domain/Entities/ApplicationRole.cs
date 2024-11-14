using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public sealed class ApplicationRole : IdentityRole<int>
{
    public string Note { get; private set; } = null!;

    public ICollection<ApplicationUserRole> Users { get; set; } = new List<ApplicationUserRole>();

    public ApplicationRole() { }

    public ApplicationRole(string roleName, string note) : base(roleName)
    {
        Note = note;
        ConcurrencyStamp = Guid.NewGuid().ToString();
    }
}
