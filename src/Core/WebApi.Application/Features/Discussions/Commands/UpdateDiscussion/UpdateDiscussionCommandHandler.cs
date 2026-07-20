using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Discussions.Commands.UpdateDiscussion;

public class UpdateDiscussionCommandHandler : IRequestHandler<UpdateDiscussionCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateDiscussionCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateDiscussionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RequestedByUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.Title))
            throw new BadRequestException("Başlıq boş ola bilməz.");

        if (request.Title.Length > 200)
            throw new BadRequestException("Başlıq maksimum 200 simvol ola bilər.");

        if (string.IsNullOrWhiteSpace(request.Content))
            throw new BadRequestException("Məzmun boş ola bilməz.");

        if (request.Content.Length > 5000)
            throw new BadRequestException("Məzmun maksimum 5000 simvol ola bilər.");

        var discussion = await _context.Discussions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (discussion is null)
            throw new NotFoundException("Müzakirə tapılmadı.");

        var isAdmin = _currentUserService.IsInRole("Admin");

        if (discussion.AuthorId != request.RequestedByUserId && !isAdmin)
            throw new UnauthorizedAccessException("Bu müzakirəni redaktə etmək hüququnuz yoxdur.");

        discussion.Title = request.Title;
        discussion.Content = request.Content;
        discussion.Category = request.Category;
        discussion.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}