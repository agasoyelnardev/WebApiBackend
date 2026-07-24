using WebApi.Domain.Entities.Base;
using WebApi.Domain.Enums;

namespace WebApi.Domain.Entities;


public class ReviewLike : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public virtual AppUser User { get; set; } = null!;

    public Guid ReviewId { get; set; }
    public virtual Review Review { get; set; } = null!;

    public ReviewLikeChoice Choice { get; set; }
}