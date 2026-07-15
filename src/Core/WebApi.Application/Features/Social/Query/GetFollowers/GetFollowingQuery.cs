using MediatR;

namespace WebApi.Application.Features.Social.Queries.GetFollowing;

public record GetFollowingQuery(string UserId)
    : IRequest<List<UserPreviewDto>>;