using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.BookVsMovies.Commands.Vote;

public class VoteCommandHandler : IRequestHandler<VoteCommand>
{
    private readonly IAppDbContext _context;

    public VoteCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(VoteCommand request, CancellationToken cancellationToken)
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
        {
            // Yeni səs
            var vote = new BookVsMovieVote
            {
                BookVsMovieId = request.BookVsMovieId,
                UserId = request.UserId,
                Choice = request.Choice
            };

            await _context.BookVsMovieVotes.AddAsync(vote, cancellationToken);

            if (request.Choice == VoteChoice.Book)
                comparison.BookVotes++;
            else
                comparison.MovieVotes++;
        }
        else if (existingVote.Choice != request.Choice)
        {
            // Seçimi dəyişir: köhnə tərəfdən çıxar, yeni tərəfə əlavə et
            if (existingVote.Choice == VoteChoice.Book)
                comparison.BookVotes--;
            else
                comparison.MovieVotes--;

            if (request.Choice == VoteChoice.Book)
                comparison.BookVotes++;
            else
                comparison.MovieVotes++;

            existingVote.Choice = request.Choice;
            existingVote.UpdatedAt = DateTime.UtcNow;
        }
        // existingVote.Choice == request.Choice olsaydı, heç nə etmirik (artıq bu seçimə səs verib)

        await _context.SaveChangesAsync(cancellationToken);
    }
}