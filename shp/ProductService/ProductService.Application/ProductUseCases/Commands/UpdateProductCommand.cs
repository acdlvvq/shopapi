using MediatR;

namespace ProductService.Application.ProductUseCases.Commands;

public record UpdateProductCommand(
    Guid Id, string Name, string Description, decimal Price, long AvailableAmount, string Token) : IRequest<bool>;
