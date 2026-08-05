using MediatR;
using WebApi.Application.Features.LiveStreams.Dtos;

namespace WebApi.Application.Features.LiveStreams.Commands.SendLiveStreamMessage;

public class SendLiveStreamMessageCommand : IRequest<LiveStreamMessageDto>
{
    public Guid LiveStreamId { get; set; }
    public string? UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserAvatar { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}