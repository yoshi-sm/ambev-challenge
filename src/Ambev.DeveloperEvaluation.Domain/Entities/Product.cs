using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Product : ExternalEntity
{
    public decimal UnitPrice { get; private set; }
}
