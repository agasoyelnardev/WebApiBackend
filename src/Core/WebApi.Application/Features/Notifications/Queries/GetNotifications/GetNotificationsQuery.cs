using MediatR;
using WebApi.Application.Features.Notifications.Dtos;

namespace WebApi.Application.Features.Notifications.Queries.GetNotifications;

public class GetNotificationsQuery : IRequest<PagedNotificationsDto>
{
    public string UserId { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public bool UnreadOnly { get; set; } = false;
}

