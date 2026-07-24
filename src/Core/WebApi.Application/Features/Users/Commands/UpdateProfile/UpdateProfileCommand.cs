using MediatR;

namespace WebApi.Application.Features.Users.Commands.UpdateProfile;

public class UpdateProfileCommand : IRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;
}