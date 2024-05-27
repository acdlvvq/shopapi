using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using ProductService.Application.ProductUseCases.Commands;
using ProductService.Core.Interfaces;
using ProductService.Core.Models;
using System.Security.Claims;

namespace ProductService.Application.ProductUseCases.Handlers;

public class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductsRepository _repository;
    private readonly IJwtReader _jwtReader;
    private readonly IValidator<Product> _validator;

    public UpdateProductCommandHandler(
        IProductsRepository repository, IJwtReader jwtReader, IValidator<Product> validator)
    {
        _repository = repository;
        _jwtReader = jwtReader;
        _validator = validator;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
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
                throw new InvalidOperationException("You Are Not Allowed To Modify This Product");
        }

        product
            .SetName(request.Name)
            .SetDescription(request.Description)
            .SetPrice(request.Price)
            .SetAvailableAmount(request.AvailableAmount);

        var validationResults = await _validator.ValidateAsync(product);

        if (validationResults.Errors.Count != 0)
            throw new ValidationException(validationResults.ToString());

        await _repository.UpdateAsync(product);
        return await _repository.SaveChangesAsync() != 0;
    }
}
