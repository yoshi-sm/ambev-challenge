using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using NSubstitute;


namespace Ambev.DeveloperEvaluation.Unit.Factories;

public class SaleItemFactory
{
    public Faker<SaleItem> Factory = new Faker<SaleItem>()
        .RuleFor(i => i.Id, f => Guid.NewGuid())
        .RuleFor(i => i.SaleId, f => Guid.NewGuid())
        .RuleFor(i => i.ProductId, f => Guid.NewGuid())
        .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
        .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(10, 500))
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
        .RuleFor(i => i.Discount, f => f.Random.Decimal(0, 50))
        .RuleFor(i => i.IsCancelled, false);

}
