using MediatR;
using WebApi.Domain.Entities;
using WebApi.Domain.Enums;

namespace WebApi.Application.Features.Reviews.Commands.ToggleReviewLike;

public class ToggleReviewLikeCommand : IRequest<bool>
{
    public Guid ReviewId { get; set; }
    public ReviewLikeChoice Choice { get; set; }
    public string UserId { get; set; } = string.Empty;
}