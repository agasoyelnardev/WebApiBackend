using MediatR;
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

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageCommand command)
    {
        var result = await _mediator.Send(command);
        if (result) return Ok("Mesaj uğurla göndərildi.");
        return BadRequest("Mesaj göndərilərkən xəta baş verdi.");
    }

    [HttpPost("room/{roomId}")]
    public async Task<IActionResult> GetRoomMessage(Guid roomId)
    {
        var query = new GetMessagesQuery(roomId);
        var messages = await _mediator.Send(query);
        return Ok(messages);
    }

}