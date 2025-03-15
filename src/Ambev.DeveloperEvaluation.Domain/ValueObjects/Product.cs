using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public class Product
{
    public Guid ExternalId { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
}
