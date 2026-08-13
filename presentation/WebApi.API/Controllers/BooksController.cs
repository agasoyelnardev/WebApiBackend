using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Books.Commands.CreateBook;
using WebApi.Application.Features.Books.Commands.DeleteBook;
using WebApi.Application.Features.Books.Commands.ImportBookFromGoogleBooks;
using WebApi.Application.Features.Books.Commands.UpdateBook;
using WebApi.Application.Features.Books.Commands.UploadPdf;
using WebApi.Application.Features.Books.Queries.GetBookById;
using WebApi.Application.Features.Books.Queries.GetFilteredBooks;
using WebApi.Application.Features.Books.Queries.SearchGoogleBooks;
using WebApi.Application.Interfaces;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public BooksController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
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
        var book = await _mediator.Send(new GetBookByIdQuery(id, _currentUserService.UserId));

        if (book is null)
            return NotFound();

        return Ok(book);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [RequestSizeLimit(50 * 1024 * 1024)] // 50MB
    public async Task<IActionResult> Create([FromForm] CreateBookCommand command)
    {
        var bookId = await _mediator.Send(command);
        return Ok(bookId);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [RequestSizeLimit(50 * 1024 * 1024)] // 50MB
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateBookCommand command)
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
    
    [Authorize(Roles = "Admin")]
    [HttpGet("googlebooks/search")]
    public async Task<IActionResult> SearchGoogleBooks([FromQuery] string query)
    {
        var results = await _mediator.Send(new SearchGoogleBooksQuery(query));
        return Ok(results);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("googlebooks/import/{googleBooksId}")]
    public async Task<IActionResult> ImportFromGoogleBooks(string googleBooksId)
    {
        var bookId = await _mediator.Send(new ImportBookFromGoogleBooksCommand { GoogleBooksId = googleBooksId });
        return Ok(bookId);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost("upload-pdf")]
    [RequestSizeLimit(50 * 1024 * 1024)] // 50MB
    public async Task<IActionResult> UploadPdf(IFormFile file)
    {
        var pdfUrl = await _mediator.Send(new UploadPdfCommand { File = file });
        return Ok(new { PdfUrl = pdfUrl });
    }
}