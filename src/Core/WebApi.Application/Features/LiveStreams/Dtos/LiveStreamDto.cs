namespace WebApi.Application.Features.LiveStreams.Dtos;

public class LiveStreamDto
{
    public Guid Id { get; set; }
    public string ChannelKey { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string StreamUrl { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public bool IsLive { get; set; }
    public int ViewerCount { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime? StartedAt { get; set; }
}

