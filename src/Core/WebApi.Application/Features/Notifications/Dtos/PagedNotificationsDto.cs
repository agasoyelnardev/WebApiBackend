using WebApi.Application.Features.Notifications.Dtos;

public class PagedNotificationsDto
{
    public List<NotificationDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int UnreadCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}