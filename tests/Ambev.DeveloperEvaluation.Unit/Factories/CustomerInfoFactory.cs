using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Factories;

public static class CustomerInfoFactory
{
    private static readonly Faker<CustomerInfo> _factory = new Faker<CustomerInfo>()
        .RuleFor(c => c.Id, f => Guid.NewGuid())
        .RuleFor(c => c.Name, f => f.Person.FullName);

    public static CustomerInfo Create()
    {
        return _factory.Generate();
    }

    public static CustomerInfo Create(Action<CustomerInfo> customizer)
    {
        var customer = _factory.Generate();
        customizer(customer);
        return customer;
    }
}