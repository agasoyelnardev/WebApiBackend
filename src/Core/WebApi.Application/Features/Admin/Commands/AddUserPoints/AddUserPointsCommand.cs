using MediatR;

namespace WebApi.Application.Features.Admin.Commands.AddUserPoints;

public class AddUserPointsCommand : IRequest<int>
{
    public string UserId { get; set; } = string.Empty;
    public int PointsToAdd { get; set; }
}