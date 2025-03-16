using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public abstract class DomainEvent
{
    public Guid Id { get; protected set; }
    public DateTime Timestamp { get; protected set; }

    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        Timestamp = DateTime.UtcNow;
    }
}
