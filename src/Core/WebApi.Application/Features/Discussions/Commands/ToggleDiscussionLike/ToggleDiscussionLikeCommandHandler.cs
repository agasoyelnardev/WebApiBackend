using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Discussions.Commands.ToggleDiscussionLike;

public class ToggleDiscussionLikeCommandHandler : IRequestHandler<ToggleDiscussionLikeCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public ToggleDiscussionLikeCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(ToggleDiscussionLikeCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var discussionExists = await _context.Discussions
            .AnyAsync(d => d.Id == request.DiscussionId, cancellationToken);

        if (!discussionExists)
            throw new NotFoundException("Müzakirə tapılmadı.");

        var existing = await _context.DiscussionLikes.FirstOrDefaultAsync(
            x => x.DiscussionId == request.DiscussionId && x.UserId == currentUserId,
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
            UserId = currentUserId
        };

        await _context.DiscussionLikes.AddAsync(like, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}