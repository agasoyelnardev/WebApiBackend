using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Books.Commands.CreateBook;
using WebApi.Application.Features.Books.Commands.DeleteBook;
using WebApi.Application.Features.Books.Commands.UpdateBook;
using WebApi.Application.Features.Books.Queries.GetBookById;
using WebApi.Application.Features.Books.Queries.GetFilteredBooks;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetFiltered([FromQuery] GetFilteredBooksQuery query)
    {
        var books = await _mediator.Send(query);
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var book = await _mediator.Send(new GetBookByIdQuery(id));

        if (book is null)
            return NotFound();

        return Ok(book);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateBookCommand command)
    {
        var bookId = await _mediator.Send(command);
        return Ok(bookId);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, UpdateBookCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteBookCommand(id));
        return NoContent();
    }
}