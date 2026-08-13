using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class Notification : BaseEntity
{
    // "follower", "friend_request", "review", "party_invite", "system"
    public string Type { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;

    public string UserId { get; set; } = string.Empty;
    public virtual AppUser User { get; set; } = null!;

    // Bildirişin aid olduğu obyekt (RoomId, FriendshipId və s.) 
    public Guid? RelatedEntityId { get; set; }
}