using Ambev.DeveloperEvaluation.Domain.ReadModels;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class ItemCancelledEvent : DomainEvent, INotification
{
    public SaleDocument SaleDocument { get; set; }

    public ItemCancelledEvent(SaleDocument saleItemDocument)
    {
        SaleDocument = saleItemDocument;
    }
}