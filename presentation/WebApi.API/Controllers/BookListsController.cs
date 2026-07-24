using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.BookLists.Commands.ToggleBookFavorite;
using WebApi.Application.Features.BookLists.Commands.ToggleBookLike;
using WebApi.Application.Features.BookLists.Commands.ToggleBookWatchlist;
using WebApi.Application.Features.BookLists.Queries.GetUserBookFavorites;
using WebApi.Application.Features.BookLists.Queries.GetUserBookWatchlist;
using WebApi.Application.Interfaces;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookListsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public BookListsController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpPost("favorites/{bookId}/toggle")]
    public async Task<IActionResult> ToggleFavorite(Guid bookId)
    {
        var isFavorite = await _mediator.Send(new ToggleBookFavoriteCommand
        {
            BookId = bookId,
            UserId = _currentUserService.UserId
        });

        return Ok(new { IsFavorite = isFavorite });
    }

    [HttpGet("favorites")]
    public async Task<IActionResult> GetFavorites()
    {
        var books = await _mediator.Send(new GetUserBookFavoritesQuery(_currentUserService.UserId));
        return Ok(books);
    }
    
    [HttpPost("likes/{bookId}/toggle")]
    public async Task<IActionResult> ToggleLike(Guid bookId)
    {
        var isLiked = await _mediator.Send(new ToggleBookLikeCommand
        {
            BookId = bookId,
            UserId = _currentUserService.UserId
        });

        return Ok(new { IsLiked = isLiked });
    }
    
    [HttpPost("watchlist/{bookId}/toggle")]
    public async Task<IActionResult> ToggleWatchlist(Guid bookId)
    {
        var isInWatchlist = await _mediator.Send(new ToggleBookWatchlistCommand
        {
            BookId = bookId,
            UserId = _currentUserService.UserId
        });

        return Ok(new { IsInWatchlist = isInWatchlist });
    }

    [HttpGet("watchlist")]
    public async Task<IActionResult> GetWatchlist()
    {
        var books = await _mediator.Send(new GetUserBookWatchlistQuery(_currentUserService.UserId));
        return Ok(books);
    }
}