using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.LiveStreams.Commands.DeleteLiveStream;

public class DeleteLiveStreamCommandHandler : IRequestHandler<DeleteLiveStreamCommand, Unit>
{
    private readonly IAppDbContext _context;

    public DeleteLiveStreamCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteLiveStreamCommand request, CancellationToken cancellationToken)
    {
        var stream = await _context.LiveStreams
                         .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken)
                     ?? throw new NotFoundException("Canlı yayım kanalı tapılmadı.");

        _context.LiveStreams.Remove(stream);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}