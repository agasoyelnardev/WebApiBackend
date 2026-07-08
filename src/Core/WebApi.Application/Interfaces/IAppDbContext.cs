using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;

namespace WebApi.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Movie> Movies { get; }
    DbSet<Review> Reviews { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}