using Ambev.DeveloperEvaluation.Domain.Events.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.ORM.Events;

public class EventPublisher : IEventPublisher
{
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(ILogger<EventPublisher> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync<T>(T @event) where T : DomainEvent
    {
        _logger.LogInformation($"Event published: {typeof(T).Name} - {JsonSerializer.Serialize(@event)}");

        return Task.CompletedTask;
    }
}