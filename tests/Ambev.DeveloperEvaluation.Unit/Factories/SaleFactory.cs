using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Factories;

public class SaleFactory
{
    public Faker<Sale> Factory = new Faker<Sale>()
        .RuleFor(s => s.Id, f => Guid.NewGuid())
        .RuleFor(s => s.SaleNumber, f => f.Commerce.Product() + "-" + f.Random.Number(1000, 9999))
        .RuleFor(s => s.SaleDate, f => f.Date.Recent(30))
        .RuleFor(s => s.CustomerId, f => Guid.NewGuid())
        .RuleFor(s => s.CustomerName, f => f.Person.FullName)
        .RuleFor(s => s.BranchId, f => Guid.NewGuid())
        .RuleFor(s => s.BranchName, f => f.Commerce.Department())
        .RuleFor(s => s.TotalAmount, f => f.Random.Decimal(100, 5000))
        .RuleFor(s => s.IsCancelled, f => false);

    public Sale Create()
    {
        return Factory.Generate();
    }

}