namespace Ukid.Domain.Common;

/// <summary>
/// Base entity includes create and modify information.
/// </summary>
public abstract class BaseAuditableEntity : BaseEntity
{
    public int? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int? ModifiedBy { get; set; }
    public DateTime ModifiedAt { get; set; }
}
