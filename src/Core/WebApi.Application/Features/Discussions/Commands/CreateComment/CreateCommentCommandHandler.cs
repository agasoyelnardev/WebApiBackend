using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Discussions.Commands.CreateComment;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateCommentCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.AuthorId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        if (string.IsNullOrWhiteSpace(request.Content))
            throw new BadRequestException("Şərh boş ola bilməz.");

        if (request.Content.Length > 1000)
            throw new BadRequestException("Şərh maksimum 1000 simvol ola bilər.");

        var discussionExists = await _context.Discussions
            .AnyAsync(d => d.Id == request.DiscussionId, cancellationToken);

        if (!discussionExists)
            throw new NotFoundException("Müzakirə tapılmadı.");

        var comment = new Comment
        {
            DiscussionId = request.DiscussionId,
            Content = request.Content,
            AuthorId = request.AuthorId
        };

        await _context.Comments.AddAsync(comment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return comment.Id;
    }
}