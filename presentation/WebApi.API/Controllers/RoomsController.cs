using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Rooms.Commands;
using WebApi.Application.Features.Rooms.Queries;
using WebApi.Application.Interfaces;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoomsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public RoomsController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<IActionResult> GetActiveRooms()
    {
        var rooms = await _mediator.Send(new GetActiveRoomsQuery());
        return Ok(rooms);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomCommand command)
    {
        command.CreatedByUserId = _currentUserService.UserId;

        var roomId = await _mediator.Send(command);
        return Ok(new { Message = "Otaq yarandı", RoomId = roomId });
    }

    [HttpDelete("{roomId}")]
    public async Task<IActionResult> DeleteRoom(Guid roomId)
    {
        await _mediator.Send(new DeleteRoomCommand(roomId)
        {
            RequestedByUserId = _currentUserService.UserId
        });

        return Ok(new { Message = "Otaq silindi" });
    }

    [HttpPut("{roomId}/close")]
    public async Task<IActionResult> CloseRoom(Guid roomId)
    {
        await _mediator.Send(new CloseRoomCommand(roomId)
        {
            RequestedByUserId = _currentUserService.UserId
        });

        return Ok(new { Message = "Otaq bağlandı" });
    }
}