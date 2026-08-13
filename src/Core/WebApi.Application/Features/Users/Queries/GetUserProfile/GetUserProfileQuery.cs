using MediatR;

namespace WebApi.Application.Features.Users.Queries.GetUserProfile;

public record GetUserProfileQuery(string UserId) : IRequest<UserProfileDto?>;