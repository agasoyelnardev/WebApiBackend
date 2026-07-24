namespace WebApi.Application.Features.Users.Queries.GetUserProfile;

public class UserProfileDto
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public int Points { get; set; }
    public string Badge { get; set; } = string.Empty;
    public bool IsPremium { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
}