using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Enums;

namespace WebApi.Application.Features.Subscriptions.Commands.SubscribeToPremium;

public class SubscribeToPremiumCommandHandler : IRequestHandler<SubscribeToPremiumCommand>
{
    private readonly IAppDbContext _context;
    private readonly INotificationService _notificationService;

    public SubscribeToPremiumCommandHandler(IAppDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task Handle(SubscribeToPremiumCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new NotFoundException("İstifadəçi tapılmadı.");

        var duration = request.Plan == PremiumPlan.Monthly
            ? TimeSpan.FromDays(30)
            : TimeSpan.FromDays(365);

        // Əgər istifadəçinin artıq aktiv abunəliyi varsa, üzərinə əlavə et (uzat), yoxdursa indidən başlat
        var baseDate = user.PremiumEndDate.HasValue && user.PremiumEndDate.Value > DateTime.UtcNow
            ? user.PremiumEndDate.Value
            : DateTime.UtcNow;

        user.PremiumEndDate = baseDate.Add(duration);

        await _context.SaveChangesAsync(cancellationToken);

        await _notificationService.NotifyAsync(
            userId: request.UserId,
            type: "premium_activated",
            title: "Premium Aktivləşdirildi! ✨",
            description: $"Premium üzvlüyünüz {(request.Plan == PremiumPlan.Monthly ? "1 ay" : "1 il")} müddətinə aktivləşdirildi.",
            cancellationToken: cancellationToken);
    }
}