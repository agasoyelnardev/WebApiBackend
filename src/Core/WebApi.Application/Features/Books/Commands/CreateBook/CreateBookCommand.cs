using MediatR;

namespace WebApi.Application.Features.Books.Commands.CreateBook;

public class CreateBookCommand : IRequest<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Cover { get; set; } = string.Empty;
    public string Language { get; set; } = "az";
    public int Year { get; set; }
    public int Pages { get; set; }
    public string? DownloadUrl { get; set; }
    public string? PdfUrl { get; set; }
    public string? CustomContent { get; set; }
    public bool IsTrending { get; set; }
    public bool IsTopRated { get; set; }
    public bool IsNewRelease { get; set; }
}