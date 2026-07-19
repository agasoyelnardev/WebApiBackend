using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.BookVsMovies.Commands.Unvote;

public class UnvoteCommandHandler : IRequestHandler<UnvoteCommand>
{
    private readonly IAppDbContext _context;

    public UnvoteCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UnvoteCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var comparison = await _context.BookVsMovies
            .FirstOrDefaultAsync(x => x.Id == request.BookVsMovieId, cancellationToken);

        if (comparison is null)
            throw new NotFoundException("Müqayisə tapılmadı.");

        var existingVote = await _context.BookVsMovieVotes
            .FirstOrDefaultAsync(
                v => v.BookVsMovieId == request.BookVsMovieId && v.UserId == request.UserId,
                cancellationToken);

        if (existingVote is null)
            throw new NotFoundException("Sizin bu müqayisəyə səsiniz yoxdur.");

        if (existingVote.Choice == VoteChoice.Book)
            comparison.BookVotes--;
        else
            comparison.MovieVotes--;

        _context.BookVsMovieVotes.Remove(existingVote);

        await _context.SaveChangesAsync(cancellationToken);
    }
}