using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Discussions.Commands.DeleteComment;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteCommentCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var comment = await _context.Comments
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (comment is null)
            throw new NotFoundException("Şərh tapılmadı.");

        var isAdmin = _currentUserService.IsInRole("Admin");

        if (comment.AuthorId != currentUserId && !isAdmin)
            throw new UnauthorizedAccessException("Bu şərhi silmək hüququnuz yoxdur.");

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync(cancellationToken);
    }
}