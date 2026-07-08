using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Rooms.Commands;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoomsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("create")]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomCommand command)
    {
        var roomId = await _mediator.Send(command);
        return Ok(new { Message = "Otaq yarandı", RoomId = roomId });
    }
}