using MediatR;
using ProductService.Application.ProductUseCases.Queries;
using ProductService.Core.Interfaces;
using ProductService.Core.Models;
using System.Security.Claims;

namespace ProductService.Application.ProductUseCases.Handlers;

public class GetAllProductsQueryHandler
    : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
{
    private readonly IProductsRepository _repository;
    private readonly IJwtReader _jwtReader;

    public GetAllProductsQueryHandler(
        IProductsRepository repository, IJwtReader jwtReader)
    {
        _repository = repository;
        _jwtReader = jwtReader;
    }

    public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var jwt = _jwtReader.GetTokenFromString(request.AccessToken);
        var claims = _jwtReader.GetClaimsFromToken(jwt);

        if (!claims.Any(c => c.Key == "nameid"))
            return [];
        if (!claims.Any(c => c.Key == "role"))
            return [];

        var id = claims.FirstOrDefault(c => c.Key == "nameid").Value;
        var role = claims.FirstOrDefault(c => c.Key == "role").Value;

        if (role is "Admin")
            return await Task.Run(_repository.GetAll);

        return await Task.Run(() =>
            _repository.Get(p => p.CreatorId.ToString() == id));
    }
}
