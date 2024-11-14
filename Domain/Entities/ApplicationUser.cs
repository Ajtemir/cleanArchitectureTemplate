using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public sealed class ApplicationUser : IdentityUser<int>
{
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    
    
    public string? PatronymicName { get; set; }
    public byte[]? Photo { get; set; }

    public ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();

    public ApplicationUser()
    {
        // EF Core needs this constructor even though it is never called by 
        // my code in the application. EF Core needs it to set up the contexts

        // Failure to have it will result in a 
        // No suitable constructor found for entity type 'User'. exception
    }

    public ApplicationUser(string username,
        string lastname,
        string firstName,
        string? patronymicName = null,
        string? pin = null,
        string? email = null,
        int? branchId = null)
        : base(username)
    {
        LastName = lastname;
        FirstName = firstName;
        PatronymicName = patronymicName;
        Email = email;
        SecurityStamp = Guid.NewGuid().ToString();
    }
}
