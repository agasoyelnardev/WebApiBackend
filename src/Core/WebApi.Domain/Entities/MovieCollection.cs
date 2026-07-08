using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class MovieCollection:BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Cover { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public int LikesCount { get; set; }
    public List<string> MovieIds { get; set; } = new();
}