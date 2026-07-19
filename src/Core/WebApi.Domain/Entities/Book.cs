using WebApi.Domain.Entities.Base;

namespace WebApi.Domain.Entities;

public class Book : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Cover { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string Language { get; set; } = "az";
    public int Year { get; set; }
    public int Pages { get; set; }
    public int Likes { get; set; }

    public string? DownloadUrl { get; set; }
    public string? PdfUrl { get; set; }
    public string? CustomContent { get; set; }

    public bool IsTrending { get; set; }
    public bool IsTopRated { get; set; }
    public bool IsNewRelease { get; set; }

 
    public virtual ICollection<Movie> MovieAdaptations { get; set; } = new List<Movie>();

    public virtual ICollection<BookReview> Reviews { get; set; } = new List<BookReview>();
    public virtual ICollection<BookCollectionItem> CollectionItems { get; set; } = new List<BookCollectionItem>();
}