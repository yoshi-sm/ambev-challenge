using Ambev.DeveloperEvaluation.Domain.Events.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.ORM.Events;

public class DomainEventPublisher : IEventPublisher
{
    private readonly ILogger<DomainEventPublisher> _logger;

    public DomainEventPublisher(ILogger<DomainEventPublisher> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync<T>(T @event) where T : DomainEvent
    {
        _logger.LogInformation($"Domain Event:{typeof(T).Name}:{JsonSerializer.Serialize(@event)}");

        return Task.CompletedTask;
    }
}