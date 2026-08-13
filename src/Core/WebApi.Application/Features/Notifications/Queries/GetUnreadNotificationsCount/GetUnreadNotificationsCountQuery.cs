using MediatR;

namespace WebApi.Application.Features.Notifications.Queries.GetUnreadNotificationsCount;

public class GetUnreadNotificationsCountQuery : IRequest<int>
{
    public string UserId { get; set; } = string.Empty;
}