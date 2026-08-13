using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class LiveStream : BaseEntity
{
    public string ChannelKey { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string StreamUrl { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public bool IsLive { get; set; } = false;
    public int ViewerCount { get; set; } = 0;
    public string Category { get; set; } = string.Empty; // Movie, Book, Discussion
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }

    public ICollection<LiveStreamMessage> Messages { get; set; } = new List<LiveStreamMessage>();
}