using MediatR;

namespace WebApi.Application.Features.Books.Commands.DeleteBook;

public record DeleteBookCommand(Guid Id) : IRequest<bool>;