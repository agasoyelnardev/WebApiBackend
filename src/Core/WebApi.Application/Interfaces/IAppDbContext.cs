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
    DbSet<StreamRoom> StreamRooms { get; }
    DbSet<BookReview> BookReviews { get; }
    DbSet<BookCollection> BookCollections { get; }
    DbSet<BookCollectionItem> BookCollectionItems { get; }
    DbSet<BookVsMovie> BookVsMovies { get; }
    DbSet<BookVsMovieVote> BookVsMovieVotes { get;  }
    DbSet<MovieCollection> MovieCollections { get; }
    DbSet<MovieCollectionItem> MovieCollectionItems { get; }
    DbSet<Notification> Notifications { get;  }
    DbSet<UserMovieList> UserMovieLists { get; }
    DbSet<UserBookFavorite> UserBookFavorites { get; }
    DbSet<Discussion> Discussions { get; }
    DbSet<Comment> Comments { get; }
    DbSet<DiscussionLike> DiscussionLikes { get; }
    DbSet<ReadingProgress> ReadingProgresses { get; }
    DbSet<MovieLike> MovieLikes { get; }
    DbSet<BookLike> BookLikes { get; }
    DbSet<SavedMovieCollection> SavedMovieCollections { get; }
    DbSet<MovieCollectionLike> MovieCollectionLikes { get; }
    DbSet<WatchHistory> WatchHistories { get; }
    DbSet<UserBookWatchlistItem> UserBookWatchlistItems { get; }
    DbSet<ReviewLike> ReviewLikes { get; }
    DbSet<BookReviewLike> BookReviewLikes { get; }
    DbSet<BookCollectionLike> BookCollectionLikes { get; }
    DbSet<SavedBookCollection> SavedBookCollections { get; }
    DbSet<AdminActivityLog> AdminActivityLogs { get; }
    DbSet<LiveStream> LiveStreams { get; }
    DbSet<LiveStreamMessage> LiveStreamMessages { get; }
    DbSet<LiveStreamSchedule> LiveStreamSchedules { get; }
}