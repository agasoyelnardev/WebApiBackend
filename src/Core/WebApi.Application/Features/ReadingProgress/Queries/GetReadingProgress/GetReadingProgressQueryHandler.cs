using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.ReadingProgress.Queries.GetReadingProgress;

public class GetReadingProgressQueryHandler : IRequestHandler<GetReadingProgressQuery, int>
{
    private readonly IAppDbContext _context;

    public GetReadingProgressQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(GetReadingProgressQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            return 0;

        var progress = await _context.ReadingProgresses
            .Where(x => x.UserId == request.UserId && x.BookId == request.BookId)
            .Select(x => x.PercentageComplete)
            .FirstOrDefaultAsync(cancellationToken);

        return progress;
    }
}