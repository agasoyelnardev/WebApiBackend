using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.BookVsMovies.Commands.CreateBookVsMovie;
using WebApi.Application.Features.BookVsMovies.Commands.DeleteBookVsMovie;
using WebApi.Application.Features.BookVsMovies.Commands.Unvote;
using WebApi.Application.Features.BookVsMovies.Commands.Vote;
using WebApi.Application.Features.BookVsMovies.Queries.GetAllBookVsMovies;
using WebApi.Application.Features.BookVsMovies.Queries.GetBookVsMovieById;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookVsMoviesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public BookVsMoviesController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var comparisons = await _mediator.Send(new GetAllBookVsMoviesQuery());
        return Ok(comparisons);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var comparison = await _mediator.Send(
            new GetBookVsMovieByIdQuery(id, _currentUserService.UserId));

        if (comparison is null)
            return NotFound();

        return Ok(comparison);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateBookVsMovieCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteBookVsMovieCommand(id));
        return NoContent();
    }

    [Authorize]
    [HttpPost("{id}/vote")]
    public async Task<IActionResult> Vote(Guid id, [FromBody] VoteChoice choice)
    {
        await _mediator.Send(new VoteCommand
        {
            BookVsMovieId = id,
            Choice = choice,
            UserId = _currentUserService.UserId
        });

        return Ok(new { Message = "Səsiniz qeydə alındı" });
    }

    [Authorize]
    [HttpDelete("{id}/vote")]
    public async Task<IActionResult> Unvote(Guid id)
    {
        await _mediator.Send(new UnvoteCommand
        {
            BookVsMovieId = id,
            UserId = _currentUserService.UserId
        });

        return Ok(new { Message = "Səsiniz geri çəkildi" });
    }
}