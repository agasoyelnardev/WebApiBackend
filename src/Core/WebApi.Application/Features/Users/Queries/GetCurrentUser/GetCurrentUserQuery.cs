using MediatR;

namespace WebApi.Application.Features.Users.Queries.GetCurrentUser;

public record GetCurrentUserQuery(string UserId) : IRequest<CurrentUserDto>;