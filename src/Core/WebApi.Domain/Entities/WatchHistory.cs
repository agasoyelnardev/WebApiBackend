using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class WatchHistory : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public virtual AppUser User { get; set; } = null!;

    public Guid MovieId { get; set; }
    public virtual Movie Movie { get; set; } = null!;
}