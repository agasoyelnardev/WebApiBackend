using Microsoft.AspNetCore.Identity;

namespace WebApi.Domain.Entities;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    public string Avatar { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    
    //Follow
    public ICollection<UserFollow> Followers { get; set; } = new List<UserFollow>();
    public ICollection<UserFollow> Following { get; set; } = new List<UserFollow>();
    
    // Dostluq sistemi
    public ICollection<Friendship> SentFriendRequests { get; set; } = new List<Friendship>();
    public ICollection<Friendship> ReceivedFriendRequests { get; set; } = new List<Friendship>();
}