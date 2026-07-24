using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.ReadingProgress.Commands.UpdateReadingProgress;
using WebApi.Application.Features.ReadingProgress.Queries.GetAllReadingProgress;
using WebApi.Application.Features.ReadingProgress.Queries.GetReadingProgress;
using WebApi.Application.Interfaces;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReadingProgressController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public ReadingProgressController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpPut("{bookId}")]
    public async Task<IActionResult> UpdateProgress(Guid bookId, [FromBody] int percentageComplete)
    {
        await _mediator.Send(new UpdateReadingProgressCommand
        {
            BookId = bookId,
            PercentageComplete = percentageComplete,
            UserId = _currentUserService.UserId
        });

        return NoContent();
    }

    [HttpGet("{bookId}")]
    public async Task<IActionResult> GetProgress(Guid bookId)
    {
        var progress = await _mediator.Send(new GetReadingProgressQuery
        {
            BookId = bookId,
            UserId = _currentUserService.UserId
        });

        return Ok(progress);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProgress()
    {
        var progress = await _mediator.Send(new GetAllReadingProgressQuery(_currentUserService.UserId));
        return Ok(progress);
    }
}