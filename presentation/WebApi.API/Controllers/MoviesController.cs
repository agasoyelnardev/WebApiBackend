using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Movies.Commands.CreateMovie;
using WebApi.Application.Features.Movies.Commands.DeleteMovie;
using WebApi.Application.Features.Movies.Commands.UpdateMovie;
using WebApi.Application.Features.Movies.Queries.GetFilteredMovies;
using WebApi.Application.Features.Movies.Queries.GetMovieById;
using WebApi.Domain.Entities;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MoviesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<MovieDto>>> GetFiltered([FromQuery] GetFilteredMoviesQuery query)
    {
        var movies = await _mediator.Send(query);
        return Ok(movies);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var movie = await _mediator.Send(new GetMovieByIdQuery(id));

        if (movie is null)
            return NotFound();

        return Ok(movie);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateMovieCommand command)
    {
        var movieId = await _mediator.Send(command);

        return Ok(movieId);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, UpdateMovieCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteMovieCommand(id));
        return NoContent();
    }
}