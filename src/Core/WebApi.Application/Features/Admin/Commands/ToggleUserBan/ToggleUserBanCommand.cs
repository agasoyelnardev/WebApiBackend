using MediatR;

namespace WebApi.Application.Features.Admin.Commands.ToggleUserBan;

public class ToggleUserBanCommand : IRequest<bool>
{
    public string UserId { get; set; } = string.Empty;
    public string? BanReason { get; set; }
}