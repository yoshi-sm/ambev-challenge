using Ambev.DeveloperEvaluation.Domain.Events.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Serilog.Events;
using Serilog;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.ORM.Events;

public class DomainEventSink : ILogEventSink
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventSink(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Emit(LogEvent logEvent)
    {
        try
        {
            if (logEvent.Properties.TryGetValue("SourceContext", out var sourceContext) &&
                sourceContext.ToString().Contains("DomainEventPublisher"))
            {
                var message = logEvent.RenderMessage();
                if (message.StartsWith("Domain Event:"))
                {
                    ProcessDomainEvent(message);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in DomainEventSink");
        }
    }

    private void ProcessDomainEvent(string message)
    {
        var parts = message.Split(':', 3);
        if (parts.Length == 3)
        {
            var eventType = parts[1];
            var eventData = parts[2];

            // Find the event type
            var eventClrType = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == eventType);

            if (eventClrType != null && eventClrType.BaseType == typeof(DomainEvent))
            {
                var eventObject = JsonSerializer.Deserialize(eventData, eventClrType);
                var handlerType = typeof(IEventHandler<>).MakeGenericType(eventClrType);
                Task.Run(async () =>
                {
                    try
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var handlers = scope.ServiceProvider.GetServices(handlerType);

                        foreach (var handler in handlers)
                        {
                            var method = handlerType.GetMethod("HandleAsync");
                            await (Task)method.Invoke(handler, new[] { eventObject, CancellationToken.None });
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error processing event handlers");
                    }
                });
            }
        }
    }
}