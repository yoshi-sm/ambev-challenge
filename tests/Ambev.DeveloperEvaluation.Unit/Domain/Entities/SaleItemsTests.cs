using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Factories;
using FluentValidation.Results;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleItemsTests
{
    private readonly SaleFactory _sale = new();
    private readonly SaleItemFactory _item = new();



    [Fact]
    public void Validate_ShouldReturnValidationsResult()
    {
        // Arrange
        var saleItem = _item.Factory.Generate();

        // Act
        var result = saleItem.Validate();

        // Assert
        Assert.NotNull(result);
    }


    [Fact]
    public void Cancel_ShouldSetIsCancelledToTrue()
    {
        // Arrange
        var saleItem = _item.Factory.Generate();

        // Act
        saleItem.Cancel();

        // Assert
        Assert.True(saleItem.IsCancelled);
    }

    [Fact]
    public void SetSaleItem_ShouldSetSaleId_AndCalculatePrices()
    {
        // Arrange
        var saleItem = _item.Factory.RuleFor(x => x.UnitPrice, 10m).RuleFor(x => x.Quantity, 5).Generate();

        // Act
        saleItem.SetSaleItem(Guid.NewGuid());

        // Assert
        Assert.Equal(0.1m, saleItem.Discount);
        Assert.NotEqual(Guid.Empty, saleItem.SaleId);
        Assert.Equal(45m, saleItem.TotalPrice);
    }

    [Fact]
    public void SetSaleItem_ShouldApplyCorrectDiscount_ForQuantityBetween10And20()
    {
        // Arrange
        var saleItem = _item.Factory.RuleFor(x => x.UnitPrice, 20m).RuleFor(x => x.Quantity, 15).Generate(); ;

        // Act
        saleItem.SetSaleItem(Guid.NewGuid());

        // Assert
        Assert.Equal(240m, saleItem.TotalPrice); // 20 * 15 - 20% discount
    }

    [Fact]
    public void SetSaleItem_ShouldApplyNoDiscount_ForQuantityLessThan4()
    {
        // Arrange
        var saleItem = _item.Factory.RuleFor(x => x.UnitPrice, 50m).RuleFor(x => x.Quantity, 2).Generate(); ;

        // Act
        saleItem.SetSaleItem(Guid.NewGuid());

        // Assert
        Assert.Equal(100m, saleItem.TotalPrice); // 50 * 2 - no discount
    }

    [Fact]
    public void SetSaleItem_ShouldHandleZeroQuantity()
    {
        // Arrange
        var saleItem = _item.Factory.RuleFor(x => x.UnitPrice, 10m).RuleFor(x => x.Quantity, 0).Generate(); ;

        // Act
        saleItem.SetSaleItem(Guid.NewGuid());

        // Assert
        Assert.Equal(0m, saleItem.TotalPrice); // No items, total price is 0
    }

    [Fact]
    public void Cancel_ShouldNotAffectTotalPrice()
    {
        // Arrange
        var saleItem = _item.Factory.RuleFor(x => x.TotalPrice, 135m).Generate();

        // Act
        saleItem.Cancel();

        // Assert
        Assert.True(saleItem.IsCancelled);
        Assert.Equal(135m, saleItem.TotalPrice); // Total price remains unchanged
    }

    [Fact]
    public void Constructor_SetsAllProperties_Correctly()
    {
        // Arrange
        Guid saleId = Guid.NewGuid();
        Guid productId = Guid.NewGuid();
        string productName = "Test Product";
        decimal unitPrice = 19.99m;
        int quantity = 5;
        bool isCancelled = false;
        decimal discount = 0.2m;
        decimal totalPrice = 30m;


        // Act
        SaleItem saleItem = new SaleItem(
            saleId,
            productId,
            productName,
            unitPrice,
            quantity,
            discount,
            totalPrice,
            isCancelled
        );

        // Assert

        Assert.Equal(productId, saleItem.ProductId);
        Assert.Equal(productName, saleItem.ProductName);
        Assert.Equal(unitPrice, saleItem.UnitPrice);
        Assert.Equal(quantity, saleItem.Quantity);
        Assert.Equal(isCancelled, saleItem.IsCancelled);
        Assert.Equal(saleId, saleItem.SaleId);
        Assert.Equal(discount, saleItem.Discount);
        Assert.Equal(totalPrice, saleItem.TotalPrice);
    }
}
