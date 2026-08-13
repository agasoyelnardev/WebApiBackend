using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.BookVsMovies.Commands.Vote;

public class VoteCommandHandler : IRequestHandler<VoteCommand>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    public VoteCommandHandler(IAppDbContext context,ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService=currentUserService;
    }

    public async Task Handle(VoteCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("İstifadəçi səlahiyyəti yoxdur.");

        var comparison = await _context.BookVsMovies
            .FirstOrDefaultAsync(x => x.Id == request.BookVsMovieId, cancellationToken);

        if (comparison is null)
            throw new NotFoundException("Müqayisə tapılmadı.");

        var existingVote = await _context.BookVsMovieVotes
            .FirstOrDefaultAsync(
                v => v.BookVsMovieId == request.BookVsMovieId && v.UserId == userId,   
                cancellationToken);

        if (existingVote is null)
        {
            var vote = new BookVsMovieVote
            {
                BookVsMovieId = request.BookVsMovieId,
                UserId = userId,  
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

        await _context.SaveChangesAsync(cancellationToken);
    }
}