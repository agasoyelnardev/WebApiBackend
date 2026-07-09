using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Reviews.Commands.DeleteReview;

public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand>
{
    private readonly IAppDbContext _context;

    public DeleteReviewCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteReviewCommand request,
        CancellationToken cancellationToken)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (review is null)
            throw new Exception("Rəy tapılmadı");

        _context.Reviews.Remove(review);

        await _context.SaveChangesAsync(cancellationToken);
    }
}