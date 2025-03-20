using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Factories;

public static class SaleDocumentFactory
{
    private static readonly Faker<SaleDocument> Factory = new Faker<SaleDocument>()
        .RuleFor(d => d.Id, f => Guid.NewGuid())
        .RuleFor(d => d.SaleNumber, f => f.Commerce.Product() + "-" + f.Random.Number(1000, 9999))
        .RuleFor(d => d.SaleDate, f => f.Date.Recent(30))
        .RuleFor(d => d.Customer, f => CustomerInfoFactory.Create())
        .RuleFor(d => d.Branch, f => BranchInfoFactory.Create())
        .RuleFor(d => d.TotalAmount, f => f.Random.Decimal(100, 5000))
        .RuleFor(d => d.IsCancelled, f => f.Random.Bool(0.1f))
        .RuleFor(d => d.Items, f => new List<SaleItemDocument>());

    public static SaleDocument Create()
    {
        return Factory.Generate();
    }

    public static SaleDocument Create(Action<SaleDocument> customizer)
    {
        var document = Factory.Generate();
        customizer(document);
        return document;
    }

    public static IEnumerable<SaleDocument> CreateMany(int count)
    {
        return Factory.Generate(count);
    }

    public static IEnumerable<SaleDocument> CreateMany(int count, Action<SaleDocument> customizer)
    {
        var documents = Factory.Generate(count);
        foreach (var document in documents)
        {
            customizer(document);
        }
        return documents;
    }

    public static SaleDocument WithItems(this SaleDocument document, int count = 3)
    {
        document.Items = SaleItemDocumentFactory.CreateMany(count, item =>
        {
            item.SaleId = document.Id;
        }).ToList();

        // Recalculate TotalAmount based on items
        document.TotalAmount = document.Items.Sum(i => i.TotalPrice);

        return document;
    }
}
