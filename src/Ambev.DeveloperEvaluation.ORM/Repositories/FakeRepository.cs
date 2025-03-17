using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class FakeRepository : IFakeRepository
{
    private readonly List<CustomerInfo> _customers;
    private readonly List<BranchInfo> _branches;
    private readonly List<ProductInfo> _products;

    public FakeRepository(int seedCount = 100)
    {
        // Set a consistent seed for reproducible results if needed
        Randomizer.Seed = new Random(8675309);

        // Generate fake customers
        var customerFaker = new Faker<CustomerInfo>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => f.Company.CompanyName());

        _customers = customerFaker.Generate(seedCount);

        // Generate fake branches
        var branchFaker = new Faker<BranchInfo>()
            .RuleFor(b => b.Id, f => f.Random.Guid())
            .RuleFor(b => b.Name, f => $"{f.Address.City()} Branch");

        _branches = branchFaker.Generate(seedCount);

        // Generate fake products
        var productFaker = new Faker<ProductInfo>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.UnitPrice, f => decimal.Parse(f.Commerce.Price(0.99m, 1999.99m)));

        _products = productFaker.Generate(seedCount);
    }

    public CustomerInfo GetCustomerById(Guid id)
    {
        var randomIndex = Math.Abs(id.GetHashCode()) % _customers.Count;
        var customer = _customers[randomIndex];
        customer.Id = id;
        return customer;
    }

    public BranchInfo GetBranchById(Guid id)
    {
        var randomIndex = Math.Abs(id.GetHashCode()) % _branches.Count;
        var branch = _branches[randomIndex];
        branch.Id = id;
        return branch;
    }

    public IEnumerable<ProductInfo> GetProductsByIds(IEnumerable<Guid> ids)
    {
        List<ProductInfo> products = new();
        foreach(var id in ids)
        {
            var randomIndex = Math.Abs(id.GetHashCode()) % _products.Count;
            var product = _products[randomIndex];
            product.Id = id;
            products.Add(product);
        }
        return products;
    }
}


