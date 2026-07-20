using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Notifications.Queries.GetUnreadNotificationsCount;

public class GetUnreadNotificationsCountQueryHandler
    : IRequestHandler<GetUnreadNotificationsCountQuery, int>
{
    private readonly IAppDbContext _context;

    public GetUnreadNotificationsCountQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(GetUnreadNotificationsCountQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        return await _context.Notifications
            .CountAsync(n => n.UserId == request.UserId && !n.IsRead, cancellationToken);
    }
}