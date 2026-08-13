using MediatR;
using WebApi.Domain.Enums;

namespace WebApi.Application.Features.Subscriptions.Commands.SubscribeToPremium;

public record SubscribeToPremiumCommand(PremiumPlan Plan)
    : IRequest;