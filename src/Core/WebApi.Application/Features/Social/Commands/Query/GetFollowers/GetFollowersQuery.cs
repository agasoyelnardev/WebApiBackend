using MediatR;

namespace WebApi.Application.Features.Social.Queries.GetFollowers;

public record GetFollowersQuery(string UserId)
    : IRequest<List<UserPreviewDto>>;