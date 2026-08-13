using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Features.Search.Queries.GlobalSearch;

namespace WebApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly IMediator _mediator;

    public SearchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] int limit = 5)
    {
        var result = await _mediator.Send(new GlobalSearchQuery { Query = q, Limit = limit });
        return Ok(result);
    }
}