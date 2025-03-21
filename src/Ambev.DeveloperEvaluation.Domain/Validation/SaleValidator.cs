﻿using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(sale => sale.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required");

        RuleFor(sale => sale.BranchId)
            .NotEmpty().WithMessage("Branch ID is required");

        RuleFor(sale => sale.SaleDate)
            .NotEmpty().WithMessage("Sale date is required");

        RuleFor(sale => sale.Items)
            .NotEmpty().WithMessage("Sale must have at least one item");

        // Validate that each item is unique by product ID
        RuleFor(sale => sale.Items)
            .Must(items =>
            {
                var activeItems = items.Where(x => !x.IsCancelled).ToList();
                if (activeItems.Count <= 1)
                    return true;
                return activeItems.Select(i => i.ProductId).Distinct().Count() == activeItems.Count;
            })
            .When(sale => sale.Items != null && sale.Items.Any())
            .WithMessage("Duplicate products are not allowed in a sale");

        // Apply validation for each item
        RuleForEach(sale => sale.Items)
            .SetValidator(new SaleItemValidator());
    }
}
