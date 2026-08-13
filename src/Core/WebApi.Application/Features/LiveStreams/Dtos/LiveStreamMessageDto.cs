namespace WebApi.Application.Features.LiveStreams.Dtos;

public class LiveStreamMessageDto
{
    public Guid Id { get; set; }
    public Guid LiveStreamId { get; set; }
    public string? UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserAvatar { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}