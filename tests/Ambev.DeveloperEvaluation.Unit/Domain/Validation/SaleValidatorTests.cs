using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Factories;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

public class SaleValidatorTest
{
    private readonly SaleValidator _validator;
    private readonly SaleFactory _sale = new();
    private readonly SaleItemFactory _item = new();

    public SaleValidatorTest()
    {
        _validator = new SaleValidator();
    }

    [Fact]
    public void Should_HaveError_When_CustomerIdIsEmpty()
    {
        // Arrange
        var sale = _sale.Factory.RuleFor(x => x.CustomerId, Guid.Empty).Generate();

        // Act & Assert
        _validator.TestValidate(sale).ShouldHaveValidationErrorFor(s => s.CustomerId);
    }

    [Fact]
    public void Should_HaveError_When_BranchIdIsEmpty()
    {
        // Arrange
        var sale = _sale.Factory.RuleFor(x => x.BranchId, Guid.Empty).Generate();

        // Act & Assert
        _validator.TestValidate(sale).ShouldHaveValidationErrorFor(s => s.BranchId);
    }

    [Fact]
    public void Should_HaveError_When_ItemsAreEmpty()
    {
        // Arrange
        var sale = _sale.Factory.RuleFor(x => x.Items, new List<SaleItem>()).Generate();

        // Act & Assert
        _validator.TestValidate(sale).ShouldHaveValidationErrorFor(s => s.Items);
    }

    [Fact]
    public void Should_HaveError_When_ItemsContainDuplicateProductIds()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var items = _item.Factory.RuleFor(x => x.ProductId, guid).Generate(2);
        var sale = _sale.Create();
        sale.SetSale(items);

        // Act & Assert
        _validator.TestValidate(sale).ShouldHaveValidationErrorFor(s => s.Items)
            .WithErrorMessage("Duplicate products are not allowed in a sale");
    }

    [Fact]
    public void Should_NotHaveError_When_ItemsContainDuplicateProductIdsButAreCancelled()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var items = _item.Factory.RuleFor(x => x.ProductId, guid).RuleFor(x => x.IsCancelled, true).Generate(2);
        var sale = _sale.Create();

        sale.SetSale(items);

        // Act & Assert
        _validator.TestValidate(sale).ShouldNotHaveValidationErrorFor(s => s.Items);
    }
   
}
