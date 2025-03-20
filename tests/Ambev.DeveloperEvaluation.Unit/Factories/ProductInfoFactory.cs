using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Factories;

public static class ProductInfoFactory
{
    private static readonly Faker<ProductInfo> _factory = new Faker<ProductInfo>()
        .RuleFor(p => p.Id, f => Guid.NewGuid())
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.UnitPrice, f => f.Random.Decimal(10, 500));

    public static ProductInfo Create()
    {
        return _factory.Generate();
    }

    public static List<ProductInfo> CreateList()
    {
        return _factory.Generate(3).ToList();
    }

    public static ProductInfo Create(Action<ProductInfo> customizer)
    {
        var product = _factory.Generate();
        customizer(product);
        return product;
    }
}