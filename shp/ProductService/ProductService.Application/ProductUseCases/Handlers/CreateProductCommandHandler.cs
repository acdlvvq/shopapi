using FluentValidation;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using ProductService.Application.ProductUseCases.Commands;
using ProductService.Core.Interfaces;
using ProductService.Core.Models;
using System.Security.Claims;

namespace ProductService.Application.ProductUseCases.Handlers;

public class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductsRepository _repository;
    private readonly IJwtReader _jwtReader;
    private readonly IValidator<Product> _validator;

    public CreateProductCommandHandler(
        IProductsRepository repository, IJwtReader jwtReader, IValidator<Product> validator)
    {
        _repository = repository;
        _jwtReader = jwtReader;
        _validator = validator;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var jwt = _jwtReader.GetTokenFromString(request.Token);
        var claims = _jwtReader.GetClaimsFromToken(jwt);

        if (!claims.Any(c => c.Key == "nameid"))
            throw new SecurityTokenInvalidSignatureException("Token Does Not Contain ID Claim");
        if (!claims.Any(c => c.Key == "role"))
            throw new SecurityTokenInvalidSignatureException("Token Does Not Contain Role Claim");

        var id = claims.FirstOrDefault(c => c.Key == "nameid").Value;
        var role = claims.FirstOrDefault(c => c.Key == "role").Value;

        var product = Product.Create(
            request.Name, request.Description, request.Price, request.AvailableAmount, Guid.Parse(id));
        var validationResults = await _validator.ValidateAsync(product, cancellationToken);
        if (validationResults.Errors.Count != 0)
            throw new ValidationException(validationResults.ToString());

        var productId = await _repository.AddAsync(product);
        await _repository.SaveChangesAsync();

        return productId;
    }
}
