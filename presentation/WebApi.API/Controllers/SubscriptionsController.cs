using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Subscriptions.Commands.CancelPremium;
using WebApi.Application.Features.Subscriptions.Commands.SubscribeToPremium;
using WebApi.Application.Interfaces;
using WebApi.Domain.Enums;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubscriptionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public SubscriptionsController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] PremiumPlan plan)
    {
        await _mediator.Send(new SubscribeToPremiumCommand
        {
            Plan = plan,
            UserId = _currentUserService.UserId
        });

        return Ok(new { Message = "Premium üzvlük aktivləşdirildi" });
    }

    [HttpPost("cancel")]
    public async Task<IActionResult> Cancel()
    {
        await _mediator.Send(new CancelPremiumCommand
        {
            UserId = _currentUserService.UserId
        });

        return Ok(new { Message = "Premium üzvlük ləğv edildi" });
    }
}