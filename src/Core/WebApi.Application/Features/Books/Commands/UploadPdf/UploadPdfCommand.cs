using MediatR;
using Microsoft.AspNetCore.Http;

namespace WebApi.Application.Features.Books.Commands.UploadPdf;

public class UploadPdfCommand : IRequest<string>
{
    public IFormFile File { get; set; } = null!;
}