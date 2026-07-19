using MediatR;
using WebApi.Application.Features.BookReviews.Dtos;

namespace WebApi.Application.Features.BookReviews.Queries.GetBookReviewsByBookId;

public record GetBookReviewsByBookIdQuery(Guid BookId) : IRequest<List<BookReviewDto>>;