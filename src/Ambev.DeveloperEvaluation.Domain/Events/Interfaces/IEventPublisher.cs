
namespace Ambev.DeveloperEvaluation.Domain.Events.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event) where T : DomainEvent;
}
