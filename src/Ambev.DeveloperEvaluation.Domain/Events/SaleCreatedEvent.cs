using Ambev.DeveloperEvaluation.Domain.ReadModels;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCreatedEvent : DomainEvent, INotification
{
    public SaleDocument Sale { get;}

    public SaleCreatedEvent(SaleDocument sale)
    {
        Sale = sale;
    }
}