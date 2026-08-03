using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Admin.Commands.DeleteBookReview;

public class DeleteBookReviewCommandHandler : IRequestHandler<DeleteBookReviewCommand, Unit>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteBookReviewCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteBookReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _context.BookReviews
                         .FirstOrDefaultAsync(r => r.Id == request.BookReviewId && !r.IsDeleted, cancellationToken)
                     ?? throw new NotFoundException("Kitab rəyi tapılmadı.");

        review.IsDeleted = true;
        review.UpdatedAt = DateTime.UtcNow;

        _context.AdminActivityLogs.Add(new AdminActivityLog
        {
            AdminUsername = _currentUserService.Username ?? "Unknown",
            Action = "DELETE_BOOK_REVIEW",
            Description = "Kitab rəyi silindi.",
            TargetEntityType = "BookReview",
            TargetEntityId = request.BookReviewId
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}