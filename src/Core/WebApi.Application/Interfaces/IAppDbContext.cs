using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;

namespace WebApi.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Movie> Movies { get; }
    DbSet<Review> Reviews { get; }
    DbSet<ChatMessage> ChatMessages { get; }
    DbSet<UserFollow> UserFollows { get; }
    DbSet<Friendship> Friendships { get; }
    DbSet<AppUser> Users { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DbSet<Book> Books { get; }
    DbSet<BookReview> BookReviews { get; }
    DbSet<BookCollection> BookCollections { get; }
    DbSet<BookCollectionItem> BookCollectionItems { get; }
    DbSet<BookVsMovie> BookVsMovies { get; } 
    DbSet<BookVsMovieVote> BookVsMovieVotes { get; set; }
}