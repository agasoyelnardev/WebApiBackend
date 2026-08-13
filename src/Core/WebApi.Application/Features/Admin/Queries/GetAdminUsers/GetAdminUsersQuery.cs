using MediatR;
using WebApi.Application.Common.Models;
using WebApi.Application.Features.Admin.Dtos;

namespace WebApi.Application.Features.Admin.Queries.GetAdminUsers;

public class GetAdminUsersQuery : IRequest<PaginatedList<AdminUserDto>>
{
    public string? Search { get; set; }
    public string? Role { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}