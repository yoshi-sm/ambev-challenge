using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Factories;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

public class SaleItemValidatorTest
{
    private readonly SaleItemValidator _validator;
    private readonly SaleItemFactory _item = new();

    public SaleItemValidatorTest()
    {
        _validator = new SaleItemValidator();

    }

    [Fact]
    public void Should_HaveError_When_ProductIdIsEmpty()
    {
        // Arrange
        var saleItem = _item.Factory
            .RuleFor(x => x.ProductId, Guid.Empty)
            .Generate();

        // Act & Assert
        _validator.TestValidate(saleItem).ShouldHaveValidationErrorFor(item => item.ProductId);
    }

    [Fact]
    public void Should_HaveError_When_QuantityIsZeroOrNegative()
    {
        // Arrange
        var saleItem = _item.Factory
            .RuleFor(x => x.Quantity, 0)
            .Generate();

        // Act & Assert
        _validator.TestValidate(saleItem).ShouldHaveValidationErrorFor(item => item.Quantity);
    }

    [Fact]
    public void Should_HaveError_When_QuantityExceeds20()
    {
        // Arrange
        var saleItem = _item.Factory
            .RuleFor(x => x.Quantity, 25)
            .Generate();

        // Act & Assert
        _validator.TestValidate(saleItem).ShouldHaveValidationErrorFor(item => item.Quantity);
    }

    [Fact]
    public void Should_HaveError_When_UnitPriceIsZeroOrNegative()
    {
        // Arrange
        var saleItem = _item.Factory
            .RuleFor(x => x.UnitPrice, 0)
            .Generate();

        // Act & Assert
        _validator.TestValidate(saleItem).ShouldHaveValidationErrorFor(item => item.UnitPrice);
    }

    [Fact]
    public void Should_HaveError_When_DiscountIsNotZero_ForQuantityLessThan4()
    {
        // Arrange
        var saleItem = _item.Factory
            .RuleFor(x => x.Quantity, 3)
            .RuleFor(x => x.Discount, 0.1m)
            .Generate();

        // Act & Assert
        _validator.TestValidate(saleItem).ShouldHaveValidationErrorFor(item => item.Discount);
    }

    [Fact]
    public void Should_HaveError_When_DiscountIsNot10Percent_ForQuantityBetween4And9()
    {
        // Arrange
        var saleItem = _item.Factory
            .RuleFor(x => x.Quantity, 5)
            .RuleFor(x => x.Discount, 0.2m)
            .Generate();

        // Act & Assert
        _validator.TestValidate(saleItem).ShouldHaveValidationErrorFor(item => item.Discount);
    }

    [Fact]
    public void Should_HaveError_When_DiscountIsNot20Percent_ForQuantityBetween10And20()
    {
        // Arrange
        var saleItem = _item.Factory
            .RuleFor(x => x.Quantity, 11)
            .RuleFor(x => x.Discount, 0.1m)
            .Generate();

        // Act & Assert
        _validator.TestValidate(saleItem).ShouldHaveValidationErrorFor(item => item.Discount);
    }

    [Fact]
    public void Should_NotHaveError_ForValidSaleItem()
    {
        // Arrange
        var saleItem = _item.Factory
             .RuleFor(x => x.Quantity, 4)
             .RuleFor(x => x.Discount, 0.1m)
             .RuleFor(x => x.UnitPrice, 3)
             .RuleFor(x => x.ProductId, Guid.NewGuid())
             .Generate();

        // Act & Assert
        _validator.TestValidate(saleItem).ShouldNotHaveAnyValidationErrors();
    }
}