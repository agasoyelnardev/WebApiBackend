using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class ReadingProgress : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public virtual AppUser User { get; set; } = null!;

    public Guid BookId { get; set; }
    public virtual Book Book { get; set; } = null!;

    // 0-100 arası faiz
    public int PercentageComplete { get; set; }
}