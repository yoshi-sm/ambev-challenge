using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Factories;

public static class BranchInfoFactory
{
    private static readonly Faker<BranchInfo> _factory = new Faker<BranchInfo>()
        .RuleFor(b => b.Id, f => Guid.NewGuid())
        .RuleFor(b => b.Name, f => f.Commerce.Department());

    public static BranchInfo Create()
    {
        return _factory.Generate();
    }

    public static BranchInfo Create(Action<BranchInfo> customizer)
    {
        var branch = _factory.Generate();
        customizer(branch);
        return branch;
    }
}