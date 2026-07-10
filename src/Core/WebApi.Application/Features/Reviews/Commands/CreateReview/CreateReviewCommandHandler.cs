using MediatR;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace WebApi.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandHandler
    : IRequestHandler<CreateReviewCommand, Guid>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateReviewCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(
        CreateReviewCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId))
            throw new UnauthorizedAccessException();

        var review = new Review
        {
            Content = request.Content,
            Rating = request.Rating,
            MovieId = request.MovieId,
            UserId = _currentUserService.UserId
        };

        await _context.Reviews.AddAsync(review, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return review.Id;
    }
}