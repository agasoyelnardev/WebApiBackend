namespace WebApi.Application.Features.Users.Queries.GetCurrentUser;

public class CurrentUserDto
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }

    // Yüngül ID siyahıları — ürək/bookmark ikonlarının statusunu yoxlamaq üçün
    public List<Guid> FavoriteMovieIds { get; set; } = new();
    public List<Guid> WatchlistMovieIds { get; set; } = new();
    public List<Guid> FavoriteBookIds { get; set; } = new();
}