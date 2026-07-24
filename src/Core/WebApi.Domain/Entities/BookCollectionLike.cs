using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class BookCollectionLike : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public virtual AppUser User { get; set; } = null!;

    public Guid BookCollectionId { get; set; }
    public virtual BookCollection BookCollection { get; set; } = null!;
}