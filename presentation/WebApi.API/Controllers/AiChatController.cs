using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WebApi.Application.Features.AiChat.Commands.AskAiChat;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AiChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public AiChatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [EnableRateLimiting("AuthPolicy")]
    [HttpPost("ask")]
    public async Task<IActionResult> Ask([FromBody] AskAiChatRequest request)
    {
        var reply = await _mediator.Send(new AskAiChatCommand(request.Message));
        return Ok(new { reply });
    }
}

public record AskAiChatRequest(string Message);