using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Discussions.Commands.UpdateComment;

public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Unit>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateCommentCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.Content))
            throw new BadRequestException("Şərh boş ola bilməz.");

        if (request.Content.Length > 1000)
            throw new BadRequestException("Şərh maksimum 1000 simvol ola bilər.");

        var comment = await _context.Comments
                          .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
                      ?? throw new NotFoundException("Şərh tapılmadı.");

        var isAdmin = _currentUserService.IsInRole("Admin");

        if (comment.AuthorId != currentUserId && !isAdmin)
            throw new UnauthorizedAccessException("Bu şərhi redaktə etmək hüququnuz yoxdur.");

        comment.Content = request.Content;
        comment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}