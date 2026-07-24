using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Data;


    public class AppDbContext : IdentityDbContext<AppUser>, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<StreamRoom> StreamRooms { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Movie> Movies { get; set; }
        
        public DbSet<UserFollow> UserFollows { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookReview> BookReviews { get; set; }
        public DbSet<BookCollection> BookCollections { get; set; }
        public DbSet<BookCollectionItem> BookCollectionItems { get; set; }
        public DbSet<BookVsMovie> BookVsMovies { get; set; }
        public DbSet<BookVsMovieVote> BookVsMovieVotes { get; set; }
        public DbSet<MovieCollection> MovieCollections { get; set; } 
        public DbSet<MovieCollectionItem> MovieCollectionItems { get; set; } 
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserMovieList> UserMovieLists { get; set; }
        public DbSet<UserBookFavorite> UserBookFavorites { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<DiscussionLike> DiscussionLikes { get; set; }
        public DbSet<ReadingProgress> ReadingProgresses { get; set; }
        public DbSet<MovieLike> MovieLikes { get; set;}
        public DbSet<BookLike> BookLikes { get; set;}
        public DbSet<SavedMovieCollection> SavedMovieCollections { get; set;}
        public DbSet<MovieCollectionLike> MovieCollectionLikes { get; set;}
        public DbSet<WatchHistory> WatchHistories { get; set; }
        public DbSet<UserBookWatchlistItem> UserBookWatchlistItems { get; set; }
        public DbSet<ReviewLike> ReviewLikes { get; set; }
        public DbSet<BookReviewLike> BookReviewLikes { get; set; }   
        public DbSet<BookCollectionLike> BookCollectionLikes { get; }
        public DbSet<SavedBookCollection> SavedBookCollections { get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
