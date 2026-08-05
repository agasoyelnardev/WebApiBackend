using MediatR;
using WebApi.Application.Features.LiveStreams.Dtos;

namespace WebApi.Application.Features.LiveStreams.Queries.GetLiveStreamChatHistory;

public record GetLiveStreamChatHistoryQuery(Guid LiveStreamId) : IRequest<List<LiveStreamMessageDto>>;