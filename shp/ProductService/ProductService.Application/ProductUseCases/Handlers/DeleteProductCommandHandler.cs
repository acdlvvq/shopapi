using MediatR;
using Microsoft.IdentityModel.Tokens;
using ProductService.Application.ProductUseCases.Commands;
using ProductService.Core.Interfaces;
using System.Security.Claims;

namespace ProductService.Application.ProductUseCases.Handlers;

public class DeleteProductCommandHandler 
    : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductsRepository _repository;
    private readonly IJwtReader _jwtReader;

    public DeleteProductCommandHandler(
        IProductsRepository repository, IJwtReader jwtReader)
    {
        _repository = repository;
        _jwtReader = jwtReader;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var jwt = _jwtReader.GetTokenFromString(request.Token);
        var claims = _jwtReader.GetClaimsFromToken(jwt);

        if (!claims.Any(c => c.Key == "nameid"))
            throw new SecurityTokenInvalidSignatureException("Token Does Not Contain ID Claim");
        if (!claims.Any(c => c.Key == "role"))
            throw new SecurityTokenInvalidSignatureException("Token Does Not Contain Role Claim");

        var id = claims.FirstOrDefault(c => c.Key == "nameid").Value;
        var role = claims.FirstOrDefault(c => c.Key == "role").Value;
        var product = _repository
            .Get(p => p.Id == request.Id)
            .FirstOrDefault() ?? throw new ArgumentException("There Is No Product With This Id");

        if (role is not "Admin")
        {
            if (product.CreatorId.ToString() != id)
                throw new InvalidOperationException("You Are Not Allowed To Delete This Product");
        }

        await _repository.DeleteAsync(request.Id);
        return (await _repository.SaveChangesAsync()) != 0;
    }
}
