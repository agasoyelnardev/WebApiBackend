using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Admin.Commands.AddUserPoints;

public class AddUserPointsCommandHandler : IRequestHandler<AddUserPointsCommand, int>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AddUserPointsCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(AddUserPointsCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
                       .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken)
                   ?? throw new NotFoundException("İstifadəçi tapılmadı.");

        user.Points = Math.Max(0, user.Points + request.PointsToAdd);

        _context.AdminActivityLogs.Add(new AdminActivityLog
        {
            AdminUsername = _currentUserService.Username ?? "Unknown",
            Action = "ADD_POINTS",
            Description = $"{user.UserName} istifadəçisinə {request.PointsToAdd} xal {(request.PointsToAdd >= 0 ? "əlavə" : "çıxılma")} edildi. Yeni balans: {user.Points}",
            TargetEntityType = "User",
            TargetEntityId = Guid.TryParse(user.Id, out var guid) ? guid : null
        });

        await _context.SaveChangesAsync(cancellationToken);

        return user.Points;
    }
}