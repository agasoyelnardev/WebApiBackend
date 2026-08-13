using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.BookVsMovies.Commands.CreateBookVsMovie;

public class CreateBookVsMovieCommandHandler : IRequestHandler<CreateBookVsMovieCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateBookVsMovieCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateBookVsMovieCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new BadRequestException("Başlıq boş ola bilməz.");

        var bookExists = await _context.Books
            .AnyAsync(x => x.Id == request.BookId && !x.IsDeleted, cancellationToken);

        if (!bookExists)
            throw new NotFoundException("Kitab tapılmadı.");

        var movieExists = await _context.Movies
            .AnyAsync(x => x.Id == request.MovieId && !x.IsDeleted, cancellationToken);

        if (!movieExists)
            throw new NotFoundException("Film tapılmadı.");

        var alreadyExists = await _context.BookVsMovies
            .AnyAsync(x => x.BookId == request.BookId && x.MovieId == request.MovieId, cancellationToken);

        if (alreadyExists)
            throw new ConflictException("Bu kitab-film müqayisəsi artıq mövcuddur.");

        var comparison = new BookVsMovie
        {
            Title = request.Title,
            Description = request.Description,
            BookId = request.BookId,
            MovieId = request.MovieId
        };

        await _context.BookVsMovies.AddAsync(comparison, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return comparison.Id;
    }
}