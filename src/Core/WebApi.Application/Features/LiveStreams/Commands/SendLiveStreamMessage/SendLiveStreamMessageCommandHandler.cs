using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Features.LiveStreams.Dtos;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.LiveStreams.Commands.SendLiveStreamMessage;

public class SendLiveStreamMessageCommandHandler : IRequestHandler<SendLiveStreamMessageCommand, LiveStreamMessageDto>
{
    private readonly IAppDbContext _context;

    public SendLiveStreamMessageCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<LiveStreamMessageDto> Handle(SendLiveStreamMessageCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            throw new BadRequestException("Mesaj boş ola bilməz.");

        if (request.Message.Length > 500)
            throw new BadRequestException("Mesaj maksimum 500 simvol ola bilər.");

        var stream = await _context.LiveStreams
            .FirstOrDefaultAsync(s => s.Id == request.LiveStreamId, cancellationToken)
            ?? throw new NotFoundException("Canlı yayım kanalı tapılmadı.");

        if (!stream.IsLive)
            throw new BadRequestException("Bu yayım hazırda aktiv deyil.");

        var message = new LiveStreamMessage
        {
            LiveStreamId = request.LiveStreamId,
            UserId = request.UserId,
            UserName = request.UserName,
            UserAvatar = request.UserAvatar,
            Message = request.Message.Trim()
        };

        _context.LiveStreamMessages.Add(message);
        await _context.SaveChangesAsync(cancellationToken);

        return new LiveStreamMessageDto
        {
            Id = message.Id,
            LiveStreamId = message.LiveStreamId,
            UserId = message.UserId,
            UserName = message.UserName,
            UserAvatar = message.UserAvatar,
            Message = message.Message,
            CreatedAt = message.CreatedAt
        };
    }
}