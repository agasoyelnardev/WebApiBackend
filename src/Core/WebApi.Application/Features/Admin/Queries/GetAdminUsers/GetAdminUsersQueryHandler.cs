using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Models;
using WebApi.Application.Features.Admin.Dtos;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Admin.Queries.GetAdminUsers;

public class GetAdminUsersQueryHandler : IRequestHandler<GetAdminUsersQuery, PaginatedList<AdminUserDto>>
{
    private readonly IAppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public GetAdminUsersQueryHandler(IAppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<PaginatedList<AdminUserDto>> Handle(GetAdminUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(u =>
                (u.UserName != null && u.UserName.ToLower().Contains(search)) ||
                (u.Email != null && u.Email.ToLower().Contains(search)));
        }

        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(request.Role);
            var idsInRole = usersInRole.Select(u => u.Id).ToList();
            query = query.Where(u => idsInRole.Contains(u.Id));
        }

        var paged = await PaginatedList<AppUser>.CreateAsync(
            query.OrderByDescending(u => u.CreatedAt),
            request.Page,
            request.PageSize);

        var result = new List<AdminUserDto>();

        foreach (var user in paged.Items)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var reviewCount = await _context.Reviews
                .CountAsync(r => r.UserId == user.Id && !r.IsDeleted, cancellationToken);

            result.Add(new AdminUserDto
            {
                Id = user.Id,
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Avatar = user.Avatar,
                Roles = roles.ToList(),
                IsBanned = user.IsBanned,
                BanReason = user.BanReason,
                IsPremium = user.IsPremium,
                PremiumEndDate = user.PremiumEndDate,
                Points = user.Points,
                CreatedAt = user.CreatedAt,
                ReviewCount = reviewCount
            });
        }

        return new PaginatedList<AdminUserDto>(result, paged.TotalCount, paged.PageNumber, request.PageSize);
    }
}