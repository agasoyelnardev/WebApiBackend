using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class DiscussionLike : BaseEntity
{
    public Guid DiscussionId { get; set; }
    public virtual Discussion Discussion { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;
    public virtual AppUser User { get; set; } = null!;
}