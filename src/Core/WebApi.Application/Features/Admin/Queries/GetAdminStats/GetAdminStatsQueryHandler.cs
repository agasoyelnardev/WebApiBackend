using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Constants;
using WebApi.Application.Features.Admin.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Admin.Queries.GetAdminStats;

public class GetAdminStatsQueryHandler : IRequestHandler<GetAdminStatsQuery, AdminStatsDto>
{
    private readonly IAppDbContext _context;

    public GetAdminStatsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<AdminStatsDto> Handle(GetAdminStatsQuery request, CancellationToken cancellationToken)
    {
        var activePremiumUsers = await _context.Users
            .Where(u => u.PremiumEndDate != null && u.PremiumEndDate > DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        var monthlyCount = activePremiumUsers.Count(u => u.LastPremiumPlan == "Monthly");
        var yearlyCount = activePremiumUsers.Count(u => u.LastPremiumPlan == "Yearly");

        var estimatedRevenue = (monthlyCount * PremiumPricing.MonthlyPrice) + (yearlyCount * PremiumPricing.YearlyPrice);

        return new AdminStatsDto
        {
            TotalUsers = await _context.Users.CountAsync(cancellationToken),
            TotalMovies = await _context.Movies.CountAsync(m => !m.IsDeleted, cancellationToken),
            TotalBooks = await _context.Books.CountAsync(b => !b.IsDeleted, cancellationToken),
            TotalReviews = await _context.Reviews.CountAsync(cancellationToken),
            TotalBookReviews = await _context.BookReviews.CountAsync(cancellationToken),
            TotalDiscussions = await _context.Discussions.CountAsync(cancellationToken),
            PremiumUsersCount = activePremiumUsers.Count,
            ActiveRoomsCount = await _context.StreamRooms.CountAsync(r => r.IsLive, cancellationToken),
            MonthlyPlanUsersCount = monthlyCount,
            YearlyPlanUsersCount = yearlyCount,
            EstimatedRevenue = estimatedRevenue
        };
    }
}