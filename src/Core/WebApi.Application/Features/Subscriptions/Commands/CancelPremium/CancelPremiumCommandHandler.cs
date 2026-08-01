using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Subscriptions.Commands.CancelPremium;

public class CancelPremiumCommandHandler : IRequestHandler<CancelPremiumCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;   

    public CancelPremiumCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(CancelPremiumCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);  

        if (user is null)
            throw new NotFoundException("İstifadəçi tapılmadı.");

        user.PremiumEndDate = null;
        await _context.SaveChangesAsync(cancellationToken);
    }
}