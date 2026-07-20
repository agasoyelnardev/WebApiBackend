using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Discussions.Commands.ToggleDiscussionLike;

public class ToggleDiscussionLikeCommandHandler : IRequestHandler<ToggleDiscussionLikeCommand, bool>
{
    private readonly IAppDbContext _context;

    public ToggleDiscussionLikeCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ToggleDiscussionLikeCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var discussionExists = await _context.Discussions
            .AnyAsync(d => d.Id == request.DiscussionId, cancellationToken);

        if (!discussionExists)
            throw new NotFoundException("Müzakirə tapılmadı.");

        var existing = await _context.DiscussionLikes.FirstOrDefaultAsync(
            x => x.DiscussionId == request.DiscussionId && x.UserId == request.UserId,
            cancellationToken);

        if (existing is not null)
        {
            _context.DiscussionLikes.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
            return false;
        }

        var like = new DiscussionLike
        {
            DiscussionId = request.DiscussionId,
            UserId = request.UserId
        };

        await _context.DiscussionLikes.AddAsync(like, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}