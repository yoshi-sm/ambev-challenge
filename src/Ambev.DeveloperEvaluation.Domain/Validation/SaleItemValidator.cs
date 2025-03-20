using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;


namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
    {
        RuleFor(item => item.ProductId)
            .NotEmpty().WithMessage("Product ID is required");

        RuleFor(item => item.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero")
            .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 identical items");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than zero");

        // Discount validation based on quantity
        RuleFor(item => item.Discount)
            .Equal(0).When(item => item.Quantity < 4)
            .WithMessage("Purchases below 4 items cannot have a discount");

        RuleFor(item => item.Discount)
             .Equal(0.1m)
             .When(item => item.Quantity >= 4 && item.Quantity < 10)
             .WithMessage("Purchases between 4 and 9 items must have a 10% discount");

        RuleFor(item => item.Discount)
            .Equal(0.2m)
            .When(item => item.Quantity >= 10 && item.Quantity <= 20)
            .WithMessage("Purchases between 10 and 20 items must have a 20% discount");
    }
}
