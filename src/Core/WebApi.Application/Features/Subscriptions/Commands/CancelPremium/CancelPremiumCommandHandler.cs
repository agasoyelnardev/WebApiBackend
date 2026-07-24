using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Subscriptions.Commands.CancelPremium;

public class CancelPremiumCommandHandler : IRequestHandler<CancelPremiumCommand>
{
    private readonly IAppDbContext _context;

    public CancelPremiumCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CancelPremiumCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new NotFoundException("İstifadəçi tapılmadı.");

        user.PremiumEndDate = null;
        await _context.SaveChangesAsync(cancellationToken);
    }
}