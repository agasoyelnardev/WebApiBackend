using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class StreamRoom : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string StreamUrl { get; set; } = string.Empty;
    public string Type { get; set; } = "Match"; // Match, Movie, TV
    public bool IsLive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public int ViewerCount { get; set; } = 0;
    public string CoverImageUrl { get; set; } = string.Empty;

    // Əlaqəli mesajlar siyahısı
    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}