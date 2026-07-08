using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.Movies.Queries.GetMovieById;

public class GetMovieByIdQueryHandler 
    : IRequestHandler<GetMovieByIdQuery, Movie?>
{
    private readonly IAppDbContext _context;

    public GetMovieByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Movie?> Handle(
        GetMovieByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Movies
            .Include(x => x.Reviews)
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken);
        
        
    }
}