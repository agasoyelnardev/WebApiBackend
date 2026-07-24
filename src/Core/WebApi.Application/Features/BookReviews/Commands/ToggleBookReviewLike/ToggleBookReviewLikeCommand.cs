using MediatR;
using WebApi.Domain.Entities;
using WebApi.Domain.Enums;

namespace WebApi.Application.Features.BookReviews.Commands.ToggleBookReviewLike;

public class ToggleBookReviewLikeCommand : IRequest<bool>
{
    public Guid BookReviewId { get; set; }
    public ReviewLikeChoice Choice { get; set; }
    public string UserId { get; set; } = string.Empty;
}