using Microsoft.AspNetCore.Identity;

namespace WebApi.Domain.Entities;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    public string Avatar { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}