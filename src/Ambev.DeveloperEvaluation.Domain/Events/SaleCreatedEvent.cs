using Ambev.DeveloperEvaluation.Domain.ReadModels;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCreatedEvent : DomainEvent
{
    public SaleDocument Sale { get;}

    public SaleCreatedEvent(SaleDocument sale)
    {
        Sale = sale;
    }
}