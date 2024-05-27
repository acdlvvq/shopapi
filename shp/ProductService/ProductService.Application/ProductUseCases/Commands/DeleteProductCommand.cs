using MediatR;

namespace ProductService.Application.ProductUseCases.Commands;

public record DeleteProductCommand(
    Guid Id, string Token) : IRequest<bool>;
