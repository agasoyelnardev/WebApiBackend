using MediatR;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Discussions.Commands.CreateDiscussion;

public class CreateDiscussionCommandHandler : IRequestHandler<CreateDiscussionCommand, Guid>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateDiscussionCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateDiscussionCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.Title))
            throw new BadRequestException("Başlıq boş ola bilməz.");

        if (request.Title.Length > 200)
            throw new BadRequestException("Başlıq maksimum 200 simvol ola bilər.");

        if (string.IsNullOrWhiteSpace(request.Content))
            throw new BadRequestException("Məzmun boş ola bilməz.");

        if (request.Content.Length > 5000)
            throw new BadRequestException("Məzmun maksimum 5000 simvol ola bilər.");

        var discussion = new Discussion
        {
            Title = request.Title,
            Content = request.Content,
            Category = request.Category,
            AuthorId = currentUserId
        };

        await _context.Discussions.AddAsync(discussion, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return discussion.Id;
    }
}