using MediatR;
using WebApi.Application.Features.Search.Dtos;

namespace WebApi.Application.Features.Search.Queries.GlobalSearch;

public class GlobalSearchQuery : IRequest<GlobalSearchResultDto>
{
    public string Query { get; set; } = string.Empty;
    public int Limit { get; set; } = 5;
}