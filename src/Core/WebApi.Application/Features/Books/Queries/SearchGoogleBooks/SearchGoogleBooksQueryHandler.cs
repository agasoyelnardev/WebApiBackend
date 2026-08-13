using MediatR;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Books.Queries.SearchGoogleBooks;

public class SearchGoogleBooksQueryHandler : IRequestHandler<SearchGoogleBooksQuery, List<GoogleBooksSearchResultDto>>
{
    private readonly IBookImportService _importService;

    public SearchGoogleBooksQueryHandler(IBookImportService importService)
    {
        _importService = importService;
    }

    public async Task<List<GoogleBooksSearchResultDto>> Handle(SearchGoogleBooksQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
            throw new BadRequestException("Axtarış sorğusu boş ola bilməz.");

        return await _importService.SearchAsync(request.Query, cancellationToken);
    }
}