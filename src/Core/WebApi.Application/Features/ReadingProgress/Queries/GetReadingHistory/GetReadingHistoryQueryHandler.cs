using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Features.ReadingProgress.Dtos;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.ReadingProgress.Queries.GetReadingHistory;

public class GetReadingHistoryQueryHandler : IRequestHandler<GetReadingHistoryQuery, List<ReadingProgressDetailDto>>
{
    private readonly IAppDbContext _context;

    public GetReadingHistoryQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ReadingProgressDetailDto>> Handle(GetReadingHistoryQuery request, CancellationToken cancellationToken)
    {
        return await _context.ReadingProgresses
            .Where(x => x.UserId == request.UserId && !x.Book.IsDeleted)
            .OrderByDescending(x => x.UpdatedAt)
            .Select(x => new ReadingProgressDetailDto
            {
                BookId = x.Book.Id,
                Title = x.Book.Title,
                Author = x.Book.Author,
                Cover = x.Book.Cover,
                Pages = x.Book.Pages,
                PercentageComplete = x.PercentageComplete,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync(cancellationToken);
    }
}