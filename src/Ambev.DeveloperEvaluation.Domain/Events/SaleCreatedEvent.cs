using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCreatedEvent : DomainEvent
{
    public Guid SaleId { get; private set; }
    public string SaleNumber { get; private set; }
    public DateTime SaleDate { get; private set; }
    public Customer Customer { get; private set; }
    public Branch Branch { get; private set; }
    public decimal TotalAmount { get; private set; }
    public List<SaleItem> Items { get; private set; }
}