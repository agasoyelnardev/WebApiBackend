using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Chats.Commands;
using WebApi.Application.Features.Chats.Queries;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageCommand command, CancellationToken cancellationToken)
    {
        var messageId = await _mediator.Send(command, cancellationToken);
        return Ok(new { Message = "Mesaj göndərildi", MessageId = messageId });
    }

    [Authorize]
    [HttpGet("room/{roomId}")]
    public async Task<IActionResult> GetRoomMessage(Guid roomId, CancellationToken cancellationToken)
    {
        var messages = await _mediator.Send(new GetRoomMessagesQuery(roomId), cancellationToken);
        return Ok(messages);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteChatMessageCommand(id), cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMessage(Guid id, [FromBody] UpdateChatMessageCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}