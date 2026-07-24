using MediatR;

namespace WebApi.Application.Features.Subscriptions.Commands.CancelPremium;

public class CancelPremiumCommand : IRequest
{
    public string UserId { get; set; } = string.Empty;
}