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
    private readonly ICurrentUserService _currentUserService;  

    public SubscribeToPremiumCommandHandler(
        IAppDbContext context,
        INotificationService notificationService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _notificationService = notificationService;
        _currentUserService = currentUserService;
    }

    public async Task Handle(SubscribeToPremiumCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);  

        if (user is null)
            throw new NotFoundException("İstifadəçi tapılmadı.");

        if (!user.PremiumEndDate.HasValue || user.PremiumEndDate.Value <= DateTime.UtcNow)
            user.PremiumStartDate = DateTime.UtcNow;

        var duration = request.Plan == PremiumPlan.Monthly
            ? TimeSpan.FromDays(30)
            : TimeSpan.FromDays(365);

        var baseDate = user.PremiumEndDate.HasValue && user.PremiumEndDate.Value > DateTime.UtcNow
            ? user.PremiumEndDate.Value
            : DateTime.UtcNow;

        user.PremiumEndDate = baseDate.Add(duration);
        user.LastPremiumPlan = request.Plan.ToString();

        await _context.SaveChangesAsync(cancellationToken);

        await _notificationService.NotifyAsync(
            userId: currentUserId,   
            type: "premium_activated",
            title: "Premium Aktivləşdirildi! ✨",
            description: $"Premium üzvlüyünüz {(request.Plan == PremiumPlan.Monthly ? "1 ay" : "1 il")} müddətinə aktivləşdirildi.",
            cancellationToken: cancellationToken);
    }
}