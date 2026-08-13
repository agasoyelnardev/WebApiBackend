using MediatR;

namespace WebApi.Application.Features.LiveStreams.Commands.DeleteLiveStream;

public record DeleteLiveStreamCommand(Guid Id) : IRequest<Unit>;