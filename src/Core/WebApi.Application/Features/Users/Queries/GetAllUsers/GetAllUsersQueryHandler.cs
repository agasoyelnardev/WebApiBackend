using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.Users.Dtos;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<AdminUserDto>>
{
    private readonly IAppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public GetAllUsersQueryHandler(IAppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<List<AdminUserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var pageSize = request.PageSize > 100 ? 100 : (request.PageSize < 1 ? 20 : request.PageSize);
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;

        var query = _context.Users.AsQueryable();

        if (request.IsBlocked.HasValue)
            query = query.Where(u => u.IsBanned == request.IsBlocked);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(u =>
                EF.Functions.Like(u.UserName!, $"%{term}%") ||
                EF.Functions.Like(u.Email!, $"%{term}%"));
        }

        // Role filtri Skip/Take-dən ƏVVƏL tətbiq olunur (pagination düzgün işləsin deyə)
        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(request.Role);
            var userIdsInRole = usersInRole.Select(u => u.Id).ToHashSet();
            query = query.Where(u => userIdsInRole.Contains(u.Id));
        }

        var users = await query
            .OrderBy(u => u.UserName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var result = new List<AdminUserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";

            result.Add(new AdminUserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                Role = role,
                IsPremium = user.IsPremium,
                Points = user.Points,
                IsBlocked = user.IsBanned,
                BanReason = user.BanReason,
                BannedAt = user.BannedAt
            });
        }

        return result;
    }
}