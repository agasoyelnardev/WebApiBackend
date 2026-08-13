using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class BookCollectionItem : BaseEntity
{
    public Guid BookCollectionId { get; set; }
    public virtual BookCollection BookCollection { get; set; } = null!;

    public Guid BookId { get; set; }
    public virtual Book Book { get; set; } = null!;
}