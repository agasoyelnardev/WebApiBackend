using MediatR;

namespace WebApi.Application.Features.BookReviews.Commands.DeleteBookReview;

public record DeleteBookReviewCommand(Guid Id) : IRequest { }