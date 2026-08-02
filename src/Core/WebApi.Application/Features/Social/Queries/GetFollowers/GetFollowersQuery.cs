using MediatR;
using WebApi.Application.Features.Search.Dtos;

namespace WebApi.Application.Features.Social.Queries.GetFollowers;

public record GetFollowersQuery(string UserId)
    : IRequest<List<UserPreviewDto>>;