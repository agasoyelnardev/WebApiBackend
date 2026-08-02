using MediatR;
using WebApi.Application.Features.Search.Dtos;

namespace WebApi.Application.Features.Social.Queries.GetFollowing;

public record GetFollowingQuery(string UserId)
    : IRequest<List<UserPreviewDto>>;