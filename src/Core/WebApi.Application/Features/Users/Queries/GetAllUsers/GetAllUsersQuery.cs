using MediatR;
using WebApi.Application.Features.Users.Dtos;

namespace WebApi.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<List<AdminUserDto>>
{
    public string? SearchTerm { get; set; }
    public string? Role { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}