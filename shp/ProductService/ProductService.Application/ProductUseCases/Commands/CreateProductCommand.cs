using MediatR;

namespace ProductService.Application.ProductUseCases.Commands;

public record CreateProductCommand(
    string Name, string Description, decimal Price, long AvailableAmount, string Token) : IRequest<Guid>;
