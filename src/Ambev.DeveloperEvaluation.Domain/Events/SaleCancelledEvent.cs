using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCancelledEvent : DomainEvent
{
    public Guid SaleId { get; private set; }
    public string SaleNumber { get; private set; }
    public DateTime CancellationDate { get; private set; }
}
