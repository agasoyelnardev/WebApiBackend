using MediatR;
using WebApi.Domain.Enums;

namespace WebApi.Application.Features.Subscriptions.Commands.SubscribeToPremium;

public class SubscribeToPremiumCommand : IRequest
{
    public PremiumPlan Plan { get; set; }
    public string UserId { get; set; } = string.Empty;
}