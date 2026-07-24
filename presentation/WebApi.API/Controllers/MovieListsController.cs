using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.MovieLists.Commands.MarkMovieAsWatched;
using WebApi.Application.Features.MovieLists.Commands.ToggleFavorite;
using WebApi.Application.Features.MovieLists.Commands.ToggleMovieLike;
using WebApi.Application.Features.MovieLists.Commands.ToggleWatchlist;
using WebApi.Application.Features.MovieLists.Queries.GetUserMovieList;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MovieListsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public MovieListsController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpPost("favorites/{movieId}/toggle")]
    public async Task<IActionResult> ToggleFavorite(Guid movieId)
    {
        var isFavorite = await _mediator.Send(new ToggleFavoriteCommand
        {
            MovieId = movieId,
            UserId = _currentUserService.UserId
        });

        return Ok(new { IsFavorite = isFavorite });
    }

    [HttpGet("favorites")]
    public async Task<IActionResult> GetFavorites()
    {
        var movies = await _mediator.Send(new GetUserMovieListQuery
        {
            UserId = _currentUserService.UserId,
            Type = MovieListType.Favorite
        });

        return Ok(movies);
    }

    [HttpPost("watchlist/{movieId}/toggle")]
    public async Task<IActionResult> ToggleWatchlist(Guid movieId)
    {
        var isInWatchlist = await _mediator.Send(new ToggleWatchlistCommand
        {
            MovieId = movieId,
            UserId = _currentUserService.UserId
        });

        return Ok(new { IsInWatchlist = isInWatchlist });
    }

    [HttpGet("watchlist")]
    public async Task<IActionResult> GetWatchlist()
    {
        var movies = await _mediator.Send(new GetUserMovieListQuery
        {
            UserId = _currentUserService.UserId,
            Type = MovieListType.Watchlist
        });

        return Ok(movies);
    }
    
    [HttpPost("likes/{movieId}/toggle")]
    public async Task<IActionResult> ToggleLike(Guid movieId)
    {
        var isLiked = await _mediator.Send(new ToggleMovieLikeCommand
        {
            MovieId = movieId,
            UserId = _currentUserService.UserId
        });

        return Ok(new { IsLiked = isLiked });
    }
    
    [Authorize]
    [HttpPost("start-watching/{movieId}")]
    public async Task<IActionResult> StartWatching(Guid movieId)
    {
        await _mediator.Send(new MarkMovieAsWatchedCommand
        {
            MovieId = movieId,
            UserId = _currentUserService.UserId
        });

        return Ok(new { Message = "İzləməyə başladınız" });
    }
}