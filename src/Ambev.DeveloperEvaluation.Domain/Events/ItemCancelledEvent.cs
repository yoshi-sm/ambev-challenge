using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class ItemCancelledEvent : DomainEvent
{
    public Guid SaleId { get; private set; }
    public Guid ItemId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public decimal RefundAmount { get; private set; }
}