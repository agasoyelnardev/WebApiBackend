using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.ReadingProgress.Queries.GetAllReadingProgress;

public class GetAllReadingProgressQueryHandler
    : IRequestHandler<GetAllReadingProgressQuery, Dictionary<Guid, int>>
{
    private readonly IAppDbContext _context;

    public GetAllReadingProgressQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<Guid, int>> Handle(
        GetAllReadingProgressQuery request, CancellationToken cancellationToken)
    {
        return await _context.ReadingProgresses
            .Where(x => x.UserId == request.UserId)
            .ToDictionaryAsync(x => x.BookId, x => x.PercentageComplete, cancellationToken);
    }
}