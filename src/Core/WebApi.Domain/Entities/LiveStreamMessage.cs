using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class LiveStreamMessage : BaseEntity
{
    public Guid LiveStreamId { get; set; }
    public LiveStream LiveStream { get; set; } = null!;

    public string? UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserAvatar { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;


}