using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.LiveStreams.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.LiveStreams.Queries.GetLiveStreamChatHistory;

public class GetLiveStreamChatHistoryQueryHandler : IRequestHandler<GetLiveStreamChatHistoryQuery, List<LiveStreamMessageDto>>
{
    private readonly IAppDbContext _context;

    public GetLiveStreamChatHistoryQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<LiveStreamMessageDto>> Handle(GetLiveStreamChatHistoryQuery request, CancellationToken cancellationToken)
    {
        var messages = await _context.LiveStreamMessages
            .Where(m => m.LiveStreamId == request.LiveStreamId && !m.IsDeleted)
            .OrderByDescending(m => m.CreatedAt)
            .Take(50)
            .Select(m => new LiveStreamMessageDto
            {
                Id = m.Id,
                LiveStreamId = m.LiveStreamId,
                UserId = m.UserId,
                UserName = m.UserName,
                UserAvatar = m.UserAvatar,
                Message = m.Message,
                CreatedAt = m.CreatedAt
            })
            .ToListAsync(cancellationToken);

        messages.Reverse(); 
        return messages;
    }
}