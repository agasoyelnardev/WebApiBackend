using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class ChatMessage : BaseEntity
{
    public Guid StreamRoomId { get; set; }
    public StreamRoom StreamRoom { get; set; } = null!;
    
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string UserAvatarUrl { get; set; } = string.Empty;
    public string MessageText { get; set; } = string.Empty;
    public bool IsSystemMessage { get; set; } = false;
}