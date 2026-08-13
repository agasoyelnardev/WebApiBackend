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

    [EnableRateLimiting("AiChatPolicy")]
    [HttpPost("ask")]
    public async Task<IActionResult> Ask([FromBody] AskAiChatRequest request)
    {
        var message = (request.Message ?? request.Prompt ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(message))
            return BadRequest(new { message = "Mesaj boş ola bilməz." });

        var response = await _mediator.Send(new AskAiChatCommand(message));
        return Ok(response);
    }
}

public record AskAiChatRequest(string? Message, string? Prompt);