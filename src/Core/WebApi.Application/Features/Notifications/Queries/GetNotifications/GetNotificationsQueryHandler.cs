using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.Notifications.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Notifications.Queries.GetNotifications;

public class GetNotificationsQueryHandler
    : IRequestHandler<GetNotificationsQuery, PagedNotificationsDto>
{
    private readonly IAppDbContext _context;

    public GetNotificationsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedNotificationsDto> Handle(
        GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var pageSize = request.PageSize > 100 ? 100 : (request.PageSize < 1 ? 20 : request.PageSize);
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;

        var baseQuery = _context.Notifications
            .Where(n => n.UserId == request.UserId);

        var totalCountForBadge = await baseQuery.CountAsync(cancellationToken);
        var unreadCount = await baseQuery.CountAsync(n => !n.IsRead, cancellationToken);

        var filteredQuery = request.UnreadOnly
            ? baseQuery.Where(n => !n.IsRead)
            : baseQuery;

        var totalCount = request.UnreadOnly ? unreadCount : totalCountForBadge;

        var items = await filteredQuery
            .OrderByDescending(n => n.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Type = n.Type,
                Title = n.Title,
                Description = n.Description,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt,
                RelatedEntityId = n.RelatedEntityId
            })
            .ToListAsync(cancellationToken);

        return new PagedNotificationsDto
        {
            Items = items,
            TotalCount = totalCount,
            UnreadCount = unreadCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}