using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;

namespace WebApi.Persistence.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<StreamRoom> StreamRooms { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<MovieCollection> MovieCollections { get; set; }
    
    
    public DbSet<Movie> Movies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // MovieConfiguration daxil olmaqla bütün konfiqurasiyaları avtomatik tətbiq edir
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}