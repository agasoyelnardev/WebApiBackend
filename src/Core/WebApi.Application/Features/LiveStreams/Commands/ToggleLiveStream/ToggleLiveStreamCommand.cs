using MediatR;

namespace WebApi.Application.Features.LiveStreams.Commands.ToggleLiveStream;

public class ToggleLiveStreamCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}