using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class Movie : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string OriginalTitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Poster { get; set; } = string.Empty;
    public string Banner { get; set; } = string.Empty; 
    public double Rating { get; set; }
    public int Year { get; set; }
    public string Duration { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string TrailerUrl { get; set; } = string.Empty;
    public string? VideoUrl { get; set; }
    public int Likes { get; set; }
    
    // AI-ın təklif etdiyi vacib siyahılar:
    public List<string> Genres { get; set; } = new();
    public List<string> Cast { get; set; } = new();
    public string? BookAdaptationId { get; set; }
    
    // İlişki (Relationship):
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}