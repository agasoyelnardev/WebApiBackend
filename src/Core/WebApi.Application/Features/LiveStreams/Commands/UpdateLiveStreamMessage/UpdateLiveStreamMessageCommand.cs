using MediatR;

namespace WebApi.Application.Features.LiveStreams.Commands.UpdateLiveStreamMessage;

public class UpdateLiveStreamMessageCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string MessageText { get; set; } = string.Empty;
}
