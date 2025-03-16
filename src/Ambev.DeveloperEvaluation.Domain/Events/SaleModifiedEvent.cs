using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleModifiedEvent : DomainEvent
{
    public Guid SaleId { get; private set; }
    public decimal NewTotalAmount { get; private set; }
    public List<SaleItem> UpdatedItems { get; private set; }
}
