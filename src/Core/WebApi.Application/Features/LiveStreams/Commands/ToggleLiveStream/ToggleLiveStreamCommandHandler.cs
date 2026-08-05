using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.LiveStreams.Commands.ToggleLiveStream;

public class ToggleLiveStreamCommandHandler : IRequestHandler<ToggleLiveStreamCommand, bool>
{
    private readonly IAppDbContext _context;

    public ToggleLiveStreamCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ToggleLiveStreamCommand request, CancellationToken cancellationToken)
    {
        var stream = await _context.LiveStreams
                         .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken)
                     ?? throw new NotFoundException("Canlı yayım kanalı tapılmadı.");

        stream.IsLive = !stream.IsLive;

        if (stream.IsLive)
        {
            stream.StartedAt = DateTime.UtcNow;
            stream.EndedAt = null;
        }
        else
        {
            stream.EndedAt = DateTime.UtcNow;
            stream.ViewerCount = 0; // yayım bitəndə izləyici sayı sıfırlanır
        }

        await _context.SaveChangesAsync(cancellationToken);

        return stream.IsLive;
    }
}