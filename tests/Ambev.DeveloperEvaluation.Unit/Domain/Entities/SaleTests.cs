using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Factories;
using Bogus;
using FluentValidation.Results;
using NSubstitute;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    private readonly SaleFactory _sale = new();
    private readonly SaleItemFactory _item = new();

    [Fact]
    public void Cancel_ShouldSetIsCancelledToTrue_AndCancelAllItems()
    {
        // Arrange
        var items = _item.Factory.Generate(3).ToList();
        var sale = _sale.Create();
        sale.SetSale(items);

        // Act
        sale.Cancel();

        // Assert
        Assert.True(sale.IsCancelled);
        Assert.All(sale.Items, item => Assert.True(item.IsCancelled));
    }

    [Fact]
    public void SetSale_ShouldGenerateSaleNumber_AndCalculateTotalAmount()
    {
        // Arrange && Act
        var saleItems = _item.Factory.Generate(3);
        var sale = _sale.Create();

        sale.SetSale(saleItems);

        // Assert
        Assert.NotEqual(sale.SaleNumber, "empty");
        Assert.Equal(saleItems.Sum(x => x.TotalPrice), sale.TotalAmount);
    }

    [Fact]
    public void SetSale_CalculateTotalAmount_ShouldSetTotalAmount_AndCancelSaleIfTotalIsZero()
    {
        // Arrange 
        var items1 = _item.Factory
            .RuleFor(x => x.UnitPrice, 0)
            .RuleFor(x => x.Quantity, 4)
            .Generate(3).ToList();
        var sale = _sale.Create();

        // Act
        sale.SetSale(items1);

        // Assert
        Assert.Equal(0m, sale.TotalAmount);
        Assert.True(sale.IsCancelled);
    }

    [Fact]
    public void SetSale_CalculateTotalAmount_ShouldIgnoreCancelledItems()
    {
        // Arrange && Act
        var items1 = _item.Factory
           .RuleFor(x => x.UnitPrice, 3)
           .RuleFor(x => x.Quantity, 1)
           .Generate(3).ToList();
        var items2 = _item.Factory
            .RuleFor(x => x.IsCancelled, true).Generate();

        items1.Add(items2);
        var sale = _sale.Create();

        sale.SetSale(items1);

        // Assert
        Assert.Equal(9m, sale.TotalAmount);
    }

    [Fact]
    public void Constructor_SetsAllProperties_Correctly()
    {
        // Arrange
        string saleNumber = "S12345";
        DateTime saleDate = DateTime.Now;
        Guid customerId = Guid.NewGuid();
        string customerName = "John Doe";
        Guid branchId = Guid.NewGuid();
        string branchName = "Main Branch";
        decimal totalAmount = 1250.75m;
        bool isCancelled = false;
        ICollection<SaleItem> items = new List<SaleItem>();

        // Act
        Sale sale = new Sale(saleNumber,
            saleDate,
            customerId,
            customerName,
            branchId,
            branchName,
            totalAmount,
            isCancelled,
            items
        );

        // Assert
        Assert.Equal(saleNumber, sale.SaleNumber);
        Assert.Equal(saleDate, sale.SaleDate);
        Assert.Equal(customerId, sale.CustomerId);
        Assert.Equal(customerName, sale.CustomerName);
        Assert.Equal(branchId, sale.BranchId);
        Assert.Equal(branchName, sale.BranchName);
        Assert.Equal(totalAmount, sale.TotalAmount);
        Assert.Equal(isCancelled, sale.IsCancelled);
        Assert.Same(items, sale.Items);
    }
}