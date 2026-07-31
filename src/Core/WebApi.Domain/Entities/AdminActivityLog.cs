using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class AdminActivityLog : BaseEntity
{
    public string AdminUserId { get; set; } = string.Empty;
    public virtual AppUser AdminUser { get; set; } = null!;

    public string AdminUsername { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;     
    public string Description { get; set; } = string.Empty;  

    public Guid? TargetEntityId { get; set; }                 
    public string? TargetEntityType { get; set; }              
}