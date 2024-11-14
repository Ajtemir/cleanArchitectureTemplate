using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class ApplicationUserRole : IdentityUserRole<int>
{

    public override int UserId { get; set; }
    public required ApplicationUser User { get; set; }
    public override int RoleId { get; set; }

    public required ApplicationRole Role { get; set; }
}
