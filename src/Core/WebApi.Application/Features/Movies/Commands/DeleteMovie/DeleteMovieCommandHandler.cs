using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Features.Movies.Commands.DeleteMovie;

public class DeleteMovieCommandHandler
    : IRequestHandler<DeleteMovieCommand, bool>
{
    private readonly IAppDbContext _context;

    public DeleteMovieCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        DeleteMovieCommand request,
        CancellationToken cancellationToken)
    {
        var movie = await _context.Movies
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken);

        if (movie is null)
            throw new NotFoundException("Film tapılmadı.");

        movie.IsDeleted = true;
        movie.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}