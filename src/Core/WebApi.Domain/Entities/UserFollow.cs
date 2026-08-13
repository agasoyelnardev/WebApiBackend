using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class UserFollow:BaseEntity
{

    public string FollowerId { get; set; } = string.Empty;
    public AppUser Follower { get; set; } = null!;

    public string FollowingId { get; set; } = string.Empty;
    public AppUser Following { get; set; } = null!;

}