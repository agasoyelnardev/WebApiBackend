using WebApi.Domain.Entities.Base;
using WebApi.Domain.Enums;

namespace WebApi.Domain.Entities;

public class BookReviewLike : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public virtual AppUser User { get; set; } = null!;

    public Guid BookReviewId { get; set; }
    public virtual BookReview BookReview { get; set; } = null!;

    public ReviewLikeChoice Choice { get; set; }
}