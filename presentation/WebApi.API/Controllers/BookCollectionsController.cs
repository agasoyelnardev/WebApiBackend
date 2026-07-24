using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.BookCollections.Commands.AddBookToCollection;
using WebApi.Application.Features.BookCollections.Commands.CreateBookCollection;
using WebApi.Application.Features.BookCollections.Commands.DeleteBookCollection;
using WebApi.Application.Features.BookCollections.Commands.RemoveBookFromCollection;
using WebApi.Application.Features.BookCollections.Commands.ToggleCollectionLike;
using WebApi.Application.Features.BookCollections.Commands.ToggleSaveCollection;
using WebApi.Application.Features.BookCollections.Commands.UpdateBookCollection;
using WebApi.Application.Features.BookCollections.Queries.GetBookCollectionById;
using WebApi.Application.Features.BookCollections.Queries.GetSavedBookCollections;
using WebApi.Application.Features.BookCollections.Queries.GetUserCollections;
using WebApi.Application.Interfaces;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookCollectionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public BookCollectionsController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateBookCollectionCommand command)
    {
        command.UserId = _currentUserService.UserId;
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateBookCollectionCommand command)
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
        await _mediator.Send(new DeleteBookCollectionCommand(id)
        {
            RequestedByUserId = _currentUserService.UserId
        });
        return NoContent();
    }

    [Authorize]
    [HttpPost("{id}/books/{bookId}")]
    public async Task<IActionResult> AddBook(Guid id, Guid bookId)
    {
        await _mediator.Send(new AddBookToCollectionCommand
        {
            BookCollectionId = id,
            BookId = bookId,
            RequestedByUserId = _currentUserService.UserId
        });
        return Ok(new { Message = "Kitab kolleksiyaya əlavə edildi" });
    }

    [Authorize]
    [HttpDelete("{id}/books/{bookId}")]
    public async Task<IActionResult> RemoveBook(Guid id, Guid bookId)
    {
        await _mediator.Send(new RemoveBookFromCollectionCommand
        {
            BookCollectionId = id,
            BookId = bookId,
            RequestedByUserId = _currentUserService.UserId
        });
        return Ok(new { Message = "Kitab kolleksiyadan çıxarıldı" });
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(string userId)
    {
        var collections = await _mediator.Send(new GetUserCollectionsQuery(userId));
        return Ok(collections);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var collection = await _mediator.Send(new GetBookCollectionByIdQuery(id));

        if (collection is null)
            return NotFound();

        return Ok(collection);
    }
    
    [Authorize]
    [HttpPost("{id}/like")]
    public async Task<IActionResult> ToggleLike(Guid id)
    {
        var isLiked = await _mediator.Send(new ToggleBookCollectionLikeCommand
        {
            BookCollectionId = id,
            UserId = _currentUserService.UserId
        });

        return Ok(new { IsLiked = isLiked });
    }

    [Authorize]
    [HttpPost("{id}/save")]
    public async Task<IActionResult> ToggleSave(Guid id)
    {
        var isSaved = await _mediator.Send(new ToggleSaveBookCollectionCommand
        {
            BookCollectionId = id,
            UserId = _currentUserService.UserId
        });

        return Ok(new { IsSaved = isSaved });
    }

    [Authorize]
    [HttpGet("saved")]
    public async Task<IActionResult> GetSaved()
    {
        var collections = await _mediator.Send(new GetSavedBookCollectionsQuery(_currentUserService.UserId));
        return Ok(collections);
    }
}