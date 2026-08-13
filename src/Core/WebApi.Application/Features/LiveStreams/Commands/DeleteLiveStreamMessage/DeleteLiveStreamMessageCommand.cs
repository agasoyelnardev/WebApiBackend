using MediatR;

namespace WebApi.Application.Features.LiveStreams.Commands.DeleteLiveStreamMessage;

public record DeleteLiveStreamMessageCommand(Guid Id) : IRequest;
