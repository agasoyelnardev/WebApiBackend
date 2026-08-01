using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Common.Exceptions;
using WebApi.Application.Features.AiChat.Dtos;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Application.Features.AiChat.Commands.AskAiChat;

public class AskAiChatCommandHandler : IRequestHandler<AskAiChatCommand, AskAiChatResponse>
{
    private readonly IAiChatService _aiChatService;
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AskAiChatCommandHandler(
        IAiChatService aiChatService,
        IAppDbContext context,
        ICurrentUserService currentUserService)
    {
        _aiChatService = aiChatService;
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<AskAiChatResponse> Handle(AskAiChatCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            throw new BadRequestException("Mesaj boş ola bilməz.");

        if (request.Message.Length > 1000)
            throw new BadRequestException("Mesaj maksimum 1000 simvol ola bilər.");

        var currentUserId = _currentUserService.UserId;
        string? contextPrompt = null;

        if (!string.IsNullOrEmpty(currentUserId))
        {
            var contextParts = new List<string>();

            var favoriteMovies = await _context.UserMovieLists
                .Where(x => x.UserId == currentUserId && x.Type == MovieListType.Favorite)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => x.Movie.Title)
                .Take(5)
                .ToListAsync(cancellationToken);

            if (favoriteMovies.Count > 0)
                contextParts.Add($"İstifadəçinin sevimli filmləri: {string.Join(", ", favoriteMovies)}.");

            var favoriteBooks = await _context.UserBookFavorites
                .Where(x => x.UserId == currentUserId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => x.Book.Title)
                .Take(5)
                .ToListAsync(cancellationToken);

            if (favoriteBooks.Count > 0)
                contextParts.Add($"İstifadəçinin sevimli kitabları: {string.Join(", ", favoriteBooks)}.");

            var recentlyWatched = await _context.WatchHistories
                .Where(x => x.UserId == currentUserId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => x.Movie.Title)
                .Take(5)
                .ToListAsync(cancellationToken);

            if (recentlyWatched.Count > 0)
                contextParts.Add($"İstifadəçinin son izlədiyi filmlər: {string.Join(", ", recentlyWatched)}.");

            var currentlyReading = await _context.ReadingProgresses
                .Where(x => x.UserId == currentUserId && x.PercentageComplete > 0 && x.PercentageComplete < 100)
                .OrderByDescending(x => x.UpdatedAt)
                .Select(x => new { x.Book.Title, x.PercentageComplete })
                .Take(3)
                .ToListAsync(cancellationToken);

            if (currentlyReading.Count > 0)
            {
                var readingList = currentlyReading.Select(x => $"{x.Title} ({x.PercentageComplete}%)");
                contextParts.Add($"İstifadəçinin hazırda oxumaqda olduğu kitablar: {string.Join(", ", readingList)}.");
            }

            if (contextParts.Count > 0)
                contextPrompt = string.Join(" ", contextParts) + " Tövsiyə verərkən bunları nəzərə ala bilərsən, amma məcburi deyil — istifadəçinin sualına uyğun ən yaxşı cavabı ver.";
        }

        var aiResult = await _aiChatService.AskGeminiAsync(request.Message, contextPrompt, cancellationToken);

        var recommendedMovieIds = await ResolveMovieIdsAsync(aiResult.RecommendedMovieTitles, cancellationToken);
        var recommendedBookIds = await ResolveBookIdsAsync(aiResult.RecommendedBookTitles, cancellationToken);

        return new AskAiChatResponse(
            aiResult.Reply,
            recommendedMovieIds.Count > 0 ? recommendedMovieIds : null,
            recommendedBookIds.Count > 0 ? recommendedBookIds : null);
    }

    private async Task<List<Guid>> ResolveMovieIdsAsync(List<string> titles, CancellationToken cancellationToken)
    {
        var ids = new List<Guid>();

        foreach (var title in titles)
        {
            var trimmedTitle = title.Trim();

            var match = await _context.Movies
                .Where(m => !m.IsDeleted && EF.Functions.Like(m.Title, $"%{trimmedTitle}%"))
                .OrderBy(m => m.Title)
                .Select(m => m.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (match != Guid.Empty && !ids.Contains(match))
                ids.Add(match);
        }

        return ids;
    }

    private async Task<List<Guid>> ResolveBookIdsAsync(List<string> titles, CancellationToken cancellationToken)
    {
        var ids = new List<Guid>();

        foreach (var title in titles)
        {
            var trimmedTitle = title.Trim();

            var match = await _context.Books
                .Where(b => !b.IsDeleted && EF.Functions.Like(b.Title, $"%{trimmedTitle}%"))
                .OrderBy(b => b.Title)
                .Select(b => b.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (match != Guid.Empty && !ids.Contains(match))
                ids.Add(match);
        }

        return ids;
    }
}