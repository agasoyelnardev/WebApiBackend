using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Discussions.Commands.DeleteDiscussion;

public class DeleteDiscussionCommandHandler : IRequestHandler<DeleteDiscussionCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteDiscussionCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(DeleteDiscussionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RequestedByUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var discussion = await _context.Discussions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (discussion is null)
            throw new NotFoundException("Müzakirə tapılmadı.");

        var isAdmin = _currentUserService.IsInRole("Admin");

        if (discussion.AuthorId != request.RequestedByUserId && !isAdmin)
            throw new UnauthorizedAccessException("Bu müzakirəni silmək hüququnuz yoxdur.");

        var comments = await _context.Comments
            .Where(c => c.DiscussionId == request.Id)
            .ToListAsync(cancellationToken);

        var likes = await _context.DiscussionLikes
            .Where(l => l.DiscussionId == request.Id)
            .ToListAsync(cancellationToken);

        _context.Comments.RemoveRange(comments);
        _context.DiscussionLikes.RemoveRange(likes);
        _context.Discussions.Remove(discussion);

        await _context.SaveChangesAsync(cancellationToken);
    }
}