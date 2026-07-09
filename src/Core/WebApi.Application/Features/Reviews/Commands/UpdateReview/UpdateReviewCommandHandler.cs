using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Reviews.Commands.UpdateReview;

public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand>
{
    private readonly IAppDbContext _context;

    public UpdateReviewCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateReviewCommand request,
        CancellationToken cancellationToken)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (review is null)
            throw new Exception("Review not found");

        review.Author = request.Author;
        review.Content = request.Content;
        review.Rating = request.Rating;

        await _context.SaveChangesAsync(cancellationToken);
    }
}