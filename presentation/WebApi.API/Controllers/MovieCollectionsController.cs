using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.MovieCollections.Commands.AddMovieToCollection;
using WebApi.Application.Features.MovieCollections.Commands.CreateMovieCollection;
using WebApi.Application.Features.MovieCollections.Commands.DeleteMovieCollection;
using WebApi.Application.Features.MovieCollections.Commands.RemoveMovieFromCollection;
using WebApi.Application.Features.MovieCollections.Commands.UpdateMovieCollection;
using WebApi.Application.Features.MovieCollections.Queries.GetMovieCollectionById;
using WebApi.Application.Features.MovieCollections.Queries.GetUserMovieCollections;
using WebApi.Application.Interfaces;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieCollectionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public MovieCollectionsController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateMovieCollectionCommand command)
    {
        command.AppUserId = _currentUserService.UserId;
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateMovieCollectionCommand command)
    {
        command.Id = id;
        command.RequestedByUserId = _currentUserService.UserId;
        await _mediator.Send(command);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteMovieCollectionCommand(id)
        {
            RequestedByUserId = _currentUserService.UserId
        });
        return NoContent();
    }

    [Authorize]
    [HttpPost("{id}/movies/{movieId}")]
    public async Task<IActionResult> AddMovie(Guid id, Guid movieId)
    {
        await _mediator.Send(new AddMovieToCollectionCommand
        {
            MovieCollectionId = id,
            MovieId = movieId,
            RequestedByUserId = _currentUserService.UserId
        });
        return Ok(new { Message = "Film kolleksiyaya əlavə edildi" });
    }

    [Authorize]
    [HttpDelete("{id}/movies/{movieId}")]
    public async Task<IActionResult> RemoveMovie(Guid id, Guid movieId)
    {
        await _mediator.Send(new RemoveMovieFromCollectionCommand
        {
            MovieCollectionId = id,
            MovieId = movieId,
            RequestedByUserId = _currentUserService.UserId
        });
        return Ok(new { Message = "Film kolleksiyadan çıxarıldı" });
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(string userId)
    {
        var collections = await _mediator.Send(
            new GetUserMovieCollectionsQuery(userId, _currentUserService.UserId));
        return Ok(collections);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var collection = await _mediator.Send(
            new GetMovieCollectionByIdQuery(id, _currentUserService.UserId));

        if (collection is null)
            return NotFound();

        return Ok(collection);
    }
}