using MediatR;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandHandler
    : IRequestHandler<CreateReviewCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateReviewCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateReviewCommand request,
        CancellationToken cancellationToken)
    {
        var review = new Review
        {
            Author = request.Author,
            Content = request.Content,
            Rating = request.Rating,
            MovieId = request.MovieId
        };

        await _context.Reviews.AddAsync(review, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return review.Id;
    }
}