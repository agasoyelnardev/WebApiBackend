using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;

namespace WebApi.Infrastructure.Persistence.Services;

public class PointsService : IPointsService
{
    private readonly IAppDbContext _context;
    private readonly INotificationService _notificationService;

    public PointsService(IAppDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task AwardPointsAsync(string userId, PointAction action, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(userId))
            return;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user is null)
            return;

        var points = action switch
        {
            PointAction.WatchMovie => 15,
            PointAction.AddReview => 20,
            PointAction.ReadingProgress50 => 15,
            PointAction.ReadingProgress100 => 30,
            _ => 0
        };

        if (points <= 0)
            return;

        user.Points += points;
        await _context.SaveChangesAsync(cancellationToken);

        await _notificationService.NotifyAsync(
            userId: userId,
            type: "points_earned",
            title: "Xal Qazanıldı! 🏆",
            description: $"Siz {points} xal qazandınız!",
            cancellationToken: cancellationToken);
    }
}