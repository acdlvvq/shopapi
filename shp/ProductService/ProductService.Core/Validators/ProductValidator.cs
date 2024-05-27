using FluentValidation;
using ProductService.Core.Models;

namespace ProductService.Core.Validators;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        var message = "Error in {PropertyName}: value {PropertyValue}";

        RuleFor(p => p.Name)
            .Must(n => n.All(char.IsLetter)).WithMessage(message)
            .MinimumLength(3).WithMessage(message);

        RuleFor(p => p.Description)
            .MinimumLength(3).WithMessage(message);

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage(message);

        RuleFor(p => p.AvailableAmount)
            .GreaterThan(0).WithMessage(message);
    }
}
