using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class BookCollection : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Cover { get; set; } = string.Empty;

    // Kolleksiyanı yaradan istifadəçi
    public string UserId { get; set; } = string.Empty;
    public virtual AppUser User { get; set; } = null!;

    // Kolleksiyadakı kitablar
    public virtual ICollection<BookCollectionItem> BookItems { get; set; } = new List<BookCollectionItem>();
}