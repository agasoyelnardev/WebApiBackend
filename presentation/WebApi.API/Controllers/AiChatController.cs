using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WebApi.Application.Features.AiChat.Commands.AskAiChat;
using WebApi.Application.Interfaces;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AiChatController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public AiChatController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [EnableRateLimiting("AuthPolicy")]
    [HttpPost("ask")]
    public async Task<IActionResult> Ask([FromBody] AskAiChatRequest request)
    {
        var reply = await _mediator.Send(new AskAiChatCommand(request.Message, _currentUserService.UserId));
        return Ok(new { reply });
    }
}

public record AskAiChatRequest(string Message);