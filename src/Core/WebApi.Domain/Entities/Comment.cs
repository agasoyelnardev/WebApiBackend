using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class Comment : BaseEntity
{
    public string Content { get; set; } = string.Empty;

    public Guid DiscussionId { get; set; }
    public virtual Discussion Discussion { get; set; } = null!;

    public string AuthorId { get; set; } = string.Empty;
    public virtual AppUser Author { get; set; } = null!;
}