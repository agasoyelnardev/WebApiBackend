using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.BookVsMovies.Commands.DeleteBookVsMovie;

public class DeleteBookVsMovieCommandHandler : IRequestHandler<DeleteBookVsMovieCommand>
{
    private readonly IAppDbContext _context;

    public DeleteBookVsMovieCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteBookVsMovieCommand request, CancellationToken cancellationToken)
    {
        var comparison = await _context.BookVsMovies
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (comparison is null)
            throw new NotFoundException("Müqayisə tapılmadı.");

        var votes = await _context.BookVsMovieVotes
            .Where(v => v.BookVsMovieId == request.Id)
            .ToListAsync(cancellationToken);

        _context.BookVsMovieVotes.RemoveRange(votes);
        _context.BookVsMovies.Remove(comparison);

        await _context.SaveChangesAsync(cancellationToken);
    }
}