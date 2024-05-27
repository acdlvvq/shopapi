using MediatR;
using ProductService.Core.Models;

namespace ProductService.Application.ProductUseCases.Queries;

public record GetAllProductsQuery(
    string AccessToken) : IRequest<IEnumerable<Product>>;
