using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Chats.Commands;
using WebApi.Application.Features.Chats.Queries;
using WebApi.Application.Interfaces;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public ChatsController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageCommand command)
    {
        if (_currentUserService.UserId is null)
            return Unauthorized();

        command.UserId = _currentUserService.UserId;
        command.Username = _currentUserService.Username ?? "Anonim";

        var messageId = await _mediator.Send(command);
        return Ok(new { Message = "Mesaj göndərildi", MessageId = messageId });
    }

    [HttpGet("room/{roomId}")]
    public async Task<IActionResult> GetRoomMessage(Guid roomId)
    {
        var messages = await _mediator.Send(new GetRoomMessagesQuery(roomId));
        return Ok(messages);
    }

}