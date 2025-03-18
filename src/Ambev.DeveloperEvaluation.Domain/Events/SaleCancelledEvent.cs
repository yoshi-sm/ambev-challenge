using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCancelledEvent : DomainEvent, INotification
{
    public SaleDocument Sale { get; }

    public SaleCancelledEvent(SaleDocument sale)
    {
        Sale = sale;
    }
}
