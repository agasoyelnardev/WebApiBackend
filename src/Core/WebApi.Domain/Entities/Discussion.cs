using WebApi.Domain.Entities.Base;
using WebApi.Domain.Enums;

namespace WebApi.Domain.Entities;

public class Discussion : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DiscussionCategory Category { get; set; }

    public string AuthorId { get; set; } = string.Empty;
    public virtual AppUser Author { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<DiscussionLike> Likes { get; set; } = new List<DiscussionLike>();
}