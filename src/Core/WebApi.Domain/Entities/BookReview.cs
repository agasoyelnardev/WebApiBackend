using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class BookReview : BaseEntity
{
    // Kitab ilə əlaqə
    public Guid BookId { get; set; }
    public virtual Book Book { get; set; } = null!;

    // İstifadəçi ilə əlaqə
    public string UserId { get; set; } = string.Empty; 
    public virtual AppUser User { get; set; } = null!;

    public int Rating { get; set; } // 1-5 arası qiymətləndirmə
    public string Comment { get; set; } = string.Empty;
    public int Likes { get; set; }
    public int Dislikes { get; set; }
}