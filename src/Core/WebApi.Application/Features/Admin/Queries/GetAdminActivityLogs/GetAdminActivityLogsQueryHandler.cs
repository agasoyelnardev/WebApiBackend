using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.Admin.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Admin.Queries.GetAdminActivityLogs;

public class GetAdminActivityLogsQueryHandler : IRequestHandler<GetAdminActivityLogsQuery, List<AdminActivityLogDto>>
{
    private readonly IAppDbContext _context;

    public GetAdminActivityLogsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AdminActivityLogDto>> Handle(GetAdminActivityLogsQuery request, CancellationToken cancellationToken)
    {
        return await _context.AdminActivityLogs
            .Where(l => !l.IsDeleted)
            .OrderByDescending(l => l.CreatedAt)
            .Take(50)
            .Select(l => new AdminActivityLogDto
            {
                Id = l.Id,
                AdminUsername = l.AdminUsername,
                Action = l.Action,
                Description = l.Description,
                TargetEntityType = l.TargetEntityType,
                TargetEntityId = l.TargetEntityId,
                CreatedAt = l.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}