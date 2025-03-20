using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Factories;

public static class SaleItemDocumentFactory
{
    private static readonly Faker<SaleItemDocument> _factory = new Faker<SaleItemDocument>()
        .RuleFor(i => i.Id, f => Guid.NewGuid())
        .RuleFor(i => i.SaleId, f => Guid.NewGuid())
        .RuleFor(i => i.Product, f => new ProductInfo
        {
            Id = Guid.NewGuid(),
            Name = f.Commerce.ProductName(),
            UnitPrice = f.Random.Decimal(10, 500)
        })
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
        .RuleFor(i => i.Discount, f => f.Random.Decimal(0, 50))
        .RuleFor(i => i.IsCancelled, f => f.Random.Bool(0.1f));

    static SaleItemDocumentFactory()
    {
        // Calculate TotalPrice after other properties are set
        _factory.FinishWith((f, item) =>
        {
            decimal totalPrice = item.Product.UnitPrice * item.Quantity;
            if (item.Discount > 0)
            {
                totalPrice -= (totalPrice * item.Discount / 100);
            }
            item.TotalPrice = Math.Round(totalPrice, 2);
        });
    }

    public static SaleItemDocument Create()
    {
        return _factory.Generate();
    }

    public static SaleItemDocument Create(Action<SaleItemDocument> customizer)
    {
        var item = _factory.Generate();
        customizer(item);
        return item;
    }

    public static IEnumerable<SaleItemDocument> CreateMany(int count)
    {
        return _factory.Generate(count);
    }

    public static IEnumerable<SaleItemDocument> CreateMany(int count, Action<SaleItemDocument> customizer)
    {
        var items = _factory.Generate(count);
        foreach (var item in items)
        {
            customizer(item);
        }
        return items;
    }
}
