namespace WebApi.Application.Features.Users.Dtos;

public class SetRoleRequest
{
    public string Role { get; set; } = string.Empty;
}

public class SetStatusRequest
{
    public bool IsBlocked { get; set; }
    public string? Reason { get; set; }
}